using Rule = (int, int);

class Day5
{
    public Rule ParseRule(string line)
    {
        var parts = line.Split("|");
        return (int.Parse(parts[0]), int.Parse(parts[1]));
    }

    public Dictionary<int, List<int>> ParseRules(IEnumerable<Rule> rules)
    {
        var result = new Dictionary<int, List<int>>();
        foreach (var rule in rules)
        {
            if (!result.ContainsKey(rule.Item1))
                result[rule.Item1] = [];
            result[rule.Item1].Add(rule.Item2);
        }
        return result;
    }

    public List<int> ParseUpdate(string line)
    {
        return line.Split(",").Select(int.Parse).ToList();
    }

    bool IsRuleOrdered(Dictionary<int, List<int>> rules, List<int> update)
    {
        for (int i = 0; i < update.Count; i++)
        {
            var current = update[i];
            if (!rules.ContainsKey(current))
                continue;
            var prior = update[0..i];
            if (rules[current].Intersect(prior).Any())
                return false;
        }

        return true;
    }

    int MiddlePage(List<int> update)
    {
        return update[update.Count / 2];
    }

    List<int> Reorder(Dictionary<int, List<int>> rules, List<int> update)
    {
        var stack = new Stack<int>(update);
        var result = new List<int>();

        while (stack.Count > 0)
        {
            var current = stack.Pop();
            if (rules.ContainsKey(current))
            {
                var indexes = rules[current].Select(c => result.IndexOf(c)).Where(i => i != -1);
                if (indexes.Any())
                    result.Insert(indexes.Min(), current);
                else
                    result.Add(current);
            }
            else
                result.Add(current);
        }

        return result;
    }

    public void Solve()
    {
        var input = File.ReadAllLines("inputs/5");
        var (rules, updates) = input
            .Aggregate(
                new List<List<string>> { new List<string>() },
                (acc, line) =>
                {
                    if (string.IsNullOrEmpty(line))
                        acc.Add([]);
                    else
                        acc[^1].Add(line);
                    return acc;
                })
            .Where(group => group.Any())
            .ToList() switch
        { var l => (ParseRules(l[0].Select(ParseRule)), l[1].Select(ParseUpdate)) };

        Console.WriteLine("Part 1: {0}", updates
            .Where(update => IsRuleOrdered(rules, update))
            .Sum(MiddlePage));

        Console.WriteLine("Part 2: {0}", updates
            .Where(update => !IsRuleOrdered(rules, update))
            .Select(update => Reorder(rules, update))
            .Sum(MiddlePage));
    }
}