# MoodCat (backend)

Backend aplikacji zespołu `!jva` na konkurs [HackYeah 2024](https://hackyeah.pl/). 

## Spis treści

1. [Opis](#Opis)
2. [Architektura](#Architektura)
3. [Dobór Technologii](#Dobór-Technologii)

## Opis

Aplilkacja MoodCat jest pośrednikiem między terapeutą/lekarzem, a pacjentem.

Pacjant tworzy notatki poprzez odpowiadanie na krótkie pytania dotyczące jego samopoczucia za pośrednictwem OpenAI Whisper (Speech-to-Text model) lub wpisuje zawartość ręznie.
Pod koniec dnia OpenAI ChatGPT tworzy podsumowanie na podstawie w.w. notatek.

Lekarz ma dostęp do danych swoich pacjentów, na podstawie których może planować podalszą terapię.

## Architektura

- DDD (Domain-Driven Design)
- CQRS (Command and Query Responsibility Segregation)
- SoC (Separation of Concerns)
- Monolit

## Dobór Technologii

**Środowisko**:
- ASP.NET Core 8.0
- C#


**ORM**:
- EntityFramework Core

**Api-Explorer**:
- Swagger
- Swashbuckle

**CI/CD**:
- Azure
- GitHub

**Zewnętrzne serwisy-usługi**:
- OpenAI ChatGPT
- OpenAI Whisper

**Paczki Nuget**:
- FluentValidaton - walidacja 
- Carter - Lepsza obsługa minimal API
- Mapster - konwersja obiektów DTO na modele i vice versa
- Microsoft Identity - autoscaffoldowana autoryzacja i autentykacja
- Mediator - delegowanie zapytań do wykonania
