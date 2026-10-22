namespace ClubHub.DTOs;

public class AnalyticsSummaryResponse
{
    public int ActiveClubs { get; set; }
    public int LiveEvents { get; set; }
    public int PendingReviews { get; set; }
    public int TotalEnrollments { get; set; }
}