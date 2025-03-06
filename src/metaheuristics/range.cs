using System;

namespace Metaheuristics {
	class Range {
		Vector min, max;

		public Range(Vector min, Vector max) {
			if(min.Dimensions() != max.Dimensions()) {
				throw new Vector.MismatchedSizeException(String.Format(
					"Range's limiting vectors can't be of different size! ({0} and {1} given)",
					min.Dimensions(), max.Dimensions()
				));
			}

			for(uint i = 0; i < min.Dimensions(); i++) {
				if(max.vals[i] < min.vals[i]) {
					throw new InvalidRangeLimitsException(String.Format(
						"Range max can't be less than the min (is at {0} axis, min: {1}, max: {2})",
						i, min.vals[i], max.vals[i]
					));
				}
			}

			this.min = min;
			this.max = max;
		}

		public class InvalidRangeLimitsException : Exception {
			public InvalidRangeLimitsException() {}
			public InvalidRangeLimitsException(string message) : base(message) {}
			public InvalidRangeLimitsException(string message, Exception inner)
				: base(message, inner) {}
		}

		public bool IsInRange(Vector vector) {
			if(vector.Dimensions() != this.min.Dimensions()) {
				throw new Vector.MismatchedSizeException(String.Format(
					"Can't check if vector is in a range in different dimensions! (vector: {0}, range: {1})",
					vector.Dimensions(), this.min.Dimensions()
				));
			}

			for(uint i = 0; i < min.Dimensions(); i++) {
				if(vector.vals[i] < min.vals[i] || vector.vals[i] > max.vals[i]) {
					return false;
				}
			}

			return true;
		}

		public Vector RandomInRange(Random random) {
			Vector result = Vector.Random(min.Dimensions(), random);

			for(uint i = 0; i < result.Dimensions(); i++) {
				result.vals[i] *= (max.vals[i] - min.vals[i]);
				result.vals[i] += min.vals[i];
			}

			return result;
		}

		public Vector ClosestInRange(Vector vector) {
			if(vector.Dimensions() != this.min.Dimensions()) {
				throw new Vector.MismatchedSizeException(String.Format(
					"Can't clamp vector into a range in different dimensions! (vector: {0}, range: {1})",
					vector.Dimensions(), this.min.Dimensions()
				));
			}

			Vector result = new Vector(vector.Dimensions());

			for(uint i = 0; i < result.Dimensions(); i++) {
				result.vals[i] = Math.Min(Math.Max(vector.vals[i], min.vals[i]), max.vals[i]);
			}

			return result;
		}
	}
}