using System.Collections.Generic;
using System.Linq;

namespace CodeGen
{
    public class InstructionSet
    {
        private List<(Instruction, double)> FullList;
        private List<(Instruction, double)> NonBranchingList;

        public void PopulateInstructionList(List<(Instruction, double)> list)
        {
            FullList = new List<(Instruction, double)>(list);
            NonBranchingList = new List<(Instruction, double)>(list.Where(x => !x.Item1.IsBranch));
        }
        
        public Instruction Next(bool allowBranch)
        {
            if (allowBranch)
            {
                return Utils.ChooseWithProbability(FullList);
            }
            else
            {
                return Utils.ChooseWithProbability(NonBranchingList);
            }
        }
        
    }
}