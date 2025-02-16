using System;

namespace GWO {
    class Tester {
        EvaluationFunction function;
        Range domain;
        uint dimensionCount;
        uint[] populationSizes;
        uint[] iterationCounts;

        Optimizer optimizer;

        public Tester(
            EvaluationFunction function,
            Range domain,
            uint dimensionCount,
            uint[] populationSizes,
            uint[] iterationCounts
        ) {
            this.function = function;
            this.domain = domain;
            this.dimensionCount = dimensionCount;
            this.populationSizes = populationSizes;
            this.iterationCounts = iterationCounts;

            this.optimizer = new Optimizer(function, domain, dimensionCount);
        }

        public class TestCase {
            public string algorithmName;
            public string functionName;
            public uint dimensionCount;
            public double maxApproachFactor;
            public double obstacleFactor; 
            public uint iterationCount;
            public uint populationSize;
            public uint evaluationFunctionCalls;
            public Vector bestSolution;
            public Vector worstSolution;
            public Vector solutionStandardDeviation;
            public Vector solutionVariancy;
            public double bestValue;
            public double worstValue;
            public double valueStandardDeviation;
            public double valueVariancy;

            public TestCase(
                string algorithmName,
                string functionName,
                uint dimensionCount,
                double maxApproachFactor,
                double obstacleFactor,
                uint iterationCount,
                uint populationSize,
                uint evaluationFunctionCalls,
                Vector bestSolution,
                Vector worstSolution,
                Vector solutionStandardDeviation,
                Vector solutionVariancy,
                double bestValue,
                double valueStandardDeviation,
                double worstValue,
                double valueVariancy
            ) {
                this.algorithmName = algorithmName;
                this.functionName = functionName;
                this.dimensionCount = dimensionCount;
                this.maxApproachFactor = maxApproachFactor;
                this.obstacleFactor = obstacleFactor;
                this.iterationCount = iterationCount;
                this.populationSize = populationSize;
                this.evaluationFunctionCalls = evaluationFunctionCalls;
                this.bestSolution = bestSolution;
                this.bestValue = bestValue;
                this.worstSolution = worstSolution;
                this.worstValue = worstValue;
                this.solutionStandardDeviation = solutionStandardDeviation;
                this.valueStandardDeviation = valueStandardDeviation;
                this.solutionVariancy = solutionVariancy;
                this.valueVariancy = valueVariancy;
            }
        };

        public TestCase[] RunTests(uint retryCount = 10) {
            TestCase[] result = new TestCase[this.populationSizes.Length * this.iterationCounts.Length];
            uint index = 0;

            foreach(uint populationSize in populationSizes) {
                this.optimizer.AgentCount = populationSize;
                foreach(uint iterationCount in iterationCounts) {
                    this.optimizer.IterationCount = iterationCount;

                    // Calculate the solutions and values
                    Vector[] solutions = new Vector[retryCount];
                    double[] values = new double[retryCount];
                    uint worstIndex = 0, bestIndex = 0;

                    // (Also keep track of the evaluation function calls)
                    uint[] evaluationFunctionCalls = new uint[retryCount];

                    // Simultaneously calculate their averages
                    Vector solutionAverage = Vector.Zeros(this.dimensionCount);
                    double valueAverage = 0;

                    for(uint i = 0; i < retryCount; i++) {
                        values[i] = this.optimizer.Solve();
                        solutions[i] = this.optimizer.BestAgent;

                        evaluationFunctionCalls[i] = (uint)this.optimizer.NumberOfEvaluationFitnessFunction;

                        valueAverage += values[i];
                        solutionAverage += solutions[i];

                        if(values[i] < values[bestIndex]) {
                            bestIndex = i;
                        }

                        if(values[i] > values[worstIndex]) {
                            worstIndex = i;
                        }

                        this.optimizer.ResetEvaluationCallsCounter();
                    }

                    solutionAverage /= retryCount;
                    valueAverage /= retryCount;

                    // Calculate the standard deviation
                    Vector solutionStandardDeviation = Vector.Zeros(this.dimensionCount);
                    double valueStandardDeviation = 0;

                    for(uint i = 0; i < retryCount; i++) {
                        solutionStandardDeviation += (solutions[i] - solutionAverage) * (solutions[i] - solutionAverage);
                        valueStandardDeviation += Math.Pow(values[i] - valueAverage, 2);
                    }

                    solutionStandardDeviation /= retryCount;
                    valueStandardDeviation /= retryCount;

                    solutionStandardDeviation = solutionStandardDeviation.Sqrt();
                    valueStandardDeviation = Math.Sqrt(valueStandardDeviation);

                    // Calculate variancy
                    Vector solutionVariancy = solutionStandardDeviation / solutionAverage;
                    double valueVariancy = valueStandardDeviation / valueAverage;

                    // Save the results
                    result[index] = new TestCase(
                        this.optimizer.Name,
                        this.function.Name,
                        this.dimensionCount,
                        this.optimizer.MaxApproachFactor,
                        this.optimizer.ObstacleFactor,
                        iterationCount,
                        populationSize,
                        evaluationFunctionCalls[bestIndex],
                        solutions[bestIndex],
                        solutions[worstIndex],
                        solutionStandardDeviation,
                        solutionVariancy,
                        values[bestIndex],
                        values[worstIndex],
                        valueStandardDeviation,
                        valueVariancy
                    );
                    index++;
                }
            }

            return result;
        }
    }
}