using ClubHub.Data;
using ClubHub.DTOs;
using ClubHub.Models;
using Microsoft.EntityFrameworkCore;

namespace ClubHub.Services;

public class ClubMeetingService : IClubMeetingService
{
    private readonly AppDbContext _context;

    public ClubMeetingService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<ClubMeetingResponse?> CreateAsync(CreateClubMeetingRequest request)
    {
        var clubMeeting = new ClubMeeting
        {
            ClubId = request.ClubId,
            Title = request.Title.Trim(),
            Description = request.Description.Trim(),
            Location = request.Location.Trim(),
            MeetingDate = request.MeetingDate,
            MeetingTime = request.MeetingTime.Trim(),
            Links = request.Links,
            CreatedBy = request.CreatedBy,
            CreatedAt = DateTime.UtcNow,
            IsActive = true
        };

        _context.ClubMeetings.Add(clubMeeting);
        await _context.SaveChangesAsync();

        return MapToResponse(clubMeeting);
    }

    public async Task<List<ClubMeetingResponse>?> GetByClubAsync(int clubId)
    {
        return await _context.ClubMeetings
            .AsNoTracking()
            .Where(clubMeeting => clubMeeting.ClubId == clubId && clubMeeting.IsActive)
            .OrderBy(clubMeeting => clubMeeting.MeetingDate)
            .Select(clubMeeting => new ClubMeetingResponse
            {
                ClubMeetingId = clubMeeting.ClubMeetingId,
                ClubId = clubMeeting.ClubId,
                Title = clubMeeting.Title,
                Description = clubMeeting.Description,
                Location = clubMeeting.Location,
                MeetingDate = clubMeeting.MeetingDate,
                MeetingTime = clubMeeting.MeetingTime,
                Links = clubMeeting.Links,
                CreatedBy = clubMeeting.CreatedBy,
                CreatedAt = clubMeeting.CreatedAt,
                UpdatedAt = clubMeeting.UpdatedAt,
                IsActive = clubMeeting.IsActive
            })
            .ToListAsync();
    }

    public async Task<ClubMeetingResponse?> GetByIdAsync(int clubMeetingId)
    {
        return await _context.ClubMeetings
            .AsNoTracking()
            .Where(clubMeeting => clubMeeting.ClubMeetingId == clubMeetingId)
            .Select(clubMeeting => new ClubMeetingResponse
            {
                ClubMeetingId = clubMeeting.ClubMeetingId,
                ClubId = clubMeeting.ClubId,
                Title = clubMeeting.Title,
                Description = clubMeeting.Description,
                Location = clubMeeting.Location,
                MeetingDate = clubMeeting.MeetingDate,
                MeetingTime = clubMeeting.MeetingTime,
                Links = clubMeeting.Links,
                CreatedBy = clubMeeting.CreatedBy,
                CreatedAt = clubMeeting.CreatedAt,
                UpdatedAt = clubMeeting.UpdatedAt,
                IsActive = clubMeeting.IsActive
            })
            .FirstOrDefaultAsync();
    }

    public async Task<ClubMeetingResponse?> UpdateAsync(int clubMeetingId, UpdateClubMeetingRequest request)
    {
        var clubMeeting = await _context.ClubMeetings.FindAsync(clubMeetingId);

        if (clubMeeting == null)
        {
            return null;
        }

        clubMeeting.Title = request.Title.Trim();
        clubMeeting.Description = request.Description.Trim();
        clubMeeting.Location = request.Location.Trim();
        clubMeeting.MeetingDate = request.MeetingDate;
        clubMeeting.MeetingTime = request.MeetingTime.Trim();
        clubMeeting.Links = request.Links;
        clubMeeting.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return MapToResponse(clubMeeting);
    }

    public async Task<bool> DeleteAsync(int clubMeetingId)
    {
        var clubMeeting = await _context.ClubMeetings.FindAsync(clubMeetingId);

        if (clubMeeting == null)
        {
            return false;
        }

        clubMeeting.IsActive = false;
        clubMeeting.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return true;
    }

    private static ClubMeetingResponse MapToResponse(ClubMeeting clubMeeting)
    {
        return new ClubMeetingResponse
        {
            ClubMeetingId = clubMeeting.ClubMeetingId,
            ClubId = clubMeeting.ClubId,
            Title = clubMeeting.Title,
            Description = clubMeeting.Description,
            Location = clubMeeting.Location,
            MeetingDate = clubMeeting.MeetingDate,
            MeetingTime = clubMeeting.MeetingTime,
            Links = clubMeeting.Links,
            CreatedBy = clubMeeting.CreatedBy,
            CreatedAt = clubMeeting.CreatedAt,
            UpdatedAt = clubMeeting.UpdatedAt,
            IsActive = clubMeeting.IsActive
        };
    }
}