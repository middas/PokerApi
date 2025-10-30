using static PokerLogic.Constants;
using static PokerLogic.Games.Poker.PokerHandEvaluator;

namespace PokerLogic.Tests.Games.Poker
{
    [TestFixture]
    internal class PokerHandEvaluatorTests
    {
        [Test]
        public void EvaluateHand_Flush_ShouldReturnCorrectRankAndScore()
        {
            // Arrange
            Hand hand = new();
            hand.Cards.AddRange(
            [
                new Card(Suit.Diamonds, Rank.Two),
                new Card(Suit.Diamonds, Rank.Five),
                new Card(Suit.Diamonds, Rank.Seven),
                new Card(Suit.Diamonds, Rank.Jack),
                new Card(Suit.Diamonds, Rank.King)
            ]);
            // Act
            var (handRank, score) = EvaluateHand(hand);
            // Assert
            Assert.That(handRank, Is.EqualTo(HandRank.Flush), "The hand rank should be Flush.");
            Assert.That(score, Is.EqualTo(2 + 5 + 7 + 11 + 13), "The score should be the sum of the card ranks.");
        }

        [Test]
        public void EvaluateHand_FourOfAKind_ShouldReturnCorrectRankAndScore()
        {
            // Arrange
            Hand hand = new();
            hand.Cards.AddRange(
            [
                new Card(Suit.Clubs, Rank.Nine),
                new Card(Suit.Hearts, Rank.Nine),
                new Card(Suit.Spades, Rank.Nine),
                new Card(Suit.Diamonds, Rank.Nine),
                new Card(Suit.Clubs, Rank.Three)
            ]);
            // Act
            var (handRank, score) = EvaluateHand(hand);
            // Assert
            Assert.That(handRank, Is.EqualTo(HandRank.FourOfAKind), "The hand rank should be Four of a Kind.");
            Assert.That(score, Is.EqualTo(9 + 9 + 9 + 9 + (36 * 14) + 3), "The score should be the sum of the card ranks plus the modifier.");
        }

        [Test]
        public void EvaluateHand_FourOfAKindWithHigherCards_ShouldReturnHigherScore()
        {
            // Arrange
            Hand handOne = new();
            handOne.Cards.AddRange(
            [
                new Card(Suit.Clubs, Rank.Two),
                new Card(Suit.Hearts, Rank.Two),
                new Card(Suit.Spades, Rank.Two),
                new Card(Suit.Diamonds, Rank.Two),
                new Card(Suit.Clubs, Rank.Ace)
            ]);
            Hand handTwo = new();
            handTwo.Cards.AddRange(
            [
                new Card(Suit.Clubs, Rank.Three),
                new Card(Suit.Hearts, Rank.Three),
                new Card(Suit.Spades, Rank.Three),
                new Card(Suit.Diamonds, Rank.Three),
                new Card(Suit.Clubs, Rank.Two)
            ]);
            // Act
            var (handRankOne, scoreOne) = EvaluateHand(handOne);
            var (handRankTwo, scoreTwo) = EvaluateHand(handTwo);
            // Assert
            Assert.That(handRankOne, Is.EqualTo(HandRank.FourOfAKind), "The hand rank should be Four of a Kind.");
            Assert.That(scoreOne, Is.EqualTo(2 + 2 + 2 + 2 + (8 * 14) + 14), "The score should be the sum of the card ranks plus the modifier.");
            Assert.That(handRankTwo, Is.EqualTo(HandRank.FourOfAKind), "The hand rank should be Four of a Kind.");
            Assert.That(scoreTwo, Is.EqualTo(3 + 3 + 3 + 3 + (12 * 14) + 2), "The score should be the sum of the card ranks plus the modifier.");
            Assert.That(scoreTwo, Is.GreaterThan(scoreOne), "Hand two should have a higher score than hand one.");
        }

        [Test]
        public void EvaluateHand_FullHouse_ShouldReturnCorrectRankAndScore()
        {
            // Arrange
            Hand hand = new();
            hand.Cards.AddRange(
            [
                new Card(Suit.Clubs, Rank.Six),
                new Card(Suit.Hearts, Rank.Six),
                new Card(Suit.Spades, Rank.Six),
                new Card(Suit.Diamonds, Rank.King),
                new Card(Suit.Clubs, Rank.King)
            ]);
            // Act
            var (handRank, score) = EvaluateHand(hand);
            // Assert
            Assert.That(handRank, Is.EqualTo(HandRank.FullHouse), "The hand rank should be Full House.");
            Assert.That(score, Is.EqualTo(6 + 6 + 6 + (18 * 14 * 14) + 13 + 13 + (26 * 14)), "The score should be the sum of the card ranks plus the modifier.");
        }

        [Test]
        public void EvaluateHand_FullHouseWithHigherCards_ShouldReturnHigherScore()
        {
            // Arrange
            Hand handOne = new();
            handOne.Cards.AddRange(
            [
                new Card(Suit.Clubs, Rank.Two),
                new Card(Suit.Hearts, Rank.Ace),
                new Card(Suit.Spades, Rank.Two),
                new Card(Suit.Diamonds, Rank.Two),
                new Card(Suit.Clubs, Rank.Ace)
            ]);
            Hand handTwo = new();
            handTwo.Cards.AddRange(
            [
                new Card(Suit.Clubs, Rank.Three),
                new Card(Suit.Hearts, Rank.Three),
                new Card(Suit.Spades, Rank.Three),
                new Card(Suit.Diamonds, Rank.Two),
                new Card(Suit.Clubs, Rank.Two)
            ]);
            // Act
            var (handRankOne, scoreOne) = EvaluateHand(handOne);
            var (handRankTwo, scoreTwo) = EvaluateHand(handTwo);
            // Assert
            Assert.That(handRankOne, Is.EqualTo(HandRank.FullHouse), "The hand rank should be Full House.");
            Assert.That(scoreOne, Is.EqualTo(2 + 14 + 2 + 2 + (6 * 14 * 14) + 14 + (28 * 14)), "The score should be the sum of the card ranks plus the modifier.");
            Assert.That(handRankTwo, Is.EqualTo(HandRank.FullHouse), "The hand rank should be Full House.");
            Assert.That(scoreTwo, Is.EqualTo(3 + 3 + 3 + (9 * 14 * 14) + 2 + 2 + (4 * 14)), "The score should be the sum of the card ranks plus the modifier.");
            Assert.That(scoreTwo, Is.GreaterThan(scoreOne), "Hand two should have a higher score than hand one.");
        }

        [Test]
        public void EvaluateHand_HighCard_ShouldReturnCorrectRankAndScore()
        {
            // Arrange
            Hand hand = new();
            hand.Cards.AddRange(
            [
                new Card(Suit.Clubs, Rank.Two),
                new Card(Suit.Hearts, Rank.Five),
                new Card(Suit.Spades, Rank.Seven),
                new Card(Suit.Diamonds, Rank.Jack),
                new Card(Suit.Clubs, Rank.King)
            ]);
            // Act
            var (handRank, score) = EvaluateHand(hand);
            // Assert
            Assert.That(handRank, Is.EqualTo(HandRank.HighCard), "The hand rank should be High Card.");
            Assert.That(score, Is.EqualTo(2 + 5 + 7 + 11 + 13 + (13 * 14)), "The score should be the sum of the card ranks plus the modifier.");
        }

        [Test]
        public void EvaluateHand_OnePair_ShouldReturnCorrectRankAndScore()
        {
            // Arrange
            Hand hand = new();
            hand.Cards.AddRange(
            [
                new Card(Suit.Clubs, Rank.Three),
                new Card(Suit.Hearts, Rank.Three),
                new Card(Suit.Spades, Rank.Six),
                new Card(Suit.Diamonds, Rank.Nine),
                new Card(Suit.Clubs, Rank.Queen)
            ]);
            // Act
            var (handRank, score) = EvaluateHand(hand);
            // Assert
            Assert.That(handRank, Is.EqualTo(HandRank.OnePair), "The hand rank should be One Pair.");
            Assert.That(score, Is.EqualTo(3 + 3 + (6 * 14) + 6 + 9 + 12), "The score should be the sum of the card ranks plus the modifier.");
        }

        [Test]
        public void EvaluateHand_OnePairWithHigherCards_ShouldReturnHigherScore()
        {
            // Arrange
            Hand handOne = new();
            handOne.Cards.AddRange(
            [
                new Card(Suit.Clubs, Rank.Two),
                new Card(Suit.Hearts, Rank.Ace),
                new Card(Suit.Spades, Rank.Two),
                new Card(Suit.Diamonds, Rank.Queen),
                new Card(Suit.Clubs, Rank.King)
            ]);
            Hand handTwo = new();
            handTwo.Cards.AddRange(
            [
                new Card(Suit.Clubs, Rank.Three),
                new Card(Suit.Hearts, Rank.Three),
                new Card(Suit.Spades, Rank.Two),
                new Card(Suit.Diamonds, Rank.Four),
                new Card(Suit.Clubs, Rank.Five)
            ]);
            // Act
            var (handRankOne, scoreOne) = EvaluateHand(handOne);
            var (handRankTwo, scoreTwo) = EvaluateHand(handTwo);
            // Assert
            Assert.That(handRankOne, Is.EqualTo(HandRank.OnePair), "The hand rank should be One Pair.");
            Assert.That(scoreOne, Is.EqualTo(2 + 14 + 2 + (4 * 14) + 12 + 13), "The score should be the sum of the card ranks plus the modifier.");
            Assert.That(handRankTwo, Is.EqualTo(HandRank.OnePair), "The hand rank should be One Pair.");
            Assert.That(scoreTwo, Is.EqualTo(3 + 3 + (6 * 14) + 2 + 4 + 5), "The score should be the sum of the card ranks plus the modifier.");
            Assert.That(scoreTwo, Is.GreaterThan(scoreOne), "Hand two should have a higher score than hand one.");
        }

        [Test]
        public void EvaluateHand_RoyalFlush_ShouldReturnCorrectRankAndScore()
        {
            // Arrange
            Hand hand = new();

            hand.Cards.AddRange(
            [
                new Card(Suit.Hearts, Rank.Ten),
                new Card(Suit.Hearts, Rank.Jack),
                new Card(Suit.Hearts, Rank.Queen),
                new Card(Suit.Hearts, Rank.King),
                new Card(Suit.Hearts, Rank.Ace)
            ]);

            // Act
            var (handRank, score) = EvaluateHand(hand);
            // Assert
            Assert.That(handRank, Is.EqualTo(HandRank.RoyalFlush), "The hand rank should be Royal Flush.");
            Assert.That(score, Is.EqualTo(10 + 11 + 12 + 13 + 14), "The score should be the sum of the card ranks.");
        }

        [Test]
        public void EvaluateHand_Straight_AceLow_ShouldReturnCorrectRankAndScore()
        {
            // Arrange
            Hand hand = new();
            hand.Cards.AddRange(
            [
                new Card(Suit.Clubs, Rank.Ace),
                new Card(Suit.Hearts, Rank.Two),
                new Card(Suit.Spades, Rank.Three),
                new Card(Suit.Diamonds, Rank.Four),
                new Card(Suit.Clubs, Rank.Five)
            ]);
            // Act
            var (handRank, score) = EvaluateHand(hand);
            // Assert
            Assert.That(handRank, Is.EqualTo(HandRank.Straight), "The hand rank should be Straight.");
            Assert.That(score, Is.EqualTo(15), "The score should be 15 for Ace-low straight.");
        }

        [Test]
        public void EvaluateHand_Straight_ShouldReturnCorrectRankAndScore()
        {
            // Arrange
            Hand hand = new();
            hand.Cards.AddRange(
            [
                new Card(Suit.Clubs, Rank.Three),
                new Card(Suit.Hearts, Rank.Four),
                new Card(Suit.Spades, Rank.Five),
                new Card(Suit.Diamonds, Rank.Six),
                new Card(Suit.Clubs, Rank.Seven)
            ]);
            // Act
            var (handRank, score) = EvaluateHand(hand);
            // Assert
            Assert.That(handRank, Is.EqualTo(HandRank.Straight), "The hand rank should be Straight.");
            Assert.That(score, Is.EqualTo(3 + 4 + 5 + 6 + 7), "The score should be the sum of the card ranks.");
        }

        [Test]
        public void EvaluateHand_StraightFlush_ShouldReturnCorrectRankAndScore()
        {
            // Arrange
            Hand hand = new();
            hand.Cards.AddRange(
            [
                new Card(Suit.Spades, Rank.Six),
                new Card(Suit.Spades, Rank.Seven),
                new Card(Suit.Spades, Rank.Eight),
                new Card(Suit.Spades, Rank.Nine),
                new Card(Suit.Spades, Rank.Ten)
            ]);
            // Act
            var (handRank, score) = EvaluateHand(hand);
            // Assert
            Assert.That(handRank, Is.EqualTo(HandRank.StraightFlush), "The hand rank should be Straight Flush.");
            Assert.That(score, Is.EqualTo(6 + 7 + 8 + 9 + 10), "The score should be the sum of the card ranks.");
        }

        [Test]
        public void EvaluateHand_ThreeOfAKind_ShouldReturnCorrectRankAndScore()
        {
            // Arrange
            Hand hand = new();
            hand.Cards.AddRange(
            [
                new Card(Suit.Clubs, Rank.Five),
                new Card(Suit.Hearts, Rank.Five),
                new Card(Suit.Spades, Rank.Five),
                new Card(Suit.Diamonds, Rank.Ten),
                new Card(Suit.Clubs, Rank.Jack)
            ]);
            // Act
            var (handRank, score) = EvaluateHand(hand);
            // Assert
            Assert.That(handRank, Is.EqualTo(HandRank.ThreeOfAKind), "The hand rank should be Three of a Kind.");
            Assert.That(score, Is.EqualTo(5 + 5 + 5 + (15 * 14) + 10 + 11), "The score should be the sum of the card ranks plus the modifier.");
        }

        [Test]
        public void EvaluateHand_ThreeOfAKindWithHigherCards_ShouldReturnHigherScore()
        {
            // Arrange
            Hand handOne = new();
            handOne.Cards.AddRange(
            [
                new Card(Suit.Clubs, Rank.Three),
                new Card(Suit.Hearts, Rank.Three),
                new Card(Suit.Spades, Rank.Three),
                new Card(Suit.Diamonds, Rank.Four),
                new Card(Suit.Clubs, Rank.Two)
            ]);
            Hand handTwo = new();
            handTwo.Cards.AddRange(
            [
                new Card(Suit.Clubs, Rank.Two),
                new Card(Suit.Hearts, Rank.Two),
                new Card(Suit.Spades, Rank.Two),
                new Card(Suit.Diamonds, Rank.Ace),
                new Card(Suit.Clubs, Rank.King)
            ]);
            // Act
            var (handRankOne, scoreOne) = EvaluateHand(handOne);
            var (handRankTwo, scoreTwo) = EvaluateHand(handTwo);
            // Assert
            Assert.That(handRankOne, Is.EqualTo(HandRank.ThreeOfAKind), "The hand rank should be Three of a Kind.");
            Assert.That(scoreOne, Is.EqualTo(3 + 3 + 3 + (9 * 14) + 2 + 4), "The score should be the sum of the card ranks plus the modifier.");
            Assert.That(handRankTwo, Is.EqualTo(HandRank.ThreeOfAKind), "The hand rank should be Three of a Kind.");
            Assert.That(scoreTwo, Is.EqualTo(2 + 2 + 2 + (6 * 14) + 14 + 13), "The score should be the sum of the card ranks plus the modifier.");
            Assert.That(scoreOne, Is.GreaterThan(scoreTwo), "Hand one should have a higher score than hand two.");
        }

        [Test]
        public void EvaluateHand_TwoPair_ShouldReturnCorrectRankAndScore()
        {
            // Arrange
            Hand hand = new();
            hand.Cards.AddRange(
            [
                new Card(Suit.Clubs, Rank.Four),
                new Card(Suit.Hearts, Rank.Four),
                new Card(Suit.Spades, Rank.Eight),
                new Card(Suit.Diamonds, Rank.Eight),
                new Card(Suit.Clubs, Rank.Ace)
            ]);
            // Act
            var (handRank, score) = EvaluateHand(hand);
            // Assert
            Assert.That(handRank, Is.EqualTo(HandRank.TwoPair), "The hand rank should be Two Pair.");
            Assert.That(score, Is.EqualTo(4 + 4 + 8 + 8 + (16 * 14 * 2) + 14), "The score should be the sum of the card ranks plus the modifier.");
        }

        [Test]
        public void EvaluateHand_TwoPairTie_ShouldReturnHigherScore()
        {
            // Arrange
            Hand handOne = new();
            handOne.Cards.AddRange(
            [
                new Card(Suit.Clubs, Rank.Ace),
                new Card(Suit.Hearts, Rank.Ace),
                new Card(Suit.Spades, Rank.Two),
                new Card(Suit.Diamonds, Rank.Four),
                new Card(Suit.Clubs, Rank.Two)
            ]);
            Hand handTwo = new();
            handTwo.Cards.AddRange(
            [
                new Card(Suit.Clubs, Rank.Ace),
                new Card(Suit.Hearts, Rank.Ace),
                new Card(Suit.Spades, Rank.Two),
                new Card(Suit.Diamonds, Rank.Three),
                new Card(Suit.Clubs, Rank.Two)
            ]);
            // Act
            var (handRankOne, scoreOne) = EvaluateHand(handOne);
            var (handRankTwo, scoreTwo) = EvaluateHand(handTwo);
            // Assert
            Assert.That(handRankOne, Is.EqualTo(HandRank.TwoPair), "The hand rank should be Two Pair.");
            Assert.That(scoreOne, Is.EqualTo(14 + 14 + (28 * 14 * 2) + 2 + 4 + 2), "The score should be the sum of the card ranks plus the modifier.");
            Assert.That(handRankTwo, Is.EqualTo(HandRank.TwoPair), "The hand rank should be Two Pair.");
            Assert.That(scoreTwo, Is.EqualTo(14 + 14 + (28 * 14 * 2) + 2 + 3 + 2), "The score should be the sum of the card ranks plus the modifier.");
            Assert.That(scoreOne, Is.GreaterThan(scoreTwo), "Hand one should have a higher score than hand two.");
        }

        [Test]
        public void EvaluateHand_TwoPairWithHigherCards_ShouldReturnHigherScore()
        {
            // Arrange
            Hand handOne = new();
            handOne.Cards.AddRange(
            [
                new Card(Suit.Clubs, Rank.Ace),
                new Card(Suit.Hearts, Rank.Ace),
                new Card(Suit.Spades, Rank.Two),
                new Card(Suit.Diamonds, Rank.Three),
                new Card(Suit.Clubs, Rank.Two)
            ]);
            Hand handTwo = new();
            handTwo.Cards.AddRange(
            [
                new Card(Suit.Clubs, Rank.King),
                new Card(Suit.Hearts, Rank.King),
                new Card(Suit.Spades, Rank.Queen),
                new Card(Suit.Diamonds, Rank.Ace),
                new Card(Suit.Clubs, Rank.Queen)
            ]);
            // Act
            var (handRankOne, scoreOne) = EvaluateHand(handOne);
            var (handRankTwo, scoreTwo) = EvaluateHand(handTwo);
            // Assert
            Assert.That(handRankOne, Is.EqualTo(HandRank.TwoPair), "The hand rank should be Two Pair.");
            Assert.That(scoreOne, Is.EqualTo(14 + 14 + (28 * 14 * 2) + 2 + 3 + 2), "The score should be the sum of the card ranks plus the modifier.");
            Assert.That(handRankTwo, Is.EqualTo(HandRank.TwoPair), "The hand rank should be Two Pair.");
            Assert.That(scoreTwo, Is.EqualTo(13 + 13 + (26 * 14 * 2) + 12 + 14 + 12), "The score should be the sum of the card ranks plus the modifier.");
            Assert.That(scoreOne, Is.GreaterThan(scoreTwo), "Hand one should have a higher score than hand two.");
        }
    }
}