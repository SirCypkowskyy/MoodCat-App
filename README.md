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
- CQRS (Command-query Separation
- SoC (Separation of Concerns)
- Monolit

## Dobór Technologii

- ASP.NET Core 8.0
- EntityFramework Core
- Figma
- React
- TypeScript
- Swagger
- Azure
- GitHub
- ChatGPT
- Whisper
- Tailwind
- Vite
- Shadcn
- Tanstack
- Openapi-fetch
- C#
- FluentValidaton
- Mapster
- Microsoft Identity
- Swashbuckle
