namespace CodeGen.Instructions
{
    public class SimpleZeroBranching : Instruction
    {
        public SimpleZeroBranching(string name)
        {
            this._name = name;
        }

        public override bool IsBranch => true;

        public override string Populate(RandomConfiguration config)
        {
            return Get(new[] {config.RandomReadRegister(), config.RandomLabel()});
        }
    }
}