// Models/TrafficReport.cs
using System.ComponentModel.DataAnnotations;

namespace TrafficApiDotNet.Models // Ensure this namespace matches your project name
{
  public class TrafficReport
  {
    public int Id { get; set; } // Unique identifier

    [Required]
    [StringLength(100)]
    public string? LocationName { get; set; }

    [Required]
    public DateTime ReportTime { get; set; }

    [Required]
    [Range(0, 5)] // 0: Clear, 1: Light, 2: Moderate, 3: Heavy, 4: Standstill, 5: Road Closed
    public int CongestionLevel { get; set; }

    public string? Description { get; set; }
  }
}

