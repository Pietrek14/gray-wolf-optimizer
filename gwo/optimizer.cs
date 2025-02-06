using System;

namespace GWO {
	class Optimizer {
		Agent[] agents;
		Agent[] bestThreeAgents;
		uint maxIterations;

		public Optimizer(uint agentCount = 12, uint maxIterations = 100) {
			if(agentCount < 3) {
				throw new ArgumentException(String.Format("Agent count has to be at least three (is {0})", agentCount));
			}

			agents = new Agent[agentCount];
			bestThreeAgents = new Agent[3];
			this.maxIterations = maxIterations;
		}

		private void CalculateAgentsFitness(IFunction function) {
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

		public Vector Optimize(IFunction function, uint dimensions, Range searchRange) {
			Random random = new Random(Guid.NewGuid().GetHashCode());

			// Spawn agents
			for(uint i = 0; i < agents.Length; i++) {
				agents[i] = new Agent(searchRange.RandomInRange(random), searchRange);
			}

			Vector currentBest = null;
			double currentBestValue = Double.PositiveInfinity;

			for(uint i = 0; i < this.maxIterations; i++) {
				// Calculate the approach factor
				double approachFactor = 2.0 - (2.0 * i / this.maxIterations);

				// Update agents
				CalculateAgentsFitness(function);

				// Save the current best
				if(currentBestValue > bestThreeAgents[0].Fitness) {
					currentBestValue = bestThreeAgents[0].Fitness;
					currentBest = bestThreeAgents[0].Position;
				}

				UpdateAgentsPosition(approachFactor, random);
			}

			// Return alpha's position
			return currentBest;
		}
	}
}