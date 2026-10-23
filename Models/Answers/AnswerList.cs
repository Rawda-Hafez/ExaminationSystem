using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExaminationSystem.Models.Answers
{
    public class AnswerList : List<Answer>, ICloneable, IComparable, IComparable<AnswerList>
    {
        // Parameterless constructor chaining to capacity constructor
        public AnswerList() : this(0)
        {
        }

        // Capacity constructor chaining to base
        public AnswerList(int capacity) : base(capacity)
        {
        }

        // Collection constructor chaining to base
        public AnswerList(IEnumerable<Answer> collection) : base(collection ?? Enumerable.Empty<Answer>())
        {
        }

        public object Clone()
        {
            var clonedList = new AnswerList(this.Capacity);
            foreach (var answer in this)
            {
                clonedList.Add((Answer)answer.Clone());
            }
            return clonedList;
        }

        public int CompareTo(object? obj)
        {
            if (obj is null) return 1;
            if (obj is AnswerList other)
            {
                return CompareTo(other);
            }
            throw new ArgumentException("Object is not an AnswerList");
        }

        public int CompareTo(AnswerList? other)
        {
            if (other is null) return 1;
            return Count.CompareTo(other.Count);
        }

        public override bool Equals(object? obj)
        {
            if (obj is not AnswerList other) return false;
            if (Count != other.Count) return false;

            var thisSorted = this.OrderBy(a => a.Id).ToList();
            var otherSorted = other.OrderBy(a => a.Id).ToList();

            for (int i = 0; i < thisSorted.Count; i++)
            {
                if (!thisSorted[i].Equals(otherSorted[i]))
                    return false;
            }
            return true;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 19;
                foreach (var answer in this.OrderBy(a => a.Id))
                {
                    hash = hash * 31 + (answer?.GetHashCode() ?? 0);
                }
                return hash;
            }
        }

        public override string ToString()
        {
            if (Count == 0) return "No Answers";
            var sb = new StringBuilder();
            foreach (var answer in this)
            {
                sb.AppendLine($"\t{answer}");
            }
            return sb.ToString().TrimEnd();
        }
    }
}
