using Block = (int start, int length);
using FileBlock = (int id, int start, int length);

class Day9
{
    public void Part1(List<int> tape)
    {
        while (tape.SkipWhile(x => x != -1).Any(x => x != -1))
        {
            var idx = tape.IndexOf(-1);
            var swapIdx = tape.FindLastIndex(x => x != -1);
            var temp = tape[idx];
            tape[idx] = tape[swapIdx];
            tape[swapIdx] = temp;
        }
    }

    public IEnumerable<Block> Gaps(List<FileBlock> tape)
    {
        foreach (var (left, right) in tape.Zip(tape.Skip(1)))
        {
            var gapStart = left.start + left.length; // inclusive
            var gapEnd = right.start; // exclusive
            yield return (gapStart, gapEnd - gapStart);
        }
    }

    public void Part2(List<FileBlock> tape)
    {
        var process = new Stack<FileBlock>(tape);
        while (process.Count > 0)
        {
            var (id, start, length) = process.Pop();
            var gap = Gaps(tape).FirstOrDefault(x => x.length >= length && x.start < start);
            if (gap != default)
            {
                tape.Remove((id, start, length));
                tape.Add((id, gap.start, length));
                tape.Sort((a, b) => a.start.CompareTo(b.start));
            }
        }
    }

    public long Checksum(List<FileBlock> tape)
    {
        long result = 0;
        foreach (var (id, start, length) in tape)
        {
            result += Enumerable.Range(start, length).Sum(x => (long)id * x);
        }
        return result;
    }

    public void Solve()
    {
        var input = File.ReadAllText("inputs/9");

        // TODO should modify part 1 to work in the same way as part 2
        var fileId = 0;
        var maxFileId = 0;
        var tape1 = new List<int>();
        for (var i = 0; i < input.Length; i += 2)
        {
            maxFileId = fileId;
            var fileBlock = int.Parse(input.Substring(i, 1));
            tape1.AddRange(Enumerable.Repeat(fileId, fileBlock));
            if (i + 1 < input.Length)
            {
                var freeSpace = int.Parse(input.Substring(i + 1, 1));
                tape1.AddRange(Enumerable.Repeat(-1, freeSpace));
            }
            fileId++;
        }

        Part1(tape1);
        long checksum = tape1.TakeWhile(x => x != -1).Select((x, i) => (long)(x * i)).Sum();
        Console.WriteLine("Part: {0}", checksum);

        // parse
        fileId = 0;
        var tape = new List<FileBlock>();
        var offset = 0;
        for (var i = 0; i < input.Length; i += 2)
        {
            var blockStart = offset;
            var blockLength = int.Parse(input.Substring(i, 1));
            tape.Add((fileId, blockStart, blockLength));
            offset += blockLength;
            if (i + 1 < input.Length)
            {
                var freeSpace = int.Parse(input.Substring(i + 1, 1));
                offset += freeSpace;
            }
            fileId++;
        }

        Part2(tape);
        Console.WriteLine("Part 2: {0}", Checksum(tape));
    }
}