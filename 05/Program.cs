using System.Collections;

class Program {
  static List<Stack<char>> stacks = new List<Stack<char>>();
  static List<int[]> commands = new List<int[]>();

  static List<Stack<char>> copyStack() {
    List<Stack<char>> rv = new List<Stack<char>>();

    for(int index = 0; index < stacks.Count; index++) {
      List<char> tmp = new List<char>(stacks[index]);
      tmp.Reverse();
      Stack<char> q = new Stack<char>(tmp);
      rv.Add(q);
    }

    return rv;
  }

  static void dumpStacks() {
    int longest = 0;
    List<Stack<char>> copy_stacks = copyStack();
    foreach (Stack<char> stack in copy_stacks) {
      longest = Math.Max(longest, stack.Count);
    }

    for (; longest > 0; longest--) {
      for (int column = 0; column < copy_stacks.Count; column++) {
        if (copy_stacks[column].Count == longest) {
          Console.Write(String.Format("[{0}] ", copy_stacks[column].Pop()));
        } else {
          Console.Write("    ");
        }
      }
      Console.WriteLine("");
    }
  }

  static void extendStacks(int maxStackNo) {
    if (stacks.Count > maxStackNo) {
      return;
    }
    for (int index = stacks.Count; index <= maxStackNo; index++) {
      stacks.Add(new Stack<char>());
    }
  }

  static void move(int num, int from, int to) {
    if (num > 1) {
      for (int count = 0; count < num; count++) {
        move(1, from, to);
      }
    } else {
      stacks[to - 1].Push(stacks[from - 1].Pop());
    }
  }

  static void multi_move(int num, int from, int to) {
    Stack<char> tmp = new Stack<char>();
    for (int ind = 0; ind < num; ind++) {
      tmp.Push(stacks[from - 1].Pop());
    }
    for (int ind = 0; ind < num; ind++) {
      stacks[to - 1].Push(tmp.Pop());
    }
  }

  static string pass1() {
    foreach (int[] command in commands) {
      move(command[0], command[1], command[2]);
    }
    string rv = "";
    foreach (Stack<char> stack in stacks) {
      rv += stack.Pop();
    }
    return rv;
  }

  static string pass2() {
    foreach (int[] command in commands) {
      multi_move(command[0], command[1], command[2]);
    }
    string rv = "";
    foreach (Stack<char> stack in stacks) {
      rv += stack.Pop();
    }
    return rv;
  }

  public static void Main() {
    string[] lines = File.ReadAllLines("data.txt");

    bool setup_done = false;
    foreach (string line in lines) {
      if ((line.Length > 1) && Char.IsDigit(line, 1)) {
        setup_done = true;
        continue;
      }

      if (!setup_done) {
        for (int index = 0; index < line.Length; index += 4) {
          int stackNo = index / 4;
          extendStacks(stackNo);
          if (line[index + 1] != ' ') {
            stacks[stackNo].Push(line[index + 1]);
          }
        }
      } else {
        string[] parts = line.Split(" ");
        if (parts.Length < 6) {
          continue;
        }
        int[] command = {Int32.Parse(parts[1]), Int32.Parse(parts[3]), Int32.Parse(parts[5])};
        commands.Add(command);
      }
    }

    // Reverse the elements in the Stacks as we read the upside down.
    for (int ind = 0; ind < stacks.Count; ind++) {
      // This reverses the order.
      List<char> tmp = new List<char>(stacks[ind]);
      stacks[ind] = new Stack<char>(tmp);
    }

    List<Stack<char>> save_stack = copyStack();
    Console.WriteLine(pass1());
    stacks = save_stack;
    Console.WriteLine(pass2());
  }
}
