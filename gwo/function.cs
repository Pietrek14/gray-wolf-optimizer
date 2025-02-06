using System;

namespace GWO {
	interface IFunction {
		double Evaluate(Vector args);
	}

	class RosenbrockFunction : IFunction {
		public double Evaluate(Vector args) {
			double value = 0;

			for(uint i = 0; i < args.Dimensions() - 1; i++) {
				value += 100 * Math.Pow(args.vals[i + 1] - args.vals[i] * args.vals[i], 2);
				value += Math.Pow(1 - args.vals[i], 2);
			}

			return value;
		}
	}

	class BartekFunction : IFunction {
		public double Evaluate(Vector args) {
			return Math.Pow(args.vals[0] / 4 - 1, 2) + Math.Pow(args.vals[1] / 4 - 1, 2);
		}
	}

	class SphereFunction : IFunction {
		public double Evaluate(Vector args) {
			double value = 0;

			for(uint i = 0; i < args.Dimensions(); i++) {
				value += args.vals[i] * args.vals[i];
			}

			return value;
		}
	}

	class RastriginFunction : IFunction {
		private double Factor;

		public RastriginFunction(double factor = 10) {
			this.Factor = factor;
		}

		public double Evaluate(Vector args) {
			double value = this.Factor * args.Dimensions();

			for(uint i = 0; i < args.Dimensions(); i++) {
				value += args.vals[i] * args.vals[i] - this.Factor * Math.Cos(2 * Math.PI * args.vals[i]);
			}

			return value;
		}
	}

	class BoothFunction : IFunction {
		public double Evaluate(Vector args) {
			return Math.Pow(args.vals[0] + 2 * args.vals[1] - 7, 2) + Math.Pow(2 * args.vals[0] + args.vals[1] - 5, 2);
		}
	}
}