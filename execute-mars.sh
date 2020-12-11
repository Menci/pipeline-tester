#!/bin/sh
java -jar Mars.jar db nc 500 ae2 mc CompactDataAtZero $1 | grep '^@' 2>/dev/null | head -n 1000 # | tee /tmp/result-mars.out
exit $?
