using System;
using System.Diagnostics.SymbolStore;
using System.Linq;

namespace CodeGen
{
    public class RandomConfiguration
    {
        public RandomConfiguration()
        {
            NumberOfLabels = 3;
            NumberOfData = 3;
            MaximumRegister = 4;
            MaximumImmediate = 5;
        }

        public int NumberOfLabels { get; set; }
        public int NumberOfData { get; set; }
        public int MaximumImmediate { get; set; }

        private int _numberOfRegisters;
        public int[] ReadRegisters { get; private set; }
        public int[] WriteRegisters { get; private set; }

        private void RefreshRegister()
        {
            WriteRegisters = Enumerable.Range(_noZero ? 1 : 0, _numberOfRegisters).ToArray();
            ReadRegisters = WriteRegisters.Concat(new int[] {31}).ToArray();
        }

        private bool _noZero;

        public bool NoZeroRegister
        {
            get => _noZero;
            set
            {
                _noZero = value;
                RefreshRegister();
            }
        }

        public int MaximumRegister
        {
            get { return _numberOfRegisters; }
            set
            {
                _numberOfRegisters = value;
                RefreshRegister();
            }
        }

        public string RandomReadRegister()
        {
            int num = ReadRegisters.PickOne();
            return "$" + num;
        }

        public string RandomWriteRegister()
        {
            int num = WriteRegisters.PickOne();
            return "$" + num;
        }

        public string Random16BitImmediate(int max = 65535)
        {
            return Utils.Random.Next(0, Math.Min(Math.Min(max,MaximumImmediate), 65535)).ToString();
        }

        public string RandomSignedImmediate()
        {
            int bound = Math.Min(MaximumImmediate, 32767);
            return Utils.Random.Next(-bound,bound).ToString();
        }

        public string Random5BitImmediate()
        {
            return Utils.Random.Next(0, Math.Min(MaximumImmediate, 31)).ToString();
        }

        public string RandomMemory()
        {
            return "data" + Utils.Random.Next(0, NumberOfData);
        }

        public string RandomLabel()
        {
            return "label" + Utils.Random.Next(0, NumberOfLabels);
        }
    }
}