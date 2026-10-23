using System;
using System.Text;
using ExaminationSystem.Models.Answers;

namespace ExaminationSystem.Models.Questions
{
    public abstract class Question : ICloneable, IComparable, IComparable<Question>
    {
        public string Header { get; set; }
        public string Body { get; set; }
        public decimal Marks { get; set; }
        public AnswerList Choices { get; set; }
        public AnswerList RightAnswers { get; set; }

        // Parameterless constructor chaining to basic constructor
        protected Question() : this(string.Empty, string.Empty, 0m)
        {
        }

        // 3-param constructor chaining to full constructor
        protected Question(string header, string body, decimal marks) 
            : this(header, body, marks, new AnswerList(), new AnswerList())
        {
        }

        // Full constructor
        protected Question(string header, string body, decimal marks, AnswerList choices, AnswerList rightAnswers)
        {
            Header = header ?? string.Empty;
            Body = body ?? string.Empty;
            Marks = marks;
            Choices = choices ?? new AnswerList();
            RightAnswers = rightAnswers ?? new AnswerList();
        }

        // Copy constructor chaining
        protected Question(Question other)
            : this(
                other?.Header ?? string.Empty,
                other?.Body ?? string.Empty,
                other?.Marks ?? 0m,
                other?.Choices != null ? (AnswerList)other.Choices.Clone() : new AnswerList(),
                other?.RightAnswers != null ? (AnswerList)other.RightAnswers.Clone() : new AnswerList())
        {
        }

        // Abstract method for distinct question representations
        public abstract void DisplayQuestion();

        // Validates student answer(s) against RightAnswers
        public virtual bool ValidateAnswer(AnswerList studentAnswers)
        {
            if (studentAnswers == null || RightAnswers == null) return false;
            return RightAnswers.Equals(studentAnswers);
        }

        public abstract object Clone();

        public virtual int CompareTo(object? obj)
        {
            if (obj is null) return 1;
            if (obj is Question other)
            {
                return CompareTo(other);
            }
            throw new ArgumentException("Object is not a Question");
        }

        public virtual int CompareTo(Question? other)
        {
            if (other is null) return 1;
            // Compare primarily by Marks, then by Header
            int markComparison = Marks.CompareTo(other.Marks);
            if (markComparison != 0) return markComparison;
            return string.Compare(Header, other.Header, StringComparison.OrdinalIgnoreCase);
        }

        public override bool Equals(object? obj)
        {
            if (obj is not Question other) return false;
            return string.Equals(Header, other.Header, StringComparison.OrdinalIgnoreCase) &&
                   string.Equals(Body, other.Body, StringComparison.OrdinalIgnoreCase) &&
                   Marks == other.Marks &&
                   Equals(Choices, other.Choices) &&
                   Equals(RightAnswers, other.RightAnswers);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(
                Header?.ToLowerInvariant(),
                Body?.ToLowerInvariant(),
                Marks,
                Choices?.GetHashCode() ?? 0,
                RightAnswers?.GetHashCode() ?? 0);
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"========== {Header} (Marks: {Marks}) ==========");
            sb.AppendLine(Body);
            sb.AppendLine("Choices:");
            sb.AppendLine(Choices.ToString());
            return sb.ToString();
        }
    }
}
