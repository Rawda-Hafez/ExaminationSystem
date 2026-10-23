using System;

namespace ExaminationSystem.Models.Answers
{
    public class Answer : ICloneable, IComparable, IComparable<Answer>
    {
        public int Id { get; set; }
        public string Text { get; set; }

        // Parameterless constructor chained to parameterized constructor
        public Answer() : this(0, string.Empty)
        {
        }

        // Single parameter constructor chained to full constructor
        public Answer(int id) : this(id, string.Empty)
        {
        }

        // Primary constructor
        public Answer(int id, string text)
        {
            Id = id;
            Text = text ?? string.Empty;
        }

        // Copy constructor chained
        public Answer(Answer other) : this(other?.Id ?? 0, other?.Text ?? string.Empty)
        {
        }

        public object Clone()
        {
            return new Answer(this.Id, this.Text);
        }

        public int CompareTo(object? obj)
        {
            if (obj is null) return 1;
            if (obj is Answer other)
            {
                return CompareTo(other);
            }
            throw new ArgumentException("Object is not an Answer");
        }

        public int CompareTo(Answer? other)
        {
            if (other is null) return 1;
            return Id.CompareTo(other.Id);
        }

        public override bool Equals(object? obj)
        {
            if (obj is not Answer other) return false;
            return Id == other.Id && string.Equals(Text, other.Text, StringComparison.OrdinalIgnoreCase);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Text?.ToLowerInvariant());
        }

        public override string ToString()
        {
            return $"{Id}. {Text}";
        }
    }
}
