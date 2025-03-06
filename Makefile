SRC_FILES = main.cs gwo\optimizer.cs gwo\agent.cs  metaheuristics\metaheuristics.cs metaheuristics\vector.cs metaheuristics\range.cs metaheuristics\function.cs tests\tester.cs tests\test_case_visualizer.cs
SRC_DIR = .\src
TARGET_FILE = main.exe

build:
	print aaa
	csc -out:$(TARGET_FILE) $(patsubst %.cs,$(SRC_DIR)\\%.cs,$(SRC_FILES))

run:
	./$(TARGET_FILE)