namespace CodeGen.Instructions
{
    public class SimpleShift : Instruction
    {
        public SimpleShift(string name)
        {
            this._name = name;
        }

        public override string Populate(RandomConfiguration config)
        {
            return Get(new[] {config.RandomWriteRegister(), config.RandomReadRegister(), config.Random5BitImmediate()});
        }
    }
}