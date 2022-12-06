
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text.RegularExpressions;

Day3();
Day3Part2();
Day4();
Day4Part2();
Day5Part1();
Day5Part2();

void Day5Part2()
{
    var lines = File.ReadAllLines("input_day5.txt");

    bool cratesRead = false;
    bool columnsRead = false;
    Stack<char>[] columns = null;  
    foreach (var line in lines)
    {
        if (line.Trim().Length == 0) // separator
        {
            cratesRead = true;

            for (var i = 0; i < columns.Length; i++)
            {
                columns[i].Pop();
                columns[i] = new Stack<char>(columns[i]);
            }

            continue;
        }

        if (!cratesRead)
        {
            var columnCount = (line.Length + 1) / 4;
            if (columns == null)
            {
                columns = new Stack<char>[columnCount];
                for (var i = 0; i < columnCount; i++)
                {
                    columns[i] = new Stack<char>();
                }
            }

            var currentColumn = 0;
            for (var i = 0; i < columnCount; i ++)
            {
                var character = line[(4*i) + 1];
                if (character != ' ')
                {
                    columns[currentColumn].Push(character);
                }

                currentColumn++;
            }
        }
        else
        {
            // read instructions

            MoveCrate(columns, GetInstructions(line));

            Tuple<int, int, int> GetInstructions(string line)
            {
                var matches = Regex.Matches(line, "([0-9]+)", RegexOptions.Compiled | RegexOptions.IgnoreCase);
                var split = matches.Select(m=>m.Value).ToArray();
                    
                return Tuple.Create(int.Parse(split[0]), int.Parse(split[1]), int.Parse(split[2]));
            }
                

            void MoveCrate(Stack<char>[]? stacks, Tuple<int, int, int> instructions)
            {
                var toMove = new List<char>();
                for (var i = 0; i < instructions.Item1; i++)
                {
                    toMove.Add(columns[instructions.Item2 - 1].Pop());
                }

                toMove.Reverse();
                foreach(var c in toMove)
                    columns[instructions.Item3 - 1].Push(c);
                
            }
        }
    }
    
    Console.WriteLine("Day 5: {0}", string.Join("",columns.Select(c=>c.Pop())));
}

void Day5Part1()
{
    var lines = File.ReadAllLines("input_day5.txt");

    bool cratesRead = false;
    bool columnsRead = false;
    Stack<char>[] columns = null;  
    foreach (var line in lines)
    {
        if (line.Trim().Length == 0) // separator
        {
            cratesRead = true;

            for (var i = 0; i < columns.Length; i++)
            {
                columns[i].Pop();
                columns[i] = new Stack<char>(columns[i]);
            }

            continue;
        }

        if (!cratesRead)
        {
            var columnCount = (line.Length + 1) / 4;
            if (columns == null)
            {
                columns = new Stack<char>[columnCount];
                for (var i = 0; i < columnCount; i++)
                {
                    columns[i] = new Stack<char>();
                }
            }

            var currentColumn = 0;
            for (var i = 0; i < columnCount; i ++)
            {
                var character = line[(4*i) + 1];
                if (character != ' ')
                {
                    columns[currentColumn].Push(character);
                }

                currentColumn++;
            }
        }
        else
        {
            // read instructions

            MoveCrate(columns, GetInstructions(line));

            Tuple<int, int, int> GetInstructions(string line)
            {
                var matches = Regex.Matches(line, "([0-9]+)", RegexOptions.Compiled | RegexOptions.IgnoreCase);
                var split = matches.Select(m=>m.Value).ToArray();
                    
                return Tuple.Create(int.Parse(split[0]), int.Parse(split[1]), int.Parse(split[2]));
            }
                

            void MoveCrate(Stack<char>[]? stacks, Tuple<int, int, int> instructions)
            {
                for (var i = 0; i < instructions.Item1; i++)
                {
                    columns[instructions.Item3 - 1].Push(columns[instructions.Item2 - 1].Pop());
                }
            }
        }
    }
    
    Console.WriteLine("Day 5: {0}", string.Join("",columns.Select(c=>c.Pop())));
}

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