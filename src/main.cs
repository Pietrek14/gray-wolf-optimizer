using System;
using GWO;

class GrayWolfOptimizerProgram {
	static void Main() {
		Optimizer optimizer = new Optimizer(12, 250);

		uint dimensions = 2;

		IFunction function = new RastriginFunction();

		Vector minimum = optimizer.Optimize(
			function, dimensions,
			new Range(Vector.Ones(dimensions) * -10.0,
			Vector.Ones(dimensions) * 12.0)
		);

		Console.WriteLine("Rastrigin minimum: {0}, value: {1}", minimum, function.Evaluate(minimum));
	}
}