using Open.Collections;
using System.Xml.Linq;

namespace AdventOfCode.Year2023;

public static class Day01
{
	internal enum NextDigitResult
	{
		End = -1,
		NewLine = 0,
		Found = 1
	}

	static Trie<char, int> GetVerboseDigits()
	{
		var result = new Trie<char, int>();
		result.Add("zero", 0);
		result.Add("one", 1);
		result.Add("two", 2);
		result.Add("three", 3);
		result.Add("four", 4);
		result.Add("five", 5);
		result.Add("six", 6);
		result.Add("seven", 7);
		result.Add("eight", 8);
		result.Add("nine", 9);
		return result;
	}

	internal static readonly Trie<char, int> VerboseDigits = GetVerboseDigits();

	internal static NextDigitResult NextDigitInLine(this TextReader sr, out int c)
	{
		ITrieNode<char, int>? node = VerboseDigits;
		while ((c = sr.Read()) != -1)
		{
			if (c is '\r' or '\n')
			{
				c = sr.Peek();
				if (c is -1 or '\r' or '\n') continue;
				return NextDigitResult.NewLine;
			}

			if (c is >= '0' and <= '9')
				return NextDigitResult.Found;

			if (node is not null && c is >= 'a' and <= 'z')
			{

			}
		}

		return NextDigitResult.End;
	}

	public static int CalculateSumOfCalibrationValues_Prototype(TextReader sr)
	{
		int sum = 0;
		string? line;
		while ((line = sr.ReadLine()) is not null)
		{
			string firstDigit = line.FirstOrDefault(c => char.IsDigit(c)).ToString();
			string lastDigit = line.LastOrDefault(c => char.IsDigit(c)).ToString();
			if (string.IsNullOrEmpty(firstDigit) || string.IsNullOrEmpty(lastDigit))
				continue;

			int calibrationValue = int.Parse(firstDigit + lastDigit);
			sum += calibrationValue;
		}

		return sum;
	}

	public static int CalculateSumOfCalibrationValues(TextReader sr)
	{
		int sum = 0;

loop:
		switch (sr.NextDigitInLine(out int firstDigit))
		{
			case NextDigitResult.End:
				return sum;
			case NextDigitResult.NewLine:
				goto loop;
		}

		NextDigitResult ndr;
		int lastDigit = firstDigit;
		while ((ndr = sr.NextDigitInLine(out int d)) == NextDigitResult.Found)
			lastDigit = d;

		int f = firstDigit - '0';
		int l = lastDigit - '0';
		sum += f * 10 + l;

		if (ndr == NextDigitResult.End)
			return sum;

		goto loop;
	}

	public static int CalculateSumOfCalibrationValues_Prototype(string input)
	{
		using var sr = new StringReader(input);
		return CalculateSumOfCalibrationValues_Prototype(sr);
	}

	public static int CalculateSumOfCalibrationValues(string input)
	{
		using var sr = new StringReader(input);
		return CalculateSumOfCalibrationValues(sr);
	}
}
