using System;
using System.Collections.Generic;
using System.Linq;
using ExaminationSystem.Models.Answers;
using ExaminationSystem.Models.Enums;
using ExaminationSystem.Models.Events;
using ExaminationSystem.Models.Questions;
using ExaminationSystem.Models.Subjects;

namespace ExaminationSystem.Models.Exams
{
    // Delegate for exam started event
    public delegate void ExamStartedEventHandler(object sender, ExamEventArgs e);

    public abstract class Exam : ICloneable, IComparable, IComparable<Exam>
    {
        private ExamMode _mode;

        public int Time { get; set; } // Time in minutes
        public int NumberOfQuestions { get; set; }
        public QuestionList<Question> Questions { get; set; }
        
        // Dictionary mapping Question to its Right Answer(s) for correction
        public Dictionary<Question, AnswerList> QuestionAnswerDictionary { get; set; }
        
        // Dictionary tracking student answers during taking the exam
        public Dictionary<Question, AnswerList> StudentAnswersDictionary { get; set; }
        
        public Subject Subject { get; set; }
        public decimal TotalMarks => Questions?.Sum(q => q.Marks) ?? 0m;
        public decimal ObtainedMarks { get; protected set; }

        // Event triggered when exam mode changes to Starting
        public event ExamStartedEventHandler? ExamStarted;

        public ExamMode Mode
        {
            get => _mode;
            set
            {
                _mode = value;
                if (_mode == ExamMode.Starting)
                {
                    OnExamStarting();
                }
            }
        }

        // Parameterless constructor chained
        protected Exam() : this(0, 0, new Subject(), "Exam_Default.txt")
        {
        }

        // 3-param constructor chained
        protected Exam(int time, int numberOfQuestions, Subject subject)
            : this(time, numberOfQuestions, subject, $"{subject?.Name ?? "General"}_Questions.txt")
        {
        }

        // Full constructor
        protected Exam(int time, int numberOfQuestions, Subject subject, string logFileName)
        {
            Time = time;
            NumberOfQuestions = numberOfQuestions;
            Subject = subject ?? new Subject();
            Questions = new QuestionList<Question>(logFileName);
            QuestionAnswerDictionary = new Dictionary<Question, AnswerList>();
            StudentAnswersDictionary = new Dictionary<Question, AnswerList>();
            _mode = ExamMode.Queued;

            // Automatically subscribe enrolled subject students to exam notifications
            SubscribeSubjectStudents();
        }

        // Copy constructor chained
        protected Exam(Exam other)
            : this(other?.Time ?? 0, other?.NumberOfQuestions ?? 0, other?.Subject != null ? (Subject)other.Subject.Clone() : new Subject(), other?.Questions?.LogFilePath ?? "Cloned_Exam.txt")
        {
            if (other != null)
            {
                foreach (var q in other.Questions)
                {
                    AddQuestion((Question)q.Clone());
                }
            }
        }

        public void SubscribeSubjectStudents()
        {
            if (Subject?.Students != null)
            {
                foreach (var student in Subject.Students)
                {
                    ExamStarted -= student.OnExamStarted;
                    ExamStarted += student.OnExamStarted;
                }
            }
        }

        public void AddQuestion(Question question)
        {
            if (question == null) throw new ArgumentNullException(nameof(question));

            Questions.Add(question);
            QuestionAnswerDictionary[question] = question.RightAnswers;
            NumberOfQuestions = Questions.Count;
        }

        protected virtual void OnExamStarting()
        {
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"[Exam State Update] Exam mode changed to STARTING for subject: {Subject?.Name}");
            Console.ResetColor();

            var eventArgs = new ExamEventArgs(
                subjectName: Subject?.Name ?? "Unknown Subject",
                examType: this.GetType().Name,
                durationMinutes: Time,
                numberOfQuestions: Questions.Count,
                startTime: DateTime.Now
            );

            // Notify all subscribed students via delegate/event
            ExamStarted?.Invoke(this, eventArgs);
            Console.WriteLine();
        }

        // Abstract method deferred to subclasses
        public abstract void ShowExam();

        /// <summary>
        /// Conducts taking the exam by the student, collecting answers, and grading.
        /// </summary>
        protected void ConductExamTaking()
        {
            // Set mode to Starting to trigger notifications
            Mode = ExamMode.Starting;

            Console.WriteLine("=====================================================================");
            Console.WriteLine($"Starting {GetType().Name} for Subject: {Subject.Name}");
            Console.WriteLine($"Total Questions: {Questions.Count} | Duration: {Time} Minutes | Total Marks: {TotalMarks}");
            Console.WriteLine("=====================================================================");
            Console.WriteLine("Press any key to start answering...");
            try
            {
                if (!Console.IsInputRedirected)
                {
                    Console.ReadKey(true);
                }
            }
            catch { }
            Console.WriteLine();

            ObtainedMarks = 0m;
            StudentAnswersDictionary.Clear();

            for (int i = 0; i < Questions.Count; i++)
            {
                var question = Questions[i];
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine($"---------------- Question {i + 1} of {Questions.Count} ----------------");
                Console.ResetColor();

                // Display question with its specific representation
                question.DisplayQuestion();

                // Collect answers based on question type
                var studentAnswer = PromptForAnswer(question);
                StudentAnswersDictionary[question] = studentAnswer;

                // Validate answer using the correction dictionary
                if (QuestionAnswerDictionary.TryGetValue(question, out var correctAnswers))
                {
                    if (correctAnswers.Equals(studentAnswer))
                    {
                        ObtainedMarks += question.Marks;
                    }
                }

                Console.WriteLine();
            }

            // Mark exam as finished
            Mode = ExamMode.Finished;
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("[Exam State Update] Exam mode changed to FINISHED.");
            Console.ResetColor();
            Console.WriteLine();
        }

        private AnswerList PromptForAnswer(Question question)
        {
            var studentAnswers = new AnswerList();

            while (true)
            {
                if (question is TrueFalseQuestion)
                {
                    Console.Write("Enter your choice (1 for True, 2 for False): ");
                    string? input = Console.ReadLine()?.Trim();
                    if (input == "1" || input == "2")
                    {
                        int id = int.Parse(input);
                        string text = id == 1 ? "True" : "False";
                        studentAnswers.Add(new Answer(id, text));
                        break;
                    }
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Invalid choice! Please enter 1 or 2.");
                    Console.ResetColor();
                }
                else if (question is ChooseOneQuestion)
                {
                    Console.Write($"Enter your choice (1-{question.Choices.Count}): ");
                    string? input = Console.ReadLine()?.Trim();
                    if (int.TryParse(input, out int choiceId))
                    {
                        var matchingChoice = question.Choices.FirstOrDefault(c => c.Id == choiceId);
                        if (matchingChoice != null)
                        {
                            studentAnswers.Add(new Answer(matchingChoice.Id, matchingChoice.Text));
                            break;
                        }
                    }
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Invalid choice! Please choose a number between 1 and {question.Choices.Count}.");
                    Console.ResetColor();
                }
                else if (question is ChooseAllQuestion)
                {
                    Console.Write("Enter your choices separated by commas or spaces (e.g., 1, 3): ");
                    string? input = Console.ReadLine();
                    if (!string.IsNullOrWhiteSpace(input))
                    {
                        var tokens = input.Split(new[] { ',', ' ', ';' }, StringSplitOptions.RemoveEmptyEntries);
                        bool allValid = true;
                        var selectedAnswers = new AnswerList();

                        foreach (var token in tokens)
                        {
                            if (int.TryParse(token, out int choiceId))
                            {
                                var matchingChoice = question.Choices.FirstOrDefault(c => c.Id == choiceId);
                                if (matchingChoice != null && !selectedAnswers.Any(a => a.Id == choiceId))
                                {
                                    selectedAnswers.Add(new Answer(matchingChoice.Id, matchingChoice.Text));
                                }
                                else if (matchingChoice == null)
                                {
                                    allValid = false;
                                    break;
                                }
                            }
                            else
                            {
                                allValid = false;
                                break;
                            }
                        }

                        if (allValid && selectedAnswers.Count > 0)
                        {
                            studentAnswers = selectedAnswers;
                            break;
                        }
                    }
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Invalid selection! Please enter valid option numbers from the list.");
                    Console.ResetColor();
                }
            }

            return studentAnswers;
        }

        public abstract object Clone();

        public virtual int CompareTo(object? obj)
        {
            if (obj is null) return 1;
            if (obj is Exam other)
            {
                return CompareTo(other);
            }
            throw new ArgumentException("Object is not an Exam");
        }

        public virtual int CompareTo(Exam? other)
        {
            if (other is null) return 1;
            return Time.CompareTo(other.Time);
        }

        public override bool Equals(object? obj)
        {
            if (obj is not Exam other) return false;
            return Time == other.Time &&
                   NumberOfQuestions == other.NumberOfQuestions &&
                   Equals(Subject, other.Subject);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Time, NumberOfQuestions, Subject?.GetHashCode() ?? 0);
        }

        public override string ToString()
        {
            return $"Exam: {GetType().Name} | Subject: {Subject?.Name} | Duration: {Time} mins | Questions: {Questions.Count} | Total Marks: {TotalMarks}";
        }
    }
}
