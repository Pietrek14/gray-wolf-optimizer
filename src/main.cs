using System;
using System.Collections.Generic;
using Metaheuristics;

class GrayWolfOptimizerProgram {
	static void Main() {
		uint[] dimensionCounts = { 2, 3, 6, 10 };
		uint[] iterationCounts = { 5, 10, 20, 40, 60, 80 };
		uint[] populationSizes = { 10, 20, 40, 80 };
		double[] maxApproachFactors = { 2.0 };
		double[] obstacleFactors = { 2.0 };

		// var testResults = new List<Tester<GWO.Optimizer, GWO.Params>.TestResult>();

		uint dimensions = 2;

		Range searchRange = new Range(
			Vector.Ones(dimensions) * -10.0,
			Vector.Ones(dimensions) * 12.0
		);

		EvaluationFunction function = new RosenbrockFunction();
		function.Displacement = new Vector(new double[] { 0, 1 });

		var optimizer = new GTOA.Optimizer(function, searchRange, dimensions);

		var minValue = optimizer.Solve();
		var bestAgent = optimizer.BestAgent;

		Console.WriteLine(function.Name + " minimum: " + minValue + " (" + bestAgent.Position + ")");

		// foreach(uint dimensions in dimensionCounts) {
		// 	Range searchRange = new Range(
		// 		Vector.Ones(dimensions) * -10.0,
		// 		Vector.Ones(dimensions) * 12.0
		// 	);

		// 	var optimizer = new GWO.Optimizer(function, searchRange, dimensions);

		// 	var tester = new Tester<GWO.Optimizer, GWO.Params>(
		// 		optimizer,
		// 		function,
		// 		searchRange
		// 	);

		// 	Tester<GWO.Optimizer, GWO.Params>.TestResult[] currentResults = tester.RunTests(
		// 		Tester<GWO.Optimizer, GWO.Params>.TestCase.Combinations(
		// 			new uint[] { dimensions },
		// 			iterationCounts,
		// 			populationSizes,
		// 			GWO.Params.Combinations(
		// 				maxApproachFactors,
		// 				obstacleFactors
		// 			)
		// 		)
		// 	);

		// 	foreach(var result in currentResults) {
		// 		testResults.Add(result);
		// 	}
		// }

		// var outputter = new CSVTestResultOutputter<GWO.Optimizer, GWO.Params>();
		// outputter.Output(testResults.ToArray());
	}
}