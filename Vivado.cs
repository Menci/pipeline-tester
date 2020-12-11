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
    public class Vivado
    {
        Process process;

        const string waitVivadoCommand = "qwq";
        const string waitVivadoCommandResult = "invalid command name \"qwq\"";

        public Vivado(string vivadoExecutablePath, string vivadoProjectPath)
        {
            var startInfo = new ProcessStartInfo(vivadoExecutablePath, "-mode tcl \"" + vivadoProjectPath + "\"");
            startInfo.WorkingDirectory = "/tmp";
            startInfo.RedirectStandardInput = true;
            startInfo.RedirectStandardOutput = true;
            process = new Process();
            process.StartInfo = startInfo;
            process.Start();

            // Wait for ready
            process.StandardInput.WriteLine(waitVivadoCommand);
            while (true) {
                string line = process.StandardOutput.ReadLine();
                if (line == waitVivadoCommandResult) break;
                Console.WriteLine("Vivado: '{0}'", line);
            }

            this.ExecuteCommand("launch_simulation -simset sim_1");
        }

        private string ExecuteCommand(string command)
        {
            process.StandardInput.WriteLine(command);
            process.StandardInput.WriteLine(waitVivadoCommand);
            StringBuilder result = new StringBuilder();
            while (true) {
                string line = process.StandardOutput.ReadLine();
                if (line == waitVivadoCommandResult) break;
                if (line.StartsWith("@"))
                    result.AppendLine(line);
            }
            return result.ToString();
        }

        public string RunSimulation()
        {
            this.ExecuteCommand("restart");
            this.ExecuteCommand("run all");
            return this.ExecuteCommand("run all");
        }
    }
}
