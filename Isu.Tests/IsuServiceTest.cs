using Isu.Services;
using Isu.Entities;
using Isu.Tools;
using NUnit.Framework;

namespace Isu.Tests
{
    [TestFixture]
    public class Tests
    {
        private static IsuService _isuService;
        private static Group _m3201;
        private static Group _m3210;
        private static Student _student;

        [SetUp]
        public void Setup()
        {
            _isuService = new IsuService();
            _m3201 = _isuService.AddGroup("M3201");
            _m3210 = _isuService.AddGroup("M3210");
        }

        [Test]
        public void AddStudentToGroup_StudentHasGroupAndGroupContainsStudent()
        {
            _isuService.AddStudent(_m3201, "Misha Libchenko");

            Assert.True(_isuService.FindStudent("Misha Libchenko") != null);
        }

        [Test]
        public void ReachMaxStudentPerGroup_ThrowException()
        {
            Assert.Catch<IsuException>(() =>
            {
                const ushort maxStudentsAmount = 30;

                for (int i = 1; i <= maxStudentsAmount + 1; ++i)
                {
                    _isuService.AddStudent(_m3201, $"Misha Libchenko{i}");
                }
            });
        }
        
        [TestCase("m31111")]
        [TestCase("M4201")]
        [TestCase("M3218")]
        [TestCase("P3218")]
        [TestCase("000000000000")]
        [TestCase("0")]
        [TestCase("M3099")]
        [TestCase("     ")]
        [TestCase(null)]
        [TestCase("")]
        [TestCase("M3131")]
        [TestCase("M3141")]
        public void CreateGroupWithInvalidName_ThrowException(string groupName)
        {
            Assert.Catch<IsuException>(() => _isuService.AddGroup(groupName));
        }

        [Test]
        public void TransferStudentToAnotherGroup_GroupChanged()
        {
            _student = _isuService.AddStudent(_m3201, "Misha Libchenko");
            _isuService.ChangeStudentGroup(_student, _m3210);
            Assert.True(_isuService.GetStudentGroup(_student).GroupName == "M3210");
        }
    }
}