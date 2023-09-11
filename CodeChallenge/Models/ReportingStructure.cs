namespace CodeChallenge.Models
{
    public class ReportingStructure
    {
        // The employee for whom the report is being generated
        public Employee Employee { get; set; }
        // Total number of direct and indirect reports under this employee
        public int NumberOfReports { get; set; }    
    }

}
