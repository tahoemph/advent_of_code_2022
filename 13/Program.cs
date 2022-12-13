using System.Collections;

public enum ValueType { Number, Array };
class PacketElement {
  public ValueType valueType;
  public int number = 0;
  public List<PacketElement> element = new List<PacketElement>();
}

class Packet {
  public List<PacketElement> packet;
  public string input;

  private string getToken(string input, ref int offset) {
    string rv = "";
    while(input[offset] != ',' && input[offset] != ']') {
      rv += input[offset++];
    }
    if (input[offset] == ',')
      offset++;
    return rv;
  }
  private List<PacketElement> ParseElement(string elem, ref int offset) {
    if (elem[offset] != '[') {
      throw new ArgumentException(elem[offset].ToString(), "incorrect character");
    }
    List<PacketElement> rv = new List<PacketElement>();
    offset++;
    do {
      if (Char.IsDigit(elem[offset])) {
        var token = getToken(elem, ref offset);
        var element = new PacketElement {
          valueType = ValueType.Number,
          number = Int32.Parse(token)
        };
        rv.Add(element);
      } else if (elem[offset] == '[') {
        var subElements = ParseElement(elem, ref offset);
        var element = new PacketElement {
          valueType = ValueType.Array,
          element = subElements
        };
        rv.Add(element);
        if (elem[offset] != ']')
          throw new ArgumentException(elem[offset].ToString(), "bad character in stream");
        offset++;
        if (elem[offset] == ',')
          offset++;
      }
    } while(elem[offset] != ']');
    return rv;
  }
  public Packet(string elem) {
    int ind = 0;
    input = elem;
    packet = ParseElement(elem, ref ind);
  }
}
class Program {
  public static int PacketElementListComparer(List<PacketElement> left, List<PacketElement> right) {
    for(var ind = 0; ind < left.Count; ind++) {
      if (ind >= right.Count) {
        return 1;
      }
      var leftPE = left[ind];
      var rightPE = right[ind];
      var rv = Comparer(leftPE, rightPE);
      if (rv != 0)
        return rv;
    }
    if (left.Count < right.Count)
      return -1;
    if (right.Count < left.Count)
      return 1;
    return 0;
  }
  public static int Comparer(PacketElement left, PacketElement right) {
    var leftType = left.valueType;
    var rightType = right.valueType;

    if ((leftType == ValueType.Number && rightType == ValueType.Array) || (leftType == ValueType.Array && rightType == ValueType.Number)) {
      if (leftType == ValueType.Number) {
        var leftP = new Packet("[[" + left.number + "]]");
        left = leftP.packet[0];
        leftType = ValueType.Array;
      }
      if (rightType == ValueType.Number) {
        var rightP = new Packet("[[" + right.number + "]]");
        right = rightP.packet[0];
        rightType = ValueType.Array;
      }
    }

    if (leftType == ValueType.Number && rightType == ValueType.Number) {
      if (left.number > right.number)
        return 1;
      if (left.number < right.number)
        return -1;
    } else if (leftType == ValueType.Array && rightType == ValueType.Array) {
      var rv = PacketElementListComparer(left.element, right.element);
      if (rv != 0)
        return rv;
    };
    return 0;
  }

  private static int comparePackets(Packet x, Packet y) {
    return PacketElementListComparer(x.packet, y.packet);
  }
  public static void Main() {
    var lines = File.ReadAllLines("data.txt");
    Packet[,] input = new Packet[(lines.Length + 1)/3, 2];

    int pairNum = 0;
    for(int ind = 0; ind < lines.Length; ind += 3) {
      input[pairNum, 0] = new Packet(lines[ind]);
      input[pairNum, 1] = new Packet(lines[ind + 1]);
      pairNum++;
    }

    var indiceSum = 0;
    for(var ind = 0; ind < input.GetLength(0); ind++) {
      if (PacketElementListComparer(input[ind, 0].packet, input[ind, 1].packet) <= 0) {
        indiceSum += ind + 1;
      }
    }
    System.Console.WriteLine(indiceSum);

    var packetList = new List<Packet>();
    packetList.Add(new Packet("[[2]]"));
    packetList.Add(new Packet("[[6]]"));
    foreach(var packet in input) {
      packetList.Add(packet);
    }
    packetList.Sort(comparePackets);
    var dividerPackets = new List<int>();
    for(var ind = 0; ind < packetList.Count; ind++) {
      if (packetList[ind].input == "[[2]]")
        dividerPackets.Add(ind + 1);
      else if (packetList[ind].input == "[[6]]")
        dividerPackets.Add(ind + 1);
    }
    System.Console.WriteLine(dividerPackets[0] * dividerPackets[1]);
  }
}