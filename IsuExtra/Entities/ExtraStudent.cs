using System;
using System.Collections.Generic;
using Isu.Entities;
using Isu.Tools;

namespace IsuExtra.Entities
{
    public class ExtraStudent : Student
    {
        private static int _studentId;
        private readonly List<OgnpCourse> _ognpCourses;
        private Schedule _schedule;

        public ExtraStudent(string name)
            : base(name, ++_studentId)
        {
            _ognpCourses = new List<OgnpCourse>();
            _schedule = new Schedule();
        }

        public IReadOnlyList<OgnpCourse> OgnpCourses => _ognpCourses;
        public Schedule Schedule => _schedule.Clone();

        public void AddOgnpCourse(OgnpCourse ognpCourse, StudyingStream studyingStream)
        {
            if (_ognpCourses.Contains(ognpCourse))
                throw new IsuException($"{Name} is already added to {ognpCourse.Name}");

            _schedule.AddLessons(studyingStream.Schedule.DaysLessonsPairs);
            _ognpCourses.Add(ognpCourse);
        }

        public void RemoveOgnpCourse(OgnpCourse ognpCourse, StudyingStream studyingStream)
        {
            if (!_ognpCourses.Contains(ognpCourse))
                throw new IsuException($"{Name} isn't added to {ognpCourse.Name}");

            _schedule.RemoveLessons(studyingStream.Schedule.DaysLessonsPairs);
            _ognpCourses.Remove(ognpCourse);
        }

        public void AddGroupSchedule(ExtraGroup group)
        {
            if (group is null)
                throw new IsuException("Group to add student's schedule is null", new ArgumentException());

            _schedule = group.Schedule.Clone();
        }
    }
}