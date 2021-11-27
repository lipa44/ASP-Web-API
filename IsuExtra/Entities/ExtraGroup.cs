using System;
using Isu.Entities;
using Isu.Tools;

namespace IsuExtra.Entities
{
    public class ExtraGroup
    {
        private readonly Schedule _schedule;

        public ExtraGroup(string name, Schedule schedule, MegaFaculty megaFaculty)
        {
            GroupName = new GroupName(name);
            _schedule = schedule.Clone()
                        ?? throw new IsuException("Schedule to create group is null", new ArgumentException());
            MegaFaculty = megaFaculty
                           ?? throw new IsuException($"Mega faculty to create group {GroupName} is null", new ArgumentException());
        }

        public GroupName GroupName { get; }
        public Schedule Schedule => _schedule.Clone();
        public MegaFaculty MegaFaculty { get; }
    }
}