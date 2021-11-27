using System;
using System.Collections.Generic;
using System.Linq;
using Isu.Tools;

namespace IsuExtra.Entities
{
    public class OgnpCourse
    {
        private readonly List<StudyingStream> _studyingStreams;
        private readonly MegaFaculty _megaFaculty;
        private readonly int _maxAmountOfStudents;

        public OgnpCourse(string name, int maxAmountOfStudents, MegaFaculty megaFaculty)
        {
            if (string.IsNullOrEmpty(name))
                throw new IsuException("Ognp course name is null", new ArgumentException());

            if (maxAmountOfStudents <= 0)
                throw new IsuException("Amount of students in ognp course must be positive");

            Name = name;
            _maxAmountOfStudents = maxAmountOfStudents;
            _megaFaculty = megaFaculty
                           ?? throw new IsuException("Mega faculty name to add ognp course  is null", new ArgumentException());
            _studyingStreams = new List<StudyingStream>();
        }

        public string Name { get; }
        public int FreePlacesLeft => _maxAmountOfStudents - _studyingStreams.Sum(s => s.Students.Count);
        public IReadOnlyList<StudyingStream> StudyingStreams => _studyingStreams;

        public StudyingStream AddStudyingStream(Schedule schedule, int maxAmountOfStudents)
        {
            var newStudyingStream = new StudyingStream(schedule, ognpCourse: this, maxAmountOfStudents,  _megaFaculty);

            if (IsStreamExists(newStudyingStream)) throw new IsuException($"Studying stream in {Name} already exists");

            _studyingStreams.Add(newStudyingStream);

            return newStudyingStream;
        }

        private bool IsStreamExists(StudyingStream stream) => _studyingStreams.Any(s => ReferenceEquals(s, stream));
    }
}