namespace LoopsAndStrings.Tests
{
    public class ProgramTests
    {
        // ---------- GetTicket ----------

        // Boundary values: each age limit is tested on both sides.
        [Theory]
        [InlineData(0, "Free entry", 0)]
        [InlineData(4, "Free entry", 0)]
        [InlineData(5, "Youth price", 80)]
        [InlineData(19, "Youth price", 80)]
        [InlineData(20, "Standard price", 120)]
        [InlineData(64, "Standard price", 120)]
        [InlineData(65, "Senior price", 90)]
        [InlineData(100, "Senior price", 90)]
        [InlineData(101, "Free entry", 0)]
        public void GetTicket_Age_ReturnsCorrectCategoryAndPrice(int age, string expectedCategory, int expectedPrice)
        {
            // Act
            (string Category, int Price) ticket = Program.GetTicket(age);

            // Assert
            Assert.Equal(expectedCategory, ticket.Category);
            Assert.Equal(expectedPrice, ticket.Price);
        }

        // ---------- BuildRepeatedText ----------

        [Fact]
        public void BuildRepeatedText_ThreeTimes_ReturnsNumberedTextOnOneLine()
        {
            // Arrange
            string text = "Hello";

            // Act
            string result = Program.BuildRepeatedText(text, 3);

            // Assert
            Assert.Equal("1. Hello, 2. Hello, 3. Hello", result);
        }

        [Fact]
        public void BuildRepeatedText_TenTimes_EndsWithTenthAndHasNoLineBreak()
        {
            // Act
            string result = Program.BuildRepeatedText("Hello", 10);

            // Assert
            Assert.StartsWith("1. Hello", result);
            Assert.EndsWith("10. Hello", result);
            Assert.DoesNotContain(Environment.NewLine, result);
        }

        // ---------- TryGetThirdWord ----------

        [Fact]
        public void TryGetThirdWord_ThreeWords_ReturnsThirdWord()
        {
            // Act
            bool success = Program.TryGetThirdWord("I like C#", out string thirdWord);

            // Assert
            Assert.True(success);
            Assert.Equal("C#", thirdWord);
        }

        [Fact]
        public void TryGetThirdWord_SeveralSpacesInARow_StillReturnsThirdWord()
        {
            // Act
            bool success = Program.TryGetThirdWord("   I    like     C#   a lot ", out string thirdWord);

            // Assert
            Assert.True(success);
            Assert.Equal("C#", thirdWord);
        }

        [Fact]
        public void TryGetThirdWord_TwoWords_ReturnsFalseAndEmptyString()
        {
            // Act
            bool success = Program.TryGetThirdWord("Hello world", out string thirdWord);

            // Assert
            Assert.False(success);
            Assert.Equal(string.Empty, thirdWord);
        }
    }
}