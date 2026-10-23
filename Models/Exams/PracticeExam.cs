using System;
using System.Linq;
using ExaminationSystem.Models.Questions;
using ExaminationSystem.Models.Subjects;

namespace ExaminationSystem.Models.Exams
{
    public class PracticeExam : Exam
    {
        // Parameterless constructor chained
        public PracticeExam() : this(0, 0, new Subject())
        {
        }

        // 3-param constructor chained to base
        public PracticeExam(int time, int numberOfQuestions, Subject subject)
            : base(time, numberOfQuestions, subject, $"{subject?.Name ?? "Practice"}_PracticeExam_Questions.txt")
        {
        }

        // 4-param constructor chained to base
        public PracticeExam(int time, int numberOfQuestions, Subject subject, string logFileName)
            : base(time, numberOfQuestions, subject, logFileName)
        {
        }

        // Copy constructor chained
        public PracticeExam(PracticeExam other) : base(other)
        {
        }

        public override void ShowExam()
        {
            // First: conduct the exam taking process
            ConductExamTaking();

            // After finishing: Practice Exam shows Question, Student's Answers, and the RIGHT ANSWERS
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("=====================================================================");
            Console.WriteLine("                   PRACTICE EXAM - DETAILED REVIEW                   ");
            Console.WriteLine("=====================================================================");
            Console.ResetColor();

            for (int i = 0; i < Questions.Count; i++)
            {
                var question = Questions[i];
                var studentAnswer = StudentAnswersDictionary.ContainsKey(question) ? StudentAnswersDictionary[question] : null;
                var rightAnswer = QuestionAnswerDictionary.ContainsKey(question) ? QuestionAnswerDictionary[question] : null;
                bool isCorrect = rightAnswer != null && rightAnswer.Equals(studentAnswer);

                Console.WriteLine($"\nQ{i + 1}: {question.Body} ({question.Marks} Marks)");
                Console.WriteLine("Available Choices:");
                foreach (var choice in question.Choices)
                {
                    Console.WriteLine($"   - {choice.Id}. {choice.Text}");
                }

                Console.Write("Your Answer : ");
                if (studentAnswer != null && studentAnswer.Count > 0)
                {
                    Console.WriteLine(string.Join(", ", studentAnswer.Select(a => $"{a.Id}. {a.Text}")));
                }
                else
                {
                    Console.WriteLine("No Answer");
                }

                // Show the right answer (special for Practice Exam)
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("Right Answer: ");
                if (rightAnswer != null && rightAnswer.Count > 0)
                {
                    Console.WriteLine(string.Join(", ", rightAnswer.Select(a => $"{a.Id}. {a.Text}")));
                }
                else
                {
                    Console.WriteLine("None");
                }
                Console.ResetColor();

                if (isCorrect)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"Result      : CORRECT (+{question.Marks} Marks)");
                    Console.ResetColor();
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Result      : INCORRECT (0 / {question.Marks} Marks)");
                    Console.ResetColor();
                }
            }

            Console.WriteLine("\n---------------------------------------------------------------------");
            Console.ForegroundColor = ConsoleColor.Yellow;
            decimal percentage = TotalMarks > 0 ? (ObtainedMarks / TotalMarks) * 100m : 0m;
            Console.WriteLine($"FINAL SCORE: {ObtainedMarks} / {TotalMarks} ({percentage:F1}%)");
            Console.ResetColor();
            Console.WriteLine("=====================================================================\n");
        }

        public override object Clone()
        {
            return new PracticeExam(this);
        }

        public override string ToString()
        {
            return $"[Practice Exam] {base.ToString()}";
        }
    }
}
