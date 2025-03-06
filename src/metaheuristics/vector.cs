using System;

namespace Metaheuristics {
	class Vector {
		public double[] vals;

		public Vector(uint dimensions) {
			vals = new double[dimensions];
		}

		public Vector(double[] vals) {
			this.vals = vals;
		}

		/// <summary>
		/// Constructs a random vector in given dimensions.
		/// All components of the vector are in range [0, 1].
		/// </summary>
		/// <param name="dimensions">The size of the vector</param>
		/// <param name="random">Random number generator</param>
		/// <returns>The constructed vector</returns>
		public static Vector Random(uint dimensions, Random random) {
			double[] vals = new double[dimensions];

			for(uint i = 0; i < dimensions; i++) {
				vals[i] = random.NextDouble() % 1.0;
			}

			return new Vector(vals);
		}

		/// <summary>
		/// Constructs a vector with all components set to zero.
		/// </summary>
		/// <param name="dimensions">The size of the vector</param>
		/// <returns>The constructed vector</returns>
		public static Vector Zeros(uint dimensions) {
			double[] vals = new double[dimensions];

			for(uint i = 0; i < dimensions; i++) {
				vals[i] = 0;
			}

			return new Vector(vals);
		}

		/// <summary>
		/// Constructs a vector with all components set to one.
		/// </summary>
		/// <param name="dimensions">The size of the vector</param>
		/// <returns>The constructed vector</returns>
		public static Vector Ones(uint dimensions) {
			double[] vals = new double[dimensions];

			for(uint i = 0; i < dimensions; i++) {
				vals[i] = 1;
			}

			return new Vector(vals);
		}

		public double Length() {
			double lengthSquared = 0;

			foreach(double val in vals) {
				lengthSquared += val * val;
			}

			return Math.Sqrt(lengthSquared);
		}

		public uint Dimensions() {
			return (uint)this.vals.Length;
		}

		public class MismatchedSizeException : Exception {
			public MismatchedSizeException() {}
			public MismatchedSizeException(string message) : base(message) {}
			public MismatchedSizeException(string message, Exception inner)
				: base(message, inner) {}
		}

		public static Vector operator +(Vector vector1, Vector vector2) {
			if(vector1.Dimensions() != vector2.Dimensions()) {
				throw new MismatchedSizeException(
					String.Format("Can't add two vectors with different dimensions! ({0}, {1})",
					vector1.Dimensions(),
					vector2.Dimensions()
				));
			}

			Vector result = new Vector(vector1.Dimensions());

			for(uint i = 0; i < vector1.Dimensions(); i++) {
				result.vals[i] = vector1.vals[i] + vector2.vals[i];
			}

			return result;
		}

		public static Vector operator -(Vector vector1, Vector vector2) {
			if(vector1.Dimensions() != vector2.Dimensions()) {
				throw new MismatchedSizeException(
					String.Format("Can't subtract two vectors with different dimensions! ({0}, {1})",
					vector1.Dimensions(),
					vector2.Dimensions()
				));
			}

			Vector result = new Vector(vector1.Dimensions());

			for(uint i = 0; i < vector1.Dimensions(); i++) {
				result.vals[i] = vector1.vals[i] - vector2.vals[i];
			}

			return result;
		}

		public static Vector operator *(Vector vector1, Vector vector2) {
			if(vector1.Dimensions() != vector2.Dimensions()) {
				throw new MismatchedSizeException(
					String.Format("Can't multiply two vectors with different dimensions! ({0}, {1})",
					vector1.Dimensions(),
					vector2.Dimensions()
				));
			}

			Vector result = new Vector(vector1.Dimensions());

			for(uint i = 0; i < vector1.Dimensions(); i++) {
				result.vals[i] = vector1.vals[i] * vector2.vals[i];
			}

			return result;
		}

		public static Vector operator *(Vector vector, double scalar) {
			Vector result = new Vector(vector.Dimensions());

			for(uint i = 0; i < vector.Dimensions(); i++) {
				result.vals[i] = vector.vals[i] * scalar;
			}

			return result;
		}

		public static Vector operator *(double scalar, Vector vector) {
			return vector * scalar;
		}

		public static Vector operator /(Vector vector1, Vector vector2) {
			if(vector1.Dimensions() != vector2.Dimensions()) {
				throw new MismatchedSizeException(
					String.Format("Can't divide two vectors with different dimensions! ({0}, {1})",
					vector1.Dimensions(),
					vector2.Dimensions()
				));
			}

			Vector result = new Vector(vector1.Dimensions());

			for(uint i = 0; i < vector1.Dimensions(); i++) {
				result.vals[i] = vector1.vals[i] / vector2.vals[i];
			}

			return result;
		}

		public static Vector operator /(Vector vector, double scalar) {
			Vector result = new Vector(vector.Dimensions());

			for(uint i = 0; i < vector.Dimensions(); i++) {
				result.vals[i] = vector.vals[i] / scalar;
			}

			return result;
		}

		public Vector Dot(Vector vector) {
			if(this.Dimensions() != vector.Dimensions()) {
				throw new MismatchedSizeException(
					String.Format("Can't calculate a dot product of two vectors with different dimensions! ({0}, {1})",
					this.Dimensions(),
					vector.Dimensions()
				));
			}

			Vector result = new Vector(this.Dimensions());

			for(uint i = 0; i < this.Dimensions(); i++) {
				result.vals[i] = this.vals[i] - vector.vals[i];
			}

			return result;
		}

		public Vector Sqrt() {
			Vector result = new Vector(this.Dimensions());

			for(uint i = 0; i < this.Dimensions(); i++) {
				result.vals[i] = Math.Sqrt(this.vals[i]);
			}

			return result;
		}

		public override string ToString() {
			string typeName = String.Format("{0}d vector", this.Dimensions());
			string data = String.Join(", ", this.vals);

			return String.Format("{0} {{{1}}}", typeName, data);
		}

		public string ToPrettyString() {
			return String.Format("({0})", String.Join(", ", this.vals));
		}
	}
}