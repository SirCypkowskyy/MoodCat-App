using MoodCat.App.Common.BuildingBlocks.Abstractions.DDD;

namespace MoodCat.App.Core.Domain.Notes.Events;

/// <summary>
/// Zdarzenie domenowe reprezentujące utworzenie notatki.
/// </summary>
/// <param name="Note">
/// Utworzona notatka.
/// </param>
public record NoteCreatedDomainEvent(NoteEntity Note) : IDomainEvent;