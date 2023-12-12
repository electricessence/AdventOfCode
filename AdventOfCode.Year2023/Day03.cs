namespace AdventOfCode.Year2023;

public static class Day03
{
	readonly struct Line
	{
		private readonly int _contentLength;
		public readonly string Content;
		public readonly LinkedList<(int Index, int Length)> Numbers;
		public readonly List<(int Index, char Symbol, List<int> Values)> Symbols;
		public readonly bool IsValid = false;

		public Line(string? content)
		{
			Content = content ?? string.Empty;
			Numbers = new LinkedList<(int Index, int Length)>();
			Symbols = [];
			if (content is null) return;
			_contentLength = content.Length;
			AnalyzeContent();
			IsValid = true;
		}

		private void AnalyzeContent()
		{
			int i = 0;
			while (i < _contentLength)
			{
				char s = Content[i];
				if (char.IsDigit(s))
				{
					int start = i;

					do { i++; }
					while (i < _contentLength && char.IsDigit(Content[i]));

					Numbers.AddLast((start, i - start));
					continue;
				}

				if (IsSymbol(s))
					Symbols.Add((i, s, new List<int>()));

				i++;
			}
		}

		public int GetNumberValue(int index, int length)
		{
			if(length == 0) return 0;
			int n = Content[index] - '0';
			for (int i = 1; i < length; i++)
			{
				n *= 10;
				n += Content[index + i] - '0';
			}
			return n;
		}

		public int GetNumberValue((int Index, int Length) number)
			=> GetNumberValue(number.Index, number.Length);

		private static bool IsSymbol(char ch)
			=> ch is not '.' and not ' ' && !char.IsDigit(ch);


		public int ExtractNumberWithSymbol(Line symbolLine)
		{
			if (!IsValid || !symbolLine.IsValid) return 0;

			var node = Numbers?.First;
			int sum = 0;

			foreach (var (i, s, values) in symbolLine.Symbols)
			{
processNode:
				if (node is null) break;
				var next = node!.Next;
				var (index, length) = node.Value;
				int first = index - 1;
				int last = index + length;
				if (i < first) continue; // Too early? Next.
				if (i > last)
				{
					// Too late? Skip.
					node = next;
					goto processNode;
				}

				int n = GetNumberValue(node.Value);
				sum += n;
				values.Add(n);
				Numbers!.Remove(node);
				node = next;
				goto processNode;
			}

			return sum;
		}

		public int ExtractNumberWithSymbol()
			=> ExtractNumberWithSymbol(this);

		public int GetGearRatios()
		{
			int gearRatios = 0;
			foreach (var gear in Symbols.Where(s => s.Symbol == '*' && s.Values.Count == 2))
			{
				int a = gear.Values[0];
				int b = gear.Values[1];
				gearRatios += a * b;
			}
			return gearRatios;
		}
	}

	public static int CalculateSumOfPartNumbers(TextReader reader, out int gearRatios)
	{
		gearRatios = 0;
		var previousLine = new Line(reader.ReadLine());
		if (!previousLine.IsValid) return 0;

		int sum = previousLine.ExtractNumberWithSymbol();
		Line currentLine;

		while ((currentLine = new Line(reader.ReadLine())).IsValid)
		{
			sum += currentLine.ExtractNumberWithSymbol();
			sum += currentLine.ExtractNumberWithSymbol(previousLine);
			sum += previousLine.ExtractNumberWithSymbol(currentLine);

			gearRatios += previousLine.GetGearRatios();

			previousLine = currentLine;
		}

		gearRatios += previousLine.GetGearRatios();

		return sum;
	}
}
