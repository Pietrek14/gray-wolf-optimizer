using System;

namespace Metaheuristics {
    class Tester<T, P>
        where P : ICustomParameters
        where T : ITestable<T, P>
    {
        EvaluationFunction function;
        Range domain;

        T optimizer;

        public Tester(
            T optimizer,
            EvaluationFunction function,
            Range domain
        ) {
            this.optimizer = optimizer;
            this.function = function;
            this.domain = domain;
        }

        public class TestCase {
            public uint DimensionCount;
            public uint IterationCount;
            public uint PopulationSize;
            public P CustomParameters;

            public TestCase(
                uint dimensionCount,
                uint iterationCount,
                uint populationSize,
                P customParameters
            ) {
                DimensionCount = dimensionCount;
                IterationCount = iterationCount;
                PopulationSize = populationSize;
                CustomParameters = customParameters;
            }

            public static TestCase[] Combinations(
                uint[] dimensionCounts,
                uint[] iterationCounts,
                uint[] populationSizes,
                P[] customParameters
            ) {
                var result = new TestCase[
                    dimensionCounts.Length * iterationCounts.Length
                    * populationSizes.Length * customParameters.Length
                ];
                uint index = 0;

                foreach(uint dimensionCount in dimensionCounts) {
                    foreach(uint iterationCount in iterationCounts) {
                        foreach(uint populationSize in populationSizes) {
                            foreach(P _customParameters in customParameters) {
                                result[index] = new TestCase(
                                    dimensionCount,
                                    iterationCount,
                                    populationSize,
                                    _customParameters
                                );
                                index++;
                            }
                        }
                    }
                }

                return result;
            }
        };

        public class TestResult {
            public string AlgorithmName;
            public string FunctionName;
            public TestCase TestCase;
            public uint EvaluationFunctionCalls;
            public Vector BestSolution;
            public Vector WorstSolution;
            public Vector SolutionStandardDeviation;
            public Vector SolutionVariancy;
            public double BestValue;
            public double WorstValue;
            public double ValueStandardDeviation;
            public double ValueVariancy;

            public TestResult(
                string algorithmName,
                string functionName,
                TestCase testCase,
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
                AlgorithmName = algorithmName;
                FunctionName = functionName;
                TestCase = testCase;
                EvaluationFunctionCalls = evaluationFunctionCalls;
                BestSolution = bestSolution;
                BestValue = bestValue;
                WorstSolution = worstSolution;
                WorstValue = worstValue;
                SolutionStandardDeviation = solutionStandardDeviation;
                ValueStandardDeviation = valueStandardDeviation;
                SolutionVariancy = solutionVariancy;
                ValueVariancy = valueVariancy;
            }
        };

        public TestResult[] RunTests(TestCase[] testCases, uint retryCount = 10) {
            TestResult[] result = new TestResult[testCases.Length];
            uint index = 0;

            foreach(TestCase testCase in testCases) {
                // Reconfigure the optimizer
                this.optimizer.Reconfigure(testCase);

                // Calculate the solutions and values
                Vector[] solutions = new Vector[retryCount];
                double[] values = new double[retryCount];
                uint worstIndex = 0, bestIndex = 0;

                // (Also keep track of the evaluation function calls)
                uint[] evaluationFunctionCalls = new uint[retryCount];

                // Simultaneously calculate their averages
                Vector solutionAverage = Vector.Zeros(testCase.DimensionCount);
                double valueAverage = 0;

                for(uint i = 0; i < retryCount; i++) {
                    values[i] = this.optimizer.Solve();
                    solutions[i] = new Vector(this.optimizer.XBest);

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
                Vector solutionStandardDeviation = Vector.Zeros(testCase.DimensionCount);
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
                result[index] = new TestResult(
                    this.optimizer.Name,
                    this.function.Name,
                    testCase,
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

            return result;
        }
    }
}