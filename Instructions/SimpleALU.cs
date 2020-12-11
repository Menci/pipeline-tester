namespace CodeGen.Instructions
{
    public class SimpleALU : Instruction
    {
        public SimpleALU(string name)
        {
            this._name = name;
        }

        public override string Populate(RandomConfiguration config)
        {
            return Get(new[] {config.RandomWriteRegister(), config.RandomReadRegister(), config.RandomReadRegister()});
        }
    }
}