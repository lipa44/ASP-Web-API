using System;
using System.Collections.Generic;
using System.Linq;
using Isu.Tools;

namespace IsuExtra.Entities
{
    public class StudyingStream
    {
        private static int _streamNumber;

        private readonly List<ExtraStudent> _students;
        private readonly MegaFaculty _megaFaculty;
        private readonly Schedule _schedule;

        public StudyingStream(Schedule schedule, OgnpCourse ognpCourse, int maxAmountOfStudents, MegaFaculty megaFaculty)
        {
            if (maxAmountOfStudents < 0)
                throw new IsuException("Max amount of students for stream must be positive", new ArgumentException());

            _schedule = schedule.Clone() ??
                        throw new IsuException("Schedule to add studying stream is null", new ArgumentException());
            OgnpCourse = ognpCourse ??
                          throw new IsuException("Ognp course to add stream is nill", new ArgumentException());
            MaxAmountOfStudents = maxAmountOfStudents;
            _megaFaculty = megaFaculty;

            _students = new List<ExtraStudent>();
            StreamNumber = ++_streamNumber;
        }

        public Schedule Schedule => _schedule.Clone();
        public OgnpCourse OgnpCourse { get; }
        public int MaxAmountOfStudents { get; }
        public IReadOnlyList<ExtraStudent> Students => _students;
        public int StreamNumber { get; }

        public bool IsStudentsAmountInStreamOk => _students.Count < MaxAmountOfStudents;

        public void AddStudent(ExtraStudent student)
        {
            if (_megaFaculty.IsStudentExists(student))
                throw new IsuException($"Student {student.Name} can't be added into {_megaFaculty.Name}'s ognp");

            if (IsStudentExists(student))
                throw new IsuException($"{student.Name} is already exists in this studying stream");

            if (OgnpCourse.FreePlacesLeft == 0) throw new IsuException($"No more free places in {OgnpCourse.Name}");
            if (!IsStudentsAmountInStreamOk) throw new IsuException($"Studying stream {StreamNumber} is full");

            student.AddOgnpCourse(OgnpCourse, studyingStream: this);
            _students.Add(student);
        }

        public void RemoveStudent(ExtraStudent student)
        {
            if (_megaFaculty.IsStudentExists(student))
                throw new IsuException($"Student {student.Name} can't be added into {_megaFaculty.Name}'s ognp");

            if (!IsStudentExists(student)) throw new IsuException($"{student.Name} doesn't exist in this studying stream");

            student.RemoveOgnpCourse(OgnpCourse, studyingStream: this);
            _students.Remove(student);
        }

        private bool IsStudentExists(ExtraStudent student) => _students.Any(s => ReferenceEquals(s, student));
    }
}