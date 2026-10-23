using System;
using ExaminationSystem.Models.Events;

namespace ExaminationSystem.Models.Users
{
    public class Student : ICloneable, IComparable, IComparable<Student>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }

        // Parameterless constructor chained
        public Student() : this(0, string.Empty, string.Empty)
        {
        }

        // 2-param constructor chained
        public Student(int id, string name) : this(id, name, $"{name.ToLower().Replace(" ", ".")}@univ.edu")
        {
        }

        // Full constructor
        public Student(int id, string name, string email)
        {
            Id = id;
            Name = name ?? string.Empty;
            Email = email ?? string.Empty;
        }

        // Copy constructor chained
        public Student(Student other) : this(other?.Id ?? 0, other?.Name ?? string.Empty, other?.Email ?? string.Empty)
        {
        }

        // Event handler invoked when the exam starts
        public void OnExamStarted(object? sender, ExamEventArgs e)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"   [Notification -> Student {Name} (ID: {Id})]: The {e.ExamType} for '{e.SubjectName}' has officially STARTED! Time allowed: {e.DurationMinutes} mins.");
            Console.ResetColor();
        }

        public object Clone()
        {
            return new Student(this);
        }

        public int CompareTo(object? obj)
        {
            if (obj is null) return 1;
            if (obj is Student other)
            {
                return CompareTo(other);
            }
            throw new ArgumentException("Object is not a Student");
        }

        public int CompareTo(Student? other)
        {
            if (other is null) return 1;
            return Id.CompareTo(other.Id);
        }

        public override bool Equals(object? obj)
        {
            if (obj is not Student other) return false;
            return Id == other.Id && string.Equals(Name, other.Name, StringComparison.OrdinalIgnoreCase);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Name?.ToLowerInvariant());
        }

        public override string ToString()
        {
            return $"Student [ID: {Id}, Name: {Name}, Email: {Email}]";
        }
    }
}
