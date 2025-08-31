using System;

namespace Metaheuristics {
	abstract class EvaluationFunction {
		private uint evaluationCalls = 0;
		private Vector displacement = null;

		public Vector Displacement {
			get {
				return this.displacement;
			}
			set {
				this.displacement = value;
			}
		}

		public string Name {
			get {
				if (this.displacement == null) {
					return this.name;
				} else {
					return String.Format("{0} [displaced by {1}]", this.name, this.displacement.ToPrettyString());
				}
			}
		}

		abstract protected string name { get; }

		public uint EvaluationCalls {
			get {
				return evaluationCalls;
			}
		}

		abstract protected double evaluate(Vector args);


		public double Evaluate(Vector args) {
			evaluationCalls++;

			if(this.displacement != null) {
				return evaluate(args - this.displacement);
			} else {
				return evaluate(args);
			}
		}

		public void ResetEvaluationCallsCounter() {
			evaluationCalls = 0;
		}
	}

	class RosenbrockFunction : EvaluationFunction {
		protected override string name {
			get {
				return "Rosenbrock";
			}
		}

		protected override double evaluate(Vector args) {
			double value = 0;

			for(uint i = 0; i < args.Dimensions() - 1; i++) {
				value += 100 * Math.Pow(args.vals[i + 1] - args.vals[i] * args.vals[i], 2);
				value += Math.Pow(1 - args.vals[i], 2);
			}

			return value;
		}
	}

	class BartekFunction : EvaluationFunction {
		protected override string name {
			get {
				return "Bartek";
			}
		}

		protected override double evaluate(Vector args) {
			return Math.Pow(args.vals[0] / 4 - 1, 2) + Math.Pow(args.vals[1] / 4 - 1, 2);
		}
	}

	class SphereFunction : EvaluationFunction {
		protected override string name {
			get {
				return "Sphere";
			}
		}

		protected override double evaluate(Vector args) {
			double value = 0;

			for(uint i = 0; i < args.Dimensions(); i++) {
				value += args.vals[i] * args.vals[i];
			}

			return value;
		}
	}

	class RastriginFunction : EvaluationFunction {
		protected override string name {
			get {
				return "Rastrigin";
			}
		}

		private double Factor;

		public RastriginFunction(double factor = 10) {
			this.Factor = factor;
		}

		protected override double evaluate(Vector args) {
			double value = this.Factor * args.Dimensions();

			for(uint i = 0; i < args.Dimensions(); i++) {
				value += args.vals[i] * args.vals[i] - this.Factor * Math.Cos(2 * Math.PI * args.vals[i]);
			}

			return value;
		}
	}

	class BoothFunction : EvaluationFunction {
		protected override string name {
			get {
				return "Booth";
			}
		}

		protected override double evaluate(Vector args) {
			return Math.Pow(args.vals[0] + 2 * args.vals[1] - 7, 2) + Math.Pow(2 * args.vals[0] + args.vals[1] - 5, 2);
		}
	}

	class GTOAParametrizationFunction : EvaluationFunction {
		protected override string name {
			get {
				return "GTOA Parametrization";
			}
		}

		GTOA.Optimizer optimizer;
		EvaluationFunction function;
		Vector expectedSolution;
		Range searchRange;
		uint dimensionCount;
		Tester<GTOA.Optimizer, GTOA.Params> tester;

		public GTOAParametrizationFunction(
			EvaluationFunction _function,
			Range _searchRange,
			uint _dimensionCount,
			Vector _expectedSolution
		) {
			function = _function;
			searchRange = _searchRange;
			dimensionCount = _dimensionCount;
			expectedSolution = _expectedSolution;
			optimizer = new GTOA.Optimizer(
				function,
				searchRange,
				dimensionCount
			);

			tester = new Tester<GTOA.Optimizer, GTOA.Params>(
				optimizer,
				function,
				searchRange
			);
		}

		protected override double evaluate(Vector args) {
			uint iterationCount = (uint)Math.Floor(args.vals[0]);
			uint populationSize = (uint)Math.Floor(args.vals[1]);
			var parameters = new GTOA.Params(args.vals[2]);

			Tester<GTOA.Optimizer, GTOA.Params>.TestResult[] results = tester.RunTests(
				new Tester<GTOA.Optimizer, GTOA.Params>.TestCase[] {
					new Tester<GTOA.Optimizer, GTOA.Params>.TestCase(
						dimensionCount,
						iterationCount,
						populationSize,
						parameters
					)
				}
			);

			var result = results[0];

			var distanceToSolution = (result.BestSolution - expectedSolution).Length();

			return distanceToSolution
				* result.EvaluationFunctionCalls
				* result.SolutionVariancy.Length();
		}
	}
}