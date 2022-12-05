using System.Collections;

class Program {
  public static Hashtable ChoiceValues = new Hashtable() {
    {'X', 1},
    {'Y', 2},
    {'Z', 3}
  };

  public static Hashtable ResultValues = new Hashtable() {
    {'A', new Hashtable() {
        {'X', 3},
        {'Y', 6},
        {'Z', 0}
      }
    },
    {'B', new Hashtable() {
        {'X', 0},
        {'Y', 3},
        {'Z', 6}
      }
    },
    {'C', new Hashtable() {
        {'X', 6},
        {'Y', 0},
        {'Z', 3}
      }
    }
  };

  public static void pass1(string[] lines) {
    int result = 0;
    foreach (string line in lines) {
      string[] letters = line.Split(' ');
      char opponent = letters[0][0];
      char us = letters[1][0];
      Hashtable ValueHash = (Hashtable)ResultValues[opponent];
      result += (int)ValueHash[us] + (int)ChoiceValues[us];
    }

    Console.WriteLine(result);
  }
  public static void pass2(string[] lines) {}
  public static void Main() {
    string[] lines = File.ReadAllLines("data.txt");
    pass1(lines);
    pass2(lines);
  }
}