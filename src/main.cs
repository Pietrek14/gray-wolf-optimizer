using System;
using GWO;

class GrayWolfOptimizerProgram {
	static void Main() {
		uint dimensions = 2;

		EvaluationFunction function = new SphereFunction();
		function.Displacement = new Vector(new[] { 2.0, 1.0 });

		Range searchRange = new Range(
			Vector.Ones(dimensions) * -10.0,
			Vector.Ones(dimensions) * 12.0
		);

		Optimizer optimizer = new Optimizer(function, searchRange, dimensions);

		double minimumValue = optimizer.Solve();

		Console.WriteLine(
			"{0} minimum: {1}, value: {2}",
			function.Name,
			optimizer.BestAgent,
			minimumValue
		);
		Console.WriteLine(
			"Evaluation function calls: {0}",
			optimizer.NumberOfEvaluationFitnessFunction
		);
	}
}