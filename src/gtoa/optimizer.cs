using System;
using Metaheuristics;

namespace GTOA {
	class Params : ICustomParameters {
		/// Should be either 1 or 2
		public double TeachingFactor;

		public Params(double teachingFactor) {
			TeachingFactor = teachingFactor;
		}

		public static Params[] Combinations(
			double[] teachingFactors
		) {
			Params[] result = new Params[teachingFactors.Length];
			uint index = 0;

			foreach(double teachingFactor in teachingFactors) {
				result[index] = new Params(teachingFactor);

				index++;
			}

			return result;
		}

		public string[] ParameterNames { get {
			return new string[] { "Współczynnik uczenia" };
		}}

		public string[] ParameterValues { get {
			return new string[] {
				TeachingFactor.ToString()
			};
		}}
	}

	class Agent {
		public Vector Position;
		public double Fitness;

		public Agent(Vector position) {
			this.Position = position;
			this.Fitness = Double.PositiveInfinity;
		}

		public void Evaluate(EvaluationFunction function) {
			Fitness = function.Evaluate(Position);
		}

		public static Agent Better(Agent agent1, Agent agent2) {
			if(agent1.Fitness <= agent2.Fitness) {
				return agent1;
			} else {
				return agent2;
			}
		}

		public static int Compare(Agent agent1, Agent agent2) {
			if(agent1.Fitness == agent2.Fitness) {
				return 0;
			} else if(agent1.Fitness > agent2.Fitness) {
				return 1;
			} else {
				return -1;
			}
		}
	}

	class Optimizer : ITestable<Optimizer, Params> {
		private EvaluationFunction function;
		private Range searchRange;
		private Random random;

		private Agent currentBestAgent;

		public uint DimensionCount;
		Agent[] students;
		public uint StudentCount {
			get {
				return (uint)students.Length;
			}
			set {
				students = new Agent[value];
			}
		}
		public uint IterationCount;
		public Params CustomParameters;

		public string Name {
			get {
				return "Group Teaching Optimizer";
			}
			set {}
		}

		public Optimizer(
			EvaluationFunction function,
			Range searchRange,
			uint dimensions,
			uint studentCount = 12,
			uint maxIterations = 200,
			double teachingFactor = 1
		) {
			if(studentCount < 3) {
				throw new ArgumentException(String.Format("Agent count has to be at least three (is {0})", studentCount));
			}

			this.function = function;
			this.searchRange = searchRange;
			this.DimensionCount = dimensions;
			this.StudentCount = studentCount;
			this.IterationCount = maxIterations;
			this.CustomParameters = new Params(teachingFactor);

			this.random = new Random(Guid.NewGuid().GetHashCode());
		}

		private void EvaluateStudents() {
			Agent[] newStudents = new Agent[StudentCount];
			uint index = 0;

			foreach(Agent student in students) {
				student.Evaluate(this.function);

				// Automatically sort the new student array
				bool inserted = false;

				for(uint i = 0; i < index; i++) {
					if(student.Fitness < newStudents[i].Fitness) {
						for(uint j = index; j > i; j--) {
							newStudents[j] = newStudents[j - 1];
						}

						newStudents[i] = student;

						inserted = true;
						break;
					}
				}

				if(!inserted) {
					newStudents[index] = student;
				}

				index++;
			}

			students = newStudents;
		}

		private Agent SelectTeacher() {
			Agent teacherCandidate1 = students[StudentCount - 1];

			Vector teacherCandidate2Position = Vector.Zeros(DimensionCount);

			for(uint i = 0; i < 3; i++) {
				teacherCandidate2Position += students[StudentCount - 1 - i].Position;
			}

			teacherCandidate2Position /= 3;

			Agent teacherCandidate2 = new Agent(teacherCandidate2Position);
			teacherCandidate2.Evaluate(this.function);

			return Agent.Better(teacherCandidate1, teacherCandidate2);
		}

		private void Study(Agent teacher) {
			Agent[] prevStudents = students;

			// Teacher phase

			// Average group
			double averageLearningFactor = random.NextDouble();

			for(uint i = 0; i < StudentCount / 2; i++) {
				Agent newStudent = new Agent(
					students[i].Position + 2 * averageLearningFactor * (teacher.Position - students[i].Position)
				);

				newStudent.Evaluate(this.function);

				students[i] = Agent.Better(students[i], newStudent);
			}

			// Outstanding group
			Vector mean = Vector.Zeros(DimensionCount);

			for(uint i = StudentCount / 2; i < StudentCount; i++) {
				mean += students[i].Position;
			}

			// StudentCount may be odd
			mean /= StudentCount - (StudentCount / 2);

			double outstandingLearningFactor = random.NextDouble();
			double conformism = random.NextDouble();

			for(uint i = StudentCount / 2; i < StudentCount; i++) {
				Agent newStudent = new Agent(
					students[i].Position
					+ outstandingLearningFactor * (
						teacher.Position - CustomParameters.TeachingFactor * (
							conformism * mean
							+ (1 - conformism) * students[i].Position
						)
					)
				);

				newStudent.Evaluate(this.function);

				students[i] = Agent.Better(students[i], newStudent);
			}
	
			// Student phase
			double unsignedColearningFactor = random.NextDouble();
			double inertiaFactor = random.NextDouble();

			for(uint i = 0; i < StudentCount; i++) {
				uint classmateIndex = (uint)random.Next(0, (int)(StudentCount - 1));
				if(classmateIndex >= i) {
					classmateIndex++;
				}
				Agent classmate = students[classmateIndex];

				double colearningFactor = unsignedColearningFactor
					* (students[i].Fitness < classmate.Fitness ? 1 : -1);

				Agent newStudent = new Agent(
					students[i].Position
						+ colearningFactor * (students[i].Position - classmate.Position)
						+ inertiaFactor * (students[i].Position - prevStudents[i].Position)
				);

				newStudent.Evaluate(this.function);

				students[i] = Agent.Better(students[i], newStudent);
			}
		}

		public double Solve() {
			// Spawn agents
			for(uint i = 0; i < StudentCount; i++) {
				students[i] = new Agent(searchRange.RandomInRange(random));
			}

			EvaluateStudents();

			for(uint i = 0; i < this.IterationCount; i++) {
				Agent teacher = SelectTeacher();

				Study(teacher);

				// The students are already evaluated after studying
				// So it's enough to only sort them
				Array.Sort<Agent>(students, Agent.Compare);
				currentBestAgent = students[StudentCount - 1];
			}

			// Return the current best 
			return currentBestAgent.Fitness;
		}

		public Agent BestAgent {
			get {
				return currentBestAgent;
			}
			set {}
		}

		public double[] XBest {
			get {
				return BestAgent.Position.vals;
			}
			set {}
		}

		public double FBest {
			get {
				return BestAgent.Fitness;
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
			this.StudentCount = testCase.PopulationSize;
			this.IterationCount = testCase.IterationCount;
			this.CustomParameters = testCase.CustomParameters;
		}
	}
}