namespace MoodCat.App.Core.Application.DTOs.Users;

/// <summary>
/// Obiekt transferu danych informacji o użytkowniku
/// </summary>
/// <param name="Username"></param>
/// <param name="Email"></param>
/// <param name="PhoneNumber"></param>
public record UserResponseDTO(
    string Username,
    string Email,
    string? PhoneNumber
);