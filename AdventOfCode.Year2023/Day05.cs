using Open.Text;
using System.Collections;
using System.Collections.Immutable;
using static AdventOfCode.Year2023.Day05;

namespace AdventOfCode.Year2023;

public static class Day05
{
	public readonly record struct Map(uint DestinationStart, uint SourceStart, uint RangeLength)
	{
		public uint DestinationEnd { get; } = DestinationStart + RangeLength;
		public uint SourceEnd { get; } = SourceStart + RangeLength;

		public bool IsMapped(uint source, out uint destination)
		{
			destination = 0;
			if (source < SourceStart || source >= SourceEnd)
				return false;

			destination = DestinationStart + (source - SourceStart);
			return true;
		}
	}

	public readonly record struct SeedRange(uint Start, uint Length) : IEnumerable<uint>
	{
		public uint End { get; } = Start + Length;

		public IEnumerator<uint> GetEnumerator()
		{
			for (uint i = Start; i < End; i++)
				yield return i;
		}

		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
	}

	public class MapLookup(IEnumerable<Map> maps)
	{
		private readonly Map[] _maps = maps.ToArray();

		public static readonly MapLookup Empty = new(Enumerable.Empty<Map>());

		public uint GetMappedValue(uint source)
		{
			foreach (var map in _maps)
			{
				if (map.IsMapped(source, out uint destination))
					return destination;
			}

			return source;
		}
	}

	public class Almanac
	{
		public required ImmutableArray<uint> Seeds { get; init; }
		public required ImmutableArray<SeedRange> SeedRanges { get; init; }
		public required MapLookup SeedToSoilMap { get; init; }
		public required MapLookup SoilToFertilizerMap { get; init; }
		public required MapLookup FertilizerToWaterMap { get; init; }
		public required MapLookup WaterToLightMap { get; init; }
		public required MapLookup LightToTemperatureMap { get; init; }
		public required MapLookup TemperatureToHumidityMap { get; init; }
		public required MapLookup HumidityToLocationMap { get; init; }

		public uint GetLocationNumber(uint seed)
		{
			uint soilNumber = SeedToSoilMap.GetMappedValue(seed);
			uint fertilizerNumber = SoilToFertilizerMap.GetMappedValue(soilNumber);
			uint waterNumber = FertilizerToWaterMap.GetMappedValue(fertilizerNumber);
			uint lightNumber = WaterToLightMap.GetMappedValue(waterNumber);
			uint tempNumber = LightToTemperatureMap.GetMappedValue(lightNumber);
			uint humidityNumber = TemperatureToHumidityMap.GetMappedValue(tempNumber);
			return HumidityToLocationMap.GetMappedValue(humidityNumber);
		}
	}

	public static Almanac GetAlmanac(TextReader reader)
	{
		ImmutableArray<uint> seeds = [];
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
				seeds = line.SplitAsSegments(' ', StringSplitOptions.RemoveEmptyEntries)
					.Skip(1)
					.Select(s=> uint.Parse(s))
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

		int seedsLen = seeds.Length;
		if(seedsLen % 2 == 1)
			throw new Exception("Seeds must be even");

		var seedRangesBuilder = ImmutableArray.CreateBuilder<SeedRange>(seedsLen / 2);
		for (int i = 0; i < seedsLen; i += 2)
			seedRangesBuilder.Add(new SeedRange(seeds[i], seeds[i + 1]));

		return new Almanac
		{
			Seeds = seeds,
			SeedRanges = seedRangesBuilder.DrainToImmutable(),
			SeedToSoilMap = seedToSoilMap,
			SoilToFertilizerMap = soilToFertilizerMap,
			FertilizerToWaterMap = fertilizerToWaterMap,
			WaterToLightMap = waterToLightMap,
			LightToTemperatureMap = lightToTemperatureMap,
			TemperatureToHumidityMap = temperatureToHumidityMap,
			HumidityToLocationMap = humidityToLocationMap
		};
	}

	public static uint CalculateLowestLocationNumber(TextReader reader)
	{
		var almanac = GetAlmanac(reader);
		return almanac.Seeds.AsParallel().Min(almanac.GetLocationNumber);
	}

	public static uint CalculateLowestLocationNumberFromSeedRanges(TextReader reader)
	{
		var almanac = GetAlmanac(reader);

		IEnumerable<uint> GetLocationNumberFromRanges(SeedRange seedRange)
		{
			foreach (uint seed in seedRange)
			{
				yield return almanac.GetLocationNumber(seed);
			}
		}

		return almanac.SeedRanges.AsParallel().SelectMany(GetLocationNumberFromRanges).Min();
	}


	private static IEnumerable<Map> ReadMap(TextReader reader)
	{
		string? line;
		while ((line = reader.ReadLine()) != null)
		{
			if (string.IsNullOrWhiteSpace(line))
				break;

			uint destinationStart = 0;
			uint sourceStart = 0;
			uint rangeLength = 0;

			int i = 0;
			foreach (var n in line.SplitAsMemory(' ', StringSplitOptions.RemoveEmptyEntries))
			{
				uint value = uint.Parse(n.Span);
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
