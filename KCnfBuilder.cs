using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WalkSatAI
{
    public class KCnfBuilder
    {
        private readonly Random _random = new();
        public string Formula {  get; private set; } = string.Empty;
        public List<string> Literals { get; private set; } = new();
        public List<string> Symbols { get; private set; } = new();

        public HashSet<Clause> GenerateKCnfFormula(int n, int m, int k)
        {
            Symbols = Enumerable.Range(1, n).Select(i => $"p{i}").ToList();
            Literals = Symbols.Concat(Symbols.Select(symbol => $"~{symbol}")).ToList();

            var clauses = new HashSet<List<string>>();
            while (clauses.Count < m)
            {
                var choosedClauses = ChooseRandomClauses(k);
                if (choosedClauses.Count > 0 && !clauses.Contains(choosedClauses) && !ContainsTautology(choosedClauses))
                    clauses.Add(choosedClauses);
            }

            Formula = string.Join(" & ", clauses.Select(clause => $"({string.Join(" | ", clause)})"));
            
            return new HashSet<Clause>().BuilderClauses(clauses);
        }

        private List<string> ChooseRandomClauses(int k)
        {
            //Sorts elements of the list using a randomly generated key
            var clauseLiterals = Literals.OrderBy(_ => _random.Next()).Take(k).ToList();
            return clauseLiterals;
        }

        private bool ContainsTautology(List<string> clause)
        {
            return clause.Any(literal => clause.Contains(literal.NegateSymbol()));
        }
    }
}

