using Microsoft.AspNetCore.Identity;

namespace Domain.Models;

public record UserRecord(
    string username,
    string password,
    string email,
    string role
    );