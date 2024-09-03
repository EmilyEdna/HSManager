
using NStandard;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace HSManager.Tools
{
    public class UnpackTools
    {
        public static event Action<string,object> PythonData;

        /// <summary>
        /// 解包Python
        /// </summary>
        public static void CreateUnpackPython3D()
        {
            //加载python资源
            Stream stream = Assembly.GetAssembly(typeof(App))
                .GetManifestResourceStream("HSManager.Unpack3d.exe");

            byte[] bytes = new byte[stream.Length];
            stream.Read(bytes, 0, bytes.Length);

            var files = Path.Combine(Path.GetTempPath(), "PythonUnity3dUnpack.exe");
            if (!File.Exists(files))
            {
                using var fs = new FileStream(files, FileMode.CreateNew, FileAccess.Write);
                fs.Write(bytes, 0, bytes.Length);
            }
        }

        /// <summary>
        /// 移除Python
        /// </summary>
        public static void RemoveUnpackPython3D()
        {
            var files = Path.Combine(Path.GetTempPath(), "PythonUnity3dUnpack.exe");
            if (File.Exists(files))
                File.Delete(files);
        }

        public static void RunPython(string path,object obj=null)
        {
            Process P = new Process();
            // 设置要启动的程序
            P.StartInfo.FileName = Path.Combine(Path.GetTempPath(), "PythonUnity3dUnpack.exe");
            //管理员模式
            P.StartInfo.Verb = "RunAs";
            P.StartInfo.Arguments = path;
            //工作路径
            //P.StartInfo.WorkingDirectory = AppDomain.CurrentDomain.BaseDirectory;
            // 设置启动为当前项目的子线层
            P.StartInfo.UseShellExecute = false;        //是否使用操作系统shell启动
            P.StartInfo.RedirectStandardInput = true;   //接受来自调用程序的输入信息
            P.StartInfo.RedirectStandardOutput = true;  //由调用程序获取输出信息
            P.StartInfo.RedirectStandardError = true;   //重定向标准错误输出
            P.StartInfo.CreateNoWindow = true;          //不显示程序窗口
            // 为异步获取订阅事件 
            P.OutputDataReceived += new DataReceivedEventHandler((sender, e) =>
            {
                if(!e.Data.IsNullOrEmpty())
                    PythonData?.Invoke(e.Data, obj);
            });
            // 启动
            P.Start();
            // 异步获取命令行内容  
            P.BeginOutputReadLine();
            P.StandardInput.AutoFlush = true;
        }
    }
}
