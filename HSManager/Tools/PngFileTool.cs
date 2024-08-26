using HSManager.Dto;
using HSManager.ObjectModels;
using MessagePack;
using Org.BouncyCastle.Utilities;
using System.IO;
using System.Text.RegularExpressions;
using System.Text;
using XExten.Advance.LinqFramework;
using Newtonsoft.Json.Linq;

namespace HSManager.Tools
{
    public class PngFileTool
    {
        private static readonly byte[] _pngEndChunk = { 0x49, 0x45, 0x4E, 0x44, 0xAE, 0x42, 0x60, 0x82 };

        private static long SearchForSequence(Stream stream, byte[] sequence)
        {
            const int bufferSize = 4096;
            var origPos = stream.Position;

            var buffer = new byte[bufferSize];
            int read;

            var scanByte = sequence[0];

            while ((read = stream.Read(buffer, 0, bufferSize)) > 0)
            {
                for (var i = 0; i < read; i++)
                {
                    if (buffer[i] != scanByte)
                        continue;

                    var flag = true;

                    for (var x = 1; x < sequence.Length; x++)
                    {
                        i++;

                        if (i >= bufferSize)
                        {
                            if ((read = stream.Read(buffer, 0, bufferSize)) < bufferSize)
                                return -1;

                            i = 0;
                        }

                        if (buffer[i] != sequence[x])
                        {
                            flag = false;
                            break;
                        }
                    }

                    if (flag)
                    {
                        var result = (stream.Position + 1) - (bufferSize - i) - sequence.Length;
                        stream.Position = origPos;
                        return result;
                    }
                }
            }

            return -1;
        }

        public static long SearchForPngEnd(Stream stream)
        {
            var result = SearchForSequence(stream, _pngEndChunk);
            if (result >= 0) result += _pngEndChunk.Length;
            return result;
        }

        public static byte[] PngBytes(string files)
        {
            FileInfo file = new FileInfo(files);
            using var stream = File.Open(files, FileMode.Open, FileAccess.Read, FileShare.Read);
            byte[] buffer = new byte[stream.Length];
            stream.Read(buffer, 0, buffer.Length);
            return buffer;
        }
        public static CharaDataInfo ReadCharaInfo(byte[] bytes)
        {
            using Stream stream = new MemoryStream(bytes);
            using var reader = new BinaryReader(stream);
            var pngEnd = SearchForPngEnd(stream);

            if (pngEnd == -1 || pngEnd >= stream.Length)
                return null;
            stream.Position = pngEnd;

            var loadProductNo = reader.ReadInt32();
            var marker = reader.ReadString();
            var version = reader.ReadString();
            var language = reader.ReadInt32();
            var userID = reader.ReadString();
            var dataID = reader.ReadString();
            var count = reader.ReadInt32();
            var databyte = reader.ReadBytes(count);
            var blockHeader = MessagePackSerializer.Deserialize<CharaInfo>(databyte);
            var num = reader.ReadInt64();
            var position = reader.BaseStream.Position;

            var info = blockHeader.lstInfo.First(t => t.name == "KKEx");

            Dictionary<string, PluginData> extData = null;
            if (info != null)
            {
                reader.BaseStream.Seek(position + info.pos, SeekOrigin.Begin);
                var parameterBytes = reader.ReadBytes((int)info.size);
                extData = MessagePackSerializer.Deserialize<Dictionary<string, PluginData>>(parameterBytes);
            }

            foreach (var data in extData.Values.Where(t => t != null))
            {
                DeserializeObjects(data);
            }
            var PluginInfo = extData.Keys.ToList();
            var ModsInfo = extData.Where(t => t.Value != null).SelectMany(x => x.Value.RequiredZipmodGUIDs).Distinct().ToList();

            CharaDataInfo chara = new CharaDataInfo();
            chara.PluginInfo = new(PluginInfo);
            chara.ModInfo = new(ModsInfo);
            return chara;
        }

        public static List<Dictionary<string, string>> ReadSceneInfo(byte[] bytes)
        {

            List<Dictionary<string, string>> sb = new List<Dictionary<string, string>>();

            var Charas = Encoding.ASCII.GetString(bytes).WithRegex("universalautoresolver\\W+itemInfo[a-zA-Z0-9\\s\\S!@#$%^&*()]+")
                .Replace("\0", "").Replace(">", "").Replace("\r", "").Replace("\n", "")
                .Split("?", StringSplitOptions.RemoveEmptyEntries)
                .Where(t => !t.IsNullOrEmpty()).Where(t => Regex.IsMatch(t, "[a-zA-Z0-9]+")).ToList();

            Charas.ForEnumerEach((node, index) =>
            {

                if (node.ToUpper() == "MODID")
                {
                    var data = Regex.Replace(Charas.ElementAtOrDefault(index + 1), "[!|@|#|&|*|?|^|$]", "");
                    sb.Add(new Dictionary<string, string> { { "ModId", data } });
                }

                if (node.ToUpper() == "AUTHOR")
                {
                    var data = Regex.Replace(Charas.ElementAtOrDefault(index + 1), "[!|@|#|&|*|?|^|$]", "");
                    sb.Add(new Dictionary<string, string> { { "Author", data } });
                }

                if (node.ToUpper() == "NAME")
                {
                    var data = Regex.Replace(Charas.ElementAtOrDefault(index + 1), "[!|@|#|&|*|?|^|$]", "");
                    sb.Add(new Dictionary<string, string> { { "Name", data } });
                }

            });

            return sb;
        }

        private static void DeserializeObjects(PluginData data)
        {
            foreach (var item in data.data.ToList())
            {
                switch (item.Key)
                {
                    case "info":
                        var resolveInfos = ((Array)item.Value).Cast<byte[]>().Select(ResolveInfo.Deserialize).ToList();
                        data.data[item.Key] = resolveInfos;
                        data.RequiredZipmodGUIDs.AddRange(resolveInfos.Select(x => x.GUID));
                        break;
                }
            }
        }

    }
}
