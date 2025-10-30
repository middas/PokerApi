namespace PokerLogic
{
    /// <summary>
    /// Provides enumerations for poker hand rankings, card ranks, and suits in a standard deck of playing cards.
    /// </summary>
    /// <remarks>The <see cref="Constants"/> class contains enumerations that define the possible rankings for
    /// poker hands, the ranks of playing cards, and the suits in a standard deck. These enumerations are used to
    /// facilitate the comparison and categorization of poker hands and playing cards in card games.</remarks>
    public static class Constants
    {
        /// <summary>
        /// Represents the ranking of a poker hand in increasing order of strength.
        /// </summary>
        /// <remarks>The <see cref="HandRank"/> enumeration defines the possible rankings for poker hands,
        /// starting from <see cref="HighCard"/> as the weakest and ending with <see cref="RoyalFlush"/> as the
        /// strongest. This enumeration is used to compare the relative strength of poker hands in a game.</remarks>
        public enum HandRank
        {
            HighCard = 1,
            OnePair,
            TwoPair,
            ThreeOfAKind,
            Straight,
            Flush,
            FullHouse,
            FourOfAKind,
            StraightFlush,
            RoyalFlush
        }

        /// <summary>
        /// Represents the rank of a playing card in a standard deck.
        /// </summary>
        /// <remarks>The <see cref="Rank"/> enumeration defines the possible ranks for playing cards,
        /// ranging from <see cref="Two"/> to <see cref="Ace"/>. Each rank is associated with a numerical value,
        /// starting from 2 for <see cref="Two"/> up to 14 for <see cref="Ace"/>.</remarks>
        public enum Rank
        {
            Two = 2,
            Three,
            Four,
            Five,
            Six,
            Seven,
            Eight,
            Nine,
            Ten,
            Jack,
            Queen,
            King,
            Ace
        }

        /// <summary>
        /// Represents the four suits in a standard deck of playing cards.
        /// </summary>
        public enum Suit
        {
            Hearts,
            Diamonds,
            Clubs,
            Spades
        }
    }
}