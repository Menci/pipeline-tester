#!/bin/sh -e
java -jar Mars.jar dump .text HexText code.txt a nc mc CompactDataAtZero $1
cp all_zero.txt /tmp/code.txt
dd if=code.txt of=/tmp/code.txt conv=notrunc 2>/dev/null
# ~/Xilinx/Vivado/2019.1/bin/vivado -mode batch -source scripts/run_simulation.tcl ../pipeline.xpr | grep --color=none '^@' 2>/dev/null | head -n "$(wc -l /tmp/result-mars.out | cut -d' ' -f 1)" | tee /tmp/result-mine.out
