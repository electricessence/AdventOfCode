using FluentAssertions;

namespace AdventOfCode.Year2023.Tests;

public class Day01Tests
{
	[Theory]
	[InlineData(
		142,
		"""
		1abc2
		pqr3stu8vwx
		a1b2c3d4e5f
		treb7uchet
		""")]
	[InlineData(
		281,
		"""
		two1nine
		eightwothree
		abcone2threexyz
		xtwone3four
		4nineeightseven2
		zoneight234
		7pqrstsixteen
		""")]
	public void CalculateSumOfCalibrationValues_ShouldReturnCorrectSum(int expectedSum, string lines)
		=> Day01.CalculateSumOfCalibrationValues(lines)
			.Should()
			.Be(expectedSum);
}
