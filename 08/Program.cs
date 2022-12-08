using System.Collections;

class Program {
  static int height = 0;
  static int width = 0;

  static int[] colValues(int[,] input, int col, int rowStart, int rowEnd) {
    int[] rv = new int[rowEnd - rowStart + 1];
    for(int ind = rowStart; ind <= rowEnd; ind++) {
      rv[ind - rowStart] = input[ind, col];
    }
    return rv;
  }

  static int[] rowValues(int[,] input, int row, int colStart, int colEnd) {
    int[] rv = new int[colEnd - colStart + 1];
    for(int ind = colStart; ind <= colEnd; ind++) {
      rv[ind - colStart] = input[row, ind];
    }
    return rv;
  }

  static bool isVisible(int[,] input, int row, int col) {
    if (row == 0 || row == height - 1) {
      return true;
    }
    if (col == 0 || col == width - 1) {
      return true;
    }

    var treeHeight = input[row, col];
    // Couldn't find a much better way to do a slick of a m-d array.
    var up = colValues(input, col, 0, row - 1);
    var down = colValues(input, col, row + 1, height - 1);
    var left = rowValues(input, row, 0, col - 1);
    var right = rowValues(input, row, col + 1, width - 1);

    if (up.Max() >= treeHeight && down.Max() >= treeHeight && left.Max() >= treeHeight && right.Max() >= treeHeight) {
      return false;
    }

    return true;
  }

  static int dist(int[] heights, int treeHeight) {
    int score = 0;
    foreach(int height in heights) {
      if (height >= treeHeight) {
        return score + 1;
      }
      score++;
    }
    return score;
  }

  static int senicScore(int[,] input, int row, int col) {
    if (row == 0 || row == height - 1) {
      return 0;
    }
    if (col == 0 || col == width - 1) {
      return 0;
    }

    var treeHeight = input[row, col];
    // Couldn't find a much better way to do a slick of a m-d array.
    var up = colValues(input, col, 0, row - 1);
    Array.Reverse(up);
    var down = colValues(input, col, row + 1, height - 1);
    var left = rowValues(input, row, 0, col - 1);
    Array.Reverse(left);
    var right = rowValues(input, row, col + 1, width - 1);

    return dist(up, treeHeight) * dist(down, treeHeight) * dist(left, treeHeight) * dist(right, treeHeight);
  }

  public static void Main() {
    var lines = File.ReadAllLines("data.txt");
    height = lines.Length;
    width = lines[0].Length;
    int[,] data = new int[height, width];

    for(int row = 0; row < height; row++) {
      char[] elements = lines[row].ToCharArray();
      for(int col = 0; col < width; col++) {
        data[row, col] = Int32.Parse(string.Format("{0}", elements[col]));
      }
    }

    int visible = 0;
    for(int row = 0; row < height; row++) {
      for(int col = 0; col < width; col++) {
        if (isVisible(data, row, col)) {
          visible++;
        }
      }
    }

    Console.WriteLine(visible);

    int bestScore = 0;
    for(int row = 0; row < height; row++) {
      for(int col = 0; col < width; col++) {
        bestScore = Math.Max(bestScore, senicScore(data, row, col));
      }
    }

    Console.WriteLine(bestScore);
  }
}
