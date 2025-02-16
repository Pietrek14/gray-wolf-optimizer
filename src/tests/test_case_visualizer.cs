using System;

namespace GWO {
    abstract class TestCaseOutputter {
        abstract public void Output(Tester.TestCase[] testCases);

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

    class LatexTestCaseOutputter : TestCaseOutputter {
        private bool headers;

        public LatexTestCaseOutputter(bool headers = true) {
            this.headers = headers;
        }

        override public void Output(Tester.TestCase[] testCases) {
            this.OutLine(@"\begin{table}[H]");
            this.OutLine(@"\begin{tabular}{[|c|c|c|c|c|c|c|c|c|c|c|]}");
            this.OutLine(@"\hline");

            if(this.headers) {
                this.OutLine(
                    @"Algorytm & Funkcja testowa & Liczba szukanych parametrów & Współczynnik podejścia
& Współczynnik przeszkody & Liczba iteracji & Rozmiar populacji & Znalezione minimum
& Odchylenie standardowe poszukiwanych parametrów & Wartość funkcji celu
& Odchylenie standardowe wartości funkcji celu & Liczba wywołań funkcji celu \\"
                );
                this.OutLine(@"\hline");
            }

            foreach(var testCase in testCases) {
                this.OutLine(
                    String.Format(
                        @"{0} & {1} & {2} & {3} & {4} & {5} & {6} & {7} & {8} & {9} & {10} & {11} \\",
                        testCase.algorithmName, testCase.functionName, testCase.dimensionCount,
                        testCase.maxApproachFactor, testCase.obstacleFactor,
                        testCase.iterationCount, testCase.populationSize,
                        testCase.bestSolution.ToPrettyString(), testCase.solutionStandardDeviation.ToPrettyString(),
                        testCase.bestValue, testCase.valueStandardDeviation,
                        testCase.evaluationFunctionCalls
                    )
                );
            }

            this.OutLine(@"\hline");
            this.OutLine(@"\end{tabular}");
            this.OutLine(@"\end{table}");
        }
    }

    class CSVTestCaseOutputter : TestCaseOutputter {
        private bool headers;

        public CSVTestCaseOutputter(bool headers = true) {
            this.headers = headers;
        }

        override public void Output(Tester.TestCase[] testCases) {
            if(this.headers) {
                this.OutLine(
                    "Algorytm;Funkcja testowa;Liczba szukanych parametrów;Współczynnik podejścia;Współczynnik przeszkody;"
                    + "Liczba iteracji;Rozmiar populacji;Znalezione minimum;"
                    + "Odchylenie standardowe poszukiwanych parametrów;Wartość funkcji celu;"
                    + "Odchylenie standardowe wartości funkcji celu;Liczba wywołań funkcji celu"
                );
            }

            foreach(var testCase in testCases) {
                this.OutLine(
                    String.Format(
                        "{0};{1};{2};{3};{4};{5};{6};{7};{8};{9};{10};{11}",
                        testCase.algorithmName, testCase.functionName, testCase.dimensionCount,
                        testCase.maxApproachFactor, testCase.obstacleFactor,
                        testCase.iterationCount, testCase.populationSize,
                        testCase.bestSolution.ToPrettyString(), testCase.solutionStandardDeviation.ToPrettyString(),
                        testCase.bestValue, testCase.valueStandardDeviation,
                        testCase.evaluationFunctionCalls
                    )
                );
            }
        }
    }
}