class Day11
{
    public IEnumerable<string> Blink(IEnumerable<string> line)
    {
        foreach (var stone in line)
        {
            if (stone == "0")
            {
                yield return "1";
            }
            else if (stone.Length % 2 == 0)
            {
                var half = stone.Length / 2;
                yield return stone[0..half];
                yield return RemoveLeadingZeros(stone[half..]);
            }
            else
            {
                yield return (long.Parse(stone) * 2024).ToString();
            }
        }
    }

    public string RemoveLeadingZeros(string input)
    {
        var result = input.TrimStart('0');
        if (result == "")
        {
            return "0";
        }
        return result;
    }

    public int Part1(IEnumerable<string> input)
    {
        for (var i = 0; i < 25; i++)
        {
            input = Blink(input.ToList());
        }
        return input.Count();
    }

    public long Part2(IEnumerable<string> input)
    {
        Dictionary<string, long> cache = new();
        foreach (var stone in input)
        {
            if (!cache.ContainsKey(stone))
            {
                cache[stone] = 0;
            }
            cache[stone] += 1;
        }

        for (var i = 0; i < 75; i++)
        {
            Dictionary<string, long> next = new();
            foreach (var stone in cache)
            {
                if (stone.Key == "0")
                {
                    next["1"] = next.GetValueOrDefault("1", 0) + stone.Value;
                }
                else if (stone.Key.Length % 2 == 0)
                {
                    var half = stone.Key.Length / 2;
                    var left = stone.Key[0..half];
                    var right = RemoveLeadingZeros(stone.Key[half..]);
                    next[left] = next.GetValueOrDefault(left, 0) + stone.Value;
                    next[right] = next.GetValueOrDefault(right, 0) + stone.Value;
                }
                else
                {
                    var k = (long.Parse(stone.Key) * 2024).ToString();
                    next[k] = next.GetValueOrDefault(k, 0) + stone.Value;
                }
            }
            cache = next;
        }

        return cache.Values.Sum();
    }

    public void Solve()
    {
        var input = File.ReadAllText("inputs/11").Split().ToList();
        Console.WriteLine(Part1(input));
        Console.WriteLine(Part2(input));
    }
}