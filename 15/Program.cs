using System.Collections;

class Sensor {
  private int _sensorRow = 0;
  private int _sensorCol = 0;
  private int _beaconRow = 0;
  private int _beaconCol = 0;
  private int _radius = 0;

  public Sensor(int sensorRow, int sensorCol, int beaconRow, int beaconCol) {
    _sensorRow = sensorRow;
    _sensorCol = sensorCol;
    _beaconRow = beaconRow;
    _beaconCol = beaconCol;
    _radius = distance(beaconRow, beaconCol);
  }

  public int minCol() {
    return _sensorCol - _radius;
  }

  public int maxCol() {
    return _sensorCol + _radius;
  }

  private int radius() {
    return _radius;
  }

  private int distance(int row, int col) {
    return Math.Abs(_sensorRow - row) + Math.Abs(_sensorCol - col);
  }

  public bool canSee(int row, int col) {
    return distance(row, col) <= radius();
  }

  public bool isBeacon(int row, int col) {
    return row == _beaconRow && col == _beaconCol;
  }
}

class Program {
  public static void Main() {
    var lines = File.ReadAllLines("data.txt");

    var sensors = new List<Sensor>();
    foreach(var line in lines) {
      var parts = line.Split(":");
      var sensorInfo = parts[0];
      var beaconInfo = parts[1];

      var sensorParts = sensorInfo.Split(",");
      var sensorCol = Int32.Parse(sensorParts[0].Split("=")[1]);
      var sensorRow = Int32.Parse(sensorParts[1].Split("=")[1]);

      var beaconParts = beaconInfo.Split(",");
      var beaconCol = Int32.Parse(beaconParts[0].Split("=")[1]);
      var beaconRow = Int32.Parse(beaconParts[1].Split("=")[1]);

      sensors.Add(new Sensor(sensorRow, sensorCol, beaconRow, beaconCol));
    }

    int minCol = 100000;
    int maxCol = 0;
    foreach(var sensor in sensors) {
      minCol = Math.Min(minCol, sensor.minCol());
      maxCol = Math.Max(maxCol, sensor.maxCol());
    } 

    var notPossible = 0;
    var row = 2000000;
    for(var col = minCol - 1; col <= maxCol + 1; col++) {
      int sensorInd;
      for(sensorInd = 0; sensorInd < sensors.Count(); sensorInd++) {
        var sensor = sensors[sensorInd];
        if (sensor.canSee(row, col) || sensor.isBeacon(row, col)) {
          break;
        }
      }
      if (sensorInd < sensors.Count && !sensors[sensorInd].isBeacon(row, col)) {
        notPossible++;
      }
    }

    System.Console.WriteLine(notPossible);
  }
}