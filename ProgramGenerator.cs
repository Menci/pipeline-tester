namespace CodeGen
{
    public abstract class ProgramGenerator
    {
        public abstract string Generate();
        public abstract string Name { get; }
    }
}