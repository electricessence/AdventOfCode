using Microsoft.Extensions.Primitives;
using Open.Text;

namespace AdventOfCode.Year2023;

public static class Day02
{
	public static int CalculatePossibleGameSum(string gameData)
	{
		var maxCubes = new Dictionary<StringSegment, int> ()
			{ { "red", 12 }, { "green", 13 }, { "blue", 14 } };

		return ParseGames(gameData)
			.Where(game => IsGamePossible(game.Value, maxCubes))
			.Sum(game => game.Key);
	}

	private static Dictionary<int, List<StringSegment>> ParseGames(string data)
	{
		using var sr = new StringReader(data);
		return ParseGames(sr);
	}

	private static Dictionary<int, List<StringSegment>> ParseGames(TextReader reader)
	{
		var games = new Dictionary<int, List<StringSegment>>();
		string? line;
		while ((line = reader.ReadLine()) != null)
		{
			var parts = line.SplitAsSegments(':').ToArray();
			int gameId = int.Parse(parts[0].SplitAsSegments(' ').ElementAt(1));
			var sets = parts[1].SplitAsSegments(';').Select(s => s.Trim()).ToList();
			games[gameId] = sets;
		}

		return games;
	}

	private static bool IsGamePossible(List<StringSegment> sets, Dictionary<StringSegment, int> maxCubes)
	{
		foreach (var set in sets)
		{
			foreach (var cubeInfo in set.SplitAsSegments(','))
			{
				int i = 0;
				int count = 0;
				StringSegment color = string.Empty;
				foreach (var part in cubeInfo.Trim().SplitAsSegments(' '))
				{
					switch (i++)
					{
						case 0:
							count = int.Parse(part);
							break;

						case 1:
							color = part;
							break;

						default:
							if (part.Length == 0) break;
							throw new Exception("Unexpected fragment.");
					}
				}

				if (count > maxCubes[color])
					return false;
			}
		}

		return true;
	}
}
