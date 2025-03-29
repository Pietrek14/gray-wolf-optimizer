namespace Metaheuristics {
	interface ITestable<Self, P> : IOptimizationAlgorithm
		where P : ICustomParameters
		where Self : ITestable<Self, P>
	{
		void ResetEvaluationCallsCounter();
		void Reconfigure(Tester<Self, P>.TestCase testCase);
	}
}