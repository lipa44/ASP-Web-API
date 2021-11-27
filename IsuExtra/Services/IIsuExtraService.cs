using IsuExtra.Entities;

namespace IsuExtra.Services
{
    public interface IIsuExtraService
    {
        MegaFaculty AddMegaFaculty(string megaFacultyName);
        MegaFaculty FindStudentMegaFaculty(ExtraStudent student);
    }
}