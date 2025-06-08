// Controllers/TrafficReportsController.cs
using Microsoft.AspNetCore.Mvc;
using TrafficApiDotNet.Models; // Using your model's namespace
using System.Collections.Generic;
using System.Linq;

namespace TrafficApiDotNet.Controllers // Ensure this namespace matches your project name
{
  [ApiController]
  [Route("api/[controller]")] // Route will be /api/TrafficReports
  public class TrafficReportsController : ControllerBase
  {
    // In-memory list to store traffic reports (for simplicity)
    private static List<TrafficReport> _trafficReports = new List<TrafficReport>()
        {
            new TrafficReport { Id = 1, LocationName = "Main St & 1st Ave", ReportTime = DateTime.UtcNow.AddHours(-1), CongestionLevel = 2, Description = "Moderate due to rush hour." },
            new TrafficReport { Id = 2, LocationName = "Highway 101 Exit 5", ReportTime = DateTime.UtcNow.AddMinutes(-30), CongestionLevel = 3, Description = "Accident reported." }
        };
    private static int _nextId = 3;

    // GET: api/TrafficReports
    [HttpGet]
    public ActionResult<IEnumerable<TrafficReport>> GetAllTrafficReports()
    {
      // Simulate some work for profiling if APM is attached
      for (int i = 0; i < 10000; i++) { /* no-op */ }
      return Ok(_trafficReports);
    }

    // GET: api/TrafficReports/{id}
    [HttpGet("{id}")]
    public ActionResult<TrafficReport> GetTrafficReportById(int id)
    {
      // Simulate some work
      for (int i = 0; i < 5000; i++) { /* no-op */ }
      var report = _trafficReports.FirstOrDefault(r => r.Id == id);
      if (report == null)
      {
        return NotFound($"Traffic report with ID {id} not found.");
      }
      return Ok(report);
    }

    // POST: api/TrafficReports
    [HttpPost]
    public ActionResult<TrafficReport> CreateTrafficReport([FromBody] TrafficReport newReport)
    {
      if (newReport == null || !ModelState.IsValid)
      {
        return BadRequest(ModelState);
      }

      newReport.Id = _nextId++;
      newReport.ReportTime = DateTime.UtcNow;
      _trafficReports.Add(newReport);

      return CreatedAtAction(nameof(GetTrafficReportById), new { id = newReport.Id }, newReport);
    }

    // PUT: api/TrafficReports/{id}
    [HttpPut("{id}")]
    public IActionResult UpdateTrafficReport(int id, [FromBody] TrafficReport updatedReport)
    {
      if (updatedReport == null || id != updatedReport.Id || !ModelState.IsValid)
      {
        return BadRequest(ModelState);
      }

      var existingReport = _trafficReports.FirstOrDefault(r => r.Id == id);
      if (existingReport == null)
      {
        return NotFound($"Traffic report with ID {id} not found.");
      }

      existingReport.LocationName = updatedReport.LocationName;
      existingReport.CongestionLevel = updatedReport.CongestionLevel;
      existingReport.Description = updatedReport.Description;
      existingReport.ReportTime = updatedReport.ReportTime != default ? updatedReport.ReportTime : existingReport.ReportTime;

      return NoContent();
    }

    // DELETE: api/TrafficReports/{id}
    [HttpDelete("{id}")]
    public IActionResult DeleteTrafficReport(int id)
    {
      var reportToDelete = _trafficReports.FirstOrDefault(r => r.Id == id);
      if (reportToDelete == null)
      {
        return NotFound($"Traffic report with ID {id} not found.");
      }

      _trafficReports.Remove(reportToDelete);
      return NoContent();
    }
  }
}

/*

```    ***Added No - Op Loops: **I've added simple `for` loops in `GetAllTrafficReports` and `GetTrafficReportById` to simulate a tiny bit of CPU work. This will make CPU flame graphs (if you enable profiling later) slightly more interesting than if the methods did almost nothing. You can remove these loops if you prefer.
* **Namespace Check: **Again, ensure `namespace TrafficApiDotNet.Controllers` and `using TrafficApiDotNet.Models;` align with your project's naming.

*/