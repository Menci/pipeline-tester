using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace CodeGen
{
    public class SpecificInstructionGenerator : GenerateByInstructionSetAndRandomConfig
    {
        private InstructionSet _instructionSet;

        public override InstructionSet InstructionSet => _instructionSet;
        private RandomConfiguration _config;
        public override RandomConfiguration Config => _config;
        public ReadOnlyCollection<Instruction> Instructions { get; }
        public override string Name => "Test for " + String.Join(", ", Instructions.Select(i => i.Name)) + (_large ? " (Large)" : "");
        private bool _large;
        public override bool PreloadValue => _large;

        public SpecificInstructionGenerator(bool large, params Instruction[] ins)
        {
            InstructionCount = 50;
            this.Instructions = new ReadOnlyCollection<Instruction>(ins);
            _instructionSet = new InstructionSet();
            _config = new RandomConfiguration();
            _config.NoZeroRegister = true;
            var ilist =
                new List<(Instruction, double)>()
                {
                    (new Addu(), 1),
                    (new Ori(), 2),
                    (new Subu(), 1),
                    (new Nop(), 1),
                    (new TwoOperandBranching("beq"), 1),
                };
            foreach (var i in this.Instructions)
            {
                ilist.Add((i, 2));
            }
            if (large)
            {
                ilist.Add((new Lui(), 2));
            }
            if (ilist.Any(i => i.Item1.NeedMemory))
            {
                _config.NumberOfData = 3;
                ilist.Add((new Lw(), 1));
                ilist.Add((new Sw(), 1));
            }
            else
            {
                _config.NumberOfData = 0;
            }


            _instructionSet.PopulateInstructionList(ilist );
            _large = large;
            if (large)
            {
                _config.MaximumImmediate = 2147483647;
                _config.MaximumRegister = 3;
                _config.NumberOfLabels = 2;
            }
            else
            {
                _config.MaximumImmediate = 5;
                _config.MaximumRegister = 3;
                _config.NumberOfLabels = 2;
            }
        }
    }
}