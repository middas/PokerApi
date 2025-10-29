namespace PokerLogic
{
    public static class Constants
    {
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

        public enum Suit
        {
            Hearts,
            Diamonds,
            Clubs,
            Spades
        }
    }
}