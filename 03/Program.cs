using System.Collections;

class Program {
  static List<HashSet<char>[]> bags = new List<HashSet<char>[]>();

  static int value(char c) {
    int lower = c - 'a';
    int upper = c - 'A';    
    int rv =-1;
    if (c >= 'a' && c <= 'z') {
      rv = c - 'a' + 1;
    } else if (c >= 'A' && c <= 'Z') {
      rv = c - 'A' + 27;
    }
    return rv;
  }

  private static int pass1(List<HashSet<char>[]> bags) {
    int rv = 0;
    foreach (HashSet<char>[] sacks in bags) {
      HashSet<char> lhs = new HashSet<char>(sacks[0]);
      lhs.IntersectWith(sacks[1]);
      foreach (char c in lhs) {
        rv += value(c);
      }
    }
    return rv;
  }

  private static int pass2(List<HashSet<char>[]> bags) {
    int rv = 0;
    for (int index = 0; index < bags.Count; index += 3) {
      HashSet<char> first = bags[index + 0][0];
      first.UnionWith(bags[index + 0][1]);

      HashSet<char> second = bags[index + 1][0];
      second.UnionWith(bags[index + 1][1]);

      HashSet<char> third = bags[index + 2][0];
      third.UnionWith(bags[index + 2][1]);

      HashSet<char> answer = first;
      first.IntersectWith(second);
      first.IntersectWith(third);

      foreach (char c in first) {
        rv += value(c);
      }
    }
    return rv;
  }

  public static void Main() {
    string[] lines = File.ReadAllLines("data.txt");

    foreach (string line in lines) {
      int length = line.Length;
      HashSet<char>[] sacks = new HashSet<char>[2];
      sacks[0] = new HashSet<char>(line.Substring(0, length/2));
      sacks[1] = new HashSet<char>(line.Substring(length/2));
      bags.Add(sacks);
    }

    Console.WriteLine(pass1(bags));
    Console.WriteLine(pass2(bags));
  }
}