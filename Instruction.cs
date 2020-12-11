using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;

namespace CodeGen
{
    public abstract class Instruction
    {
        protected string _name;
        public string Name => _name;
        

        public abstract string Populate(RandomConfiguration config);

        public string Get(params string[] Parameters)
        {
            return _name + " " + string.Join(',', Parameters);
        }
        
        public static string Get(string name, string[] Parameters)
        {
            return name + " " + string.Join(',', Parameters);
        }

        public virtual bool IsBranch => false;
        public virtual bool NeedMemory => false;
    }

    public class Addu : Instruction
    {
        public Addu()
        {
            _name = "addu";
        }

        public override string Populate(RandomConfiguration config)
        {
            return Get(new[] {config.RandomWriteRegister(), config.RandomReadRegister(), config.RandomReadRegister()});
        }
    }
    
    public class Lui : Instruction
    {
        public Lui()
        {
            _name = "lui";
        }

        public override string Populate(RandomConfiguration config)
        {
            return Get(new[] {config.RandomWriteRegister(), config.Random16BitImmediate()});
        }
    }
    
    public class Subu : Instruction
    {
        public Subu()
        {
            _name = "subu";
        }

        public override string Populate(RandomConfiguration config)
        {
            return Get(new[] {config.RandomWriteRegister(), config.RandomReadRegister(), config.RandomReadRegister()});
        }
    }
    
    public class Ori : Instruction
    {
        public Ori()
        {
            _name = "ori";
        }

        public override string Populate(RandomConfiguration config)
        {
            return Get(new[] {config.RandomWriteRegister(), config.RandomReadRegister(), config.Random16BitImmediate()});
        }
    }
    
    public class Lw : Instruction
    {
        public Lw()
        {
            _name = "lw";
        }

        public override string Populate(RandomConfiguration config)
        {
            return Get(new[] {config.RandomWriteRegister(), config.RandomMemory()});
        }

        public override bool NeedMemory => true;
    }
    
    public class Nop : Instruction
    {
        public Nop()
        {
            _name = "nop";
        }

        public override string Populate(RandomConfiguration config)
        {
            return Get(new string[0]);
        }
    }
    
    public class Sw : Instruction
    {
        public Sw()
        {
            _name = "sw";
        }

        public override string Populate(RandomConfiguration config)
        {
            return Get(new[] {config.RandomReadRegister(), config.RandomMemory()});
        }
        
        public override bool NeedMemory => true;
    }

    public class TwoOperandBranching : Instruction
    {
        public TwoOperandBranching(string name)
        {
            _name = name;
        }

        public override string Populate(RandomConfiguration config)
        {
            return Get(new[] {config.RandomReadRegister(), config.RandomReadRegister(), config.RandomLabel()});
        }

        public override bool IsBranch => true;
    }

    public class J : Instruction
    {
        public J()
        {
            _name = "j";
        }
        
        public override string Populate(RandomConfiguration config)
        {
            return Get(new[] {config.RandomLabel()});
        }
        
        public override bool IsBranch => true;
    }
    
    public class Jal : Instruction
    {
        public Jal()
        {
            _name = "jal";
        }
        
        public override string Populate(RandomConfiguration config)
        {
            return Get(new[] {config.RandomLabel()});
        }
        
        public override bool IsBranch => true;
    }
    
    public class Jr : Instruction
    {
        public Jr()
        {
            _name = "jr";
        }
        
        public override string Populate(RandomConfiguration config)
        {
            return Get(new[] {"$ra"});
        }
        
        public override bool IsBranch => true;
    }
    
    public class Jalr : Instruction
    {
        public Jalr()
        {
            _name = "jalr";
        }
        
        public override string Populate(RandomConfiguration config)
        {
            // https://stackoverflow.com/a/23226806
            // jalr's rs and rd must be non-equal
            string register = config.RandomReadRegister();
            while (register == "$31") register = config.RandomReadRegister();
            return Get(new[] {register, "$ra"});
        }
        
        public override bool IsBranch => true;
    }
}
