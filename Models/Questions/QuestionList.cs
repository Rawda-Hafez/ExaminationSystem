using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ExaminationSystem.Models.Questions
{
    /// <summary>
    /// Generic QuestionList inheriting from List&lt;T&gt; with constraint where T : Question.
    /// Constraints guarantee that elements have question properties and support cloning and comparison.
    /// </summary>
    /// <typeparam name="T">Question type</typeparam>
    public class QuestionList<T> : List<T>, ICloneable, IComparable, IComparable<QuestionList<T>> 
        where T : Question
    {
        public string LogFilePath { get; set; }

        // Parameterless constructor chaining to default file path
        public QuestionList() : this($"Questions_Log_{Guid.NewGuid():N}.txt")
        {
        }

        // Constructor accepting logFilePath chained
        public QuestionList(string logFilePath) : this(logFilePath, new List<T>())
        {
        }

        // Full constructor with logFilePath and collection
        public QuestionList(string logFilePath, IEnumerable<T> collection) : base(collection ?? new List<T>())
        {
            LogFilePath = string.IsNullOrWhiteSpace(logFilePath) ? $"Questions_Log_{Guid.NewGuid():N}.txt" : logFilePath;
            
            // Ensure directory exists if path contains directory
            string? directory = Path.GetDirectoryName(LogFilePath);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
        }

        /// <summary>
        /// Adds a question to the list, keeps default behavior, and logs it to the list's file using TextWriter.
        /// </summary>
        public new void Add(T item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));

            // Keep default behavior
            base.Add(item);

            // Open file and log the question using TextWriter (StreamWriter)
            try
            {
                using (TextWriter writer = new StreamWriter(LogFilePath, append: true, Encoding.UTF8))
                {
                    writer.WriteLine("==================================================");
                    writer.WriteLine($"Logged At: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
                    writer.WriteLine($"Question Type: {item.GetType().Name}");
                    writer.WriteLine(item.ToString());
                    writer.WriteLine("==================================================");
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"[Warning] Failed to log question to file: {ex.Message}");
                Console.ResetColor();
            }
        }

        /// <summary>
        /// Reads and prints the logged questions from the file using TextReader.
        /// </summary>
        public string ReadQuestionsFromFile()
        {
            if (!File.Exists(LogFilePath))
            {
                return "No log file found.";
            }

            using (TextReader reader = new StreamReader(LogFilePath, Encoding.UTF8))
            {
                return reader.ReadToEnd();
            }
        }

        public object Clone()
        {
            var clonedList = new QuestionList<T>($"Cloned_{LogFilePath}");
            foreach (var q in this)
            {
                clonedList.Add((T)q.Clone());
            }
            return clonedList;
        }

        public int CompareTo(object? obj)
        {
            if (obj is null) return 1;
            if (obj is QuestionList<T> other)
            {
                return CompareTo(other);
            }
            throw new ArgumentException("Object is not a QuestionList<T>");
        }

        public int CompareTo(QuestionList<T>? other)
        {
            if (other is null) return 1;
            return Count.CompareTo(other.Count);
        }

        public override bool Equals(object? obj)
        {
            if (obj is not QuestionList<T> other) return false;
            if (Count != other.Count) return false;
            for (int i = 0; i < Count; i++)
            {
                if (!this[i].Equals(other[i])) return false;
            }
            return true;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                foreach (var q in this)
                {
                    hash = hash * 23 + (q?.GetHashCode() ?? 0);
                }
                return hash;
            }
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"QuestionList (Count: {Count}, LogFile: {LogFilePath}):");
            for (int i = 0; i < Count; i++)
            {
                sb.AppendLine($"[Question {i + 1}]");
                sb.AppendLine(this[i].ToString());
            }
            return sb.ToString();
        }
    }
}
