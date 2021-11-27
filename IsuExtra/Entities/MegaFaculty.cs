using System;
using System.Collections.Generic;
using System.Linq;
using Isu.Tools;
using IsuExtra.Services;

namespace IsuExtra.Entities
{
    public class MegaFaculty
    {
        private const ushort MaxGroupsAmount = 15;

        private readonly List<OgnpCourse> _courses;
        private readonly Dictionary<ExtraGroup, List<ExtraStudent>> _groups;

        public MegaFaculty(string name, IsuExtraService isuExtraService)
        {
            if (string.IsNullOrEmpty(name))
                throw new IsuException("Mega faculty name is null", new ArgumentException());

            Name = name;
            _groups = new Dictionary<ExtraGroup, List<ExtraStudent>>();
            _courses = new List<OgnpCourse>();
        }

        public string Name { get; }
        public IReadOnlyDictionary<ExtraGroup, List<ExtraStudent>> Groups => _groups;
        public IReadOnlyList<OgnpCourse> Courses => _courses;

        private bool IsGroupsAmountOk => _groups.Count < MaxGroupsAmount;

        public OgnpCourse AddOgnpCourse(string name, int maxAmountOfStudents)
        {
            var newOgnpCourse = new OgnpCourse(name, maxAmountOfStudents, megaFaculty: this);
            if (IsOgnpCourseExists(newOgnpCourse))
                throw new IsuException($"Course to add in {Name} is already exists");

            _courses.Add(newOgnpCourse);

            return newOgnpCourse;
        }

        public ExtraGroup AddGroup(string groupName, Schedule schedule)
        {
            if (!IsGroupsAmountOk)
                throw new IsuException($"{Name} can't contain more than {MaxGroupsAmount} groups");

            var newGroup = new ExtraGroup(groupName, schedule, megaFaculty: this);

            if (IsGroupExists(newGroup)) throw new IsuException($"Group {groupName} is already exists in {Name}");

            _groups.Add(newGroup, new List<ExtraStudent>());

            return newGroup;
        }

        public ExtraStudent AddStudent(ExtraGroup group, string name)
        {
            if (!IsGroupExists(group)) throw new IsuException($"Cannot find entity group in {Name} to add student");

            var newStudent = new ExtraStudent(name);

            newStudent.AddGroupSchedule(group);
            _groups[group].Add(newStudent);

            return newStudent;
        }

        public void ChangeStudentGroup(ExtraStudent student, ExtraGroup newGroup)
        {
            if (!IsStudentExists(student))
                throw new IsuException($"Cannot find entity student in {Name} to change student's group");

            if (!IsGroupExists(newGroup))
                throw new IsuException($"Cannot find entity group in {Name} to change student's group");

            RemoveStudentFromGroup(student);
            AddStudentToGroup(student, newGroup);
            student.AddGroupSchedule(newGroup);
        }

        public List<ExtraStudent> GetStudentsWithoutOgnp(ExtraGroup group)
        {
            if (!IsGroupExists(group))
                throw new IsuException($"Cannot find entity group in {Name} to get students without OGNP");

            return _groups[group].Where(student => student.OgnpCourses.Count == 0).ToList();
        }

        public bool IsStudentExists(ExtraStudent student) =>
            _groups.Values
                .Any(students => students
                    .Any(existedStudent => ReferenceEquals(existedStudent, student)));

        private void AddStudentToGroup(ExtraStudent student, ExtraGroup group) =>
            _groups.Single(existedGroup => ReferenceEquals(existedGroup.Key, group)).Value.Add(student);

        private void RemoveStudentFromGroup(ExtraStudent student)
            => _groups.Single(existedGroup => existedGroup.Value.Contains(student)).Value.Remove(student);

        private bool IsOgnpCourseExists(OgnpCourse newCourse) => _courses.Any(existedCourse => ReferenceEquals(newCourse, existedCourse));

        private bool IsGroupExists(ExtraGroup group) => _groups.Keys.Any(existedGroup =>
            existedGroup.GroupName == group.GroupName && ReferenceEquals(existedGroup, group));
    }
}