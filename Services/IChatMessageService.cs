using ClubHub.DTOs;

namespace ClubHub.Services;

public interface IChatMessageService
{
    Task<ChatMessageResponse?> SendAsync(CreateChatMessageRequest request);
    Task<List<ChatMessageResponse>?> GetByClubAsync(int clubId);
    Task<List<ChatMessageResponse>?> GetAfterIdAsync(int clubId, int afterId);
    Task<bool> DeleteAsync(int chatMessageId);
}