using Microsoft.AspNetCore.Identity;
using MoodCat.App.Core.Domain.DaySummaries;

namespace MoodCat.App.Core.Domain.Users;

/// <summary>
/// Encja użytkownika   
/// </summary>
public class User : IdentityUser
{
    /// <summary>
    /// Czy użytkownik jest specjalistą (lekarzem, terapeutą), czy zwykłym
    /// użytkownikiem
    /// </summary>
    public bool IsHealthSpecialist { get; set; }
    
    /// <summary>
    /// Id specjalisty przypisanego do pacjenta
    /// </summary>
    public string? AssignedSpecialistId { get; set; }
    
    /// <summary>
    /// Podsumowania dnia
    /// </summary>
    private readonly List<DaySummaryEntity> _daySummaries = [];
    
    /// <summary>
    /// Podsumowania dnia użytkownika
    /// </summary>
    public IReadOnlyList<DaySummaryEntity> DaySummaries => _daySummaries.AsReadOnly();
}