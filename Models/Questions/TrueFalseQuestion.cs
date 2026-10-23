using System;
using ExaminationSystem.Models.Answers;

namespace ExaminationSystem.Models.Questions
{
    public class TrueFalseQuestion : Question
    {
        // Parameterless constructor chained
        public TrueFalseQuestion() : this(string.Empty, string.Empty, 0m, 1)
        {
        }

        // Main constructor for True/False question: correctAnswerId: 1 for True, 2 for False
        public TrueFalseQuestion(string header, string body, decimal marks, int correctAnswerId)
            : base(
                header,
                body,
                marks,
                new AnswerList { new Answer(1, "True"), new Answer(2, "False") },
                new AnswerList { new Answer(correctAnswerId, correctAnswerId == 1 ? "True" : "False") }
            )
        {
        }

        // Copy constructor chained
        public TrueFalseQuestion(TrueFalseQuestion other) : base(other)
        {
        }

        public override void DisplayQuestion()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"[True or False Question] {Header} ({Marks} Marks)");
            Console.ResetColor();
            Console.WriteLine($"Question: {Body}");
            Console.WriteLine("Options:");
            foreach (var choice in Choices)
            {
                Console.WriteLine($"   [{choice.Id}] {choice.Text}");
            }
        }

        public override object Clone()
        {
            return new TrueFalseQuestion(this);
        }

        public override string ToString()
        {
            return $"[Type: True/False] {base.ToString()}";
        }
    }
}
