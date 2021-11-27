using System.Collections.Generic;

namespace Reports.Employees
{
    public class TeamLead : Employee
    {
        private List<Supervisor> _supervisors;

        public TeamLead(string name, string surname)
            : base(name, surname)
        {
            _supervisors = new List<Supervisor>();
        }
    }
}