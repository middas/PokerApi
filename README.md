# Poker Game Solution

This repository contains a modular, testable implementation of a Poker game engine and a RESTful API built with .NET9 and C#13. The solution is organized into three main projects:

- `PokerLogic` - Core game logic, including deck, hand, and player management.
- `Poker.Api` - ASP.NET Core Web API exposing endpoints for dealing and evaluating poker hands.
- `PokerLogic.Tests` - NUnit-based unit tests for the core logic and coverage configuration.

---

## Projects Overview

### PokerLogic

- Implements interfaces and classes for cards, decks, hands, players, and game logic.
- Supports standard poker rules and hand evaluation.
- Designed for extensibility and testability.

### Poker.Api

- Provides REST endpoints to manage games and players and to deal/evaluate poker hands.
- Uses OpenAPI/Swagger for API documentation.
- Returns detailed error information using Problem Details.

### PokerLogic.Tests

- Contains NUnit tests that validate core logic and ensure correctness.
- Can be used with Coverlet for code coverage reporting.

---

## Prerequisites

- [.NET9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- (Optional) Visual Studio2022 or later or any editor that supports .NET9 projects

---

## Build and run

From the solution root:

```bash
dotnet restore
dotnet build
dotnet run --project Poker.Api
```

When running the API in development, OpenAPI/Swagger is available at `https://localhost:<port>/swagger` or `/openapi`.

---

## API Endpoints

The controller exposes the following endpoints (base route `/{controller}` => `/Poker`):

- `POST /Poker/New`
  - Creates a new game and optionally accepts a JSON array of player names to add to the game.
  - Body: optional JSON array of player names, e.g. `["Alice", "Bob"]`
  - Response: `200 OK` with `{ "gameId": "GUID" }` on success.

- `PUT /Poker/{gameId}/Player/{playerName}`
  - Adds a player with name `playerName` to the existing game `gameId`.
  - Response: `200 OK` on success, `404 Not Found` if the game does not exist.

- `DELETE /Poker/{gameId}/Player/{playerName}`
  - Removes the player named `playerName` from the game `gameId`. If no players remain, the game is removed.
  - Response: `200 OK` on success, `404 Not Found` if the game does not exist.

- `GET /Poker/{gameId}/Deal`
  - Resets and deals a new hand for the specified game. Returns the dealt hands and player data.
  - Response: `200 OK` with dealt hands payload or `404 Not Found` if the game does not exist.

- `GET /Poker/{gameId}/Evaluate`
  - Validates hands and determines winners for the specified game.
  - Response: `200 OK` with evaluation results or `404 Not Found` if the game does not exist.

Note: The API design in this project uses explicit game lifecycles: create a game (`/New`), add players, then call `/Deal` and `/Evaluate` for that game.

---

## Running tests

From the solution root run:

```bash
dotnet test PokerLogic.Tests
```

For coverage, the tests can be run with Coverlet or other coverage tools supported by the .NET tooling.

---

## Contributing

Contributions are welcome. Please open issues or submit pull requests with clear descriptions of changes and any relevant tests.

---

## License

This project is licensed under the MIT License. See the [LICENSE] file for details.
