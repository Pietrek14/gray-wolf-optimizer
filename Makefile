SRC_FILES = main.cs .\gwo\optimizer.cs .\gwo\agent.cs .\gwo\vector.cs .\gwo\function.cs .\gwo\range.cs
TARGET_FILE = main.exe

build:
	csc -out:$(TARGET_FILE) $(SRC_FILES)

run:
	./$(TARGET_FILE)