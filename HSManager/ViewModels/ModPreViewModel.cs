using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HSManager.ObjectModels;
using HSManager.properties;
using HSManager.Tools;
using ICSharpCode.SharpZipLib.Zip;
using SkiaSharp;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Xml;
using XExten.Advance.IocFramework;
using XExten.Advance.LinqFramework;

namespace HSManager.ViewModels
{
    public partial class ModPreViewModel : ObservableObject
    {

        private List<ModsInfo> _MemeryData;
        public ModPreViewModel()
        {
            _MemeryData = [];
            Mods = [];
        }

        [ObservableProperty]
        private ObservableCollection<ModsInfo> _Mods;
        [ObservableProperty]
        private string _Route;

        private string _Filter;
        public string Filter
        {
            get => _Filter;
            set {
                SetProperty(ref _Filter, value);
                Mods = new(_MemeryData.Where(t => t.Guid.ToUpper().Contains(value.ToUpper())).ToList());
            }
        }

        [RelayCommand]
        public void Handle(string cate)
        {
            if (cate.AsInt() == 1)
            {
                Route = DependencyTool.GetRoute();
                LoadMod();
            }
            else
            {
                foreach (ModsInfo mod in Mods)
                {
                    try
                    {
                        FileInfo fi = new FileInfo(mod.Route);
                        var targert = Path.Combine(fi.DirectoryName, mod.Guid + ".zipmod");
                        File.Move(mod.Route, targert);
                        mod.Route = targert;
                    }
                    catch
                    {
                    }
                }
            }
        }

        [RelayCommand]
        public void Rename(ModsInfo mods)
        {
            FileInfo fi = new FileInfo(mods.Route);
            var targert = Path.Combine(fi.DirectoryName, mods.Guid + ".zipmod");
            File.Move(mods.Route, targert);
            mods.Route = targert;
        }

        private void LoadMod()
        {
            List<string> files = [];
            Directory.GetFiles(Route).Where(t => Path.GetExtension(t).ToLower().Contains("zipmod")).ForEnumerEach(files.Add);
            EachFolder(Route, files);
            ReadMod(files);
        }
        private void ReadMod(List<string> files) 
        {
            Task.Run(() =>
            {
                files.ForEnumerEach((item, index) =>
                {
                    ModsInfo info = new()
                    {
                        Route = item,
                        Index = index + 1,
                        U3d = []
                    };
                    try
                    {
                        using var zipread = new ZipFile(item);
                        foreach (ZipEntry entry in zipread)
                        {
                            if (entry.Name.Contains("manifest.xml"))
                            {
                                using var stream = zipread.GetInputStream(entry);
                                using var reader = new StreamReader(stream);
                                var xmldata = reader.ReadToEnd();

                                XmlDocument xml = new XmlDocument();
                                xml.LoadXml(xmldata);
                                var doc = xml.DocumentElement;

                                info.Guid = doc.SelectSingleNode("guid")?.InnerText?.ToUpper();
                                info.Name = doc.SelectSingleNode("name")?.InnerText?.ToUpper();
                                info.Author = doc.SelectSingleNode("author")?.InnerText?.ToUpper();
                                info.Version = doc.SelectSingleNode("version")?.InnerText?.ToUpper();

                                Application.Current.Dispatcher.Invoke(() =>
                                {
                                    if (!Mods.Any(t => t.Guid == info.Guid))
                                        Mods.Add(info);
                                    else
                                    {
                                        var tempm = Mods.FirstOrDefault(t => t.Guid == info.Guid);
                                        var RV = (ModRepeatViewModel)IocDependency.ResolveByNamed(typeof(ModRepeatViewModel), "ModRepeatViewModel");

                                        if (!RV.Mods.Any(t => t.Guid == tempm.Guid && t.Route == tempm.Route))
                                            RV.Mods.Add(tempm);
                                        RV.Mods.Add(info);
                                    }
                                });
                            }
                        }
                        foreach (ZipEntry entry in zipread)
                        {
                            if (entry.Name.Contains(".png") || entry.Name.Contains(".jpg"))
                            {
                                try
                                {
                                    using var stream = zipread.GetInputStream(entry);
                                    byte[] bytes = new BinaryReader(stream).ReadBytes((int)entry.Size);
                                    info.PreImage = SKBitmap.Decode(bytes).Resize(new SKImageInfo(80, 80), SKFilterQuality.High);
                                }
                                catch
                                {
                                    Console.WriteLine(entry.Name);
                                }
                            }
                            if (entry.Name.Contains(".csv"))
                            {
                                List<Dictionary<string, string>> datas = new List<Dictionary<string, string>>();

                                using var stream = zipread.GetInputStream(entry);
                                using var reader = new StreamReader(stream);
                                var csvData = reader.ReadToEnd().Replace("\r", "").Split("\n")
                                .Where(t => !t.IsNullOrEmpty()).ToList();
                                csvData.RemoveRange(0, 3);
                                var Keys = csvData.First().Split(",");
                                csvData.RemoveAt(0);

                                csvData.ForEach(node =>
                                {
                                    Dictionary<string, string> keyValues = new Dictionary<string, string>();

                                    node.Split(",").ForEnumerEach((n, i) =>
                                    {
                                        keyValues.Add(Keys[i].ToLower(), n);
                                    });

                                    datas.Add(keyValues);

                                });

                                datas.ForEach(t =>
                                {

                                    var key = t.Keys.First(t => t == "mainab");
                                    if (t[key].Contains("chara") && !Soft.Default.GameRoute.IsNullOrEmpty())
                                    {
                                        U3d _3d = new U3d();
                                        _3d.SortRoute = Path.Combine("abdata", t[key]).Replace("/", "\\");
                                        _3d.Route = Path.Combine(Soft.Default.GameRoute, _3d.SortRoute);
                                        if (File.Exists(_3d.Route))
                                            info.U3d.Add(_3d);
                                    }
                                });
                            }
                        }


                        info.U3d = info.U3d.Distinct().ToList();

                        var MV  = (CharaViewModel)IocDependency.ResolveByNamed(typeof(CharaViewModel), "CharaViewModel");
                        var SV = (SceneViewModel)IocDependency.ResolveByNamed(typeof(SceneViewModel), "SceneViewModel");
                        if (MV.MissMod != null)
                        {
                            MV.MissMod.ToList().ForEach(item =>
                            {
                                if (Mods.Any(t => t.Guid.ToUpper().Equals(item.ToUpper())))
                                {
                                    MV.MissMod.Remove(item);
                                }
                            });
                        }
                        if (SV.Scene != null)
                        {
                            SV.Scene.ToList().ForEach(item =>
                            {
                                if (Mods.Any(t => t.Guid.ToUpper().Equals(item.Guid.ToUpper())))
                                {
                                    item.IsMiss = "存在";
                                }
                            });
                        }
                    }
                    catch
                    {
                    }

                });

                _MemeryData = Mods.ToList();
            });

        }
        private void EachFolder(string dir, List<string> files)
        {
            var Dirs = Directory.GetDirectories(dir);
            if (Dirs.Count() > 0)
            {
                Dirs.ForArrayEach<string>(node =>
                {

                    var Mods = Directory.GetFiles(node).Where(t => Path.GetExtension(t).ToLower().Contains("zipmod"));
                    if (Mods.Count() > 0)
                        files.AddRange(Mods);
                    EachFolder(node, files);
                });
            }
        }
    }
}
