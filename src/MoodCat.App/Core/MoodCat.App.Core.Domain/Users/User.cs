using Microsoft.AspNetCore.Identity;
using MoodCat.App.Core.Domain.Users.ValueObjects;

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
    /// Id użytkownika w formie scastowanej
    /// </summary>
    public UserId CastedUserId => UserId.Of(Guid.Parse(Id));
}