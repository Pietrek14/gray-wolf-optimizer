using System;
using System.Collections.Generic;
using Metaheuristics;
using GWO;

class GrayWolfOptimizerProgram {
	static void Main() {
		uint[] dimensionCounts = { 2, 3, 6, 10 };

		var testResults = new List<Tester.TestCase>();

		foreach(uint dimensions in dimensionCounts) {
			EvaluationFunction function = new RosenbrockFunction();

			Range searchRange = new Range(
				Vector.Ones(dimensions) * -10.0,
				Vector.Ones(dimensions) * 12.0
			);

			Tester tester = new Tester(
				function,
				searchRange,
				dimensions,
				new uint[] { 10, 20, 40, 80 },
				new uint[] { 5, 10, 20, 40, 60, 80 }
			);

			Tester.TestCase[] currentResults = tester.RunTests();

			foreach(var result in currentResults) {
				testResults.Add(result);
			}
		}

		var outputter = new CSVTestCaseOutputter();
		outputter.Output(testResults.ToArray());
	}
}