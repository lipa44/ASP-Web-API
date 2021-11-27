using System.Collections.Generic;
using Isu.Tools;

namespace Isu.Entities
{
    public class Group
    {
        private readonly GroupName _groupName;

        public Group(GroupName groupName)
        {
            Students = new List<Student>();
            _groupName = groupName ?? throw new IsuException("Group name can not be null");
        }

        public List<Student> Students { get; }
        public string GroupName => _groupName.Name;
    }
}