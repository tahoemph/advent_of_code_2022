using System.Collections;

class DirectoryEntry {
  public string name { get; set; }
  public int size { get; set; }

  List<DirectoryEntry> children;

  public DirectoryEntry(string dirName, int dirSize) {
    name = dirName;
    size = dirSize;
    children = new List<DirectoryEntry>();
  }

  public DirectoryEntry addEntry(string name, int size) {
    var newChild = new DirectoryEntry(name, size);
    children.Add(newChild);
    return newChild;
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
    foreach(DirectoryEntry child in children) {
      child.dump(offset + 2);
    }
  }
}

class Program {
  static DirectoryEntry parseInput(Queue<string> input) {
    DirectoryEntry root = new DirectoryEntry("/", 0);

    input.Dequeue();
    parseEntry(root, input);
    return root;
  }

  static int leadingSpaces(string str) {
    var ind = 0;
    while(str[ind++] == ' ');
    return ind + 1;
  }

  static void pass1(DirectoryEntry root) {
    List<DirectoryEntry> dirs = pass1Filter(root.getChildren());
    var total = 0;
    foreach(var dir in dirs) {
      total += dir.getSize();
    }
    Console.WriteLine(total);
  }

  static List<DirectoryEntry> pass1Filter(List<DirectoryEntry> children) {
    var rv = new List<DirectoryEntry>();
    foreach (var child in children) {
      if (child.size == 0) {
        if (child.getSize() < 100_000) {
          rv.Add(child);
        }
        rv.AddRange(pass1Filter(child.getChildren()));
      }
    }

    return rv;
  }

  static void parseEntry(DirectoryEntry parent, Queue<string> input) {
    var first = input.Peek();
    var level = leadingSpaces(first);
    DirectoryEntry last = parent;

    do {
      var hint = input.Peek();
      var newLevel = leadingSpaces(hint);

      if (newLevel > level) {
        parseEntry(last, input);
      } else if (newLevel < level) {
        return;
      } else {
        var description = input.Dequeue();
        var parts = description.Split(' ');
        if (description.Contains("(dir)")) {
          last = parent.addEntry(parts[level - 1], 0);
        } else {
          string[] sizeParts = parts[level + 1].Split('=');
          last = parent.addEntry(parts[level - 1], Int32.Parse(sizeParts[1].Trim(')')));
        }
      }
    } while(input.Count > 0);
  }

  public static void Main() {
    var lines = File.ReadAllLines("test.txt");

    DirectoryEntry root = parseInput(new Queue<string>(lines));
    Console.WriteLine(root.getSize());
    pass1(root);
  }
}
