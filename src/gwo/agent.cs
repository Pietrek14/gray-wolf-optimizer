using System;
using Metaheuristics;

namespace GWO {
	class Agent {
		public Vector Position { get; private set; }
		public double Fitness { get; private set; }
		public Range SearchRange { get; private set; }
		public uint Dimensions {
			get {
				return Position.Dimensions();
			}
			private set {}
		}

		private double obstacleFactor;

		public Agent(Vector position, Range searchRange, double obstacleFactor = 2.0) {
			this.Position = position;
			this.SearchRange = searchRange;

			this.obstacleFactor = obstacleFactor;
		}

		public void CalculateFitness(EvaluationFunction function) {
			this.Fitness = function.Evaluate(this.Position);
		}

		public void UpdatePosition(Vector[] bestThreePositions, double approachFactor, Random random) {
			Vector target = Vector.Zeros(Dimensions);

			Vector random1 = Vector.Random(Dimensions, random), random2 = Vector.Random(Dimensions, random);

			for(uint i = 0; i < 3; i++) {
				Vector approachVector = Vector.Ones(Dimensions) * approachFactor;
				Vector movementVector = 2.0 * approachVector * random1 - approachVector;
				Vector obstacleVector = this.obstacleFactor * random2;

				double distanceFactor = (obstacleVector * bestThreePositions[i] - this.Position).Length();

				target += bestThreePositions[i] - movementVector * distanceFactor;
			}

			target /= 3;

			this.Position = SearchRange.ClosestInRange(target);
		}
	}
}