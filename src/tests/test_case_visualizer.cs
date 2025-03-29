using System;

namespace Metaheuristics {
    abstract class TestResultOutputter<T, P>
        where P : ICustomParameters
        where T : ITestable<T, P>
    {
        abstract public void Output(Tester<T, P>.TestResult[] TestResults);

        /// Use these instead of raw output to console.
        /// In the future there will be an option to output to a file,
        /// which this method will handle.
        protected void Out(string line) {
            Console.Write(line);
        }

        protected void OutLine(string line) {
            this.Out(line + '\n');
        }
    };

    class LatexTestResultOutputter<T, P> : TestResultOutputter<T, P>
        where P : ICustomParameters
        where T : ITestable<T, P>
    {
        private bool headers;

        public LatexTestResultOutputter(bool headers = true) {
            this.headers = headers;
        }

        override public void Output(Tester<T, P>.TestResult[] testResults) {
            this.OutLine(@"\begin{table}[H]");
            this.OutLine(@"\begin{tabular}{[|c|c|c|c|c|c|c|c|c|c|c|]}");
            this.OutLine(@"\hline");

            if(this.headers) {
                this.OutLine(
                    @"Algorytm & Funkcja testowa & Liczba szukanych parametrów &" + 
                    string.Join(" & ", testResults[0].TestCase.CustomParameters.ParameterNames)
                    + @"& Liczba iteracji & Rozmiar populacji & Znalezione minimum
& Odchylenie standardowe poszukiwanych parametrów & Wartość funkcji celu
& Odchylenie standardowe wartości funkcji celu & Liczba wywołań funkcji celu \\"
                );
                this.OutLine(@"\hline");
            }

            foreach(var testResult in testResults) {
                this.OutLine(
                    String.Format(
                        @"{0} & {1} & {2} & {3} & {4} & {5} & {6} & {7} & {8} & {9} & {10} \\",
                        testResult.AlgorithmName, testResult.FunctionName, testResult.TestCase.DimensionCount,
                        string.Join(" & ", testResult.TestCase.CustomParameters.ParameterValues),
                        testResult.TestCase.IterationCount, testResult.TestCase.PopulationSize,
                        testResult.BestSolution.ToPrettyString(), testResult.SolutionStandardDeviation.ToPrettyString(),
                        testResult.BestValue, testResult.ValueStandardDeviation,
                        testResult.EvaluationFunctionCalls
                    )
                );
            }

            this.OutLine(@"\hline");
            this.OutLine(@"\end{tabular}");
            this.OutLine(@"\end{table}");
        }
    }

    class CSVTestResultOutputter<T, P> : TestResultOutputter<T, P>
        where P : ICustomParameters
        where T : ITestable<T, P>
    {
        private bool headers;

        public CSVTestResultOutputter(bool headers = true) {
            this.headers = headers;
        }

        override public void Output(Tester<T, P>.TestResult[] testResults) {
            if(this.headers) {
                this.OutLine(
                    "Algorytm;Funkcja testowa;Liczba szukanych parametrów;"
                    + string.Join(";", testResults[0].TestCase.CustomParameters.ParameterNames)
                    + ";Liczba iteracji;Rozmiar populacji;Znalezione minimum;"
                    + "Odchylenie standardowe poszukiwanych parametrów;Wartość funkcji celu;"
                    + "Odchylenie standardowe wartości funkcji celu;Liczba wywołań funkcji celu"
                );
            }

            foreach(var testResult in testResults) {
                this.OutLine(
                    String.Format(
                        "{0};{1};{2};{3};{4};{5};{6};{7};{8};{9};{10}",
                        testResult.AlgorithmName, testResult.FunctionName, testResult.TestCase.DimensionCount,
                        string.Join(";", testResult.TestCase.CustomParameters.ParameterValues),
                        testResult.TestCase.IterationCount, testResult.TestCase.PopulationSize,
                        testResult.BestSolution.ToPrettyString(), testResult.SolutionStandardDeviation.ToPrettyString(),
                        testResult.BestValue, testResult.ValueStandardDeviation,
                        testResult.EvaluationFunctionCalls
                    )
                );
            }
        }
    }
}