using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myapp
{
    public enum GPIODirection
    {
        In,
        Out
    }

    public class TinyGPIO
    {
        public class Debug
        {
            public static string DebugPath { get { return AppDomain.CurrentDomain.BaseDirectory; } }
        }

        private static string BasePath { get { return "/sys/class/gpio"; } }

        private static string ConvertPath(string path)
        {
            if (Environment.OSVersion.Platform == PlatformID.Win32NT)
            {
                var debugDevPath = Debug.DebugPath;
                path = path.TrimStart('/').Replace('/', '\\');
                return Path.Combine(debugDevPath, path);
            }
            else
            {
                return path;
            }
        }

        private static void RawWrite(string path, object value)
        {
            File.WriteAllText(ConvertPath(path), value.ToString());
        }

        private static string RawRead(string path)
        {
            return File.ReadAllLines(ConvertPath(path)).FirstOrDefault();
        }

        public static TinyGPIO Export(int port)
        {
            var portPath = GetGPIOPortPath(port);
            if (!Directory.Exists(portPath))
            {
                RawWrite(BasePath + "/export", port.ToString());
            }
            return new TinyGPIO(port);
        }

        private string PortPath { get; set; }

        public int Value
        {
            get { return int.Parse(this.Read("value")); }
            set { this.Write("value", value); }
        }

        public GPIODirection Direction
        {
            get { return (GPIODirection)Enum.Parse(typeof(GPIODirection), this.Read("direction"), ignoreCase: true); }
            set { this.Write("direction", value.ToString().ToLower()); }
        }

        private TinyGPIO(int port)
        {
            this.PortPath = GetGPIOPortPath(port);
        }

        private static string GetGPIOPortPath(int port)
        {
            return BasePath + "/gpio" + port.ToString();
        }

        private void Write(string name, object value)
        {
            name = name.TrimStart('/');
            RawWrite(this.PortPath + "/" + name, value);
        }

        private string Read(string name)
        {
            name = name.TrimStart('/');
            return RawRead(this.PortPath + "/" + name);
        }
    }
}
