using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stawis {
  public class Order {
    string orderId;
    long weight;
    int stationNo;

    public Order(string orderId, long weight) {
      this.orderId = orderId;
      this.weight = weight;
      stationNo = -1;
    }

    public string OrderId {
      get {return this.orderId;}
      set {this.orderId = value;}
    }
    public long Weight {
      get {return this.weight;}
      set {this.weight = value;}
    }
    public int StationNo {
      get {return this.stationNo;}
      set {this.stationNo = value;}
    }
  }
}
