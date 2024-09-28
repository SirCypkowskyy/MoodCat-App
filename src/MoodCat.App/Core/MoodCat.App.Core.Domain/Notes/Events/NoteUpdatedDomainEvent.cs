using MoodCat.App.Common.BuildingBlocks.Abstractions.DDD;

namespace MoodCat.App.Core.Domain.Notes.Events;

/// <summary>
/// Zdarzenie domenowe reprezentujące aktualizację notatki.
/// </summary>
/// <param name="Note">
/// Aktualizowana notatka.
/// </param>
public record NoteUpdatedDomainEvent(NoteEntity Note) : IDomainEvent;