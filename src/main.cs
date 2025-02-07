using System;
using GWO;

class GrayWolfOptimizerProgram {
	static void Main() {
		EvaluationFunction function = new RosenbrockFunction();
		uint dimensions = 2;
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
	}
}