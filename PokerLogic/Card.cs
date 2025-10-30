using static PokerLogic.Constants;

namespace PokerLogic
{
    /// <summary>
    /// Represents a playing card with a specific suit and rank.
    /// </summary>
    /// <remarks>The <see cref="Card"/> class provides functionality to create and compare playing cards. Each
    /// card has a <see cref="Suit"/> and a <see cref="Rank"/>. Cards can be compared for equality based on their suit
    /// and rank.</remarks>
    public sealed class Card
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Card"/> class.
        /// </summary>
        public Card()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Card"/> class with the specified suit and rank.
        /// </summary>
        /// <param name="suit">The suit of the card, such as Hearts, Diamonds, Clubs, or Spades.</param>
        /// <param name="rank">The rank of the card, such as Ace, King, Queen, or a numeric value.</param>
        public Card(Suit suit, Rank rank)
        {
            Suit = suit;
            Rank = rank;
        }

        /// <summary>
        /// Gets or sets the rank of the entity.
        /// </summary>
        public Rank Rank { get; set; }

        /// <summary>
        /// Gets or sets the suit of a playing card.
        /// </summary>
        public Suit Suit { get; set; }

        /// <summary>
        /// Determines whether the specified object is equal to the current card instance.
        /// </summary>
        /// <param name="obj">The object to compare with the current card instance.</param>
        /// <returns><see langword="true"/> if the specified object is a <c>Card</c> and has the same suit and rank as the
        /// current card; otherwise, <see langword="false"/>.</returns>
        public override bool Equals(object? obj)
        {
            if (obj == null) return false;
            if (ReferenceEquals(this, obj)) return true;

            if (obj is Card otherCard)
            {
                return Suit == otherCard.Suit && Rank == otherCard.Rank;
            }
            return false;
        }

        /// <summary>
        /// Returns a hash code for the current object.
        /// </summary>
        /// <remarks>The hash code is generated based on the <see cref="Suit"/> and <see cref="Rank"/>
        /// properties. This method is suitable for use in hashing algorithms and data structures like a hash
        /// table.</remarks>
        /// <returns>An integer hash code representing the current object.</returns>
        public override int GetHashCode()
        {
            return HashCode.Combine(Suit, Rank);
        }

        /// <summary>
        /// Returns a string representation of the card, indicating its rank and suit.
        /// </summary>
        /// <returns>A string in the format "Rank of Suit", representing the card's rank and suit.</returns>
        public override string ToString()
        {
            return $"{Rank} of {Suit}";
        }
    }
}