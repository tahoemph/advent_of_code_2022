string[] lines = File.ReadAllLines("data.txt");
List<int> counts = new List<int>();

int total = 0;
foreach (string line in lines) {
  if (line == "") {
    counts.Add(total);
    total = 0;
  } else {
    total += Int32.Parse(line);
  }
}

int FirstMax = counts.Max(x => x);
counts.Remove(FirstMax);

int SecondMax = counts.Max(x => x);
counts.Remove(SecondMax);

int ThirdMax = counts.Max(x => x);

Console.WriteLine(FirstMax);
Console.WriteLine(SecondMax);
Console.WriteLine(ThirdMax);
Console.WriteLine(FirstMax + SecondMax + ThirdMax);