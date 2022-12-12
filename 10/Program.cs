using System.Collections;

class Screen {
  public Char[,] display = new Char[7, 40];

  public void setScreen(int cycle, int spriteLeftEdge) {
    int row = (int)Math.Floor(cycle/40.0);
    int col = cycle % 40;
    // Console.WriteLine($"setScreen({cycle}, {spriteLeftEdge}) {row} {col}");
    if (col >= spriteLeftEdge && col <= spriteLeftEdge + 2) {
      display[row, col] = '#';
    } else {
      display[row, col] = '.';
    }
  }

  public void dump() {
    for(int row = 0; row < 6; row++) {
      var displayRow = new char[40];
      for(int ind = 0; ind < 40; ind++) {
        displayRow[ind] = display[row, ind];
      }
      Console.WriteLine(string.Join(" ", displayRow));
    }
  }
}
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
  static Screen display = new Screen();

  static int watcher(int cycle, int registerX) {
    // Console.WriteLine($"cycle {cycle} register {registerX}");
    if (cycle == 20 || cycle == 60 || cycle == 100 || cycle == 140 || cycle == 180 || cycle == 220) {
      signalStrength += cycle * registerX;
      // Console.WriteLine($"new signal strength {signalStrength} (delta {cycle*registerX})");
    }
    return signalStrength;
  }

  static int screenBuilder(int cycle, int registerX) {
    display.setScreen(cycle, registerX);
    return 0;
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

    cpu = new CPU();
    cpu.watcher = screenBuilder;
    foreach(var operation in operations) {
      cpu.operation(operation.Item1, operation.Item2);
    }
    display.dump();
  }
}
