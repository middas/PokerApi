using static PokerLogic.Constants;

namespace PokerLogic
{
    public sealed class Card
    {
        public Card()
        {
        }

        public Card(Suit suit, Rank rank)
        {
            Suit = suit;
            Rank = rank;
        }

        public Rank Rank { get; set; }
        public Suit Suit { get; set; }

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

        public override int GetHashCode()
        {
            return HashCode.Combine(Suit, Rank);
        }

        public override string ToString()
        {
            return $"{Rank} of {Suit}";
        }
    }
}