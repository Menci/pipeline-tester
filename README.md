# Pipeline Tester

Random tests generator & runner for your pipelined MIPS processor.

Originally authored by [@t123yh](https://github.com/t123yh). Modified for running with Vivado.

# Requirements

You need Microsoft .NET SDK to build and run this program.

You can download and install Microsoft .NET SDK [here](https://dotnet.microsoft.com/download).

You must have Java (to run Mars) and Vivado installed.

Your MIPS cpu should be a Vivado project.

# Preparation

You should have a single testbench file in your Vivado project. It should be (replace `TopLevel` with your top level module name, e.g. `mips`):

```verilog
module mips_tb;

reg reset, clock;

// !!! Replace here
TopLevel topLevel(.reset(reset), .clock(clock));

integer k;
initial begin
    reset = 1;
    clock = 0; #1;
    clock = 1; #1;
    clock = 0; #1;
    reset = 0; #1;
    
    $stop;

    #1;
    for (k = 0; k < 5000; k = k + 1) begin
        clock = 1; #5;
        clock = 0; #5;
    end

    $finish;
end
    
endmodule
```

You should read instruction memory from `/tmp/code.txt`.

```verilog
initial
    $readmemh("/tmp/code.txt", memory);
```

Since we check your registers and memory writes. You must `$display` these information when writing registers and memory. The output is like:

```java
@00003000: $31 <= 00003000
@00003004: $ 2 <= 00000000
@00003008: $ 2 <= 00000000
@0000300c: $ 0 <= 00030000
@00003010: $ 0 <= 00000000
@00003014: $ 1 <= 00040000
@00003018: *00000004 <= 00003000
@0000301c: $ 1 <= 00006000
@00003020: *00000008 <= 00003000
@00003024: $ 1 <= 00003000
```

You can add these code snippets to your Verilog code to print that information:

```verilog
// When writing to data memory (in your `always @ (posedge/negedge clock)` block, after your assign statement to data memory, remember to write `begin ... end`)
// PC value must be passed from program counter module to data memory module.
// Address is 32-bit
$display("@%h: *%h <= %h", programCounter, address, dataWrite);

// When writing to registers (in your `always @ (posedge/negedge clock)` block, after your assign statement to registers, remember to write `begin ... end`)
// PC value must be passed from program counter module to data memory module.
$display("@%h: $%d <= %h", programCounter, registerId, dataWrite);
```

You must **NOT** `$display` any other lines starting with `@`. Any other lines not starting with `@` is ignored.

Before running tests, you should start simulation in your Vivado IDE and **make sure your code compiles and runs**. I didn't handle compilation errors in tests.

# Usage

Clone this repository:

```bash
git clone https://github.com/Menci/pipeline-tester
cd pipeline-tester
```

You need to provide your Vivado executable and project to this program.

```bash
dotnet run <vivado executable path> <project file path>
```

For example:

```bash
dotnet run ~/Xilinx/Vivado/2019.1/bin/vivado ~/CPU/pipeline/pipeline.xpr
```

> **Note**: It's **`dotnet run`**, not `docker run`.

If Vivado successfully loads your project, it will print something like:

```bash
Vivado: ''
Vivado: '****** Vivado v2019.1 (64-bit)'
Vivado: '  **** SW Build 2552052 on Fri May 24 14:47:09 MDT 2019'
Vivado: '  **** IP Build 2548770 on Fri May 24 18:01:18 MDT 2019'
Vivado: '    ** Copyright 1986-2019 Xilinx, Inc. All Rights Reserved.'
Vivado: ''
Vivado: 'open_project /home/Menci/Courses/CPU/pipeline/pipeline.xpr'
Vivado: 'Scanning sources...'
Vivado: 'Finished scanning sources'
```

After starting running tests, it will print something like:

```
Running Comprehensive test for P5 #1
Running mine
Total Valid lines: Mars: 8 vs mine: 48
Total Valid lines: Mars: 33 vs mine: 192
Running Comprehensive test for P5 (Large) #1
Running mine
Total Valid lines: Mars: 60 vs mine: 616
Total Valid lines: Mars: 418 vs mine: 4153
Running Comprehensive test for P5 #2
Running mine
Total Valid lines: Mars: 44 vs mine: 444
Total Valid lines: Mars: 256 vs mine: 2553
Running Comprehensive test for P5 (Large) #2
Running mine
Total Valid lines: Mars: 64 vs mine: 665
Total Valid lines: Mars: 385 vs mine: 3754
```

If a test fails, it will exit with a error message. You should check the outputs according to the error message, for example, check `mine-registers.txt` and `mars-registers.txt`. The first is the correct registers write operations and the second is what you did.

```bash
vimdiff mine-registers.txt mars-registers.txt
# Your wrong operations are on the left side
# [Type ":wq" and press enter] twice to exit `vimdiff`
```

You may need to check `mars-memory.txt` and `mine-memory.txt` if the error message tells you.

If your CPU doesn't fail, it will exit after running 100 groups of tests. You may want to let it run continuously:

```bash
while dotnet run <vivado executable path> <project file path>; do :; done
```

For debugging your CPU with failed tests, Mars GUI may be helpful. You can view the instructions near by the instruction you failed and run to see the correct result.

# Customization

By default it generates instructions below:

```
* addu, subu
* ori
* lw, sw
* beq
* lui
* jr, jal
```

To change the instruction set, **uncomment** the `new List`'s arguments in `P5Generator.cs` file.

There're also **specific instruction tests** in `Program.cs` file, you can uncomment them to test your individual instruction (or group of instructions). But testing individual instruction could NOT guarantee that your pipeline is implemented properly since some data/control hazards only occur on combination of different instructions.

# Known Issue

It don't generate `lw` with a register other than `$zero` as the base address (no this issue for unaligned loads e.g. `lh`).
