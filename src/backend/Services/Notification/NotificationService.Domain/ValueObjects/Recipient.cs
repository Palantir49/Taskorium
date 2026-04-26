using NotificationService.Domain.Enums;
using Taskorium.IntegrationEvents.Dto;

namespace NotificationService.Domain.ValueObjects;

public record Recipient
{
    private Recipient(
        string userId,
        string fullName,
        string? email,
        string? phone,
        bool isMuted,
        List<ChannelType> preferredChannels)
    {
        if (string.IsNullOrWhiteSpace(userId))
        {
            throw new ArgumentException("UserId is required");
        }

        if (string.IsNullOrWhiteSpace(fullName))
        {
            throw new ArgumentException("FullName is required");
        }

        Id = RecipientId.FromUser(userId);
        UserId = userId;
        FullName = fullName;
        Email = email;
        Phone = phone;
        IsMuted = isMuted;
        PreferredChannels = preferredChannels?.AsReadOnly() ?? new List<ChannelType>().AsReadOnly();
    }

    public RecipientId Id { get; }
    public string UserId { get; }
    public string FullName { get; }
    public string? Email { get; }
    public string? Phone { get; }
    public bool IsMuted { get; }
    public IReadOnlyCollection<ChannelType> PreferredChannels { get; }

    public static Recipient FromIntegrationEvent(NotificationRecipient dto)
    {
        var preferredChannels = dto.PreferredChannels?
            .Select(c => Enum.Parse<ChannelType>(c, true))
            .ToList() ?? [];


        return new Recipient(
            dto.UserId,
            dto.FullName,
            dto.Email,
            dto.Phone,
            dto.IsMuted,
            preferredChannels);
    }

    public bool HasChannel(ChannelType channelType)
    {
        return channelType switch
        {
            ChannelType.Email => !string.IsNullOrWhiteSpace(Email),
            ChannelType.Sms => !string.IsNullOrWhiteSpace(Phone),
            _ => false
        };
    }

    public string? GetChannelAddress(ChannelType channelType)
    {
        return channelType switch
        {
            ChannelType.Email => Email,
            ChannelType.Sms => Phone,
            _ => null
        };
    }
}
