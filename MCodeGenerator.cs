using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;
using System.Runtime.InteropServices;

namespace mcode_generator
{
    public class MCodeGeneratorOptions
    {
        public bool _IsX86 { get; set; }
        public bool _IsX64 { get; set; }
        public string OptimizationLevel { get; set; }
    }
    public class MCodeGenerator : IDisposable
    {
        bool disposed = false;
        private readonly string _TempCodeFilePath = null;
        private readonly string _ResultFilePath = null;
        private MCodeGeneratorOptions _Options = null;
        public string _ConsoleOutput;
        public string _Output;
        public MCodeGenerator(string code, MCodeGeneratorOptions options)
        {
            _Options = options;
            
            _TempCodeFilePath = Path.GetTempFileName();
            File.Move(_TempCodeFilePath, $"{_TempCodeFilePath}.c");
            _TempCodeFilePath += ".c";

            _ResultFilePath = Path.GetTempFileName();
            File.Move(_ResultFilePath, $"{_ResultFilePath}.c");
            _ResultFilePath += ".c";

            FileStream fs =  File.Create(_TempCodeFilePath);
            fs.Write(Encoding.UTF8.GetBytes(code), 0, code.Length);
            fs.Close();
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        // Protected implementation of Dispose pattern.
        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                File.Delete(_TempCodeFilePath);
                File.Delete(_ResultFilePath);
            }

            disposed = true;
        }
        ~MCodeGenerator()
        {
            Dispose(true);
        }
        public bool Generate()
        {
            var p = new Process();
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardOutput = true;
            string errorOutput = null;
            p.StartInfo.RedirectStandardError = true;
            p.ErrorDataReceived += new DataReceivedEventHandler((sender, e) =>
            { errorOutput += e.Data + "\n"; });
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux)) {
                if (_Options._IsX64)
                {
                    p.StartInfo.FileName = "x86_64-w64-mingw32-gcc";
                }
                else if (_Options._IsX86)
                {
                    p.StartInfo.FileName = "i686-w64-mingw32-gcc";
                }
            }
            else if(RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                p.StartInfo.FileName = @"C:\TDM-GCC-64\bin\gcc.exe";
            }
            p.StartInfo.Arguments = $"-g0 {(_Options._IsX86 ? "-m32" : "-m64")} -Wa,-aln=\"{_ResultFilePath}\" \"{_TempCodeFilePath}\"";
            p.Start();
            p.BeginErrorReadLine();
            string output = p.StandardOutput.ReadToEnd();
            p.WaitForExit();
            _ConsoleOutput = $"#Output\n{output}\n#ErrorOutput\n{errorOutput}";

            string text = File.ReadAllText(_ResultFilePath);
            _Output = MCodeParser.Parser(text,(_Options._IsX64?64:86));
            
            if(_Output.Equals("2,x86:") || _Output.Equals("2,x64:"))
            {
                _Output = string.Empty;
                return false;
            }
            else
            {
                string[] split = StrSubStringArray(_Output, 50);
                string result = $"\nMCode := \"{split[0]}\"\n";
                for(int i = 1; i < split.Length; i++)
                {
                    result += $". \"{split[i]}\"\n";
                }
                result += "_Function := BentschiMCode(MCode)\n";
                result += "return\n";
                result += "BentschiMCode(mcode)\n" +
                    "{\n" +
                    @"  static e := {1:4, 2:1}, c := (A_PtrSize=8) ? ""x64"" : ""x86""" + "\n" +
                    @"  if (!regexmatch(mcode, ""^([0-9]+),("" c "":|.*?,"" c "":)([^,]+)"", m))" + "\n" +
                    "    return\n" +
                    @"  if (!DllCall(""crypt32\CryptStringToBinary"", ""str"", m3, ""uint"", 0, ""uint"", e[m1], ""ptr"", 0, ""uint*"", s, ""ptr"", 0, ""ptr"", 0))" + "\n" +
                    "    return\n" +
                    @"  p := DllCall(""GlobalAlloc"", ""uint"", 0, ""ptr"", s, ""ptr"")" + "\n" +
                    @"  if (c=""x64"")" + "\n" +
                    @"    DllCall(""VirtualProtect"", ""ptr"", p, ""ptr"", s, ""uint"", 0x40, ""uint*"", op)" + "\n" +
                    @"  if (DllCall(""crypt32\CryptStringToBinary"", ""str"", m3, ""uint"", 0, ""uint"", e[m1], ""ptr"", p, ""uint*"", s, ""ptr"", 0, ""ptr"", 0))" + "\n" +
                    "    return p\n" +
                    @"  DllCall(""GlobalFree"", ""ptr"", p)" + "\n" +
                    "}";
                _Output = result;
            }
            return true;
        }
        string[] StrSubStringArray(string str,int len)
        {
            List<string> list = new List<string>();
            int strLength = str.Length;
            for(int i = 0; i < strLength; i+= len)
            {
                string subStr = string.Empty;
                if (strLength - i > len)
                {
                    subStr = str.Substring(i, len);
                }
                else
                {
                    subStr = str.Substring(i, strLength - i);
                }
                list.Add(subStr);
            }
            return list.ToArray<string>();
        }
        
        class MCodeParser
        {
            private const string _Newline = "\r\n";
            private static string Clean(string data)
            {
                
                List<string> line = data.Split(Environment.NewLine).ToList<string>();
                int i;
                for(i = 0; i < line.Count; i++)
                {
                    if(line[i].Contains(".ident	\"GCC"))
                    {
                        line.RemoveAt(i);
                        break;
                    }
                }
                for (; i < line.Count; i++)
                {
                    line.RemoveAt(i);
                }
                data = string.Join(Environment.NewLine, line);
                return data;
            }
            public static string Parser(string data,int platform)
            {
                data = Clean(data);
                string result = "";
                Regex regex = new Regex(@"^\s*\d+(\s[\dA-F]{4}\s|\s{6})([\dA-F]+)",RegexOptions.Multiline|RegexOptions.IgnoreCase);
                foreach(Match m in regex.Matches(data))
                {
                    result += m.Groups[2].Value;
                    
                }
                result = MCodeToBentschiStyle(result,platform);
                return result;
            }
            private static string MCodeToBentschiStyle(string data,int platform)
            {
                byte[] bin = StringToBinary(data);
                data = Convert.ToBase64String(bin);
                data = Regex.Replace(data, @"\s+", ""); //Remove whitespaces
                return data.Insert(0, $"2,x{platform}:");

                byte[] StringToBinary(string datastr)
                {
                    List<byte> list = new List<byte>();
                    for (int i = 0; i < datastr.Length; i += 2)
                    {
                        string str = datastr.Substring(i, 2);
                        if(str.Length == 1)
                        {

                        }
                        byte num = byte.Parse(str, System.Globalization.NumberStyles.HexNumber);
                        list.Add(num);
                    }
                    return list.ToArray<byte>();
                }
            }
            
        }
    }
}
