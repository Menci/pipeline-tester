namespace CodeGen.Instructions
{
    public class MulMove : Instruction
    {
        public MulMove(string name)
        {
            this._name = name;
        }

        public override string Populate(RandomConfiguration config)
        {
            return Get(new[] {config.RandomWriteRegister()});
        }
    }
}