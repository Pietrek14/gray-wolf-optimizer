using System;

namespace GWO {
	class Optimizer : IOptimizationAlgorithm {
		private EvaluationFunction function;
		private Range searchRange;
		private uint dimensions;

		private double maxApproachFactor;
		private double obstacleFactor;

		Agent[] agents;
		public AgentCount {
			get {
				return agents.Length;
			}
			set {
				agents = new Agent[value];
			}
		};
		uint maxIterations;
		public IterationCount {
			get {
				return maxIterations;
			}
			set {
				maxIterations = value;
			}
		};

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
			this.dimensions = dimensions;
			agents = new Agent[agentCount];
			bestThreeAgents = new Agent[3];
			this.maxIterations = maxIterations;
			this.maxApproachFactor = maxApproachFactor;
			this.obstacleFactor = obstacleFactor;

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
				agents[i] = new Agent(searchRange.RandomInRange(random), searchRange);
			}

			currentBestAgent = null;
			currentBestValue = Double.PositiveInfinity;

			for(uint i = 0; i < this.maxIterations; i++) {
				// Calculate the approach factor
				double approachFactor = this.maxApproachFactor - (this.maxApproachFactor * i / this.maxIterations);

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
	}
}