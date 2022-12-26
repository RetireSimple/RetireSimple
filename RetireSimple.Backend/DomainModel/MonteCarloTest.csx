#r "D:\Onedrive\SeniorProject\RetireSimple\RetireSimple.Backend\bin\Debug\net6.0\RetireSimple.Backend.dll"
#r "D:\Onedrive\SeniorProject\RetireSimple\RetireSimple.Backend\bin\Debug\net6.0\MathNet.Numerics.dll"
using System.Collections.Generic;

using RetireSimple.Backend.DomainModel.Analysis;

//var testDict = new Dictionary<string, string>();
//testDict["stockMu"] = "0.5";
//testDict["stockSigma"] = "0.05";
//testDict["randomVarMu"] = "0";
//testDict["randomVarSigma"] = "1";
//testDict["stockPrice"] = "100";

//var testModel = RetireSimple.Backend.DomainModel.Analysis.MonteCarlo.MonteCarloSim_GeometricBrownianMotion(100, (decimal) 100, testDict);

var testAggregate = new List<List<(string, decimal)>>();
for (int i = 0; i < 100; i++) {
	testAggregate.Add(MonteCarlo.MonteCarloSim_GeometricBrownianMotion(100, 100.0, 0, 1));
}


var csvStrings = new List<string>();
for (var i = 0; i < 100; i++) {
	var stepRow = i.ToString();
	foreach (var testModel in testAggregate) {
		var modelStepValue = testModel.ElementAt(i).Item2;
		stepRow += ("," + modelStepValue.ToString());
	}
	csvStrings.Add(stepRow);
}


File.WriteAllLines("D:\\Onedrive\\SeniorProject\\RetireSimple\\RetireSimple.Backend\\DomainModel\\MonteCarloTest.csv", csvStrings);
//foreach (var csvString in csvStrings) {
	//Console.WriteLine(csvString);
	//File.AppendAllText("D:\\Onedrive\\SeniorProject\\RetireSimple\\RetireSimple.Backend\\DomainModel\\MonteCarloTest.csv", csvString + Environment.NewLine);
//}

//foreach (var step in testModel) {
//		Console.WriteLine(step);
//	}