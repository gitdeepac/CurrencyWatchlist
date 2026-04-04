using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Models
{
  public class AlertEvent
  {
    public int Id { get; set; }
    public int? AlertRuleId { get; set; }
    public DateTime TriggerAt { get; set; } = DateTime.UtcNow;
    public string Rate { get; set; } = String.Empty;
    public string Message { get; set; } = String.Empty;
  }
}