using System.Collections.Generic;

namespace CodeGen
{
    public class P6MemTestGenerator : GenerateByInstructionSetAndRandomConfig
    {
        private InstructionSet _instructionSet;
        public override InstructionSet InstructionSet => _instructionSet;
        private RandomConfiguration _config;
        public override RandomConfiguration Config => _config;
        public override string Name => "Memory load & store for P6";

        public P6MemTestGenerator()
        {
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
                }
            );
            _config = new RandomConfiguration();
            _config.MaximumImmediate = 5;
            _config.MaximumRegister = 5;
            _config.NumberOfData = 3;
            _config.NumberOfLabels = 3;
        }
    }
}