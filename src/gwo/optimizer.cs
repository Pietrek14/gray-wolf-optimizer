using System;
using Metaheuristics;

namespace GWO {
	class Params : ICustomParameters {
		public double MaxApproachFactor;
		public double ObstacleFactor;

		public Params(double maxApproachFactor, double obstacleFactor) {
			MaxApproachFactor = maxApproachFactor;
			ObstacleFactor = obstacleFactor;
		}

		public static Params[] Combinations(
			double[] maxApproachFactors,
			double[] obstacleFactors
		) {
			Params[] result = new Params[
				maxApproachFactors.Length * obstacleFactors.Length
			];
			uint index = 0;

			foreach(double maxApproachFactor in maxApproachFactors) {
				foreach(double obstacleFactor in obstacleFactors) {
					result[index] = new Params(maxApproachFactor, obstacleFactor);
					index++;
				}
			}

			return result;
		}

		public string[] ParameterNames { get {
			return new string[] { "Współczynnik podejścia", "Współczynnik przeszkody" };
		}}

		public string[] ParameterValues { get {
			return new string[] {
				MaxApproachFactor.ToString(),
				ObstacleFactor.ToString()
			};
		}}
	}

	class Optimizer : ITestable<Optimizer, Params> {
		private EvaluationFunction function;
		private Range searchRange;

		public uint DimensionCount;
		Agent[] agents;
		public uint AgentCount {
			get {
				return (uint)agents.Length;
			}
			set {
				agents = new Agent[value];
			}
		}
		public uint IterationCount;
		public Params CustomParameters;

		private Agent[] bestThreeAgents;
		private Random random;
		private Vector currentBestAgent;
		private double currentBestValue;


		public string Name {
			get {
				return "Gray Wolf Optimizer";
			}
			set {}
		}

		public Optimizer(
			EvaluationFunction function,
			Range searchRange,
			uint dimensions,
			uint agentCount = 12,
			uint maxIterations = 200,
			double maxApproachFactor = 2,
			double obstacleFactor = 2
		) {
			if(agentCount < 3) {
				throw new ArgumentException(String.Format("Agent count has to be at least three (is {0})", agentCount));
			}

			this.function = function;
			this.searchRange = searchRange;
			this.DimensionCount = dimensions;
			agents = new Agent[agentCount];
			bestThreeAgents = new Agent[3];
			this.IterationCount = maxIterations;
			this.CustomParameters = new Params(maxApproachFactor, obstacleFactor);

			this.random = new Random(Guid.NewGuid().GetHashCode());
		}

		private void CalculateAgentsFitness(EvaluationFunction function) {
			foreach(Agent agent in agents) {
				agent.CalculateFitness(function);

				for(int i = 2; i >= 0; i--) {
					if(bestThreeAgents[i] == null || bestThreeAgents[i].Fitness > agent.Fitness) {
						if(i != 2) {
							bestThreeAgents[i + 1] = bestThreeAgents[i];
						}

						bestThreeAgents[i] = agent;
					}
				}
			}
		}

		private void UpdateAgentsPosition(double approachFactor, Random random) {
			Vector[] bestThreePositions = new Vector[3];

			for(uint i = 0; i < 3; i++) {
				bestThreePositions[i] = bestThreeAgents[i].Position;
			}

			foreach(Agent agent in agents) {
				agent.UpdatePosition(bestThreePositions, approachFactor, random);
			}
		}

		public double Solve() {
			// Spawn agents
			for(uint i = 0; i < agents.Length; i++) {
				agents[i] = new Agent(searchRange.RandomInRange(random), searchRange, CustomParameters.ObstacleFactor);
			}

			currentBestAgent = null;
			currentBestValue = Double.PositiveInfinity;

			for(uint i = 0; i < this.IterationCount; i++) {
				// Calculate the approach factor
				double approachFactor = this.CustomParameters.MaxApproachFactor
					- (this.CustomParameters.MaxApproachFactor * i / this.IterationCount);

				// Update agents
				CalculateAgentsFitness(function);

				// Save the current best
				if(currentBestValue > bestThreeAgents[0].Fitness) {
					currentBestValue = bestThreeAgents[0].Fitness;
					currentBestAgent = bestThreeAgents[0].Position;
				}

				UpdateAgentsPosition(approachFactor, random);
			}

			// Return the current best 
			return currentBestValue;
		}

		public Vector BestAgent {
			get {
				return currentBestAgent;
			}
			set {}
		}

		public double[] XBest {
			get {
				return BestAgent.vals;
			}
			set {}
		}

		public double FBest {
			get {
				return currentBestValue;
			}
			set {}
		}

		public int NumberOfEvaluationFitnessFunction {
			get {
				return (int)this.function.EvaluationCalls;
			}
			set {}
		}

		public void ResetEvaluationCallsCounter() {
			this.function.ResetEvaluationCallsCounter();
		}

		public void Reconfigure(Tester<Optimizer, Params>.TestCase testCase) {
			this.DimensionCount = testCase.DimensionCount;
			this.AgentCount = testCase.PopulationSize;
			this.IterationCount = testCase.IterationCount;
			this.CustomParameters = testCase.CustomParameters;
		}
	}
}