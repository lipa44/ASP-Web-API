using System;
using System.Collections.Generic;
using System.Linq;
using IsuExtra.Entities;
using IsuExtra.Types;

namespace IsuExtra.Tools
{
    public abstract class SchedulePrototype
    {
        public SchedulePrototype() => Schedule = FillScheduleWithEmptyLessons();

        public SchedulePrototype(IReadOnlyDictionary<StudyingDays, List<Lesson>> schedule)
        {
            Schedule = FillScheduleWithEmptyLessons();
            foreach ((StudyingDays day, List<Lesson> lessons) in schedule)
                Schedule[day] = new List<Lesson>(lessons);
        }

        protected Dictionary<StudyingDays, List<Lesson>> Schedule { get; }

        public abstract SchedulePrototype Clone();

        private Dictionary<StudyingDays, List<Lesson>> FillScheduleWithEmptyLessons() => Enum
            .GetValues<StudyingDays>()
            .ToDictionary(day => day, lessons => new List<Lesson>());
    }
}