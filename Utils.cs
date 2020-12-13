using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CodeGen
{
    public static class Utils
    {
        public readonly static Random Random = new Random();

        public static T PickOne<T>(this T[] arr)
        {
            return arr[Random.Next(arr.Length)];
        }

        public static T ChooseWithProbability<T>(List<(T, double)> list)
        {
            double sum = list.Sum(x => x.Item2);
            double val = Random.NextDouble() * sum;
            T result = default(T);
            double cur = 0;
            using (var enumerator = list.GetEnumerator())
            {
                while (cur < val)
                {
                    result = enumerator.Current.Item1;
                    cur += enumerator.Current.Item2;
                    if (!enumerator.MoveNext())
                        break;
                }
            }

            return result;
        }

        public static string Run(ProcessStartInfo info)
        {
            info.WorkingDirectory = Environment.CurrentDirectory;
            info.RedirectStandardOutput = true;
            using (Process process = new Process())
            {
                process.StartInfo = info;
                process.Start();
                process.WaitForExit();
                if (process.ExitCode != 0)
                    throw new Exception("tyh-ed");
                return process.StandardOutput.ReadToEnd();
            }
        }

        static Regex memoryRegex = new Regex(@"@[0-9a-fxX]+: \* *[0-9a-fxX]+ <= [0-9a-fxX]+$");
        static Regex registerRegex = new Regex(@"@[0-9a-fxX]+: \$ *([0-9a-fxX]+) <= [0-9a-fxX]+$");

        public static bool StringEqual(string[] a, string[] b)
        {
            Console.WriteLine($"Total Valid lines: Mars: {a.Length} vs mine: {b.Length}");
            for (int i = 0; i < a.Length; i++)
            {
                if (i >= b.Length || a[i] != b[i])
                    return false;
            }

            return true;
        }

        public static (bool, bool) CompareOutput(string mars, string mine)
        {
            string[] marsLines = mars.Split("\n").Select(s => s.Trim(' ', '\r')).ToArray();
            string[] mineLines = mine.Split("\n").Select(s => s.Trim(' ', '\r')).ToArray();

            var marsLinesMemory = marsLines.Select(l =>
            {
                var match = memoryRegex.Match(l);
                if (match.Success)
                    return match.Groups[0].ToString();
                return null;
            }).Where(x => x != null).ToArray();
            var mineLinesMemory = mineLines.Select(l =>
            {
                var match = memoryRegex.Match(l);
                if (match.Success)
                    return match.Groups[0].ToString();
                return null;
            }).Where(x => x != null).ToArray();
            bool memoryEqual = StringEqual(marsLinesMemory, mineLinesMemory);
            if (!memoryEqual)
            {
                File.WriteAllLines("mars-memory.txt", marsLinesMemory);
                File.WriteAllLines("mine-memory.txt", mineLinesMemory);
            }

            var marsLinesRegister = marsLines.Select(l =>
            {
                var m = registerRegex.Match(l);
                if (m.Success && m.Groups[1].ToString() != "0")
                    return m.Groups[0].ToString();
                return null;
            }).Where(x => x != null).ToArray();
            var mineLinesRegister = mineLines.Select(l =>
            {
                var m = registerRegex.Match(l);
                if (m.Success && m.Groups[1].ToString() != "0")
                    return m.Groups[0].ToString();
                return null;
            }).Where(x => x != null).ToArray();

            bool registerEqual = StringEqual(marsLinesRegister, mineLinesRegister);
            if (!registerEqual)
            {
                File.WriteAllLines("mars-registers.txt", marsLinesRegister);
                File.WriteAllLines("mine-registers.txt", mineLinesRegister);
            }

            return (memoryEqual, registerEqual);
        }
    }
}
