using System;
using System.Linq;
using ExaminationSystem.Models.Questions;
using ExaminationSystem.Models.Subjects;

namespace ExaminationSystem.Models.Exams
{
    public class FinalExam : Exam
    {
        // Parameterless constructor chained
        public FinalExam() : this(0, 0, new Subject())
        {
        }

        // 3-param constructor chained to base
        public FinalExam(int time, int numberOfQuestions, Subject subject)
            : base(time, numberOfQuestions, subject, $"{subject?.Name ?? "Final"}_FinalExam_Questions.txt")
        {
        }

        // 4-param constructor chained to base
        public FinalExam(int time, int numberOfQuestions, Subject subject, string logFileName)
            : base(time, numberOfQuestions, subject, logFileName)
        {
        }

        // Copy constructor chained
        public FinalExam(FinalExam other) : base(other)
        {
        }

        public override void ShowExam()
        {
            // First: conduct the exam taking process
            ConductExamTaking();

            // After finishing: Final Exam ONLY shows the Question and Student's Answers (Does NOT reveal Right Answers)
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("=====================================================================");
            Console.WriteLine("                     FINAL EXAM - SUBMISSION SUMMARY                 ");
            Console.WriteLine("=====================================================================");
            Console.ResetColor();

            for (int i = 0; i < Questions.Count; i++)
            {
                var question = Questions[i];
                var studentAnswer = StudentAnswersDictionary.ContainsKey(question) ? StudentAnswersDictionary[question] : null;

                Console.WriteLine($"\nQ{i + 1}: {question.Body} ({question.Marks} Marks)");
                Console.WriteLine("Choices:");
                foreach (var choice in question.Choices)
                {
                    Console.WriteLine($"   - {choice.Id}. {choice.Text}");
                }

                Console.Write("Your Submitted Answer: ");
                if (studentAnswer != null && studentAnswer.Count > 0)
                {
                    Console.WriteLine(string.Join(", ", studentAnswer.Select(a => $"{a.Id}. {a.Text}")));
                }
                else
                {
                    Console.WriteLine("No Answer Submitted");
                }
            }

            Console.WriteLine("\n---------------------------------------------------------------------");
            Console.ForegroundColor = ConsoleColor.Yellow;
            decimal percentage = TotalMarks > 0 ? (ObtainedMarks / TotalMarks) * 100m : 0m;
            Console.WriteLine($"YOUR FINAL SCORE: {ObtainedMarks} / {TotalMarks} ({percentage:F1}%)");
            Console.WriteLine("Note: Correct answers are withheld according to final exam policies.");
            Console.ResetColor();
            Console.WriteLine("=====================================================================\n");
        }

        public override object Clone()
        {
            return new FinalExam(this);
        }

        public override string ToString()
        {
            return $"[Final Exam] {base.ToString()}";
        }
    }
}
