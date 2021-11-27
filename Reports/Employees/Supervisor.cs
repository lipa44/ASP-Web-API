using System.Collections.Generic;

namespace Reports.Employees
{
    public class Supervisor : Employee
    {
        private List<OrdinaryEmployee> _employees;

        public Supervisor(string name, string surname)
            : base(name, surname)
        {
            _employees = new List<OrdinaryEmployee>();
        }
    }
}