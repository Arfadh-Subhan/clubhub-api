using ClubHub.Data;
using ClubHub.DTOs;
using Microsoft.EntityFrameworkCore;

namespace ClubHub.Services;

public class AnalyticsService : IAnalyticsService
{
    private readonly AppDbContext _context;

    public AnalyticsService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<AnalyticsSummaryResponse> GetSummaryAsync()
    {
        var now = DateTime.UtcNow;

        int activeClubs = await _context.Clubs
            .CountAsync(club => club.IsActive);

        int liveEvents = await _context.Events
            .CountAsync(eventItem => eventItem.IsActive && eventItem.EventStartAt >= now);

        int pendingReviews = await _context.Events
            .CountAsync(eventItem => eventItem.IsActive && eventItem.Status == "Pending")
            + await _context.ClubPosts
            .CountAsync(clubPost => clubPost.IsActive && clubPost.Status == "Pending")
            + await _context.ClubAnnouncements
            .CountAsync(clubAnnouncement => clubAnnouncement.IsActive && clubAnnouncement.Status == "Pending")
            + await _context.ClubProducts
            .CountAsync(clubProduct => clubProduct.IsActive && clubProduct.Status == "Pending")
            + await _context.ClubRequests
            .CountAsync(clubRequest => clubRequest.Status == "Pending");

        int totalEnrollments = await _context.Orders
            .CountAsync(order => order.Status != "Cancelled");

        return new AnalyticsSummaryResponse
        {
            ActiveClubs = activeClubs,
            LiveEvents = liveEvents,
            PendingReviews = pendingReviews,
            TotalEnrollments = totalEnrollments
        };
    }

    public async Task<List<ClubMembersChartResponse>> GetMembersByClubAsync()
    {
        return await _context.Clubs
            .AsNoTracking()
            .Where(club => club.IsActive)
            .OrderBy(club => club.Name)
            .Select(club => new ClubMembersChartResponse
            {
                ClubId = club.ClubId,
                ClubName = club.Name,
                MemberCount = _context.ClubMembers.Count(clubMember => clubMember.ClubId == club.ClubId && clubMember.Status == "Approved")
            })
            .ToListAsync();
    }

    public async Task<List<ClubEventsChartResponse>> GetEventsByClubAsync()
    {
        return await _context.Clubs
            .AsNoTracking()
            .Where(club => club.IsActive)
            .OrderBy(club => club.Name)
            .Select(club => new ClubEventsChartResponse
            {
                ClubId = club.ClubId,
                ClubName = club.Name,
                EventCount = _context.Events.Count(eventItem => eventItem.ClubId == club.ClubId && eventItem.IsActive)
            })
            .ToListAsync();
    }
}