using System.Collections.Generic;
using System.Linq;
using Isu.Tools;
using IsuExtra.Entities;
using IsuExtra.Services;
using IsuExtra.Types;
using NUnit.Framework;

namespace IsuExtra.Tests
{
    [TestFixture]
    public class IsuExtraServiceTest
    {
        private static readonly IsuExtraService IsuExtraService = IsuExtraService.GetInstance();
        private static readonly MegaFaculty Fitp = IsuExtraService.AddMegaFaculty("FITP");
        private static readonly MegaFaculty Btins = IsuExtraService.AddMegaFaculty("BTINS");
        private static Schedule _m3201Schedule;
        private static ExtraGroup _m3201;

        [SetUp]
        public void InitializingM3201Group()
        {
            var m3201MondayLesson1 = new Lesson("OOP", new LessonTime(10, 00), "Nosovitsky Evgeny", 403);
            var m3201MondayLesson2 = new Lesson("Higher math", new LessonTime(11, 40), "Anna Victorovna", 331);
            var m3201MondayLesson3 = new Lesson("Higher math", new LessonTime(13, 30), "Maksim Vladimirovich", 146);
            var m3201MondayLesson4 = new Lesson("Probability theory", new LessonTime(15, 20), "Irina Suslina", 146);

            var m3201TuesdayLesson1 = new Lesson("OOP", new LessonTime(8, 20), "Fredi Cats && Annchous", 151);
            var m3201TuesdayLesson2 = new Lesson("OS", new LessonTime(10, 00), "Some lecturer", 326);
            var m3201TuesdayLesson3 = new Lesson("OS", new LessonTime(11, 40), "Some lecturer", 326);
            var m3201TuesdayLesson4 = new Lesson("OS", new LessonTime(13, 30), "Mayatin Aleksandr", 466);

            var m3201ThursdayLesson1 = new Lesson("Physics", new LessonTime(8, 20), "Timofeeva Elvira", 535);
            var m3201ThursdayLesson2 = new Lesson("Physics", new LessonTime(10, 00), "Zinchik Aleksandr", 535);

            var m3201MondayLessons = new List<Lesson> {m3201MondayLesson1, m3201MondayLesson2, m3201MondayLesson3, m3201MondayLesson4};
            var m3201TuesdayLessons = new List<Lesson> {m3201TuesdayLesson1, m3201TuesdayLesson2, m3201TuesdayLesson3, m3201TuesdayLesson4};
            var m3201ThursdayLessons = new List<Lesson> {m3201ThursdayLesson1, m3201ThursdayLesson2};

            if (_m3201Schedule is null)
            {
                _m3201Schedule = new Schedule(new Dictionary<StudyingDays, List<Lesson>>
                {
                    {StudyingDays.Monday, m3201MondayLessons},
                    {StudyingDays.Tuesday, m3201TuesdayLessons},
                    {StudyingDays.Thursday, m3201ThursdayLessons},
                });
            }

            if (Fitp.Groups.All(g => g.Key != _m3201))
                _m3201 = Fitp.AddGroup("M3201", _m3201Schedule);
        }

        [Test]
        public void AddSecondIsuInstance_Exception()
        {
            Assert.Catch<IsuException>(() =>
            {
                var isu2 = IsuExtraService.GetInstance();
            });
        }

        [TestCase("Linux Systems")]
        public void AddNewOgnpCourse_Success(string courseName)
        {
            OgnpCourse someOgnpCoupse = Fitp.AddOgnpCourse(courseName, maxAmountOfStudents: 150);
            someOgnpCoupse.AddStudyingStream(new Schedule(), 100);
            someOgnpCoupse.AddStudyingStream(new Schedule(), 50);

            Assert.IsTrue(Fitp.Courses.Any(c => c.Name == courseName));
            Assert.NotNull(Fitp.Courses);
            Assert.IsNotNull(Fitp.Courses);
            Assert.IsNotEmpty(Fitp.Courses);
            Assert.NotZero(Fitp.Courses.Count);
            Assert.AreEqual(2, someOgnpCoupse.StudyingStreams.Count);
        }

        [Test]
        public void AddStudentToOgnpCourse_Success()
        {
            OgnpCourse brewingOgnp = Btins.AddOgnpCourse("Brewing1", maxAmountOfStudents: 3);

            var ognpMondayLesson1 = new Lesson("Brewing", new LessonTime(17, 00), "Some lecturer", 403);
            var ognpMondayLesson2 = new Lesson("Brewing", new LessonTime(18, 40), "Some lecturer", 331);

            var ognpFridayLesson1 = new Lesson("Brewing", new LessonTime(17, 00), "Some lecturer 2", 403);
            var ognpFridayLesson2 = new Lesson("Brewing", new LessonTime(18, 40), "Some lecturer 2", 331);

            var ognpMondayLessons = new List<Lesson> {ognpMondayLesson1, ognpMondayLesson2};
            var ognpFridayLessons = new List<Lesson> {ognpFridayLesson1, ognpFridayLesson2};

            var brewingSchedule = new Schedule(new Dictionary<StudyingDays, List<Lesson>>
            {
                {StudyingDays.Monday, ognpMondayLessons},
                {StudyingDays.Friday, ognpFridayLessons},
            });

            StudyingStream brewingStream1 = brewingOgnp.AddStudyingStream(brewingSchedule, maxAmountOfStudents: 3);

            Assert.NotZero(Btins.Courses.Count);
            Assert.AreSame(brewingOgnp, Btins.Courses.SingleOrDefault(c => c.Equals(brewingOgnp)));

            ExtraStudent student1 = Fitp.AddStudent(_m3201, "Ksusha Vasutinskaya");
            ExtraStudent student2 = Fitp.AddStudent(_m3201, "Mikhail Tarasov");
            ExtraStudent student3 = Fitp.AddStudent(_m3201, "Poznayskiy Kirill");

            IsuExtraService.AddStudentToOgnpStream(student1, brewingStream1);
            IsuExtraService.AddStudentToOgnpStream(student2, brewingStream1);
            IsuExtraService.AddStudentToOgnpStream(student3, brewingStream1);

            // Students are added && ognp stream contains students 
            Assert.True(brewingStream1.Students.Count == 3);
            Assert.AreEqual(student1, brewingStream1.Students.SingleOrDefault(s => s.Equals(student1)));
            Assert.AreEqual(student2, brewingStream1.Students.SingleOrDefault(s => s.Equals(student2)));
            Assert.AreEqual(student3, brewingStream1.Students.SingleOrDefault(s => s.Equals(student3)));
        }

        [Test]
        public void AddStudentToOgnpCourse_CrossingInSchedule()
        {
            OgnpCourse brewingOgnp = Btins.AddOgnpCourse("Brewing2", maxAmountOfStudents: 3);

            var ognpMondayLesson1 = new Lesson("Brewing", new LessonTime(10, 00), "Some lecturer", 403);
            var ognpMondayLesson2 = new Lesson("Brewing", new LessonTime(18, 40), "Some lecturer", 331);

            var ognpFridayLesson1 = new Lesson("Brewing", new LessonTime(17, 00), "Some lecturer 2", 403);
            var ognpFridayLesson2 = new Lesson("Brewing", new LessonTime(18, 40), "Some lecturer 2", 331);

            var ognpMondayLessons = new List<Lesson> {ognpMondayLesson1, ognpMondayLesson2};
            var ognpFridayLessons = new List<Lesson> {ognpFridayLesson1, ognpFridayLesson2};

            var brewingSchedule = new Schedule(new Dictionary<StudyingDays, List<Lesson>>
            {
                {StudyingDays.Monday, ognpMondayLessons},
                {StudyingDays.Friday, ognpFridayLessons},
            });

            StudyingStream brewingStream1 = brewingOgnp.AddStudyingStream(brewingSchedule, maxAmountOfStudents: 3);

            ExtraStudent student1 = Fitp.AddStudent(_m3201, "Some student");
            ExtraStudent student2 = Fitp.AddStudent(_m3201, "Some student");
            ExtraStudent student3 = Fitp.AddStudent(_m3201, "Some student");

            Assert.Catch<IsuException>(() =>
            {
                IsuExtraService.AddStudentToOgnpStream(student1, brewingStream1);
                IsuExtraService.AddStudentToOgnpStream(student2, brewingStream1);
                IsuExtraService.AddStudentToOgnpStream(student3, brewingStream1);
            });
        }

        [Test]
        public void GetOgnpStream_Success()
        {
            OgnpCourse brewingOgnp = Btins.AddOgnpCourse("Brewing3", maxAmountOfStudents: 13);

            var ognpStream1MondayLesson1 = new Lesson("Brewing", new LessonTime(17, 00), "Some lecturer", 403);
            var ognpStream1MondayLesson2 = new Lesson("Brewing", new LessonTime(18, 40), "Some lecturer", 331);
            var ognpStream1FridayLesson1 = new Lesson("Brewing", new LessonTime(17, 00), "Some lecturer 2", 403);
            var ognpStream1FridayLesson2 = new Lesson("Brewing", new LessonTime(18, 40), "Some lecturer 2", 331);
            
            var ognpStream2MondayLesson1 = new Lesson("Brewing", new LessonTime(17, 00), "Some lecturer", 403);
            var ognpStream2MondayLesson2 = new Lesson("Brewing", new LessonTime(18, 40), "Some lecturer", 331);
            var ognpStream2WednesdayLesson1 = new Lesson("Brewing", new LessonTime(8, 20), "Some lecturer 2", 403);
            var ognpStream2WednesdayLesson2 = new Lesson("Brewing", new LessonTime(10, 00), "Some lecturer 2", 331);


            var ognpStream1MondayLessons = new List<Lesson> {ognpStream1MondayLesson1, ognpStream1MondayLesson2};
            var ognpStream1FridayLessons = new List<Lesson> {ognpStream1FridayLesson1, ognpStream1FridayLesson2};

            var ognpStream2MondayLessons = new List<Lesson> {ognpStream2MondayLesson1, ognpStream2MondayLesson2};
            var ognpStream2WednesdayLessons = new List<Lesson> {ognpStream2WednesdayLesson1, ognpStream2WednesdayLesson2};

            var brewingStream1Schedule = new Schedule(new Dictionary<StudyingDays, List<Lesson>>
            {
                {StudyingDays.Monday, ognpStream1MondayLessons},
                {StudyingDays.Friday, ognpStream1FridayLessons},
            });

            var brewingStream2Schedule = new Schedule(new Dictionary<StudyingDays, List<Lesson>>
            {
                {StudyingDays.Monday, ognpStream2MondayLessons},
                {StudyingDays.Wednesday, ognpStream2WednesdayLessons},
            });

            StudyingStream brewingStream1 =
                brewingOgnp.AddStudyingStream(brewingStream1Schedule, maxAmountOfStudents: 3);
            StudyingStream brewingStream2 =
                brewingOgnp.AddStudyingStream(brewingStream2Schedule, maxAmountOfStudents: 10);

            ExtraStudent student1 = Fitp.AddStudent(_m3201, "Some student");
            ExtraStudent student2 = Fitp.AddStudent(_m3201, "Some student");
            ExtraStudent student3 = Fitp.AddStudent(_m3201, "Some student");

            var studentsStream1 = new List<ExtraStudent> {student1, student2};
            var studentsStream2 = new List<ExtraStudent> {student3};

            IsuExtraService.AddStudentToOgnpStream(studentsStream1[0], brewingStream1);
            IsuExtraService.AddStudentToOgnpStream(studentsStream1[1], brewingStream1);

            IsuExtraService.AddStudentToOgnpStream(studentsStream2[0], brewingStream2);

            Assert.AreEqual(studentsStream1, brewingStream1.Students);
            Assert.AreEqual(studentsStream2, brewingStream2.Students);
            Assert.AreNotEqual(studentsStream1, brewingStream2.Students);
            Assert.AreNotEqual(studentsStream2, brewingStream1.Students);
        }

        [Test]
        public void AddMoreStudentsToOgnpCourseThenMaxAmount_Exception()
        {
            OgnpCourse brewingOgnp = Btins.AddOgnpCourse("Brewing4", maxAmountOfStudents: 2);

            var ognpMondayLesson1 = new Lesson("Brewing", new LessonTime(17, 00), "Some lecturer", 403);
            var ognpMondayLesson2 = new Lesson("Brewing", new LessonTime(18, 40), "Some lecturer", 331);

            var ognpFridayLesson1 = new Lesson("Brewing", new LessonTime(17, 00), "Some lecturer 2", 403);
            var ognpFridayLesson2 = new Lesson("Brewing", new LessonTime(18, 40), "Some lecturer 2", 331);

            var ognpMondayLessons = new List<Lesson> {ognpMondayLesson1, ognpMondayLesson2};
            var ognpFridayLessons = new List<Lesson> {ognpFridayLesson1, ognpFridayLesson2};

            var brewingSchedule = new Schedule(new Dictionary<StudyingDays, List<Lesson>>
            {
                {StudyingDays.Monday, ognpMondayLessons},
                {StudyingDays.Friday, ognpFridayLessons},
            });

            StudyingStream brewingStream1 = brewingOgnp.AddStudyingStream(brewingSchedule, maxAmountOfStudents: 1);
            StudyingStream brewingStream2 = brewingOgnp.AddStudyingStream(brewingSchedule, maxAmountOfStudents: 3);

            ExtraStudent student1 = Fitp.AddStudent(_m3201, "Some student");
            ExtraStudent student2 = Fitp.AddStudent(_m3201, "Some student");
            ExtraStudent student3 = Fitp.AddStudent(_m3201, "Some student");

            Assert.Catch<IsuException>(() =>
            {
                IsuExtraService.AddStudentToOgnpStream(student1, brewingStream1);
                IsuExtraService.AddStudentToOgnpStream(student2, brewingStream1);
                IsuExtraService.AddStudentToOgnpStream(student3, brewingStream2);
            });
        }

        [Test]
        public void FindStudentsWithoutOgnp_Success()
        {
            OgnpCourse brewingOgnp = Btins.AddOgnpCourse("Brewing5", maxAmountOfStudents: 3);

            var ognpMondayLesson1 = new Lesson("Brewing", new LessonTime(17, 00), "Some lecturer", 403);
            var ognpMondayLesson2 = new Lesson("Brewing", new LessonTime(18, 40), "Some lecturer", 331);

            var ognpFridayLesson1 = new Lesson("Brewing", new LessonTime(17, 00), "Some lecturer 2", 403);
            var ognpFridayLesson2 = new Lesson("Brewing", new LessonTime(18, 40), "Some lecturer 2", 331);

            var ognpMondayLessons = new List<Lesson> {ognpMondayLesson1, ognpMondayLesson2};
            var ognpFridayLessons = new List<Lesson> {ognpFridayLesson1, ognpFridayLesson2};

            var brewingSchedule = new Schedule(new Dictionary<StudyingDays, List<Lesson>>
            {
                {StudyingDays.Monday, ognpMondayLessons},
                {StudyingDays.Friday, ognpFridayLessons},
            });

            StudyingStream brewingStream1 = brewingOgnp.AddStudyingStream(brewingSchedule, maxAmountOfStudents: 3);

            ExtraStudent student1 = Fitp.AddStudent(_m3201, "Some student");

            IsuExtraService.AddStudentToOgnpStream(student1, brewingStream1);

            List<ExtraStudent> studentsWithoutOgnpFromIsu = Fitp.GetStudentsWithoutOgnp(_m3201);

            // Isu is static (because of singleton) => students added in previous tests are exist too 
            // That's why I use Contain instead of Equals or smth like Equals
            foreach (ExtraStudent studentWithoutOgnp in studentsWithoutOgnpFromIsu)
                Assert.IsTrue(Fitp.GetStudentsWithoutOgnp(_m3201).Contains(studentWithoutOgnp));
        }

        [Test]
        public void AddStudentToGroupReferenceNotExistingInIsu_Exception()
        {
            var m3201NotFacultyGroup = new ExtraGroup("M3201", _m3201Schedule, Fitp);

            Assert.AreNotSame(m3201NotFacultyGroup, _m3201);
            Assert.Catch<IsuException>(() => Fitp.AddStudent(m3201NotFacultyGroup, "Misha"));
        }

        [Test]
        public void AddStudentToOgnpStreamReferenceNotExistingInIsu_Exception()
        {
            var notIsuStudent = new ExtraStudent("Misha Libchenko");

            OgnpCourse course = Fitp.AddOgnpCourse("Some course", 1);
            StudyingStream stream1 = course.AddStudyingStream(new Schedule(), 1);

            Assert.Catch<IsuException>(() => IsuExtraService.AddStudentToOgnpStream(notIsuStudent, stream1));
        }
    }
}