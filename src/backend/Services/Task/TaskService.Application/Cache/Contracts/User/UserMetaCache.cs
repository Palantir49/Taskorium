namespace TaskService.Application.Cache.Contracts.User;

public record class UserMetaCache(
    Guid Id,
    string UserName,
    string Email,
    string FullName);
