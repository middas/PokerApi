using static PokerLogic.Constants;

namespace PokerLogic.Games
{
    /// <summary>
    /// Represents a player in the game, holding a name and an optional hand of cards.
    /// </summary>
    /// <remarks>The <see cref="Player"/> class is immutable with respect to the player's name,  but allows
    /// modification of the player's hand. This class is sealed and cannot be inherited.</remarks>
    public sealed class Player
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Player"/> class with the specified name.
        /// </summary>
        /// <param name="name">The name to give the player.</param>
        public Player(string name)
        {
            ArgumentNullException.ThrowIfNull(name);
            Name = name;
        }

        /// <summary>
        /// The player's hand of cards.
        /// </summary>
        public Hand Hand { get; } = new();

        /// <summary>
        /// Gets or sets the rank of the hand in a card game.
        /// </summary>
        public HandRank? HandRank { get; set; }

        /// <summary>
        /// The name of the player.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Whether the player has a valid hand.
        /// </summary>
        internal bool HasValidHand { get; set; }

        /// <summary>
        /// Provides <see cref="Card"/>s to the player's <see cref="Hand"/>.
        /// </summary>
        /// <param name="cards">The <see cref="Card"/>s to be given to the player.</param>
        internal void GiveCards(IEnumerable<Card> cards)
        {
            Hand.Cards.AddRange(cards);
        }
    }
}