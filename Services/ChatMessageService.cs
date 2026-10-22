using ClubHub.Data;
using ClubHub.DTOs;
using ClubHub.Models;
using Microsoft.EntityFrameworkCore;

namespace ClubHub.Services;

public class ChatMessageService : IChatMessageService
{
    private readonly AppDbContext _context;

    public ChatMessageService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<ChatMessageResponse?> SendAsync(CreateChatMessageRequest request)
    {
        bool isMember = await _context.ClubMembers
            .AnyAsync(clubMember => clubMember.ClubId == request.ClubId
                && clubMember.UserId == request.UserId
                && clubMember.Status == "Approved");

        if (!isMember)
        {
            return null;
        }

        var chatMessage = new ChatMessage
        {
            ClubId = request.ClubId,
            UserId = request.UserId,
            Message = request.Message.Trim(),
            SentAt = DateTime.UtcNow,
            IsDeleted = false
        };

        _context.ChatMessages.Add(chatMessage);
        await _context.SaveChangesAsync();

        var club = await _context.Clubs
            .AsNoTracking()
            .FirstOrDefaultAsync(club => club.ClubId == request.ClubId);

        string clubName = club != null ? club.Name : "your club";

        var otherMemberIds = await _context.ClubMembers
            .AsNoTracking()
            .Where(clubMember => clubMember.ClubId == request.ClubId
                && clubMember.Status == "Approved"
                && clubMember.UserId != request.UserId)
            .Select(clubMember => clubMember.UserId)
            .ToListAsync();

        var notificationTime = DateTime.UtcNow;

        foreach (var memberId in otherMemberIds)
        {
            _context.Notifications.Add(new Notification
            {
                UserId = memberId,
                Title = "New message in " + clubName,
                Message = chatMessage.Message,
                Type = "ChatMessage",
                IsRead = false,
                CreatedAt = notificationTime
            });
        }

        await _context.SaveChangesAsync();

        return await MapToResponseAsync(chatMessage);
    }

    public async Task<List<ChatMessageResponse>?> GetByClubAsync(int clubId)
    {
        var messages = await _context.ChatMessages
            .AsNoTracking()
            .Where(chatMessage => chatMessage.ClubId == clubId && !chatMessage.IsDeleted)
            .OrderByDescending(chatMessage => chatMessage.SentAt)
            .Take(50)
            .OrderBy(chatMessage => chatMessage.SentAt)
            .ToListAsync();

        return await MapListAsync(messages);
    }

    public async Task<List<ChatMessageResponse>?> GetAfterIdAsync(int clubId, int afterId)
    {
        var messages = await _context.ChatMessages
            .AsNoTracking()
            .Where(chatMessage => chatMessage.ClubId == clubId
                && chatMessage.ChatMessageId > afterId
                && !chatMessage.IsDeleted)
            .OrderBy(chatMessage => chatMessage.ChatMessageId)
            .ToListAsync();

        return await MapListAsync(messages);
    }

    public async Task<bool> DeleteAsync(int chatMessageId)
    {
        var chatMessage = await _context.ChatMessages.FindAsync(chatMessageId);

        if (chatMessage == null)
        {
            return false;
        }

        chatMessage.IsDeleted = true;

        await _context.SaveChangesAsync();

        return true;
    }

    private async Task<List<ChatMessageResponse>> MapListAsync(List<ChatMessage> messages)
    {
        var userIds = messages.Select(chatMessage => chatMessage.UserId).Distinct().ToList();

        var users = await _context.Users
            .AsNoTracking()
            .Where(user => userIds.Contains(user.UserId))
            .ToListAsync();

        var clubId = messages.Count > 0 ? messages[0].ClubId : 0;

        var memberships = await _context.ClubMembers
            .AsNoTracking()
            .Where(clubMember => clubMember.ClubId == clubId && userIds.Contains(clubMember.UserId))
            .ToListAsync();

        var responses = new List<ChatMessageResponse>();

        foreach (var chatMessage in messages)
        {
            var user = users.FirstOrDefault(user => user.UserId == chatMessage.UserId);
            var membership = memberships.FirstOrDefault(clubMember => clubMember.UserId == chatMessage.UserId);

            responses.Add(new ChatMessageResponse
            {
                ChatMessageId = chatMessage.ChatMessageId,
                ClubId = chatMessage.ClubId,
                UserId = chatMessage.UserId,
                SenderName = user != null ? user.FullName : "User #" + chatMessage.UserId,
                SenderPosition = membership != null && !string.IsNullOrEmpty(membership.Position)
                    ? membership.Position
                    : (membership != null ? membership.ClubRole : ""),
                Message = chatMessage.Message,
                SentAt = chatMessage.SentAt
            });
        }

        return responses;
    }

    private async Task<ChatMessageResponse> MapToResponseAsync(ChatMessage chatMessage)
    {
        var user = await _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(user => user.UserId == chatMessage.UserId);

        var membership = await _context.ClubMembers
            .AsNoTracking()
            .FirstOrDefaultAsync(clubMember => clubMember.ClubId == chatMessage.ClubId
                && clubMember.UserId == chatMessage.UserId);

        return new ChatMessageResponse
        {
            ChatMessageId = chatMessage.ChatMessageId,
            ClubId = chatMessage.ClubId,
            UserId = chatMessage.UserId,
            SenderName = user != null ? user.FullName : "User #" + chatMessage.UserId,
            SenderPosition = membership != null && !string.IsNullOrEmpty(membership.Position)
                ? membership.Position
                : (membership != null ? membership.ClubRole : ""),
            Message = chatMessage.Message,
            SentAt = chatMessage.SentAt
        };
    }
}