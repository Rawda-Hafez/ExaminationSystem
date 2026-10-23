using System;
using ExaminationSystem.Models.Answers;

namespace ExaminationSystem.Models.Questions
{
    public class ChooseAllQuestion : Question
    {
        // Parameterless constructor chained
        public ChooseAllQuestion() : this(string.Empty, string.Empty, 0m, new AnswerList(), new AnswerList())
        {
        }

        // Full constructor chained to base
        public ChooseAllQuestion(string header, string body, decimal marks, AnswerList choices, AnswerList rightAnswers)
            : base(header, body, marks, choices, rightAnswers)
        {
        }

        // Copy constructor chained
        public ChooseAllQuestion(ChooseAllQuestion other) : base(other)
        {
        }

        public override void DisplayQuestion()
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine($"[Choose All Question] {Header} ({Marks} Marks)");
            Console.ResetColor();
            Console.WriteLine($"Question: {Body}");
            Console.WriteLine("Options (Choose all correct options separated by commas or spaces, e.g., 1, 3):");
            foreach (var choice in Choices)
            {
                Console.WriteLine($"   [ ] {choice.Id}. {choice.Text}");
            }
        }

        public override object Clone()
        {
            return new ChooseAllQuestion(this);
        }

        public override string ToString()
        {
            return $"[Type: Choose All] {base.ToString()}";
        }
    }
}
