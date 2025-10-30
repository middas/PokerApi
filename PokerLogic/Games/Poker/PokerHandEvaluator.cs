using PokerLogic.Decks;
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
        /// The score is determined based on the value of the cards and an optional modifier for certain types of hand ranks.</returns>
        internal static (HandRank handRank, int score) EvaluateHand(IEnumerable<Card> hand)
        {
            bool isStraight = IsStraight(hand, out Rank highCard);
            bool isFlush = IsFlush(hand);
            int sumRanks = hand.Sum(c => (int)c.Rank);

            if (isStraight && isFlush)
            {
                if (highCard == Rank.Ace)
                {
                    return (HandRank.RoyalFlush, sumRanks);
                }

                return (HandRank.StraightFlush, sumRanks);
            }
            if (isFlush)
            {
                return (HandRank.Flush, sumRanks);
            }

            if (isStraight)
            {
                return (HandRank.Straight, highCard == Rank.Five ? 15 : sumRanks);
            }

            var rankGroups = hand.GroupBy(c => c.Rank).ToList();
            var quads = rankGroups.FirstOrDefault(g => g.Count() == 4);
            var trips = rankGroups.Where(g => g.Count() == 3).OrderByDescending(g => g.Key).ToList();
            var pairs = rankGroups.Where(g => g.Count() == 2).OrderByDescending(g => g.Key).ToList();

            if (quads != null)
            {
                return (HandRank.FourOfAKind, (int)quads.Key * 4 * 14 + sumRanks);
            }

            if (trips.Count > 0 && pairs.Count > 0)
            {
                return (HandRank.FullHouse, (int)trips[0].Key * 3 * 14 * 14 + (int)pairs[0].Key * 2 * 14 + sumRanks);
            }

            if (trips.Count > 0)
            {
                return (HandRank.ThreeOfAKind, (int)trips[0].Key * 3 * 14 + sumRanks);
            }

            if (pairs.Count >= 2)
            {
                return (HandRank.TwoPair, pairs[0].Sum(g => (int)g.Rank * 2 * 14) + sumRanks);
            }

            if (pairs.Count == 1)
            {
                return (HandRank.OnePair, (int)pairs[0].Key * 2 * 14 + sumRanks);
            }

            int high = hand.Max(c => (int)c.Rank);
            return (HandRank.HighCard, high * 14 + sumRanks);
        }

        /// <summary>
        /// Determines whether the specified hand contains a flush.
        /// </summary>
        /// <param name="hand">The hand to evaluate, which contains a collection of cards.</param>
        /// <returns><see langword="true"/> if the hand contains at least five cards of the same suit; otherwise, <see
        /// langword="false"/>.</returns>
        private static bool IsFlush(IEnumerable<Card> hand)
        {
            return hand.GroupBy(c => c.Suit).Any(g => g.Count() >= 5);
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
        private static bool IsStraight(IEnumerable<Card> hand, out Rank highCard)
        {
            var ranks = hand.Select(c => (int)c.Rank).Distinct().OrderBy(r => r).ToList();
            if (ranks.Count < 5)
            {
                highCard = Rank.Two;
                return false;
            }

            // Check for normal straights
            for (int i = 0; i <= ranks.Count - 5; i++)
            {
                // The low card minus the high card should equal 4 for a straight
                if (ranks[i + 4] - ranks[i] == 4)
                {
                    highCard = (Rank)ranks[i + 4];
                    return true;
                }
            }

            // Check for wheel straight (A-2-3-4-5)
            if (ranks.Contains((int)Rank.Ace) &&
                ranks.Contains((int)Rank.Two) &&
                ranks.Contains((int)Rank.Three) &&
                ranks.Contains((int)Rank.Four) &&
                ranks.Contains((int)Rank.Five))
            {
                highCard = Rank.Five;
                return true;
            }

            highCard = Rank.Two;
            return false;
        }
    }
}