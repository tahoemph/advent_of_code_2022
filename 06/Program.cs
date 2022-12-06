using System.Collections;

class Program {
  static void rotl(char[] chars) {
    for(int ind = 0; ind < chars.Length - 1; ind++) {
      chars[ind] = chars[ind + 1];
    }
  }

  static bool checkUniqueness(char[] chars) {
    for(int indI = 0; indI < chars.Length - 1; indI++) {
      for (int indJ = indI + 1; indJ < chars.Length; indJ++) {
        if (chars[indI] == chars[indJ]) {
          return false;
        }
      }
    }
    return true;
  }

  static int findDistinct(string input, int num) {
    char[] last = new char[num];
    int ind;
    for (ind = 0; ind < num; ind++) {
      last[ind] = input[ind];
    }

    while(!checkUniqueness(last)) {
      if (ind >= input.Length) {
        break;
      }
      rotl(last);
      last[num - 1] = input[ind];
      ind++;
    }

    return ind;
  }

  public static void Main() {
    string[] lines = File.ReadAllLines("data.txt");
    string input = lines[0];

    Console.WriteLine(findDistinct(input, 4));
    Console.WriteLine(findDistinct(input, 14));
  }
}
