using System.Collections.Generic;

namespace CodeGen
{
    public class P5Generator : GenerateByInstructionSetAndRandomConfig
    {
        private InstructionSet _instructionSet;
        public override InstructionSet InstructionSet => _instructionSet;
        private RandomConfiguration _config;
        public override RandomConfiguration Config => _config;
        public override string Name => "Comprehensive test for P5" + (_large ? " (Large)" : "");
        private bool _large;
        public override bool PreloadValue => _large;

        public P5Generator(bool large = false)
        {
            _large = large;
            InstructionCount = 100;
            _instructionSet = new InstructionSet();
            _instructionSet.PopulateInstructionList(
                new List<(Instruction, double)>()
                {
                    (new Addu(), 1),
                    (new Subu(), 1),
                    (new Lw(), 1),
                    (new Lui(), 1),
                    (new Sw(), 1),
                    (new Ori(), 1),
                    // (new Subu(), 1),
                    // (new Nop(), 1),
                    (new TwoOperandBranching("beq"), 1),
                    // (new J(), 1),
                    (new Jal(), 1),
                    (new Jr(), 0.3)
                }
            );
            _config = new RandomConfiguration();
            if (large)
            {
                _config.MaximumImmediate = 2147483647;
                _config.MaximumRegister = 20;
                _config.NumberOfLabels = 7;
            }
            else
            {
                _config.MaximumImmediate = 5;
                _config.MaximumRegister = 3;
                _config.NumberOfLabels = 3;
            }

            _config.NumberOfData = 3;
        }
    }
}
