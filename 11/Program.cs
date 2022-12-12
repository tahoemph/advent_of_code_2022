using System.Collections;
using System.Numerics;

class Monkey {
  public static int commonBase = 1;
  public List<long> items = new List<long>();
  public char operation;
  public int operand;
  public int test;
  public int trueTarget;
  public int falseTarget;
  public int inspected = 0;
  public bool worryFree = true;

  public Monkey(string monkeyOpDescription, int monkeyTest, int monkeyTrueTarget, int monkeyFalseTarget) {
    test = monkeyTest;
    trueTarget = monkeyTrueTarget;
    falseTarget = monkeyFalseTarget;

    var parts = monkeyOpDescription.Trim().Split(" ");
    if (parts[1] == "+") {
      operation = '+';
      operand = Int32.Parse(parts[2]);
    } else if (parts[1] == "*") {
      if (parts[2] == "old") {
        operation = '^';
        operand = 2;
      } else {
        operation = '*';
        operand = Int32.Parse(parts[2]);
      }
    } else {
      Console.WriteLine("unknown operation");
    }
  }

  public long newWorry(long worry) {
    long rv = 0;
    switch(operation) {
      case '+':
        rv = worry + operand;      
        break;
      case '*':
        rv = worry * operand;
        break;
      case '^':
        rv = worry * worry;
        break;
      default:
        Console.WriteLine("WTF");
        break;
    }

    if (worryFree) {
      rv = (long)Math.Floor(rv/3.0);
    }
    if (commonBase > 1) {
      return rv % commonBase;
    }
    return rv;
  }
}

class Program {
  static Monkey createMonkey(string[] lines) {
    var parts = lines[1].Split(":");
    var startingItems = parts[1].Split(",");

    parts = lines[2].Split("=");
    var op = parts[1];

    parts = lines[3].Split(" ");
    var test = Int32.Parse(parts[parts.Length - 1]);

    parts = lines[4].Split(" ");
    var trueTarget = Int32.Parse(parts[parts.Length - 1]);

    parts = lines[5].Split(" ");
    var falseTarget = Int32.Parse(parts[parts.Length - 1]);

    var rv = new Monkey(op, test, trueTarget, falseTarget);
    foreach (var startingItem in startingItems) {
      rv.items.Add(Int32.Parse(startingItem));
    }
    return rv;
  } 

  static List<Monkey> createMonkeys(string[] lines) {
    var rv = new List<Monkey>();
    for(var line = 0; line < lines.Count(); line += 7) {
      rv.Add(createMonkey(lines.Skip(line).Take(6).ToArray()));
    }
    return rv;
  }

  static void playRound(List<Monkey> monkies) {
    for(var monkeyNum = 0; monkeyNum < monkies.Count(); monkeyNum++) {
      var monkey = monkies[monkeyNum];
      foreach(var item in monkies[monkeyNum].items) {
        monkey.inspected++;
        var newWorry = monkey.newWorry(item);
        var targetMonkeyIndex = ((newWorry % monkey.test) == 0 ? monkey.trueTarget : monkey.falseTarget);
        var targetMonkey = monkies[targetMonkeyIndex];
        targetMonkey.items.Add(newWorry);
      }
      monkey.items.Clear();
    }
  }

  public static void Main() {
    string[] lines = File.ReadAllLines("data.txt");

    var monkeys = createMonkeys(lines);
    for(var i = 0; i < 20; i++) {
      playRound(monkeys);
    }

    var inspected = new long[monkeys.Count];
    for(var ind = 0; ind < monkeys.Count; ind++) {
      inspected[ind] = monkeys[ind].inspected;
    }
    Array.Sort(inspected);
    Array.Reverse(inspected);
    Console.WriteLine(inspected[0] * inspected[1]);

    // worry-full version
    monkeys = createMonkeys(lines);
    var commonBase = 1;
    foreach(var monkey in monkeys) {
      monkey.worryFree = false;
      commonBase *= monkey.test;
    }
    Monkey.commonBase = commonBase;
    for(var i = 0; i < 10000; i++) {
      playRound(monkeys);
    }
    inspected = new long[monkeys.Count];
    for(var ind = 0; ind < monkeys.Count; ind++) {
      inspected[ind] = monkeys[ind].inspected;
    }
    Array.Sort(inspected);
    Array.Reverse(inspected);
    Console.WriteLine(inspected[0] * inspected[1]);
  }
}
