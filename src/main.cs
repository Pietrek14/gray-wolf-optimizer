using System;
using System.Collections.Generic;
using Metaheuristics;
using GWO;

class GrayWolfOptimizerProgram {
	static void Main() {
		uint[] dimensionCounts = { 2, 3, 6, 10 };
		uint[] iterationCounts = { 5, 10, 20, 40, 60, 80 };
		uint[] populationSizes = { 10, 20, 40, 80 };
		double[] maxApproachFactors = { 2.0 };
		double[] obstacleFactors = { 2.0 };

		var testResults = new List<Tester<Optimizer, Params>.TestResult>();

		EvaluationFunction function = new RosenbrockFunction();

		foreach(uint dimensions in dimensionCounts) {
			Range searchRange = new Range(
				Vector.Ones(dimensions) * -10.0,
				Vector.Ones(dimensions) * 12.0
			);

			var optimizer = new Optimizer(function, searchRange, dimensions);

			var tester = new Tester<Optimizer, Params>(
				optimizer,
				function,
				searchRange
			);

			Tester<Optimizer, Params>.TestResult[] currentResults = tester.RunTests(
				Tester<Optimizer, Params>.TestCase.Combinations(
					new uint[] { dimensions },
					iterationCounts,
					populationSizes,
					Params.Combinations(
						maxApproachFactors,
						obstacleFactors
					)
				)
			);

			foreach(var result in currentResults) {
				testResults.Add(result);
			}
		}

		var outputter = new CSVTestResultOutputter<Optimizer, Params>();
		outputter.Output(testResults.ToArray());
	}
}