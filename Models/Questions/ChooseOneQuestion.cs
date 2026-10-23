using System;
using ExaminationSystem.Models.Answers;

namespace ExaminationSystem.Models.Questions
{
    public class ChooseOneQuestion : Question
    {
        // Parameterless constructor chained
        public ChooseOneQuestion() : this(string.Empty, string.Empty, 0m, new AnswerList(), new Answer())
        {
        }

        // Constructor accepting single correct answer chained
        public ChooseOneQuestion(string header, string body, decimal marks, AnswerList choices, Answer rightAnswer)
            : base(header, body, marks, choices, new AnswerList { rightAnswer })
        {
        }

        // Copy constructor chained
        public ChooseOneQuestion(ChooseOneQuestion other) : base(other)
        {
        }

        public override void DisplayQuestion()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"[Choose One Question] {Header} ({Marks} Marks)");
            Console.ResetColor();
            Console.WriteLine($"Question: {Body}");
            Console.WriteLine("Options (Pick one):");
            foreach (var choice in Choices)
            {
                Console.WriteLine($"   ( ) {choice.Id}. {choice.Text}");
            }
        }

        public override object Clone()
        {
            return new ChooseOneQuestion(this);
        }

        public override string ToString()
        {
            return $"[Type: Choose One] {base.ToString()}";
        }
    }
}
