using System;
using GWO;

class GrayWolfOptimizerTester {
	IFunction function;
	Range domain;
	uint populationSizes[];
	uint iterationCounts[];

	GrayWolfOptimizer optimizer;

	GrayWolfOptimizerTester(IFunction function, Range domain, uint populationSizes[], uint iterationCounts[]) {
		this.function = function;
		this.domain = domain;
		this.populationSizes = populationSizes;
		this.iterationCounts = iterationCounts;

		this.optimizer = new GrayWolfOptimizer(function, domain);
	}

	public class TestCase {
		public string algorithmName;
		public string functionName;
		public uint dimensionCount;
		public double maxApproachFactor;
		public double obstacleFactor; 
		public uint iterationCount;
		public uint populationSize;
		public Vector bestSolution;
		public Vector worstSolution;
		public Vector solutionStandardDeviation;
		public double bestValue;
		public double worstValue;
		public double valueStandardDeviation;

		public TestCase(
			string algorithmName,
			string functionName,
			uint dimensionCount,
			double maxApproachFactor,
			double obstacleFactor,
			uint iterationCount,
			uint populationSize,
			Vector bestSolution,
			double bestValue,
			Vector worstSolution,
			double worstValue,
			Vector solutionStandardDeviation,
			double valueStandardDeviation
		) {
			this.algorithmName = algorithmName;
			this.functionName = functionName;
			this.dimensionCount = dimensionCount;
			this.maxApproachFactor = maxApproachFactor;
			this.obstacleFactor = obstacleFactor;
			this.iterationCount = iterationCount;
			this.populationSize = populationSize;
			this.bestSolution = bestSolution;
			this.bestValue = bestValue;
			this.worstSolution = worthSolution;
			this.worstValue = worthValue;
			this.solutionStandardDeviation = solutionStandardDeviation;
			this.valueStandardDeviation = valueStandardDeviation;
		}
	};

	TestCase[] RunTests(uint retryCount = 10) {
		TestCase[] result = new TestCase[this.populationSizes.Length];
		uint index = 0;

		foreach(uint populationSize in populationSizes) {
			foreach(uint iterationCount in iterationCounts) {

			}
		}
	}
}

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