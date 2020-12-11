using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using CodeGen.Instructions;

namespace CodeGen
{
    class Program
    {
        static Vivado vivado;

        static bool Test(string program)
        {
            bool success = true;
            string mars;
            do
            {
                File.WriteAllText("test.asm", program);
                mars = Utils.Run(new ProcessStartInfo("./execute-mars.sh", "test.asm"));
                if (mars.Contains("Error"))
                {
                    Console.WriteLine("MARS error" + mars);
                    return true;
                }
            } while (!success);

            Console.WriteLine("Running mine");
            Utils.Run(new ProcessStartInfo("./compile.sh", "test.asm"));
            string mine = vivado.RunSimulation();
            bool result = Utils.CompareOutput(mars, mine);
            if (!result)
            {
                Console.WriteLine("Compared different!");
                Console.WriteLine("Please check mars-registers.txt and mine-registers.txt (vimdiff mars-registers.txt mine-registers.txt)");
                Console.WriteLine("Please check mars-memory.txt and mine-memory.txt (vimdiff mars-memory.txt mine-memory.txt)");
                Environment.Exit(1);
            }

            return true;
        }

        static void GenerateTest()
        {

            ProgramGenerator[] generators = new ProgramGenerator[]
            {
                new P5Generator(false),
                new P5Generator(true),
                // new SpecificInstructionGenerator(new SimpleALU("and")), 
                // new SpecificInstructionGenerator(new SimpleALU("or")), 
                // new SpecificInstructionGenerator(new SimpleALU("xor")), 
                // new SpecificInstructionGenerator(new SimpleALU("nor")), 
                // new SpecificInstructionGenerator(new SimpleShift("sll")), 
                // new SpecificInstructionGenerator(new SimpleShift("sll"), true), 
                // new SpecificInstructionGenerator(new SimpleShift("srl")), 
                // new SpecificInstructionGenerator(new SimpleShift("srl"), true), 
                // new SpecificInstructionGenerator(new SimpleShift("sra"), true), 
                // new SpecificInstructionGenerator(new SimpleALU("sllv"), true), 
                // new SpecificInstructionGenerator(new SimpleALU("srlv"), true), 
                // new SpecificInstructionGenerator(new SimpleALU("srav"), true), 
                // new SpecificInstructionGenerator(new SimpleALU("add"), true), 
                // new SpecificInstructionGenerator(new SimpleALU("sub"), true), 
                // new SpecificInstructionGenerator(new SimpleALUImmediate("addiu")), 
                // new SpecificInstructionGenerator(new SimpleALUImmediate("addiu"), true), 
                // new SpecificInstructionGenerator(new SimpleALUImmediate("addi"), true), 
                // new SpecificInstructionGenerator(new SimpleALUImmediate("ori")), 
                // new SpecificInstructionGenerator(new SimpleALUImmediate("xori"), true), 
                // new SpecificInstructionGenerator(new SimpleALUImmediate("xori")), 
                // new SpecificInstructionGenerator(new SimpleALUImmediate("andi"), true), 
                // new SpecificInstructionGenerator(new SimpleALU("slt")), 
                // new SpecificInstructionGenerator(new SimpleALU("slt"), true), 
                // new SpecificInstructionGenerator(new SimpleALU("sltu"), true), 
                // new SpecificInstructionGenerator(new SimpleALUImmediate("slti")), 
                // sltiGenerator,
                // sltiuGenerator
                // new SpecificInstructionGenerator(new SimpleZeroBranching("bltz")), 
                // new SpecificInstructionGenerator(new SimpleZeroBranching("bgtz")), 
                // new SpecificInstructionGenerator(false,new SimpleZeroBranching("bgez")), 
                // new SpecificInstructionGenerator(new SimpleZeroBranching("blez")), 
                // new SpecificInstructionGenerator(new UnalignedLoad(MemoryOperationWidth.Byte, MemoryOperationFlags.Read), true), 
                // new SpecificInstructionGenerator(new UnalignedLoad(MemoryOperationWidth.Byte, MemoryOperationFlags.ReadUnsigned), true), 
                // new SpecificInstructionGenerator(new UnalignedLoad(MemoryOperationWidth.HalfWord, MemoryOperationFlags.Read), true), 
                // new SpecificInstructionGenerator(new UnalignedLoad(MemoryOperationWidth.HalfWord, MemoryOperationFlags.ReadUnsigned), true), 
                // new SpecificInstructionGenerator(new UnalignedLoad(MemoryOperationWidth.HalfWord, MemoryOperationFlags.Write), true), 
                // new SpecificInstructionGenerator(new UnalignedLoad(MemoryOperationWidth.Byte, MemoryOperationFlags.Write), true), 
                // new P6Generator(false),
                // new P6Generator(true),
                /*
                new SpecificInstructionGenerator(false,
                    new MultiplicationInstruction("mult"),
                    new MultiplicationInstruction("multu"),
                    new MulMove("mfhi"),
                    new MulMove("mflo")),
                new SpecificInstructionGenerator(false,
                    new MultiplicationInstruction("mult"),
                    new MultiplicationInstruction("multu"),
                    new MulMove("mthi"),
                    new MulMove("mtlo"),
                    new MulMove("mfhi"),
                    new MulMove("mflo")),
                new SpecificInstructionGenerator(true,
                    new MultiplicationInstruction("mult"),
                    new MultiplicationInstruction("multu"),
                    new MultiplicationInstruction("div"),
                    new MultiplicationInstruction("divu"),
                    new MulMove("mfhi"),
                    new MulMove("mflo")),
                    */
            };
            for (int i = 0; i < 100; i++)
            {
                foreach (var gen in generators)
                {
                    Console.WriteLine("Running " + gen.Name + " #" + (i + 1).ToString());
                    if (!Test(gen.Generate()))
                    {
                        return;
                    }
                }
            }
        }

        static void TestPrograms()
        {
            foreach (var f in Directory.GetFiles("xhc", "*.asm", SearchOption.AllDirectories))
            {
                Console.WriteLine("Testing " + f);
                if (!Test(File.ReadAllText(f)))
                {
                    return;
                }
            }
        }

        static void Main(string[] args)
        {
            if (args.Length != 2)
            {
                Console.WriteLine("Usage: dotnet run <vivado executable> <vivado project>");
                Console.WriteLine("Example: dotnet run ~/vivado/2019.1/bin/vivado ~/pipeline/pipeline.xpr");
                return;
            }
            
            vivado = new Vivado(args[0], args[1]);
            // Console.WriteLine(vivado.RunSimulation());
            GenerateTest();
            // TestPrograms();
        }
    }
}
