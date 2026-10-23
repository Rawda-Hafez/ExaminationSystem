using System;

namespace ExaminationSystem.Models.Events
{
    public class ExamEventArgs : EventArgs
    {
        public string SubjectName { get; }
        public string ExamType { get; }
        public int DurationMinutes { get; }
        public int NumberOfQuestions { get; }
        public DateTime StartTime { get; }

        public ExamEventArgs(string subjectName, string examType, int durationMinutes, int numberOfQuestions, DateTime startTime)
        {
            SubjectName = subjectName ?? string.Empty;
            ExamType = examType ?? string.Empty;
            DurationMinutes = durationMinutes;
            NumberOfQuestions = numberOfQuestions;
            StartTime = startTime;
        }

        public override string ToString()
        {
            return $"Exam '{ExamType}' for Subject '{SubjectName}' started at {StartTime:HH:mm:ss} (Duration: {DurationMinutes} mins, Questions: {NumberOfQuestions})";
        }
    }
}
