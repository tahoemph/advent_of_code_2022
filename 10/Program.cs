using System.Collections;

class CPU {
  public int registerX = 1;
  public int cycle = 0;
  public Func<int, int, int> watcher;

  void callWatcher() {
    if (watcher != null)
      watcher(cycle, registerX);
  }

  public int operation(string opcode, int operand) {
    // Console.WriteLine($"{opcode} {operand}");
    switch(opcode) {
      case "noop":
        cycle++;
        callWatcher();
        break;
      case "addx":
        cycle++;
        callWatcher();
        cycle++;
        callWatcher();
        registerX += operand;
        break;
      default:
        throw new ArgumentException(opcode, "Should be either 'noop' or 'addx'");
    }

    return registerX;
  }
}
class Program {
  static int signalStrength = 0;
  static int watcher(int cycle, int registerX) {
    // Console.WriteLine($"cycle {cycle} register {registerX}");
    if (cycle == 20 || cycle == 60 || cycle == 100 || cycle == 140 || cycle == 180 || cycle == 220) {
      signalStrength += cycle * registerX;
      // Console.WriteLine($"new signal strength {signalStrength} (delta {cycle*registerX})");
    }
    return signalStrength;
  }

  public static void Main() {
    var lines = File.ReadAllLines("data.txt");
    var operations = new List<Tuple<string, int>>();

    foreach(var line in lines) {
      var parts = (line + " 0").Split(" "); // hack a 0 onto noop lines to make the line below work.
      operations.Add(new Tuple<string, int>(parts[0], Int32.Parse(parts[1])));
    }

    var cpu = new CPU();
    cpu.watcher = watcher;
    foreach(var operation in operations) {
      cpu.operation(operation.Item1, operation.Item2);
    }
    Console.WriteLine(signalStrength);
  }
}
