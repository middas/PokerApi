using PokerLogic.Decks;
using PokerLogic.Games.Exceptions;
using static PokerLogic.Constants;

namespace PokerLogic.Games.Poker
{
    /// <summary>
    /// Represents a five-card poker game, providing functionality to deal cards, determine winners, validate player
    /// hands, and reset the game state.
    /// </summary>
    /// <remarks>This class implements the <see cref="IGame"/> interface and manages the lifecycle of a poker
    /// game, including shuffling and dealing cards, evaluating player hands, and determining the winners based on poker
    /// hand rankings. The game uses a standard 52-card deck.</remarks>
    public sealed class FiveCardPokerGame : IGame
    {
        private readonly IDeck deck;
        private readonly List<Card> drawnCards = [];
        private readonly List<Player> players = [];

        /// <summary>
        /// Initializes a new instance of the <see cref="FiveCardPokerGame"/> class.
        /// </summary>
        /// <remarks>This constructor creates a new standard deck of cards and resets the game state to
        /// its initial configuration.</remarks>
        public FiveCardPokerGame()
        {
            deck = new StandardDeck();
            Reset();
        }

        public int PlayerCount => players.Count;

        public void AddPlayer(Player player)
        {
            players.Add(player);
        }

        public IEnumerable<Player> Deal()
        {
            var playerList = players as IList<Player> ?? [.. players];
            if (!playerList.Any())
            {
                throw new NoPlayerException();
            }

            // Deal 5 rounds, one card per player each round
            for (int round = 0; round < 5; round++)
            {
                foreach (var player in playerList)
                {
                    if (round == 0)
                    {
                        player.Hand.Clear();
                    }

                    if (deck.Count == 0)
                    {
                        // No more cards available; stop dealing
                        break;
                    }

                    var cards = deck.Draw(1).ToList();

                    drawnCards.AddRange(cards);
                    player.Hand.AddRange(cards);
                }

                if (deck.Count == 0)
                {
                    // No more cards available; stop dealing
                    break;
                }
            }

            return playerList;
        }

        public IEnumerable<Player> DetermineWinners()
        {
            if (players is null || !players.Any())
            {
                throw new NoPlayerException();
            }

            var playerScores = new List<PlayerScore>(players.Count());

            foreach (var player in players)
            {
                if (!player.HasValidHand) continue;

                var (handRank, score) = PokerHandEvaluator.EvaluateHand(player.Hand);
                playerScores.Add(new PlayerScore(player)
                {
                    HandRank = handRank,
                    Score = score
                });
                player.HandRank = handRank;
                player.Winner = false;
            }

            if (playerScores.Count == 0)
            {
                return [];
            }

            var highestRank = playerScores.Max(ps => ps.HandRank);
            var highestScore = playerScores
                .Where(ps => ps.HandRank == highestRank)
                .Max(ps => ps.Score);

            var winners = playerScores.Where(ps => ps.HandRank == highestRank && ps.Score == highestScore).Select(ps => ps.Player);
            foreach (var player in winners)
            {
                player.Winner = true;
            }

            return playerScores.OrderByDescending(ps => ps.Player.Winner)
                .ThenByDescending(ps => ps.HandRank)
                .ThenByDescending(ps => ps.Score)
                .Select(ps => ps.Player).Union(players).Distinct();
        }

        public void RemovePlayer(Player player)
        {
            players.Remove(player);
        }

        public void Reset()
        {
            deck.Reset();
            deck.Shuffle();
            drawnCards.Clear();
        }

        public void ValidateHands()
        {
            var playerList = players as IList<Player> ?? [.. players];
            if (playerList.Count == 0)
            {
                return;
            }

            // Build frequency map for all cards across players (to detect duplicates across players)
            var cardFrequencies = new Dictionary<Card, int>();
            foreach (var card in playerList.SelectMany(p => p.Hand))
            {
                if (cardFrequencies.TryGetValue(card, out var count))
                {
                    cardFrequencies[card] = count + 1;
                }
                else
                {
                    cardFrequencies[card] = 1;
                }
            }

            // HashSet of drawn cards for fast membership checks
            var drawnSet = new HashSet<Card>(drawnCards);

            foreach (var player in playerList)
            {
                var cardSet = new HashSet<Card>(player.Hand);
                bool hasExactlyFiveUnique = cardSet.Count == 5;
                bool allFromDrawn = hasExactlyFiveUnique && cardSet.IsSubsetOf(drawnSet);
                bool noSharedCards = hasExactlyFiveUnique && cardSet.All(c => cardFrequencies.TryGetValue(c, out var freq) && freq == 1);

                player.HasValidHand = hasExactlyFiveUnique && allFromDrawn && noSharedCards;
            }
        }

        /// <summary>
        /// Represents the score and hand rank of a player in a game.
        /// </summary>
        /// <remarks>This class associates a player with their current score and hand rank.  It is
        /// intended to encapsulate the player's performance in the context of a game.</remarks>
        private class PlayerScore
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="PlayerScore"/> class with the specified player.
            /// </summary>
            /// <param name="player">The player for whom the score is being tracked. Cannot be <see langword="null"/>.</param>
            /// <exception cref="ArgumentNullException">Thrown if <paramref name="player"/> is <see langword="null"/>.</exception>
            public PlayerScore(Player player) => Player = player ?? throw new ArgumentNullException(nameof(player));

            /// <summary>
            /// Gets or sets the rank of the hand in a card game.
            /// </summary>
            public HandRank HandRank { get; set; }

            /// <summary>
            /// Gets the player associated with the current context.
            /// </summary>
            public Player Player { get; }

            /// <summary>
            /// Gets or sets the score value.
            /// </summary>
            public int Score { get; set; }
        }
    }
}