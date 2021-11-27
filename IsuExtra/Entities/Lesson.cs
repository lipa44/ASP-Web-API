using System;
using Isu.Tools;
using IsuExtra.Types;

namespace IsuExtra.Entities
{
    public class Lesson
    {
        public Lesson(string name, LessonTime startTime, string lecturerName, ushort audienceNumber)
        {
            if (string.IsNullOrEmpty(name))
                throw new IsuException("Lesson name is null", new ArgumentException());
            if (string.IsNullOrEmpty(lecturerName))
                throw new IsuException("Lecturer name is null", new ArgumentException());

            Name = name;
            StartTime = startTime;
            LecturerName = lecturerName;
            AudienceNumber = audienceNumber;
        }

        public string Name { get; }
        public LessonTime StartTime { get; }
        public string LecturerName { get; }
        public ushort AudienceNumber { get; }
    }
}