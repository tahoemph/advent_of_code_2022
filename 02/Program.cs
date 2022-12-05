using System.Collections;

class Program {
  static readonly Hashtable GameResultValues = new Hashtable() {
    {'A', new Hashtable() {
        {'X', 3 + 1},
        {'Y', 6 + 2},
        {'Z', 0 + 3}
      }
    },
    {'B', new Hashtable() {
        {'X', 0 + 1},
        {'Y', 3 + 2},
        {'Z', 6 + 3}
      }
    },
    {'C', new Hashtable() {
        {'X', 6 + 1},
        {'Y', 0 + 2},
        {'Z', 3 + 3}
      }
    }
  };

  static readonly Hashtable ResultValues = new Hashtable() {
    {'A', new Hashtable() {
        {'X', 0 + 3},
        {'Y', 3 + 1},
        {'Z', 6 + 2}
      }
    },
    {'B', new Hashtable() {
        {'X', 0 + 1},
        {'Y', 3 + 2},
        {'Z', 6 + 3}
      }
    },
    {'C', new Hashtable() {
        {'X', 0 + 2},
        {'Y', 3 + 3},
        {'Z', 6 + 1}
      }
    }
  };

  static List<string> operations = new List<string>();

  public static void pass(string[] lines, Hashtable values) {
    int result = 0;
    foreach (string operation in operations) {
      char opponent = operation[0];
      char us = operation[1];
      Hashtable ValueHash = (Hashtable)values[opponent];
      result += (int)ValueHash[us];
    }

    Console.WriteLine(result);
  }

  public static void Main() {
    string[] lines = File.ReadAllLines("data.txt");

    foreach (string line in lines) {
      string[] letters = line.Split(' ');
      char[] chars = {letters[0][0], letters[1][0]};
      string result = new string(chars);
      operations.Add(result);
    }

    pass(lines, GameResultValues);
    pass(lines, ResultValues);
  }
}