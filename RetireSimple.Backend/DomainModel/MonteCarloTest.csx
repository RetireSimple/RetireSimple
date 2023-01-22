#r "C:\Users\nikolira\RetireSimple\RetireSimple.Backend\bin\Debug\net6.0\RetireSimple.Backend.dll"
#r "C:\Users\nikolira\RetireSimple\RetireSimple.Backend\bin\Debug\net6.0\MathNet.Numerics.dll"
global using OptionsDict = System.Collections.Generic.Dictionary<string, string>;
using System.Collections.Generic;
using RetireSimple.Backend.DomainModel.Data.Investment;
using RetireSimple.Backend.DomainModel.Analysis;

var testStonk = new StockInvestment("testAnalysis");
testStonk.StockPrice = 100;

var testDict = new OptionsDict() {
	["AnalysisLength"] = "60",
	["SimCount"] = "100",
	["RandomVariableMu"] = ".5",
	["RandomVariableSigma"] = "1",
	["RandomVariableScaleFactor"] = "2.5"
};

var testModel = MonteCarlo.MonteCarloSim_NormalDistribution(testStonk, testDict);

var csvList = new List<string>();
for (int i = 0; i < 60; i++) {
	csvList.Add(testModel.MinModelData[i].ToString() + "," + testModel.AvgModelData[i].ToString() + "," + testModel.MaxModelData[i].ToString());
	Console.WriteLine(csvList[i]);
}


File.WriteAllLines("C:\\Users\\nikolira\\RetireSimple\\RetireSimple.Backend\\DomainModel\\MonteCarloTest.csv", csvList);
