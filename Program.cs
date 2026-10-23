using System;
using ExaminationSystem.Models.Answers;
using ExaminationSystem.Models.Exams;
using ExaminationSystem.Models.Questions;
using ExaminationSystem.Models.Subjects;
using ExaminationSystem.Models.Users;

namespace ExaminationSystem
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("=====================================================================");
            Console.WriteLine("        WELCOME TO THE ADVANCED EXAMINATION MANAGEMENT SYSTEM        ");
            Console.WriteLine("=====================================================================");
            Console.ResetColor();

            // 1. Create Subject
            var csharpSubject = new Subject(101, "C# Programming & OOP");

            // 2. Create Students and Register them to Subject
            var student1 = new Student(201, "Ahmed Ali", "ahmed.ali@iti.gov.eg");
            var student2 = new Student(202, "Sara Mahmoud", "sara.m@iti.gov.eg");
            var student3 = new Student(203, "Omar Hassan", "omar.h@iti.gov.eg");

            csharpSubject.RegisterStudent(student1);
            csharpSubject.RegisterStudent(student2);
            csharpSubject.RegisterStudent(student3);

            Console.WriteLine($"Registered Subject: {csharpSubject.Name}");
            Console.WriteLine($"Enrolled Students Count: {csharpSubject.Students.Count}");
            foreach (var student in csharpSubject.Students)
            {
                Console.WriteLine($"   - {student}");
            }
            Console.WriteLine();

            // 3. Declare Practice Exam Object
            // Constructor chaining used throughout
            var practiceExam = new PracticeExam(
                time: 15,
                numberOfQuestions: 3,
                subject: csharpSubject,
                logFileName: "PracticeExam_Questions.log"
            );

            // Populate Practice Exam with different question types
            // Question 1: True / False Question
            var tfQuestion1 = new TrueFalseQuestion(
                header: "OOP Concept - Inheritance",
                body: "In C#, a class can inherit from multiple classes directly.",
                marks: 5m,
                correctAnswerId: 2 // 2 = False
            );

            // Question 2: Choose One Question
            var chooseOneChoices1 = new AnswerList
            {
                new Answer(1, "Value Type stored on the Stack"),
                new Answer(2, "Reference Type stored on the Managed Heap"),
                new Answer(3, "Unmanaged Pointer"),
                new Answer(4, "Static Class")
            };
            var chooseOne1 = new ChooseOneQuestion(
                header: "C# Memory Management",
                body: "In C#, an object instantiated from a class is a:",
                marks: 10m,
                choices: chooseOneChoices1,
                rightAnswer: new Answer(2, "Reference Type stored on the Managed Heap")
            );

            // Question 3: Choose All Question
            var chooseAllChoices1 = new AnswerList
            {
                new Answer(1, "Encapsulation"),
                new Answer(2, "Polymorphism"),
                new Answer(3, "Compilation"),
                new Answer(4, "Inheritance")
            };
            var rightAnswers1 = new AnswerList
            {
                new Answer(1, "Encapsulation"),
                new Answer(2, "Polymorphism"),
                new Answer(4, "Inheritance")
            };
            var chooseAll1 = new ChooseAllQuestion(
                header: "Core OOP Pillars",
                body: "Which of the following are foundational pillars of Object-Oriented Programming? (Select all)",
                marks: 15m,
                choices: chooseAllChoices1,
                rightAnswers: rightAnswers1
            );

            // Add Questions to Practice Exam (triggers custom Add with TextWriter logging)
            practiceExam.AddQuestion(tfQuestion1);
            practiceExam.AddQuestion(chooseOne1);
            practiceExam.AddQuestion(chooseAll1);

            // 4. Declare Final Exam Object
            var finalExam = new FinalExam(
                time: 30,
                numberOfQuestions: 3,
                subject: csharpSubject,
                logFileName: "FinalExam_Questions.log"
            );

            // Populate Final Exam with questions
            // Question 1: True / False Question
            var tfQuestion2 = new TrueFalseQuestion(
                header: "C# Structs",
                body: "In C#, struct is a value type that does not support inheritance from another struct or class.",
                marks: 5m,
                correctAnswerId: 1 // 1 = True
            );

            // Question 2: Choose One Question
            var chooseOneChoices2 = new AnswerList
            {
                new Answer(1, "ref"),
                new Answer(2, "out"),
                new Answer(3, "params"),
                new Answer(4, "in")
            };
            var chooseOne2 = new ChooseOneQuestion(
                header: "C# Keywords",
                body: "Which keyword allows passing a variable number of arguments of the specified type to a method?",
                marks: 10m,
                choices: chooseOneChoices2,
                rightAnswer: new Answer(3, "params")
            );

            // Question 3: Choose All Question
            var chooseAllChoices2 = new AnswerList
            {
                new Answer(1, "abstract"),
                new Answer(2, "virtual"),
                new Answer(3, "sealed"),
                new Answer(4, "override")
            };
            var rightAnswers2 = new AnswerList
            {
                new Answer(1, "abstract"),
                new Answer(2, "virtual"),
                new Answer(4, "override")
            };
            var chooseAll2 = new ChooseAllQuestion(
                header: "Polymorphism Keywords",
                body: "Which of the following keywords can be involved in runtime method overriding in C#? (Select all)",
                marks: 15m,
                choices: chooseAllChoices2,
                rightAnswers: rightAnswers2
            );

            // Add Questions to Final Exam (triggers custom Add with TextWriter logging to different file)
            finalExam.AddQuestion(tfQuestion2);
            finalExam.AddQuestion(chooseOne2);
            finalExam.AddQuestion(chooseAll2);

            // Ensure event subscriptions
            practiceExam.SubscribeSubjectStudents();
            finalExam.SubscribeSubjectStudents();

            // Demonstrate Menu
            bool exit = false;
            while (!exit)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("======================= EXAM SELECTION MENU =======================");
                Console.ResetColor();
                Console.WriteLine("1. Take Practice Exam (Shows Right Answers & Detailed Review at the end)");
                Console.WriteLine("2. Take Final Exam    (Only Shows Questions, Answers & Final Score)");
                Console.WriteLine("3. Test ICloneable, IComparable, Equals, GetHashCode & File Logs");
                Console.WriteLine("4. Exit");
                Console.Write("\nPlease choose an option (1-4): ");

                string? choice = Console.ReadLine()?.Trim();
                Console.WriteLine();

                switch (choice)
                {
                    case "1":
                        // Show Practice Exam
                        practiceExam.ShowExam();
                        break;

                    case "2":
                        // Show Final Exam
                        finalExam.ShowExam();
                        break;

                    case "3":
                        DemonstrateOOPFeatures(practiceExam, finalExam);
                        break;

                    case "4":
                        exit = true;
                        Console.WriteLine("Exiting Examination System. Goodbye!");
                        break;

                    default:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Invalid selection. Please enter 1, 2, 3, or 4.");
                        Console.ResetColor();
                        break;
                }

                if (!exit)
                {
                    Console.WriteLine("\nPress Enter to return to menu...");
                    Console.ReadLine();
                    try
                    {
                        if (!Console.IsOutputRedirected)
                        {
                            Console.Clear();
                        }
                    }
                    catch { }
                }
            }
        }

        private static void DemonstrateOOPFeatures(PracticeExam practiceExam, FinalExam finalExam)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("================ DEMONSTRATING OOP & SYSTEM CONTRACTS ================");
            Console.ResetColor();

            // 1. ICloneable Demonstration
            Console.WriteLine("\n[1. ICloneable Test]");
            var clonedPractice = (PracticeExam)practiceExam.Clone();
            Console.WriteLine($"Original Exam: {practiceExam}");
            Console.WriteLine($"Cloned Exam  : {clonedPractice}");
            Console.WriteLine($"Reference Equals: {ReferenceEquals(practiceExam, clonedPractice)} (Should be False - Deep Clone)");

            // 2. IComparable Demonstration
            Console.WriteLine("\n[2. IComparable Test]");
            Console.WriteLine($"Comparing PracticeExam (Time: {practiceExam.Time}m) with FinalExam (Time: {finalExam.Time}m):");
            int examComparison = practiceExam.CompareTo(finalExam);
            Console.WriteLine($"practiceExam.CompareTo(finalExam) Result: {examComparison} (Negative means practice duration < final duration)");

            Console.WriteLine("\nComparing Questions by Marks (IComparable):");
            var q1 = practiceExam.Questions[0]; // 5 marks
            var q2 = practiceExam.Questions[1]; // 10 marks
            Console.WriteLine($"Q1 ({q1.Marks} marks) vs Q2 ({q2.Marks} marks): {q1.CompareTo(q2)}");

            // 3. Equals & GetHashCode
            Console.WriteLine("\n[3. Equals & GetHashCode Test]");
            var ans1 = new Answer(1, "True");
            var ans2 = new Answer(1, "True");
            var ans3 = new Answer(2, "False");
            Console.WriteLine($"ans1 ({ans1}) Equals ans2 ({ans2}): {ans1.Equals(ans2)} (HashCodes: {ans1.GetHashCode()} vs {ans2.GetHashCode()})");
            Console.WriteLine($"ans1 ({ans1}) Equals ans3 ({ans3}): {ans1.Equals(ans3)}");

            // 4. TextReader File Logs Demonstration
            Console.WriteLine("\n[4. TextReader Verification of Logged Questions]");
            Console.WriteLine($"Reading Log File for Practice Exam: '{practiceExam.Questions.LogFilePath}'");
            Console.WriteLine("---------------------------------------------------------------");
            string loggedContent = practiceExam.Questions.ReadQuestionsFromFile();
            Console.WriteLine(loggedContent);
            Console.WriteLine("---------------------------------------------------------------");
        }
    }
}
