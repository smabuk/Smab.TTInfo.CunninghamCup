namespace Smab.TTInfo.CunninghamCup.Tests;

public class HandicapMatrixTests
{
	[Theory]
	[InlineData(0, 0)]
	[InlineData(1, 1)]
	[InlineData(16, 14)]
	[InlineData(30, 18)]   // Assuming DefaultMatrix does not contain 30
	public void GetStart_WithDifference_Returns(int difference, int expected)
	{
		int result = HandicapMatrix.GetStart(difference);
		result.ShouldBe(expected);
	}

	[Theory]
	[InlineData(0, 0, 0)]
	[InlineData(4, 4, 0)]
	[InlineData(10, 8, 2)]
	[InlineData(15, -1, 14)]
	[InlineData(-1, 15, 14)]
	[InlineData(20, 10, 10)]
	[InlineData(30, -3, 18)]
	public void GetStart_WithHandicaps(int handicapA, int handicapB, int expected)
	{
		int result = HandicapMatrix.GetStart(handicapA, handicapB);
		result.ShouldBe(expected);
	}
}
