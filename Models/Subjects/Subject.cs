using System;
using System.Collections.Generic;
using System.Linq;
using ExaminationSystem.Models.Users;

namespace ExaminationSystem.Models.Subjects
{
    public class Subject : ICloneable, IComparable, IComparable<Subject>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Student> Students { get; set; }

        // Parameterless constructor chained
        public Subject() : this(0, string.Empty)
        {
        }

        // 2-param constructor chained
        public Subject(int id, string name) : this(id, name, new List<Student>())
        {
        }

        // Full constructor
        public Subject(int id, string name, List<Student> students)
        {
            Id = id;
            Name = name ?? string.Empty;
            Students = students ?? new List<Student>();
        }

        // Copy constructor chained
        public Subject(Subject other)
            : this(
                other?.Id ?? 0,
                other?.Name ?? string.Empty,
                other?.Students?.Select(s => (Student)s.Clone()).ToList() ?? new List<Student>())
        {
        }

        public void RegisterStudent(Student student)
        {
            if (student != null && !Students.Contains(student))
            {
                Students.Add(student);
            }
        }

        public object Clone()
        {
            return new Subject(this);
        }

        public int CompareTo(object? obj)
        {
            if (obj is null) return 1;
            if (obj is Subject other)
            {
                return CompareTo(other);
            }
            throw new ArgumentException("Object is not a Subject");
        }

        public int CompareTo(Subject? other)
        {
            if (other is null) return 1;
            return string.Compare(Name, other.Name, StringComparison.OrdinalIgnoreCase);
        }

        public override bool Equals(object? obj)
        {
            if (obj is not Subject other) return false;
            return Id == other.Id && string.Equals(Name, other.Name, StringComparison.OrdinalIgnoreCase);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Name?.ToLowerInvariant());
        }

        public override string ToString()
        {
            return $"Subject [ID: {Id}, Name: {Name}, Enrolled Students: {Students.Count}]";
        }
    }
}
