using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CodeChallenge.Services;
using CodeChallenge.Models;

namespace CodeChallenge.Controllers
{
    [ApiController]
    [Route("api/employee")]
    public class EmployeeController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IEmployeeService _employeeService;

        public EmployeeController(ILogger<EmployeeController> logger, IEmployeeService employeeService)
        {
            _logger = logger;
            _employeeService = employeeService;
        }

        [HttpPost]
        public IActionResult CreateEmployee([FromBody] Employee employee)
        {
            _logger.LogDebug($"Received employee create request for '{employee.FirstName} {employee.LastName}'");

            _employeeService.Create(employee);

            return CreatedAtRoute("getEmployeeById", new { id = employee.EmployeeId }, employee);
        }

        [HttpGet("{id}", Name = "getEmployeeById")]
        public IActionResult GetEmployeeById(String id)
        {
            _logger.LogDebug($"Received employee get request for '{id}'");

            var employee = _employeeService.GetById(id);

            if (employee == null)
                return NotFound();

            return Ok(employee);
        }

        [HttpPut("{id}")]
        public IActionResult ReplaceEmployee(String id, [FromBody]Employee newEmployee)
        {
            _logger.LogDebug($"Recieved employee update request for '{id}'");

            var existingEmployee = _employeeService.GetById(id);
            if (existingEmployee == null)
                return NotFound();

            _employeeService.Replace(existingEmployee, newEmployee);

            return Ok(newEmployee);
        }

        [HttpGet("{id}/reportingstructure")]
        public ActionResult<ReportingStructure> GetReportingStructure(string id)
        {
            // Retrieve the employee using the provided id
            var employee = _employeeService.GetById(id);

            // If no employee found, return a 404 Not Found response
            if (employee == null)
            {
                return NotFound();
            }

            // Create the reporting structure for the employee
            var reportingStructure = new ReportingStructure
            {
                Employee = employee,
                NumberOfReports = GetNumberOfReports(employee)
            };

            // Return the reporting structure in the response
            return Ok(reportingStructure);
        }

        // Recursive function to calculate the total number of reports under a given employee
        private int GetNumberOfReports(Employee employee)
        {
            // if no direct reports return 0
            if (employee == null || employee.DirectReports == null || !employee.DirectReports.Any())
            {
                return 0;
            }

            // Count direct reports of the employee
            int count = employee.DirectReports.Count;

            foreach (var report in employee.DirectReports)
            {
                // Recursively calculate the reports for each direct report
                var directReportEmployee = _employeeService.GetById(report.EmployeeId);
                count += GetNumberOfReports(directReportEmployee);
            }

            return count;
        }
    }
}
