using Microsoft.Extensions.Primitives;
using Open.Text;

namespace AdventOfCode.Year2023;

public static class Day02
{
	public static int CalculatePossibleGameSum(string gameData)
	{
		using var sr = new StringReader(gameData);
		return CalculatePossibleGameSum(sr);
	}

	static readonly IReadOnlyDictionary<StringSegment, int> MaxCubes
		= new Dictionary<StringSegment, int>()
		{ { "red", 12 }, { "green", 13 }, { "blue", 14 } };

	public static int CalculatePossibleGameSum(TextReader gameData)
		=> ParseGames(gameData)
			.AsParallel()
			.Where(game => IsGamePossible(game.Value))
			.Sum(game => game.Key);

	private static Dictionary<int, IEnumerable<StringSegment>> ParseGames(TextReader reader)
	{
		var games = new Dictionary<int, IEnumerable<StringSegment>>();
		string? line;
		while ((line = reader.ReadLine()) != null)
		{
			var (a, b) = SplitAsPair(line.SplitAsSegments(':'));
			int gameId = int.Parse(a.SplitAsSegments(' ').ElementAt(1));
			games[gameId] = b.SplitAsSegments(';').Select(s => s.Trim());
		}

		return games;
	}

	private static (StringSegment first, StringSegment second) SplitAsPair(
		IEnumerable<StringSegment> parts)
	{
		var first = StringSegment.Empty;
		var second = StringSegment.Empty;
		int i = 0;

		foreach (var part in parts)
		{
			switch (i++)
			{
				case 0:
					first = part;
					break;

				case 1:
					second = part;
					break;

				default:
					if (part.Length == 0) break;
					throw new Exception("Unexpected fragment.");
			}
		}

		return (first, second);
	}

	private static bool IsGamePossible(
		IEnumerable<StringSegment> sets)
	{
		foreach (var set in sets)
		{
			foreach (var cubeInfo in set.SplitAsSegments(','))
			{
				var (count, color) = SplitAsPair(cubeInfo.Trim().SplitAsSegments(' '));
				if (int.Parse(count) > MaxCubes[color])
					return false;
			}
		}

		return true;
	}

	public static int CalculatePowerSumOfMinimumSets(string gameData)
	{
		using var sr = new StringReader(gameData);
		return CalculatePowerSumOfMinimumSets(sr);
	}

	public static int CalculatePowerSumOfMinimumSets(TextReader gameData)
	{
		var games = ParseGames(gameData);
		return games
			.AsParallel()
			.Sum(game => CalculatePowerOfMinimumSet(game.Value));
	}

	private static int CalculatePowerOfMinimumSet(IEnumerable<StringSegment> sets)
	{
		var minCubes = new Dictionary<StringSegment, int>
			{ { "red", 0 }, { "green", 0 }, { "blue", 0 } };

		foreach (var set in sets)
		{
			foreach (var cubeInfo in set.SplitAsSegments(','))
			{
				var (count, color) = SplitAsPair(cubeInfo.Trim().SplitAsSegments(' '));
				minCubes[color] = Math.Max(minCubes[color], int.Parse(count));
			}
		}

		return minCubes["red"]
			 * minCubes["green"]
			 * minCubes["blue"];
	}
}
