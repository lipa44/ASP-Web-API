using Isu.Tools;
using static System.HashCode;

namespace IsuExtra.Types
{
    public class LessonTime
    {
        public LessonTime(uint hour, uint minute)
        {
            if (!IsLessonTimeCorrect(hour, minute)) throw new IsuException("Lesson time is incorrect");
            Hour = hour;
            Minute = minute;
        }

        public uint Hour { get; }
        public uint Minute { get; }

        public override bool Equals(object? obj) => Equals(obj as LessonTime);
        public override int GetHashCode() => Combine(Hour, Minute);

        private bool IsLessonTimeCorrect(uint hour, uint minute) => hour switch
        {
            8 => minute == 20,
            10 => minute == 00,
            11 => minute == 40,
            13 => minute == 30,
            15 => minute == 20,
            17 => minute == 00,
            18 => minute == 40,
            _ => false
        };

        private bool Equals(LessonTime? lessonTime) => lessonTime is not null && lessonTime.Hour == Hour && lessonTime.Minute == Minute;
    }
}