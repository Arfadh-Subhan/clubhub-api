namespace ClubHub.DTOs;

public class ClubMembersChartResponse
{
    public int ClubId { get; set; }
    public string ClubName { get; set; } = string.Empty;
    public int MemberCount { get; set; }
}