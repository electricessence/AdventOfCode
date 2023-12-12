using AdventOfCode.Year2023;

using var file = File.OpenRead("day04-data.txt");
using var sr = new StreamReader(file);
Console.WriteLine(Day04.CalculateTotalScratchcards(sr));
