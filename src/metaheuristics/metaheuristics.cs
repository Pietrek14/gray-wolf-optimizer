namespace Metaheuristics {
	public interface IOptimizationAlgorithm {
		/// <summary>
		/// Algorithm name
		/// </summary>
		string Name { get; set; }

		/// <summary>
		/// Searches for the minimum value of the function. If the results of previous calculations
		/// are saved in a memoized file, it loads them in and resumes search from the given point.
		/// </summary>
		/// <returns>The minimum value</returns>
		double Solve();

		/// <summary>
		/// The best agent as an array of values
		/// </summary>
		double[] XBest { get; set; }

		/// <summary>
		/// Function value at the best agent's location.
		/// </summary>
		double FBest { get; set; }

		int NumberOfEvaluationFitnessFunction { get; set; }
	}
}