# Poker Game Solution

This repository contains a modular, testable implementation of a Poker game engine and a RESTful API built with .NET 9 and C# 13. The solution is organized into three main projects:

- **PokerLogic**: Core game logic, including deck, hand, and player management.
- **Poker.Api**: ASP.NET Core Web API exposing endpoints for dealing and evaluating poker hands.
- **PokerLogic.Tests**: NUnit-based unit tests for the core logic.

---

## Projects Overview

### PokerLogic

- Implements interfaces and classes for cards, decks, hands, players, and game logic.
- Supports standard poker rules and hand evaluation.
- Designed for extensibility and testability.

### Poker.Api

- Provides REST endpoints to:
  - Deal a new hand to players (`POST /Poker/Deal`)
  - Evaluate hands and determine winners (`PUT /Poker/Evaluate/{gameId}`)
- Uses OpenAPI/Swagger for API documentation.
- Returns detailed error information using Problem Details.

### PokerLogic.Tests

- Contains comprehensive NUnit tests for all core logic.
- Uses Coverlet for code coverage.

---

## Getting Started

### Prerequisites

- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- Visual Studio 2022 (or later)

### Build and Run

1. **Clone the repository:** `git clone  cd `
1. **Restore dependencies:** `dotnet restore`
1. **Build the solution:** `dotnet build`
1. **Run the API:** `dotnet run --project Poker.Api`
1. **Access the API documentation:**
   - Navigate to `https://localhost:<port>/swagger` or `/openapi` (in development mode).

### Running Tests
`dotnet test PokerLogic.Tests`

---

## API Endpoints

### Deal a New Hand

- **POST** `/Poker/Deal`
- **Body:**  `["Alice", "Bob", "Charlie"]`
- **Response:** `{ "gameId": "GUID", "players": [ { "name": "Alice", "hand": { "cards": [ ... ] }, "handRank": "OnePair", "hasValidHand": true, "winner": false }, ... ] }`

### Evaluate Hands

- **PUT** `/Poker/Evaluate/{gameId}`
- **Body:**  `[ { "name": "Alice", "hand": { "cards": [ ... ] } }, ... ]`
- **Response:** `{ "players": [ { "name": "Alice", "handRank": "OnePair", "winner": true }, ... ] }`
  
---

## Technologies Used

- **.NET 9 / C# 13**
- **ASP.NET Core Web API**
- **NUnit** (testing)
- **Coverlet** (code coverage)
- **OpenAPI/Swagger** (API documentation)

---

## Contributing

Contributions are welcome! Please open issues or submit pull requests for improvements or bug fixes.

---

## License

This project is licensed under the MIT License.