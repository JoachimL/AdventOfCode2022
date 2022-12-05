
using System.Drawing;
using System.Runtime.InteropServices.ComTypes;

Day3();
Day3Part2();
Day4();
Day4Part2();

void Day3()
{
    Console.WriteLine("Day 3: {0}",
        File.ReadLines("input_day3.txt")
            .Select(l => new Rucksack(l))
            .Sum(r => r.Value));
}

void Day3Part2()
{
    var allLines = File.ReadLines("input_day3.txt");

    var sum = 0;
    for (var i = 0; i < allLines.Count(); i += 3)
    {
        var lines = allLines.Skip(i).Take(3).ToList();
        var common = lines[0]
            .Distinct()
            .Single(l0 => lines[1].Distinct().Contains(l0) && lines[2].Distinct().Contains(l0));
        sum += Rucksack.GetValue(common);
    }
    Console.WriteLine("Day 3, part 2: {0}",sum);
}

void Day4()
{

    var count = File.ReadLines("input_day4.txt").Select(line => line.Split(','))
        .Select(line => Tuple.Create(line[0], line[1]))
        .Select(ranges => new ElfTeams(new ElfTeam(int.Parse(ranges.Item1.Split('-')[0]),
                int.Parse(ranges.Item1.Split('-')[1])),
            new ElfTeam(int.Parse(ranges.Item2.Split('-')[0]),
                int.Parse(ranges.Item2.Split('-')[1]))))
        .Count(t => t.Encompasses());

    Console.WriteLine("Day 4: {0}", count);
}

void Day4Part2()
{
    var count = File.ReadLines("input_day4.txt").Select(line => line.Split(','))
        .Select(line => Tuple.Create(line[0], line[1]))
        .Select(ranges => new ElfTeams(new ElfTeam(int.Parse(ranges.Item1.Split('-')[0]),
                int.Parse(ranges.Item1.Split('-')[1])),
            new ElfTeam(int.Parse(ranges.Item2.Split('-')[0]),
                int.Parse(ranges.Item2.Split('-')[1]))))
        .Count(t => t.Overlaps());

    Console.WriteLine("Day 4, part 2: {0}", count);
}

public class Rucksack
{
    public string Contents { get; }

    static Dictionary<char,int> Letters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ"
        .Select((letter, idx) => Tuple.Create(letter, idx+1))
        .ToDictionary(x=>x.Item1, x=>x.Item2);

    public static int GetValue(char c) => Letters[c];
    
    public Rucksack(string contents)
    {
        Contents = contents;
        var comp1 = contents.Substring(0, (contents.Length / 2)).Distinct();
        var comp2 = contents.Substring((contents.Length  /2) ).Distinct();
        var shared = comp1.Single(c1 => comp2.Contains(c1));
        Value = GetValue(shared);
    }

    public int Value { get; }

    public bool Contains(char c) => Contents.Distinct().Contains(c);
}

public record ElfTeam(int From, int To)
{
    public List<int> Range { get; } = Enumerable.Range(From, To-From+1).ToList();
}

public record ElfTeams(ElfTeam Team1, ElfTeam Team2)
{
    public bool Encompasses()
        => Team1.Range.All(t1 => Team2.Range.Contains(t1))
           || Team2.Range.All(t2 => Team1.Range.Contains(t2));
    
    public bool Overlaps()
        => Team1.Range.Any(t1 => Team2.Range.Contains(t1))
           || Team2.Range.Any(t2 => Team1.Range.Contains(t2));
}