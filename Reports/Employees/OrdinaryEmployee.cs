using System.Collections.Generic;

namespace Reports.Employees
{
    public class OrdinaryEmployee : Employee
    {
        private List<OrdinaryEmployee> _subordinates;

        public OrdinaryEmployee(string name, string surname)
            : base(name, surname)
        {
            _subordinates = new List<OrdinaryEmployee>();
        }
    }
}