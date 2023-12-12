using Open.Text;
using System.Collections.Immutable;

namespace AdventOfCode.Year2023;

public static class Day05
{
	public readonly record struct Map(int DestinationStart, int SourceStart, int RangeLength)
	{
		public int DestinationEnd { get; } = DestinationStart + RangeLength;
		public int SourceEnd { get; } = SourceStart + RangeLength;

		public bool IsMapped(int source, out int destination)
		{
			destination = -1;
			if (source < SourceStart || source >= SourceEnd)
				return false;

			destination = DestinationStart + (source - SourceStart);
			return true;
		}
	}

	public class MapLookup(IEnumerable<Map> maps)
	{
		private readonly Map[] _maps = maps.ToArray();

		public static readonly MapLookup Empty = new(Enumerable.Empty<Map>());

		public int GetMappedValue(int source)
		{
			foreach (var map in _maps)
			{
				if (map.IsMapped(source, out int destination))
					return destination;
			}

			return source;
		}
	}

	public class Almanac
	{
		public required ImmutableArray<int> Seeds { get; init; }
		public required MapLookup SeedToSoilMap { get; init; }
		public required MapLookup SoilToFertilizerMap { get; init; }
		public required MapLookup FertilizerToWaterMap { get; init; }
		public required MapLookup WaterToLightMap { get; init; }
		public required MapLookup LightToTemperatureMap { get; init; }
		public required MapLookup TemperatureToHumidityMap { get; init; }
		public required MapLookup HumidityToLocationMap { get; init; }
	}

	public static Almanac GetAlmanac(TextReader reader)
	{
		ImmutableArray<int> seeds = [];
		MapLookup seedToSoilMap = MapLookup.Empty;
		MapLookup soilToFertilizerMap = MapLookup.Empty;
		MapLookup fertilizerToWaterMap = MapLookup.Empty;
		MapLookup waterToLightMap = MapLookup.Empty;
		MapLookup lightToTemperatureMap = MapLookup.Empty;
		MapLookup temperatureToHumidityMap = MapLookup.Empty;
		MapLookup humidityToLocationMap = MapLookup.Empty;

		string? line;
		while ((line = reader.ReadLine()) != null)
		{
			if (line.StartsWith("seeds:"))
			{
				seeds = line.Split(' ', StringSplitOptions.RemoveEmptyEntries)
					.Skip(1)
					.Select(int.Parse)
					.ToImmutableArray();
			}
			else if (line.StartsWith("seed-to-soil map:"))
			{
				seedToSoilMap = GetMapLookup(reader);
			}
			else if (line.StartsWith("soil-to-fertilizer map:"))
			{
				soilToFertilizerMap = GetMapLookup(reader);
			}
			else if (line.StartsWith("fertilizer-to-water map:"))
			{
				fertilizerToWaterMap = GetMapLookup(reader);
			}
			else if (line.StartsWith("water-to-light map:"))
			{
				waterToLightMap = GetMapLookup(reader);
			}
			else if (line.StartsWith("light-to-temperature map:"))
			{
				lightToTemperatureMap = GetMapLookup(reader);
			}
			else if (line.StartsWith("temperature-to-humidity map:"))
			{
				temperatureToHumidityMap = GetMapLookup(reader);
			}
			else if (line.StartsWith("humidity-to-location map:"))
			{
				humidityToLocationMap = GetMapLookup(reader);
			}
		}

		return new Almanac
		{
			Seeds = seeds,
			SeedToSoilMap = seedToSoilMap,
			SoilToFertilizerMap = soilToFertilizerMap,
			FertilizerToWaterMap = fertilizerToWaterMap,
			WaterToLightMap = waterToLightMap,
			LightToTemperatureMap = lightToTemperatureMap,
			TemperatureToHumidityMap = temperatureToHumidityMap,
			HumidityToLocationMap = humidityToLocationMap
		};
	}

	public static int CalculateLowestLocationNumber(TextReader reader)
	{
		var almanac = GetAlmanac(reader);

		int lowestLocationNumber = int.MaxValue;
		foreach (int seed in almanac.Seeds)
		{
			int soilNumber = almanac.SeedToSoilMap.GetMappedValue(seed);
			int fertilizerNumber = almanac.SoilToFertilizerMap.GetMappedValue(soilNumber);
			int waterNumber = almanac.FertilizerToWaterMap.GetMappedValue(fertilizerNumber);
			int lightNumber = almanac.WaterToLightMap.GetMappedValue(waterNumber);
			int tempNumber = almanac.LightToTemperatureMap.GetMappedValue(lightNumber);
			int humidityNumber = almanac.TemperatureToHumidityMap.GetMappedValue(tempNumber);
			int locationNumber = almanac.HumidityToLocationMap.GetMappedValue(humidityNumber);
			lowestLocationNumber = Math.Min(lowestLocationNumber, locationNumber);
		}

		return lowestLocationNumber;
	}

	private static IEnumerable<Map> ReadMap(TextReader reader)
	{
		string? line;
		while ((line = reader.ReadLine()) != null)
		{
			if (string.IsNullOrWhiteSpace(line))
				break;

			int destinationStart = -1;
			int sourceStart = -1;
			int rangeLength = -1;

			int i = 0;
			foreach (var n in line.SplitAsMemory(' ', StringSplitOptions.RemoveEmptyEntries))
			{
				int value = int.Parse(n.Span);
				switch (i++)
				{
					case 0:
						destinationStart = value;
						break;

					case 1:
						sourceStart = value;
						break;

					case 2:
						rangeLength = value;
						break;
				}
			}

			yield return new Map(destinationStart, sourceStart, rangeLength);
		}
	}

	private static MapLookup GetMapLookup(TextReader reader)
		=> new(ReadMap(reader));
}
