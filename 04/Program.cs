using System.Collections;

class Program {
  static List<HashSet<int>[]> assignments = new List<HashSet<int>[]>();

  static int pass1() {
    int rv = 0;
    foreach (HashSet<int>[] assignment in assignments) {
      if (assignment[0].IsSubsetOf(assignment[1]) || assignment[1].IsSubsetOf(assignment[0])) {
        rv++;
      }
    }
    return rv;
  }

  static int pass2() {
    int rv = 0;
    foreach (HashSet<int>[] assignment in assignments) {
      HashSet<int> intersection = new HashSet<int>(assignment[0]);
      intersection.IntersectWith(assignment[1]);
      if (intersection.Count > 0) {
        rv++;
      }
    }
    return rv;
  }

  static HashSet<int> range(string pair) {
    string[] parts = pair.Split('-');
    int start = Int32.Parse(parts[0]);
    int count = Int32.Parse(parts[1]) - start + 1;
    return new HashSet<int>(Enumerable.Range(start, count));
  }

  public static void Main() {
    string[] lines = File.ReadAllLines("data.txt");

    foreach (string line in lines) {
      string[] pairs = line.Split(",");
      HashSet<int>[] ranges = new HashSet<int>[2];

      ranges[0] = range(pairs[0]);
      ranges[1] = range(pairs[1]);

      assignments.Add(ranges);
    }

    Console.WriteLine(pass1());
    Console.WriteLine(pass2());
  }
}
