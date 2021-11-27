using System.Collections.Generic;
using System.Linq;
using Isu.Tools;
using IsuExtra.Entities;

namespace IsuExtra.Services
{
    public class IsuExtraService : IIsuExtraService
    {
        private const ushort MaxMegaFacultiesAmount = 5;
        private static IsuExtraService? _instance;
        private readonly List<MegaFaculty> _megaFaculties;

        private IsuExtraService() => _megaFaculties = new List<MegaFaculty>();

        private bool IsMegaFacultiesAmountOk => _megaFaculties.Count < MaxMegaFacultiesAmount;

        public static IsuExtraService GetInstance() => _instance is null
            ? _instance = new IsuExtraService()
            : throw new IsuException("Isu can be created only once");

        public MegaFaculty AddMegaFaculty(string megaFacultyName)
        {
            if (IsMegaFacultyExists(megaFacultyName))
                throw new IsuException($"Mega faculty {megaFacultyName} already exists");
            if (!IsMegaFacultiesAmountOk)
                throw new IsuException($"Mega faculties amount can't be more than {MaxMegaFacultiesAmount}");

            var newMegaFaculty = new MegaFaculty(megaFacultyName, isuExtraService: this);

            _megaFaculties.Add(newMegaFaculty);

            return newMegaFaculty;
        }

        public void AddStudentToOgnpStream(ExtraStudent student, StudyingStream stream)
        {
            if (!IsStudentExistsInIsu(student))
                throw new IsuException($"Student to add in {stream.OgnpCourse.Name}'s stream {stream.StreamNumber} doesn't exist in Isu");

            stream.AddStudent(student);
        }

        public void RemoveStudentFromOgnpStream(ExtraStudent student, StudyingStream stream)
        {
            if (!IsStudentExistsInIsu(student))
                throw new IsuException($"Student to remove in {stream.OgnpCourse.Name}'s stream {stream.StreamNumber} doesn't exist in Isu");

            stream.RemoveStudent(student);
        }

        public MegaFaculty FindStudentMegaFaculty(ExtraStudent student) =>
            _megaFaculties
                .SingleOrDefault(f => f.IsStudentExists(student)) ??
            throw new IsuException("Student doesn't exits in Isu");

        public bool IsStudentExistsInIsu(ExtraStudent student) => _megaFaculties.Any(mf => mf.IsStudentExists(student));
        private bool IsMegaFacultyExists(string megaFacultyName) => _megaFaculties.Any(f => f.Name == megaFacultyName);
    }
}