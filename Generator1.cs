using System;
using System.Collections.Generic;
using System.Text;

namespace CodeGen
{
    public abstract class GenerateByInstructionSetAndRandomConfig : ProgramGenerator
    {
        public abstract InstructionSet InstructionSet { get; }
        public abstract RandomConfiguration Config { get; }
        public virtual bool PreloadValue => false;
        public int InstructionCount { get; set; }

        public static void InsertLabel(int i, List<string> current)
        {
            int pos = Utils.Random.Next(0, current.Count);
            current.Insert(pos, "label" + i.ToString() + ":");
        }

        public override string Generate()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(@".data");
            for (int i = 0; i < Config.NumberOfData; i++)
            {
                sb.AppendLine("data" + i + ": .space 4");
            }

            sb.AppendLine();
            sb.AppendLine(@".text");

            List<string> instructions = new List<string>();
            instructions.Add("ori $31, $0, 0x3000");
            if (PreloadValue)
            {
                foreach(int reg in Config.WriteRegisters)
                {
                    instructions.Add($"lui ${reg}, " + Config.Random16BitImmediate());
                    instructions.Add($"ori ${reg}, ${reg}, " + Config.Random16BitImmediate());
                }

                for (int i = 0; i < Config.NumberOfData; i++)
                {
                    instructions.Add($"sw {Config.RandomReadRegister()}, data{i}");
                }
            }

            Instruction lastInstruction = null;
            for (int i = 0; i < InstructionCount; i++)
            {
                bool allowBranch = true;
                if (i < InstructionCount * 0.4)
                {
                    allowBranch = false;
                }
                else if (lastInstruction != null && lastInstruction.IsBranch)
                {
                    allowBranch = false;
                }

                var newInst = InstructionSet.Next(allowBranch);
                instructions.Add(newInst.Populate(Config));
                lastInstruction = newInst;
            }

            for (int i = 0; i < Config.NumberOfLabels; i++)
            {
                InsertLabel(i, instructions);
            }

            instructions.Add("nop");

            foreach (var ins in instructions)
            {
                sb.AppendLine(ins);
            }

            return sb.ToString();
        }
    }
}