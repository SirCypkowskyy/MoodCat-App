using MoodCat.App.Common.BuildingBlocks.Abstractions.DDD;
using MoodCat.App.Core.Domain.Notes.ValueObjects;

namespace MoodCat.App.Core.Domain.Notes;
//
// /// <summary>
// /// Pytanie do notatki (nadesłane przez specjalistę)
// /// </summary>
// public class NoteQuestionEntity : Entity<long>
// {
//     /// <summary>
//     /// Id specjalisty, który zadał pytanie
//     /// </summary>
//     public string SpecialistId { get; private set; }
//
//     /// <summary>
//     /// Pytanie zadane przez specjalistę
//     /// </summary>
//     public string Question { get; private set; }
//
//     /// <summary>
//     /// Użytkownik, któremu zadano pytanie
//     /// </summary>
//     public string UserId { get; private set; }
//
//     /// <summary>
//     /// Domyślna, maksymalna długość pytania
//     /// </summary>
//     public const int DefaultMaxQuestionLength = 125;
//
//
//     /// <summary>
//     /// Tworzy nowe pytanie
//     /// </summary>
//     /// <param name="specialistId">
//     /// Id specjalisty, który zadał pytanie
//     /// </param>
//     /// <param name="question">
//     /// Pytanie, które zostało zapytane pacjentowi
//     /// </param>
//     /// <param name="userId"></param>
//     /// <returns></returns>
//     public static NoteQuestionEntity Create(string specialistId, string question, string userId)
//     {
//         ArgumentException.ThrowIfNullOrWhiteSpace(question);
//         ArgumentException.ThrowIfNullOrWhiteSpace(userId);
//         ArgumentException.ThrowIfNullOrWhiteSpace(specialistId);
//         
//         if(question.Length > DefaultMaxQuestionLength)
//             throw new ArgumentException($"{nameof(question)} must be less than {DefaultMaxQuestionLength} characters.");
//
//         return new NoteQuestionEntity()
//         {
//             SpecialistId = specialistId,
//             Question = question,
//             UserId = userId
//         };
//     }
// }