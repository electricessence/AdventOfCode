using AdventOfCode.Year2023;

using var file = File.OpenRead("day05-data.txt");
using var sr = new StreamReader(file);
Console.WriteLine(Day05.CalculateLowestLocationNumberFromSeedRanges(sr));
