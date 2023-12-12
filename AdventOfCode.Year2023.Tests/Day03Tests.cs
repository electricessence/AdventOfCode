using FluentAssertions;

namespace AdventOfCode.Year2023.Tests;

public class Day03Tests
{
	[Theory]
	[InlineData(4361,
		"""
        467..114..
        ...*......
        ..35..633.
        ......#...
        617*......
        .....+.58.
        ..592.....
        ......755.
        ...$.*....
        .664.598..
        """)]
	public void CalculateSumOfPartNumbers_ShouldReturnCorrectSum(int expectedSum, string schematic)
	{
		using var reader = new StringReader(schematic);
		Day03.CalculateSumOfPartNumbers(reader, out _).Should().Be(expectedSum);
	}

	[Theory]
	[InlineData(467835,
		"""
		467..114..
		...*......
		..35..633.
		......#...
		617*......
		.....+.58.
		..592.....
		......755.
		...$.*....
		.664.598..
		""")]
	public void CalculateSumOfGearRatios_ShouldReturnCorrectSum(int expectedSum, string schematic)
	{
		using var reader = new StringReader(schematic);
		Day03.CalculateSumOfPartNumbers(reader, out int gearRatios);
		gearRatios.Should().Be(expectedSum);
	}
}
