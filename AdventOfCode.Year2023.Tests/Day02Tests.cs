﻿using FluentAssertions;

namespace AdventOfCode.Year2023.Tests;

public class Day02Tests
{
	[Theory]
	[InlineData(8,
		"""
		Game 1: 3 blue, 4 red; 1 red, 2 green, 6 blue; 2 green
		Game 2: 1 blue, 2 green; 3 green, 4 blue, 1 red; 1 green, 1 blue
		Game 3: 8 green, 6 blue, 20 red; 5 blue, 4 red, 13 green; 5 green, 1 red
		Game 4: 1 green, 3 red, 6 blue; 3 green, 6 red; 3 green, 15 blue, 14 red
		Game 5: 6 red, 1 blue, 3 green; 2 blue, 1 red, 2 green
		""")]
	public void CalculatePossibleGameSum_ShouldReturnCorrectSum(int expectedSum, string gameData)
		=> Day02.CalculatePossibleGameSum(gameData)
			.Should().Be(expectedSum);

	[Theory]
	[InlineData(2286,
		"""
		Game 1: 3 blue, 4 red; 1 red, 2 green, 6 blue; 2 green
		Game 2: 1 blue, 2 green; 3 green, 4 blue, 1 red; 1 green, 1 blue
		Game 3: 8 green, 6 blue, 20 red; 5 blue, 4 red, 13 green; 5 green, 1 red
		Game 4: 1 green, 3 red, 6 blue; 3 green, 6 red; 3 green, 15 blue, 14 red
		Game 5: 6 red, 1 blue, 3 green; 2 blue, 1 red, 2 green
		""")]
	public void CalculatePowerSumOfMinimumSets_ShouldReturnCorrectSum(int expectedPowerSum, string gameData)
		=> Day02.CalculatePowerSumOfMinimumSets(gameData)
			.Should().Be(expectedPowerSum);
}
