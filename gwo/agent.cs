using System;

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

		public Agent(Vector position, Range searchRange) {
			this.Position = position;
			this.SearchRange = searchRange;
		}

		public void CalculateFitness(IFunction function) {
			this.Fitness = function.Evaluate(this.Position);
		}

		public void UpdatePosition(Vector[] bestThreePositions, double approachFactor, Random random) {
			Vector target = Vector.Zeros(Dimensions);

			Vector random1 = Vector.Random(Dimensions, random), random2 = Vector.Random(Dimensions, random);

			for(uint i = 0; i < 3; i++) {
				Vector approachVector = Vector.Ones(Dimensions) * approachFactor;
				Vector movementVector = 2.0 * approachVector * Vector.Random(Dimensions, random) - approachVector;
				Vector obstacleVector = 2.0 * Vector.Random(Dimensions, random);

				double distanceFactor = (obstacleVector * bestThreePositions[i] - this.Position).Length();

				target += bestThreePositions[i] - movementVector * distanceFactor;
			}

			target /= 3;

			this.Position = SearchRange.ClosestInRange(target);
		}
	}
}