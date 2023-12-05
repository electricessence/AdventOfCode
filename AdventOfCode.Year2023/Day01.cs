using Open.Collections;

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

	internal static readonly ITrieNode<char, int> VerboseDigits = GetVerboseDigits();

	class DigitInLineParser(TextReader reader)
	{
		private readonly LinkedList<ITrieNode<char, int>> _matches = new();
		private readonly TextReader _reader = reader;

		public NextDigitResult Next(out int c)
		{
			while ((c = _reader.Read()) != -1)
			{
				if (c is '\r' or '\n')
				{
					c = _reader.Peek();
					if (c is -1 or '\r' or '\n') continue;
					_matches.Clear();
					return NextDigitResult.NewLine;
				}

#if DEBUG
				Console.Write((char)c);
#endif

				if (c is >= '0' and <= '9')
				{
					c -= '0';
					_matches.Clear();
					return NextDigitResult.Found;
				}

				if (c is >= 'a' and <= 'z')
				{
					char x;
					checked
					{
						x = (char)c;
					}

					try
					{
						var node = _matches.First;
						while (node is not null)
						{
							var t = node.Value;
							if (t.TryGetChild(x, out var n))
							{
								if (n.TryGetValue(out c))
								{
									return NextDigitResult.Found;
								}

								node.Value = n;
								node = node.Next;
							}
							else
							{
								var remove = node;
								node = node.Next;
								_matches.Remove(remove);
							}
						}
					}
					finally
					{
						if (VerboseDigits.TryGetChild(x, out var y))
						{
							_matches.AddLast(y);
						}
					}
				}
			}

			return NextDigitResult.End;
		}
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
		var parser = new DigitInLineParser(sr);
loop:
		switch (parser.Next(out int firstDigit))
		{
			case NextDigitResult.End:
				return sum;
			case NextDigitResult.NewLine:
				goto loop;
		}

		NextDigitResult ndr;
		int lastDigit = firstDigit;
		while ((ndr = parser.Next(out int d)) == NextDigitResult.Found)
		{
			lastDigit = d;
		}

		int cal = firstDigit * 10 + lastDigit;
		sum += cal;

#if DEBUG
		Console.WriteLine(" {0}", cal);
#endif

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
