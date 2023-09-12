using System;

namespace CodeChallenge.Models
{
    public class Compensation
    {
        public int Id { get; set; }
        // The employee id for who the compensation is defined
        public String EmployeeId { get; set; }
        // The salary of the employee
        public decimal Salary { get; set; }
        // The date from which the compensation is effective
        public DateTime EffectiveDate { get; set; }
    }

}
