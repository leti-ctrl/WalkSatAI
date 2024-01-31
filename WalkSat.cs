using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WalkSatAI
{
    public class WalkSat
    {
        private readonly Random _random = new Random();

        public Dictionary<string, bool> WalkSAT(HashSet<Clause> clauses, double p, int maxFlips)
        {
            var model = InitializeRandomModel(clauses);

            for (var i = 1; i <= maxFlips; i++)
            {
                if (IsModelSatisfiesClauses(model, clauses))
                    return model;

                var randomFalseClause = GetRandomFalseClause(model, clauses);

                if (_random.NextDouble() < p)
                {
                    // Do a "random walk" move
                    FlipRandomSymbolInClause(randomFalseClause, model);
                }
                else
                {
                    // Flip whichever symbol in the clause maximizes the number of satisfied clauses
                    FlipSymbolMaximizingSatisfiedClauses(randomFalseClause, model, clauses);
                }
            }

            return null; // Failure
        }

        private Dictionary<string, bool> InitializeRandomModel(HashSet<Clause> clauses)
        {
            var model = new Dictionary<string, bool>();

            foreach (var clause in clauses)
            {
                foreach (var symbol in clause.Symbols)
                {
                    if (!model.ContainsKey(symbol))
                    {
                        var value = _random.NextDouble() < 0.5;
                        model[symbol] = value;
                        model[symbol.NegateSymbol()] = !value; 
                    }
                }
            }

            return model;
        }

        private bool IsModelSatisfiesClauses(Dictionary<string, bool> model, HashSet<Clause> clauses)
        {
            foreach (var clause in clauses)
            {
                if (!clause.Evaluate(model))
                {
                    return false;
                }
            }
            return true;
        }

        private Clause GetRandomFalseClause(Dictionary<string, bool> model, HashSet<Clause> clauses)
        {
            var falseClauses = new List<Clause>();
            foreach (var clause in clauses)
            {
                if (!clause.Evaluate(model))
                {
                    falseClauses.Add(clause);
                }
            }
            return falseClauses[_random.Next(falseClauses.Count)];
        }

        private void FlipRandomSymbolInClause(Clause clause, Dictionary<string, bool> model)
        {
            var randomSymbol = clause.Symbols.ElementAt(_random.Next(clause.Symbols.Count));
            model[randomSymbol] = !model[randomSymbol];
            model[randomSymbol.NegateSymbol()] = !model[randomSymbol];
        }

        private void FlipSymbolMaximizingSatisfiedClauses(Clause clause, Dictionary<string, bool> model, HashSet<Clause> clauses)
        {
            var maxSatisfiedClauses = 0;
            var symbolToFlip = string.Empty;

            foreach (var symbol in clause.Symbols)
            {
                model[symbol] = !model[symbol];
                var satisfiedClauses = CountSatisfiedClauses(model, clauses);

                if (satisfiedClauses > maxSatisfiedClauses)
                {
                    maxSatisfiedClauses = satisfiedClauses;
                    symbolToFlip = symbol;
                }

                // Flip back
                model[symbol] = !model[symbol]; 
                model[symbol.NegateSymbol()] = !model[symbol]; 
            }

            if (symbolToFlip != string.Empty)
            {
                // Flip the symbol that maximizes the number of satisfied clauses
                model[symbolToFlip] = !model[symbolToFlip];
                model[symbolToFlip.NegateSymbol()] = !model[symbolToFlip];
            }
        }

        private int CountSatisfiedClauses(Dictionary<string, bool> model, HashSet<Clause> clauses)
        {
            var count = 0;
            foreach (var clause in clauses)
            {
                if (clause.Evaluate(model))
                {
                    count++;
                }
            }
            return count;
        }

    }
}
