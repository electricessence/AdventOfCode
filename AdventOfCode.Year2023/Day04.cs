using Microsoft.Extensions.Primitives;
using Open.Text;

namespace AdventOfCode.Year2023;

public static class Day04
{
	public static int CalculateTotalPoints(TextReader reader)
	{
		string? line;
		int totalPoints = 0;
		HashSet<StringSegment> winningNumbers = [];

		while ((line = reader.ReadLine()) != null)
		{
			totalPoints += CalculatePointsForCard(line, winningNumbers).Points;
		}

		return totalPoints;
	}

	static IEnumerable<StringSegment> GetNumbers(StringSegment source)
		=> source.SplitAsSegments(' ', StringSplitOptions.RemoveEmptyEntries).Select(s => s.TrimStart('0'));

	readonly record struct Card(int WinCount, int Points);

	private static Card CalculatePointsForCard(StringSegment cardData, HashSet<StringSegment> winningNumbers)
	{
		StringSegment body = cardData.First([':']).Following().Trim();
		int pipeIndex = body.IndexOf('|');

		winningNumbers.Clear();
		foreach (var w in GetNumbers(body.Subsegment(0, pipeIndex).TrimEnd()))
			winningNumbers.Add(w);

		int points = 0;
		int count = 0;
		bool isFirstMatch = true;

		foreach (var number in GetNumbers(body.Subsegment(pipeIndex + 1).TrimStart()))
		{
			if (!winningNumbers.Contains(number))
				continue;

			if (isFirstMatch)
			{
				points = 1;
				isFirstMatch = false;
			}
			else
			{
				points *= 2;
			}

			count++;
		}

		return new(count, points);
	}

	public static int CalculateTotalScratchcards(TextReader reader)
	{
		int cardCount = 0;
		var lines = new List<string>();
		string? line;
		while ((line = reader.ReadLine()) != null)
		{
			cardCount++;
			lines.Add(line);
		}

		int[] copies = Enumerable.Repeat(1, cardCount).ToArray();

		HashSet<StringSegment> winningNumbers = [];

		for (int i = 0; i < cardCount; i++)
		{
			int count = CalculatePointsForCard(lines[i], winningNumbers).WinCount;
			for (int j = i + 1; j <= i + count; j++)
			{
				copies[j] += copies[i];
			}
		}

		return copies.Sum();
	}
}
