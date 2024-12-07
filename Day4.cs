class Day4
{
    HashSet<string> Seen { get; set; } = new HashSet<string>();

    string[] GetWindow(string[] grid, int x, int y, int size)
    {
        var result = new string[size];
        for (int i = 0; i < size; i++)
        {
            result[i] = grid[y + i].Substring(x, size);
        }
        return result;
    }

    string Extract(string[] window, (int x, int y)[] indices)
    {
        return string.Join("", indices.Select(i => window[i.y][i.x]));
    }

    bool IsXmas(string s)
    {
        return s == "XMAS" || s == "SAMX";
    }

    int CountXmas(string[] window, int x, int y)
    {
        int result = 0;

        (int x, int y)[][] masks =
        [
            [ (0, 0), (1, 1), (2, 2), (3, 3) ],
            [ (3, 0), (2, 1), (1, 2), (0, 3) ],
            // cols
            [ (0, 0), (0, 1), (0, 2), (0, 3) ],
            [ (1, 0), (1, 1), (1, 2), (1, 3) ],
            [ (2, 0), (2, 1), (2, 2), (2, 3) ],
            [ (3, 0), (3, 1), (3, 2), (3, 3) ],
            // rows
            [ (0, 0), (1, 0), (2, 0), (3, 0) ],
            [ (0, 1), (1, 1), (2, 1), (3, 1) ],
            [ (0, 2), (1, 2), (2, 2), (3, 2) ],
            [ (0, 3), (1, 3), (2, 3), (3, 3) ],
        ];

        foreach (var mask in masks)
        {
            var key = string.Join(";", mask.Select(i => $"{x + i.x},{y + i.y}"));
            if (!Seen.Contains(key) && IsXmas(Extract(window, mask)))
            {
                Seen.Add(key);
                result++;
            }
        }

        return result;
    }

    bool IsMas(string s)
    {
        return s == "MAS" || s == "SAM";
    }

    int CountMas(string[] window, int x, int y)
    {
        int result = 0;

        (int x, int y)[][] masks =
        [
            [ (0, 0), (1, 1), (2, 2) ],
            [ (2, 0), (1, 1), (0, 2) ],
        ];

        if (IsMas(Extract(window, masks[0])) && IsMas(Extract(window, masks[1])))
        {
            result++;
        }

        return result;
    }

    int MapWindow(string[] grid, int size, Func<string[], int, int, int> pred)
    {
        int result = 0;
        for (int y = 0; y <= grid.Length - size; y++)
        {
            for (int x = 0; x <= grid[y].Length - size; x++)
            {
                var window = GetWindow(grid, x, y, size);
                result += pred(window, x, y);
            }
        }
        return result;
    }

    public void Solve()
    {
        var input = File.ReadAllLines("inputs/4");
        Console.WriteLine(MapWindow(input, 4, CountXmas));
        Console.WriteLine(MapWindow(input, 3, CountMas));
    }
}
