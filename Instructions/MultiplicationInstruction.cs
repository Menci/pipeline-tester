namespace CodeGen.Instructions
{
    public class MultiplicationInstruction : Instruction
    {
        public MultiplicationInstruction(string name)
        {
            this._name = name;
        }

        public override string Populate(RandomConfiguration config)
        {
            string reg1 = config.RandomWriteRegister();
            return Get(new[] {config.RandomReadRegister(), reg1});
        }
    }
}