using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace WalkSatAI
{
    public static class Helper
    {
        public static string NegateSymbol(this string symbol)
        {
            return symbol.StartsWith("~") ? symbol.Substring(1) : "~" + symbol;
        }

        public static void Printer(KCnfBuilder builderFormula, Dictionary<string, bool> result)
        {
            Console.WriteLine($"Formula: {builderFormula.Formula}");
            if (result != null)
            {
                Console.WriteLine("Result: ");
                foreach (var kvp in result)
                {
                    Console.WriteLine($"{kvp.Key}: {kvp.Value}");
                }
            }

        }

        public static HashSet<Clause> BuilderClauses(this HashSet<Clause> clauses, HashSet<List<string>> clauseList)
        {
            foreach (var kvp in clauseList)
            {
                var current = new HashSet<string>();
                foreach (var kvp2 in kvp)
                {
                    current.Add(kvp2);
                }
                clauses.Add(new Clause(current));
            }
            return clauses;
        }

        public static void GenerateCsvFile(string filePath, Dictionary<string, string> colsValue, string colsName)
        {
            using (var sw = new StreamWriter(filePath, true))
            {
                sw.WriteLine(colsName);
                foreach (var kvp in colsValue.OrderBy(k => k.Key))
                    sw.WriteLine($"{kvp.Key.Replace(",", ".")},{kvp.Value.Replace(",", ".")}");
            };
        }

        public static double GetMedianValue(List<double> currentList)
        {
            var medianValue = 0.00;
            currentList.Sort();
            if (currentList.Any())
            {
                if (currentList.Count % 2 == 0)
                {
                    int middleIndex = currentList.Count / 2;
                    medianValue = (currentList[middleIndex - 1] + currentList[middleIndex]) / 2.0;
                }
                else
                {
                    int middleIndex = currentList.Count / 2;
                    medianValue = currentList[middleIndex];
                }
            }

            return medianValue;
        }
    }
}
