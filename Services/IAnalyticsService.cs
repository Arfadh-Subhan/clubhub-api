using ClubHub.DTOs;

namespace ClubHub.Services;

public interface IAnalyticsService
{
    Task<AnalyticsSummaryResponse> GetSummaryAsync();
    Task<List<ClubMembersChartResponse>> GetMembersByClubAsync();
    Task<List<ClubEventsChartResponse>> GetEventsByClubAsync();
}