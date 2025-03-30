using System;
using System.Collections.Generic;
using Metaheuristics;

class GrayWolfOptimizerProgram {
	static void Main() {
		uint[] dimensionCounts = { 2, 3, 6, 10 };
		uint[] iterationCounts = { 5, 10, 20, 40, 60, 80 };
		uint[] populationSizes = { 10, 20, 40, 80 };
		double[] teachingFactors = { 1.0, 2.0 };

		var testResults = new List<Tester<GTOA.Optimizer, GTOA.Params>.TestResult>();

		foreach(uint dimensions in dimensionCounts) {
			EvaluationFunction function = new RastriginFunction();
			Vector displacement = Vector.Ones(dimensions);
			displacement.vals[0]++;
			function.Displacement = displacement;

			Range searchRange = new Range(
				Vector.Ones(dimensions) * -10.0,
				Vector.Ones(dimensions) * 12.0
			);

			var optimizer = new GTOA.Optimizer(function, searchRange, dimensions);

			var tester = new Tester<GTOA.Optimizer, GTOA.Params>(
				optimizer,
				function,
				searchRange
			);

			Tester<GTOA.Optimizer, GTOA.Params>.TestResult[] currentResults = tester.RunTests(
				Tester<GTOA.Optimizer, GTOA.Params>.TestCase.Combinations(
					new uint[] { dimensions },
					iterationCounts,
					populationSizes,
					GTOA.Params.Combinations(
						teachingFactors
					)
				)
			);

			foreach(var result in currentResults) {
				testResults.Add(result);
			}
		}

		var outputter = new CSVTestResultOutputter<GTOA.Optimizer, GTOA.Params>();
		outputter.Output(testResults.ToArray());
	}
}