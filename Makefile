SRC_FILES = main.cs gwo\optimizer.cs gwo\agent.cs gwo\vector.cs gwo\function.cs gwo\range.cs metaheuristics\metaheuristics.cs
SRC_DIR = .\src
TARGET_FILE = main.exe

build:
	csc -out:$(TARGET_FILE) $(patsubst %.cs,$(SRC_DIR)\\%.cs,$(SRC_FILES))

run:
	./$(TARGET_FILE)