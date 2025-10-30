using PokerLogic.Decks;
using System.Text.Json.Serialization;
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
        public List<Card> Hand { get; private set; } = [];

        /// <summary>
        /// Gets or sets the rank of the hand in a card game.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public HandRank? HandRank { get; set; }

        /// <summary>
        /// Whether the player has a valid hand.
        /// </summary>
        public bool HasValidHand { get; internal set; }

        /// <summary>
        /// The name of the player.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Indicates whether the player is the winner of the game.
        /// </summary>
        public bool Winner { get; internal set; }

        public override bool Equals(object? obj)
        {
            if (obj is null) return false;

            if (ReferenceEquals(this, obj)) return true;

            if (obj is Player player)
            {
                return Name.Equals(player.Name);
            }

            return false;
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }
    }
}