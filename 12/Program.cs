using System.Collections;

class Program {
  static int shortestPath(char[,] input, int startRow, int startCol) {
    var newQueue = new Queue<Tuple<int, int>>();
    var seen = new HashSet<Tuple<int, int>>();
    var root = new Tuple<int, int>(startRow, startCol);
    var maxRows = input.GetLength(0);
    var maxCols = input.GetLength(1);
    var directions = new [] {new Tuple<int, int>(-1, 0), new Tuple<int, int>(1, 0), new Tuple<int, int>(0, -1), new Tuple<int, int>(0, 1)};

    seen.Add(root);
    newQueue.Enqueue(root);
    var depth = -1;
    while(newQueue.Count() > 0) {
      depth++;
      var queue = new Queue<Tuple<int, int>>(newQueue);
      newQueue = new Queue<Tuple<int, int>>();
      while(queue.Count() > 0) {
        var node = queue.Dequeue();
        if (input[node.Item1, node.Item2] == 'E') {
          return depth;
        }
        var curRow = node.Item1;
        var curCol = node.Item2;
        var curHeight = input[curRow, curCol];
        if (curHeight == 'S')
          curHeight = 'a';
        foreach(var direction in directions) {
          var newRow = curRow + direction.Item1;
          var newCol = curCol + direction.Item2;
          if (newRow < 0 || newRow >= maxRows || newCol < 0 || newCol >= maxCols)
            continue;
          var newNode = new Tuple<int, int>(newRow, newCol);
          if (seen.Contains(newNode)) {
            continue;
          }
          var newHeight = input[newRow, newCol];
          if (newHeight == 'E') {
            newHeight = 'z';
          } 
          if (curHeight >= newHeight - 1) {
            newQueue.Enqueue(newNode);
            seen.Add(newNode);
          }
        }
      }
    }
    return -1;
  }

  public static void Main() {
    var lines = File.ReadAllLines("data.txt");
    var numRows = lines.Count();
    var numCols = lines[0].Count();
    char[,] map = new char[numRows, numCols];

    for(int row = 0; row < numRows; row++) {
      for(int col = 0; col < numCols; col++) {
        map[row, col] = lines[row][col];
      }
    }

    var startRow = -1;
    var startCol = -1;
    for(int row = 0; row < numRows && startRow == -1; row++) {
      for(int col = 0; col < numCols && startCol == -1; col++) {
        if (map[row, col] == 'S') {
          startRow = row;
          startCol = col;
          map[row, col] = 'a';
          break;
        }
      }
    }

    var shortest = shortestPath(map, startRow, startCol);
    if (shortest == -1) {
      Console.WriteLine("no shortest path");
    } else {
      Console.WriteLine(shortest);
    }

    var minimum = shortest;
    for(int row = 0; row < numRows; row++) {
      for(int col = 0; col < numCols; col++) {
        if (map[row, col] == 'a' || map[row, col] == 'S') {
          var length = shortestPath(map, row, col);
          if (length > 0 && length < shortest) {
            shortest = length;
          }
        }
      }
    }
    Console.WriteLine(shortest);
  }
}
