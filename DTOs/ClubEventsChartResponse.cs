namespace ClubHub.DTOs;

public class ClubEventsChartResponse
{
    public int ClubId { get; set; }
    public string ClubName { get; set; } = string.Empty;
    public int EventCount { get; set; }
}