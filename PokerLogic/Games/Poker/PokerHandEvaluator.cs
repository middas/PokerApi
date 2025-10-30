using static PokerLogic.Constants;

namespace PokerLogic.Games.Poker
{
    /// <summary>
    /// Provides functionality to evaluate a poker hand and determine its rank and score.
    /// </summary>
    /// <remarks>This class includes methods to analyze a poker hand and calculate its rank (e.g., Royal
    /// Flush, Straight, etc.) and a corresponding score based on the hand's composition. The evaluation considers
    /// standard poker rules, including the detection of straights, flushes, and other hand combinations.</remarks>
    internal static class PokerHandEvaluator
    {
        /// <summary>
        /// Evaluates the rank and score of a given poker hand.
        /// </summary>
        /// <remarks>The method determines the hand's rank (e.g., Royal Flush, Straight, Full House) and
        /// calculates a score that reflects the strength of the hand. The score is influenced by the rank and the card
        /// values, with higher-ranked hands receiving higher scores. Special cases, such as a low Ace in a straight,
        /// are handled appropriately.</remarks>
        /// <param name="hand">The poker hand to evaluate, consisting of a collection of cards.</param>
        /// <returns>A tuple containing the rank of the hand as a <see cref="HandRank"/> and the calculated score as an integer.
        /// The score is determined based on the hand's rank and the values of the cards.</returns>
        internal static (HandRank handRank, int score) EvaluateHand(Hand hand)
        {
            bool isStraight = IsStraight(hand, out Rank highCard);
            bool isFlush = IsFlush(hand);

            if (isStraight && isFlush)
            {
                if (highCard == Rank.Ace)
                {
                    return (HandRank.RoyalFlush, hand.Cards.Sum(c => (int)c.Rank));
                }
                return (HandRank.StraightFlush, hand.Cards.Sum(c => (int)c.Rank));
            }
            else if (isFlush)
            {
                return (HandRank.Flush, hand.Cards.Sum(c => (int)c.Rank));
            }
            else if (isStraight)
            {
                if (highCard == Rank.Five)
                {
                    // ace is low in this case
                    return (HandRank.Straight, 15);
                }
                return (HandRank.Straight, hand.Cards.Sum(c => (int)c.Rank));
            }

            // Check for other hand ranks (Four of a Kind, Full House, etc.)
            IEnumerable<IGrouping<Rank, Card>> rankGroups = hand.Cards.GroupBy(c => c.Rank);
            HandRank rank = HandRank.HighCard;
            int scoreModifier = 0;
            if (rankGroups.Any(g => g.Count() == 4))
            {
                scoreModifier = (int)rankGroups.Where(g => g.Count() == 4).Max(g => g.Key) * 4 * 14;
                rank = HandRank.FourOfAKind;
            }
            else if (rankGroups.Any(g => g.Count() == 3) && rankGroups.Any(g => g.Count() == 2))
            {
                // More weight is put on the three of a kind than the pair
                scoreModifier = (int)rankGroups.Where(g => g.Count() == 3).Max(g => g.Key) * 3 * 14 * 14
                              + (int)rankGroups.Where(g => g.Count() == 2).Max(g => g.Key) * 2 * 14;
                rank = HandRank.FullHouse;
            }
            else if (rankGroups.Any(g => g.Count() == 3))
            {
                scoreModifier = (int)rankGroups.Where(g => g.Count() == 3).Max(g => g.Key) * 3 * 14;
                rank = HandRank.ThreeOfAKind;
            }
            else
            {
                int pairCount = rankGroups.Count(g => g.Count() == 2);
                if (pairCount >= 2)
                {
                    scoreModifier = rankGroups.Where(g => g.Count() == 2).OrderByDescending(g => g.Key).First().Sum(c => (int)c.Rank * 14 * 2);
                    rank = HandRank.TwoPair;
                }
                else if (pairCount == 1)
                {
                    scoreModifier = rankGroups.Where(g => g.Count() == 2).Sum(g => (int)g.Key * 2 * 14);
                    rank = HandRank.OnePair;
                }
            }

            if (rank == HandRank.HighCard)
            {
                scoreModifier = hand.Cards.Max(c => (int)c.Rank) * 14;
            }

            return (rank, hand.Cards.Sum(c => (int)c.Rank) + scoreModifier);
        }

        /// <summary>
        /// Determines whether the specified hand contains a flush.
        /// </summary>
        /// <param name="hand">The hand to evaluate, which contains a collection of cards.</param>
        /// <returns><see langword="true"/> if the hand contains at least five cards of the same suit; otherwise, <see
        /// langword="false"/>.</returns>
        private static bool IsFlush(Hand hand)
        {
            return hand.Cards.GroupBy(c => c.Suit).Any(g => g.Count() >= 5);
        }

        /// <summary>
        /// Determines whether the specified hand contains a straight, which is a sequence of five cards with
        /// consecutive ranks.
        /// </summary>
        /// <remarks>A straight consists of exactly five unique cards with consecutive ranks. This method
        /// also accounts for the special case of a "wheel" straight, which is composed of Ace, Two, Three, Four, and
        /// Five.</remarks>
        /// <param name="hand">The hand of cards to evaluate.</param>
        /// <param name="highCard">When this method returns, contains the highest rank in the straight if a straight is found; otherwise, <see
        /// cref="Rank.Two"/>.</param>
        /// <returns><see langword="true"/> if the hand contains a straight; otherwise, <see langword="false"/>.</returns>
        private static bool IsStraight(Hand hand, out Rank highCard)
        {
            if (hand.Cards.Count < 5)
            {
                highCard = Rank.Two;
                return false;
            }

            Card[] sortedCards = [.. hand.Cards.OrderBy(c => c.Rank).Distinct()];

            // A straight always consists of 5 unique consecutive ranks
            int consecutiveCount = 1;
            for (int i = 0; i < sortedCards.Length; i++)
            {
                if (i + 1 >= sortedCards.Length)
                {
                    highCard = Rank.Two;
                    return false;
                }

                if (consecutiveCount == 4 && sortedCards[i].Rank == Rank.Five && sortedCards[i + 1].Rank == Rank.Ace)
                {
                    highCard = Rank.Five;
                    return true;
                }

                if ((int)sortedCards[i].Rank == (int)sortedCards[i + 1].Rank - 1)
                {
                    consecutiveCount++;
                    if (consecutiveCount == 5)
                    {
                        highCard = sortedCards[i + 1].Rank;
                        return true;
                    }
                }
                else
                {
                    consecutiveCount = 1;
                }
            }

            highCard = Rank.Two;
            return false;
        }
    }
}