namespace CodeGen.Instructions
{
    public enum MemoryOperationWidth
    {
        Byte,
        HalfWord
    }

    public enum MemoryOperationFlags
    {
        Read,
        ReadUnsigned,
        Write
    }

    public class UnalignedLoad : Instruction
    {
        private MemoryOperationWidth _width;

        public UnalignedLoad(MemoryOperationWidth width, MemoryOperationFlags flags)
        {
            _width = width;
            string opstr = flags == MemoryOperationFlags.Write ? "s" : "l";
            string wstr = width switch {MemoryOperationWidth.Byte => "b", MemoryOperationWidth.HalfWord => "h"};
            string ustr = flags switch
            {
                MemoryOperationFlags.ReadUnsigned => "u",
                _ => ""
            };
            this._name = opstr + wstr + ustr;
        }

        public override string Populate(RandomConfiguration config)
        {
            string reg = config.RandomWriteRegister();
            string ins1 = Instruction.Get("andi", new[]
            {
                reg, reg, _width switch
                {
                    MemoryOperationWidth.HalfWord => "2",
                    MemoryOperationWidth.Byte => "3"
                }
            });
            return ins1 + "\n" + Get(new[] {config.RandomWriteRegister(), $"{config.RandomMemory()}({reg})"});
        }

        public override bool NeedMemory => true;
    }
}