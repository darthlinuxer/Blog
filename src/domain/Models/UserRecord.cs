namespace Domain.Models;

public record UserRecordDTO(
    string username,
    string password,
    string email,
    string role
    );