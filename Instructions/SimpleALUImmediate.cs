namespace CodeGen.Instructions
{
    public class SimpleALUImmediate : Instruction
    {
        private bool _signed;
        private int max;
        public SimpleALUImmediate(string name, bool signedImmediate = false, int max = 65535)
        {
            this.max = max;
            _signed = signedImmediate;
            this._name = name;
        }

        public override string Populate(RandomConfiguration config)
        {
            return Get(new[] {config.RandomWriteRegister(), config.RandomReadRegister(), _signed ? config.RandomSignedImmediate() : config.Random16BitImmediate(max)});
        }
    }
}