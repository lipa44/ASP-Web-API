using System.Text.RegularExpressions;
using Isu.Tools;

namespace Isu.Entities
{
    public class CourseNumber
    {
        private readonly int _courseNumber;

        public CourseNumber(int courseNumber)
        {
            if (courseNumber is < 1 or > 4)
                throw new IsuException("Wrong course number");

            _courseNumber = courseNumber;
        }

        public Regex CoursePrefixRegex => new ($"M3{_courseNumber}", RegexOptions.IgnoreCase);
    }
}