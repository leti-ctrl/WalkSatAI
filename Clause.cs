using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WalkSatAI
{
    public class Clause
    {
        public HashSet<string> Symbols { get; }

        public Clause(HashSet<string> symbols)
        {
            Symbols = symbols;
        }

        public bool Evaluate(Dictionary<string, bool> model)
        {
            foreach (var symbol in Symbols)
            {
                if (model.ContainsKey(symbol) && model[symbol])
                {
                    return true;
                }
            }
            return false;
        }


    }
}
