using System.Diagnostics;
using System.Transactions;
using WalkSatAI;

var probabilityOfSatFilePath = "../../../probability_of_sat.csv";
var runtimeFilePath = "../../../runtime_exec.csv";

int[] chooseM = [10, 20, 30, 40, 50, 60, 70, 80, 90, 100, 111, 122, 133, 144, 155, 166, 177, 188, 199, 203, 213, 220, 230, 240, 250, 260, 270, 280, 290, 303, 312, 321,334, 346, 356, 361, 378, 382, 393, 400];
var n = 50; 
var k = 3;  
var p = 0.5;
var maxFlips = 1000;
var runInternal = 100;

var builderFormula = new KCnfBuilder();
var walsatInstance = new WalkSat();

var probabilitySat = new Dictionary<string, string>();
var executionTime = new Dictionary<string, string>();

for (var i = 0; i < chooseM.Length; i++)
{
    var m = chooseM[i];  // Number of clauses

    var satisfied = 0;
    var execCurrent = new List<double>();
    var clauseOverSymbols = ((double)m) / ((double)n);

    for (var j = 0; j < runInternal; j++)
    {
        var clauses = builderFormula.GenerateKCnfFormula(n, m, k);

        var timer = new Stopwatch();
        timer.Start();

        var result = walsatInstance.WalkSAT(clauses, p, maxFlips);

        timer.Stop();   

        if (result != null)
        {
            satisfied++;
            execCurrent.Add(timer.Elapsed.TotalMilliseconds);
        }
    }

    var probabilityOfSat = (double)satisfied / (double)runInternal;
    probabilitySat.Add(clauseOverSymbols.ToString("0.00"), probabilityOfSat.ToString("0.00"));
    
    var medianExecutionTime = Helper.GetMedianValue(execCurrent);
    executionTime.Add(clauseOverSymbols.ToString("0.00"), medianExecutionTime.ToString());
}

Helper.GenerateCsvFile(probabilityOfSatFilePath, probabilitySat, "m/n,p(Sat)");
Helper.GenerateCsvFile(runtimeFilePath, executionTime, "m/n,runtime");

