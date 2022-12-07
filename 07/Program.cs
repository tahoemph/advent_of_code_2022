using System.Collections;

class DirectoryEntry {
  public string name { get; set; }
  public int size { get; set; }
  public DirectoryEntry? parent { get; set; }

  List<DirectoryEntry> children;

  public DirectoryEntry(string dirName, int dirSize) {
    name = dirName;
    size = dirSize;
    children = new List<DirectoryEntry>();
  }

  public DirectoryEntry addEntry(string name, int size) {
    var newChild = new DirectoryEntry(name, size);
    newChild.parent = this;
    children.Add(newChild);
    return newChild;
  }

  public DirectoryEntry findOrAddEntry(string newName, int newSize) {
    foreach(var child in children) {
      if (child.name == newName && child.size == newSize) {
        return child;
      }
    }
    return addEntry(newName, newSize);
  }

  public int getSize() {
    var total = size;
    foreach (DirectoryEntry child in children) {
      total += child.getSize();
    }
    return total;
  }

  public List<DirectoryEntry> getChildren() {
    return children;
  }

  public void dump(int offset = 0) {
    Console.WriteLine("{0} - {1} {2}", "".PadLeft(offset), name, size);
    foreach(DirectoryEntry child in children) {
      child.dump(offset + 2);
    }
  }
}

class Program {
  static void pass1(DirectoryEntry root) {
    List<DirectoryEntry> dirs = filterMaximum(root.getChildren());
    var total = 0;
    foreach(var dir in dirs) {
      total += dir.getSize();
    }
    Console.WriteLine(total);
  }

  static void pass2(DirectoryEntry root) {
    var rootSize = root.getSize();
    int availableSpace = 70000000 - rootSize; 
    int neededSpace = 30000000 - availableSpace;
    List<DirectoryEntry> dirs = filterMinimum(root.getChildren(), neededSpace);
    var minimum = int.MaxValue;
    foreach(var dir in dirs) {
      minimum = Math.Min(minimum, dir.getSize());
    }
    Console.WriteLine(minimum);
  }

  static List<DirectoryEntry> filterMaximum(List<DirectoryEntry> children, int limit = 100_000) {
    var rv = new List<DirectoryEntry>();
    foreach (var child in children) {
      if (child.size == 0) {
        if (child.getSize() < limit) {
          rv.Add(child);
        }
        rv.AddRange(filterMaximum(child.getChildren()));
      }
    }

    return rv;
  }

  static List<DirectoryEntry> filterMinimum(List<DirectoryEntry> children, int limit = 100_000) {
    var rv = new List<DirectoryEntry>();
    foreach (var child in children) {
      if (child.size == 0) {
        if (child.getSize() >= limit) {
          rv.Add(child);
        }
        rv.AddRange(filterMinimum(child.getChildren(), limit));
      }
    }

    return rv;
  }

  static DirectoryEntry doCommandCD(string[] command, DirectoryEntry root, DirectoryEntry current) {
    if (command[2] == "/") {
      return root;
    }

    if (command[2] == "..") {
      return current.parent;
    }

    return current.findOrAddEntry(command[2], 0);
  }

  static void doCommandLS(DirectoryEntry current, Queue<string> input) {
    var next = input.Peek();
    while (next[0] != '$') {
      var commandParts = input.Dequeue().Split(' ');;
      var name = commandParts[1];
      int size = 0;
      if (commandParts[0] != "dir") {
        size = Int32.Parse(commandParts[0]);
      }
      current.findOrAddEntry(name, size);
      if (input.Count == 0) {
        return;
      }
      next = input.Peek();
    }
  }
  static DirectoryEntry executeCommands(Queue<string> input) {
    var root = new DirectoryEntry("/", 0);
    root.parent = root;
    var current = root;

    do {
      var command = input.Dequeue();
      var commandParts = command.Split(' ');
      switch(commandParts[1]) {
        case "cd":
          current = doCommandCD(commandParts, root, current);
          break;
        case "ls":
          doCommandLS(current, input);
          break;
        default:
          Console.WriteLine("failure!");
          break;
      }
    } while (input.Count > 0);

    return root;
  }

  public static void Main() {
    var lines = File.ReadAllLines("data.txt");

    DirectoryEntry root = executeCommands(new Queue<string>(lines));
    Console.WriteLine(root.getSize());
    pass1(root);
    pass2(root);
  }
}
