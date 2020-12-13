using System.Collections.Generic;
using CodeGen.Instructions;

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
                    (new SimpleALU("addu"), 1),
                    (new SimpleALU("add"), 1),
                    (new SimpleALU("subu"), 1),
                    (new SimpleALU("sub"), 1),
                    (new SimpleALU("and"), 1),
                    (new SimpleALU("or"), 1),
                    (new SimpleALU("xor"), 1),
                    (new SimpleALU("nor"), 1),
                    (new SimpleALU("slt"), 1),
                    (new SimpleALU("sltu"), 1),
                    (new SimpleShift("sll"), 1),
                    (new SimpleShift("srl"), 1),
                    (new SimpleShift("sra"), 1),
                    (new SimpleALU("sllv"), 1),
                    (new SimpleALU("srlv"), 1),
                    (new SimpleALU("srav"), 1),
                    (new Lui(), 1),
                    (new TwoOperandBranching("beq"), 1),
                    (new TwoOperandBranching("bne"), 1),
                    (new SimpleZeroBranching("bgtz"), 1),
                    (new SimpleZeroBranching("bgez"), 1),
                    (new SimpleZeroBranching("bltz"), 1),
                    (new SimpleZeroBranching("blez"), 1),
                    (new SimpleALUImmediate("slti", true, 32768), 1),
                    (new SimpleALUImmediate("sltiu", false, 32768), 1),
                    (new SimpleALUImmediate("addi"), 1),
                    (new SimpleALUImmediate("addiu"), 1),
                    (new SimpleALUImmediate("xori"), 1),
                    (new SimpleALUImmediate("andi"), 1),
                    (new SimpleALUImmediate("ori"), 1),
                    (new J(), 1),
                    (new Jal(), 1),
                    (new Jr(), 0.3),
                    (new Jalr(), 0.3),
                    (new Lw(), 1),
                    (new Sw(), 1),
                    (new UnalignedLoad(MemoryOperationWidth.HalfWord, MemoryOperationFlags.ReadUnsigned), 1),
                    (new UnalignedLoad(MemoryOperationWidth.HalfWord, MemoryOperationFlags.Read), 1),
                    (new UnalignedLoad(MemoryOperationWidth.HalfWord, MemoryOperationFlags.Write), 1),
                    (new UnalignedLoad(MemoryOperationWidth.Byte, MemoryOperationFlags.Write), 1),
                    (new UnalignedLoad(MemoryOperationWidth.Byte, MemoryOperationFlags.ReadUnsigned), 1),
                    (new UnalignedLoad(MemoryOperationWidth.Byte, MemoryOperationFlags.Read), 1),
                    (new MultiplicationInstruction("mult"), 1),
                    (new MultiplicationInstruction("multu"), 1),
                    (new MultiplicationInstruction("div"), 1),
                    (new MultiplicationInstruction("divu"), 1),
                    (new MulMove("mthi"), 1),
                    (new MulMove("mtlo"), 1),
                    (new MulMove("mfhi"), 1),
                    (new MulMove("mflo"), 1),
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
