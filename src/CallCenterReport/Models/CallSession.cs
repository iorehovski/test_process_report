namespace CallCenterReport.Models;

public class CallSession
{
    public DateTime SessionStart { get; set; }
    public DateTime SessionEnd { get; set; }
    public string Project { get; set; }
    public string Operator { get; set; }
    public string State { get; set; }
    public int Duration { get; set; }
}