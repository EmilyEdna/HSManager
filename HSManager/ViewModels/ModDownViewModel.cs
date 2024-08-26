using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HSManager.ObjectModels;
using HSManager.properties;
using HSManager.Tools;
using System.Collections.ObjectModel;
using System.IO;
using XExten.Advance.LinqFramework;
using XExten.Advance.ThreadFramework;

namespace HSManager.ViewModels
{
    public partial class ModDownViewModel : ObservableObject
    {
        private List<TreeChild> _MemeryDic;
        private Queue<TreeChild> _DownQueue;
        private static int _DownCount = 0;
        public ModDownViewModel()
        {
            _MemeryDic = [];
            _DownQueue = [];
            LoadModWebSite();
            HttpSchedule.ReceiveAction = new((item, obj) =>
            {
                ((TreeChild)obj).Process = item;
                ((TreeChild)obj).State = "下载中";
                if (item == 100d)
                {
                    ((TreeChild)obj).State = "下载完成";
                    _DownCount -= 1;
                }
            });
        }

        [ObservableProperty]
        private TreeInfo _Tree;
        [ObservableProperty]
        private ObservableCollection<TreeChild> _Downs;
        [ObservableProperty]
        private bool _Check;
        private string _Filter;
        public string Filter
        {
            get => _Filter;
            set
            {
                SetProperty(ref _Filter, value);
                Tree.KeyValue = new(_MemeryDic.Where(t => t.Key.ToUpper().Contains(value.ToUpper())).ToList());
            }
        }

        [RelayCommand]
        public async Task Select(string path)
        {
            if (path.IsNullOrEmpty()) return;
            if (!Path.GetExtension(path).Contains(".zipmod"))
            {
                Tree = await HS2Betterrepack.GetRoot(path);
                _MemeryDic = [.. Tree.KeyValue];
            }
        }

        [RelayCommand]
        public void Handle(string cate)
        {
            var Flag = cate.AsInt();
            if (Flag == 1)
                foreach (var kv in Tree.KeyValue)
                {
                    kv.Check = true;
                }
            else if (Flag == 2)
                foreach (var kv in Tree.KeyValue)
                {
                    kv.Check = false;
                }
            else
            {
                if (Soft.Default.SaveRoute.IsNullOrEmpty()) return;
                var Temp = Tree.KeyValue.Where(t => t.Check == true);
                Temp.ForEnumerEach(_DownQueue.Enqueue);
                Downs = new(Temp);
                DownZipmod();
            }
        }

        private void DownZipmod()
        {
            ThreadFactory.Instance.StartWithRestart(() =>
            {
                if (_DownQueue.Count <= 0)
                {
                    ThreadFactory.Instance.StopTask("Down");
                    return;
                }
                if (_DownCount <= 5)
                {
                    for (int i = 0; i < 5; i++)
                    { 
                        if (_DownCount > 5) return;
                        if (_DownQueue.Count <= 0) return;
                        var Child = _DownQueue.Dequeue();

                        var URI = HS2Betterrepack.Host + Child.Value;

                        var FILE = Path.Combine(Soft.Default.SaveRoute, Path.GetFileName(URI));

                        HttpSchedule.HttpDownload(URI, FILE, Child);
                        _DownCount++;
                    }
                }
                Thread.Sleep(1000);

            }, "Down", null, false);
        }

        private async void LoadModWebSite()
        {
            Tree = await HS2Betterrepack.GetRoot();
        }
    }
}
