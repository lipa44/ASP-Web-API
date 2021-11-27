using System;
using System.Collections.Generic;
using System.Linq;
using Isu.Tools;
using IsuExtra.Tools;
using IsuExtra.Types;

namespace IsuExtra.Entities
{
    public class Schedule : SchedulePrototype
    {
        public Schedule() { }

        public Schedule(IReadOnlyDictionary<StudyingDays, List<Lesson>> schedule)
        : base(schedule) { }

        public IReadOnlyDictionary<StudyingDays, List<Lesson>> DaysLessonsPairs => Schedule;
        public override Schedule Clone() => new (Schedule);

        public void AddLessons(IReadOnlyDictionary<StudyingDays, List<Lesson>> lessons)
        {
            foreach ((StudyingDays day, List<Lesson> lLessons) in lessons)
                AddLessonsForDay(day, lLessons);
        }

        public void RemoveLessons(IReadOnlyDictionary<StudyingDays, List<Lesson>> lessons)
        {
            foreach ((StudyingDays day, List<Lesson> lLessons) in lessons)
                RemoveLessonsForDay(day, lLessons);
        }

        private void AddLessonsForDay(StudyingDays day, List<Lesson> lessons)
        {
            if (HasAnyCrossingInSchedule(day, lessons)) throw new IsuException("Has crossing in schedule");

            Schedule[day].AddRange(lessons);
        }

        private void RemoveLessonsForDay(StudyingDays day, List<Lesson> lessons)
        {
            if (lessons is null) throw new IsuException("Lessons to remove are null", new ArgumentException());

            foreach (Lesson lesson in lessons) Schedule[day].Remove(lesson);
        }

        private bool HasAnyCrossingInSchedule(StudyingDays day, List<Lesson> lessons)
        {
            if (lessons is null) throw new IsuException("Lessons is null", new ArgumentException());

            return Schedule[day]
                .Any(existedLesson => lessons
                    .Any(lesson => Equals(lesson.StartTime, existedLesson.StartTime) && !Equals(existedLesson, lesson)));
        }
    }
}