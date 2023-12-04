using AdventOfCode.Year2023;

using var file = File.OpenRead("day01-data.txt");
using var sr = new StreamReader(file);
Console.WriteLine(Day01.CalculateSumOfCalibrationValues(sr));