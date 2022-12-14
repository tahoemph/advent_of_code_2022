using System.Collections;

class Game {
  char[,] board = new char[1000, 1000];

  public Game() {
    for(int row = 0; row < board.GetLength(0); row++) {
      for(int col = 0; col < board.GetLength(1); col++) {
        board[row, col] = '.';
      }
    }
  }
  public void draw(int lastRow, int lastCol, int newRow, int newCol) {
    var minRow = Math.Min(lastRow, newRow);
    var maxRow = Math.Max(lastRow, newRow); 
    var minCol = Math.Min(lastCol, newCol);
    var maxCol = Math.Max(lastCol, newCol); 
    if (lastRow == newRow) {
      for(int col = minCol; col <= maxCol; col++) {
        board[lastRow, col] = '#';
      }
    } else {
      for(int row = minRow; row <= maxRow; row++) {
        board[row, lastCol] = '#';
      }
    }
  }

  public void dump() {
    int maxRow = 0, minRow = 1000, maxCol = 0, minCol = 1000;
    for(var row = 0; row < board.GetLength(0); row++) {
      for(var col = 0; col < board.GetLength(1); col++) {
        if (board[row, col] == 'o') {
          maxRow = maxRow > row ? maxRow : row;
          minRow = minRow < row ? minRow : row;
          maxCol = maxCol > col ? maxCol : col;
          minCol = minCol < col ? minCol : col;
        }
      }
    }

    var minimumRow = Math.Max(0, minRow - 2);
    for(int row = minimumRow; row < Math.Min(board.GetLength(0), maxRow + 2); row++) {
      for(int col = Math.Max(0, minCol - 3); col < Math.Min(board.GetLength(1), maxCol + 3); col++) {
        if (row == minimumRow && col == 500) {
          System.Console.Write('x');
        } else {
          System.Console.Write(board[row, col]);
        }
      }
      System.Console.WriteLine();
    }
  }
  public void constructPath(string input) {
    var parts = input.Split("->");
    int lastRow = -1, lastCol = -1;
    foreach(var part in parts) {
      var coords = part.Split(",");
      int newCol = Int32.Parse(coords[0]);
      int newRow = Int32.Parse(coords[1]);
      if (lastRow != -1) {
        draw(lastRow, lastCol, newRow, newCol);
        lastRow = newRow;
        lastCol = newCol;
      }
      lastRow = newRow;
      lastCol = newCol;
    }
  }

  public bool dropSand() {
    int curRow = 0;
    int curCol = 500;
    while(true) {
      if (curRow + 1 >= board.GetLength(0) || curRow < 0 || curCol >= board.GetLength(1) || curCol < 0) {
        return true;
      }

      if (board[curRow + 1, curCol] == '.') {
        curRow += 1;
      } else {
        if (board[curRow + 1, curCol - 1] == '.') {
          curRow += 1;
          curCol += -1;
        } else if (board[curRow + 1, curCol + 1] == '.') {
          curRow += 1;
          curCol += 1;
        } else {
          board[curRow, curCol] = 'o';
          return false;
        }
      }
    }
  }
}
class Program {
  public static void Main() {
    var lines = File.ReadAllLines("data.txt");

    var game = new Game();
    foreach(var line in lines) {
      game.constructPath(line);
    }

    var grains = 0;
    var done = false;
    do {
      grains++;
      done = game.dropSand();
    } while(!done);
    System.Console.WriteLine(grains);
  }
}