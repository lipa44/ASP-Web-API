#nullable enable
using System;
using System.Text.RegularExpressions;
using Isu.Tools;

namespace Isu.Entities
{
    public class GroupName
    {
        public GroupName(string groupName)
        {
            if (groupName is null) throw new IsuException("Group name is null");

            if (!Regex.IsMatch(groupName, @"^[M-Z]{1}3[1-4]{1}0[0-9]{1}|[M-Z]{1}3[1-4]{1}1[0-4]{1}$", RegexOptions.IgnoreCase))
                throw new IsuException($"Wrong group name: {groupName}");

            Name = groupName.ToUpper();
        }

        public string Name { get; }
        public override bool Equals(object? obj) => Equals(obj as GroupName);
        public override int GetHashCode() => HashCode.Combine(Name);
        private bool Equals(GroupName? groupName) => groupName is not null && groupName.Name == Name;
    }
}