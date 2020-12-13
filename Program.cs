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
            var (memoryEqual, registerEqual) = Utils.CompareOutput(mars, mine);
            if (!memoryEqual || !registerEqual)
            {
                Console.WriteLine("Compared different!");
                if (!registerEqual)
                    Console.WriteLine("Please check mine-registers.txt and mars-registers.txt (vimdiff mine-registers.txt mars-registers.txt)");
                if (!memoryEqual)
                    Console.WriteLine("Please check mine-memory.txt and mars-memory.txt (vimdiff mine-memory.txt mars-memory.txt)");
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
                // new SpecificInstructionGenerator(false, new SimpleALU("and")), 
                // new SpecificInstructionGenerator(false, new SimpleALU("or")), 
                // new SpecificInstructionGenerator(false, new SimpleALU("xor")), 
                // new SpecificInstructionGenerator(false, new SimpleALU("nor")), 
                // new SpecificInstructionGenerator(false, new SimpleShift("sll")), 
                // new SpecificInstructionGenerator(true, new SimpleShift("sll")), 
                // new SpecificInstructionGenerator(false, new SimpleShift("srl")), 
                // new SpecificInstructionGenerator(true, new SimpleShift("srl")), 
                // new SpecificInstructionGenerator(true, new SimpleShift("sra")), 
                // new SpecificInstructionGenerator(true, new SimpleALU("sllv")), 
                // new SpecificInstructionGenerator(true, new SimpleALU("srlv")), 
                // new SpecificInstructionGenerator(true, new SimpleALU("srav")), 
                // new SpecificInstructionGenerator(true, new SimpleALU("add")), 
                // new SpecificInstructionGenerator(true, new SimpleALU("sub")), 
                // new SpecificInstructionGenerator(false, new SimpleALUImmediate("addiu")), 
                // new SpecificInstructionGenerator(true, new SimpleALUImmediate("addiu")), 
                // new SpecificInstructionGenerator(true, new SimpleALUImmediate("addi")), 
                // new SpecificInstructionGenerator(false, new SimpleALUImmediate("ori")), 
                // new SpecificInstructionGenerator(true, new SimpleALUImmediate("xori")), 
                // new SpecificInstructionGenerator(false, new SimpleALUImmediate("xori")), 
                // new SpecificInstructionGenerator(true, new SimpleALUImmediate("andi")), 
                // new SpecificInstructionGenerator(false, new SimpleALU("slt")), 
                // new SpecificInstructionGenerator(true, new SimpleALU("slt")), 
                // new SpecificInstructionGenerator(true, new SimpleALU("sltu")), 
                // new SpecificInstructionGenerator(false, new SimpleALUImmediate("slti")), 
                // new SpecificInstructionGenerator(false, new SimpleZeroBranching("bltz")), 
                // new SpecificInstructionGenerator(false, new SimpleZeroBranching("bgtz")), 
                // new SpecificInstructionGenerator(false,new SimpleZeroBranching("bgez")), 
                // new SpecificInstructionGenerator(false, new SimpleZeroBranching("blez")), 
                // new SpecificInstructionGenerator(true, new UnalignedLoad(MemoryOperationWidth.Byte, MemoryOperationFlags.Read)), 
                // new SpecificInstructionGenerator(true, new UnalignedLoad(MemoryOperationWidth.Byte, MemoryOperationFlags.ReadUnsigned)), 
                // new SpecificInstructionGenerator(true, new UnalignedLoad(MemoryOperationWidth.HalfWord, MemoryOperationFlags.Read)), 
                // new SpecificInstructionGenerator(true, new UnalignedLoad(MemoryOperationWidth.HalfWord, MemoryOperationFlags.ReadUnsigned)), 
                // new SpecificInstructionGenerator(true, new UnalignedLoad(MemoryOperationWidth.HalfWord, MemoryOperationFlags.Write)), 
                // new SpecificInstructionGenerator(true, new UnalignedLoad(MemoryOperationWidth.Byte, MemoryOperationFlags.Write)), 
                // new SpecificInstructionGenerator(false,
                //     new MultiplicationInstruction("mult"),
                //     new MultiplicationInstruction("multu"),
                //     new MulMove("mfhi"),
                //     new MulMove("mflo")),
                // new SpecificInstructionGenerator(false,
                //     new MultiplicationInstruction("mult"),
                //     new MultiplicationInstruction("multu"),
                //     new MulMove("mthi"),
                //     new MulMove("mtlo"),
                //     new MulMove("mfhi"),
                //     new MulMove("mflo")),
                // new SpecificInstructionGenerator(true,
                //     new MultiplicationInstruction("mult"),
                //     new MultiplicationInstruction("multu"),
                //     new MultiplicationInstruction("div"),
                //     new MultiplicationInstruction("divu"),
                //     new MulMove("mfhi"),
                //     new MulMove("mflo")),
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
