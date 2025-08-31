using System;
using System.Collections.Generic;
using Metaheuristics;

class GrayWolfOptimizerProgram {
	static void Main() {
		uint[] dimensionCounts = { 3 };
		// uint[] iterationCounts = { 5 };
		// uint[] populationSizes = { 10 };
		// double[] teachingFactors = { 1.0, 2.0 };

		var testResults = new List<Tester<GWO.Optimizer, GWO.Params>.TestResult>();

		foreach(uint dimensions in dimensionCounts) {
			var expectedSolution = new Vector(new double[] { 2.0, 1.0 });

			var innerFunction = new SphereFunction();
			innerFunction.Displacement = expectedSolution;

			EvaluationFunction function = new GTOAParametrizationFunction(
				innerFunction,
				new Range(
					Vector.Ones(2) * -5.0,
					Vector.Ones(2) * 5.0
				),
				2,
				expectedSolution
			);
			Vector displacement = Vector.Ones(dimensions);
			displacement.vals[0]++;
			function.Displacement = displacement;

			Range searchRange = new Range(
				new Vector(new double[] { 5.0, 5.0, 0.5 }),
				new Vector(new double[] { 100.0, 100.0, 2.5 })
			);

			var optimizer = new GWO.Optimizer(function, searchRange, dimensions);

			var tester = new Tester<GWO.Optimizer, GWO.Params>(
				optimizer,
				function,
				searchRange
			);

			Tester<GWO.Optimizer, GWO.Params>.TestResult[] currentResults = tester.RunTests(
				new Tester<GWO.Optimizer, GWO.Params>.TestCase[] {
					new Tester<GWO.Optimizer, GWO.Params>.TestCase(
						dimensions,
						40,
						40,
						new GWO.Params(2.0, 2.0)
					)
				}
			);

			foreach(var result in currentResults) {
				testResults.Add(result);
			}
		}

		var outputter = new CSVTestResultOutputter<GWO.Optimizer, GWO.Params>();
		outputter.Output(testResults.ToArray());
	}
}