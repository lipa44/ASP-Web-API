#nullable enable
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Isu.Entities;
using Isu.Tools;
using Group = Isu.Entities.Group;

namespace Isu.Services
{
    public class IsuService : IIsuService
    {
        private const ushort MaxGroupsAmount = 15;

        private const ushort MaxStudentsInGroupAmount = 30;

        private readonly Map<GroupName, Group> _groups;

        private int _studentId;

        public IsuService() => _groups = new Map<GroupName, Group>();

        public Group AddGroup(string name)
        {
            var groupName = new GroupName(name);

            if (_groups.Count >= MaxGroupsAmount)
                throw new IsuException($"Isu couldn't contain more than {MaxGroupsAmount} groups");

            if (_groups.ContainsKey(groupName))
                throw new IsuException($"Group {groupName} already exists");

            _groups.Add(groupName, new Group(groupName));

            return _groups[groupName];
        }

        public Student AddStudent(Group group, string studentName)
        {
            if (FindStudent(studentName) is not null)
                throw new IsuException($"Student {studentName} is already added");

            var groupName = new GroupName(group.GroupName);

            if (!_groups.ContainsKey(groupName))
                throw new IsuException($"Group {groupName.Name} does not exists");

            if (_groups[groupName].Students.Count >= MaxStudentsInGroupAmount)
                throw new IsuException($"Group {groupName} is full");

            var newStudent = new Student(studentName, ++_studentId);

            _groups[groupName].Students.Add(newStudent);

            return newStudent;
        }

        public Student GetStudent(int id)
        {
            if (id < 0) throw new IsuException("Student id can not be opposite");

            Student? student = _groups.Groups
                .SelectMany(group => group.Students
                    .Where(student => student.Id == id))
                .SingleOrDefault();

            return student ?? throw new IsuException($"Student with id {id} can't be found");
        }

        public Student? FindStudent(string name) => _groups.Groups
            .SelectMany(group => group.Students
                .Where(student => student.Name == name))
            .FirstOrDefault();

        public List<Student>? FindStudents(string groupName)
        {
            var gGroupName = new GroupName(groupName);

            return (_groups.ContainsKey(gGroupName)
                ? _groups[gGroupName].Students : Enumerable.Empty<Student>()) as List<Student>;
        }

        public List<Student>? FindStudents(CourseNumber courseNumber)
        {
            Regex coursePrefix = courseNumber.CoursePrefixRegex;

            var students = _groups.Groups
                .Where(g => coursePrefix.IsMatch(g.GroupName[..3]))
                .SelectMany(group => group.Students)
                .ToList();

            return (students.Count != 0 ? students : Enumerable.Empty<Student>()) as List<Student>;
        }

        public Group? FindGroup(string groupName)
        {
            if (string.IsNullOrEmpty(groupName)) throw new IsuException("Group name is null");

            var gGroupName = new GroupName(groupName);

            return _groups.ContainsKey(gGroupName) ? _groups[gGroupName] : null;
        }

        public List<Group>? FindGroups(CourseNumber courseNumber)
        {
            Regex coursePrefix = courseNumber.CoursePrefixRegex;

            var groupsList = _groups.Groups
                .Where(group => coursePrefix.IsMatch(group.GroupName[..3]))
                .ToList();

            return (groupsList.Count != 0 ? groupsList : Enumerable.Empty<Group>()) as List<Group>;
        }

        public void ChangeStudentGroup(Student student, Group newGroup)
        {
            var newGroupName = new GroupName(newGroup.GroupName);

            if (!_groups.ContainsKey(newGroupName))
                throw new IsuException($"Group {newGroupName.Name} does not exists");

            if (_groups[newGroupName].Students.Contains(student))
                throw new IsuException($"Student {student.Name} is already in {newGroupName.Name}");

            var oldGroupName = new GroupName(GetStudentGroup(student).GroupName);

            _groups[oldGroupName].Students.Remove(student);

            _groups[newGroupName].Students.Add(student);
        }

        public Group GetStudentGroup(Student student)
        {
            Group? groupName = _groups.Groups
                .SingleOrDefault(group => group.Students
                    .Any(s => student.Id == s.Id));

            return groupName ?? throw new IsuException("Student's group can't be found");
        }
    }
}