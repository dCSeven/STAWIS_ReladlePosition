using System;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Stawis {
  public class Model {
    Thread converterControl;
    Thread tappingControl;
    Thread desulphurizationControl;
    static Desulphurization des1 = new Desulphurization(1,50,50);
    static Desulphurization des2 = new Desulphurization(2,50,50);
    static Converter conv1 = new Converter(1,-50,-50);
    static Converter conv2 = new Converter(2,-50,-50);
    static Converter conv3 = new Converter(3,-50,-50);
    static Ladle chgl = new Ladle(1);
    static SendMessage sender = new SendMessage();

    Station[] stations = {conv1,conv2,conv3,des1,des2,chgl};
    List<Order> orderPool = new List<Order>();

    List<string> alarmList = new List<string>();

    Form1 form;

    public Model(Form1 form) {
      this.form = form;
    }

    public Station TapStation { get; set; }
    public Station ConvStation { get; set; }

    public Boolean OrderRefresh { get; set; }
    public Boolean AlarmRefresh { get; set; }
    public List<Order> OrderPool {
      get { return orderPool; }
    }
    public List<string> AlarmList {
      get { return alarmList; }
    }

    public void resetAll() {
      for (int stNo = 0; stNo < stations.Length; stNo++) {
        Station station = stations[stNo];
        station.State = States.FREE;
        station.Center = station.ParkPosition;
        station.Order = null;
        station.Refresh = true;
        if (station is Ladle) station.Move = true;
      }
      for (int i = orderPool.Count - 1; i >= 0; i--) {
        orderPool.RemoveAt(i);
      }
      OrderRefresh = true;
      for (int i = alarmList.Count - 1; i >= 0; i--) {
        alarmList.RemoveAt(i);
      }
      AlarmRefresh = true;
    }

    public void setParkPositionOfLadle(int ladleNo, Point parkPosition) {
      int stNo = ladleNo + 4;   // ladleNo 1 --> stationNo 5
      Station ladle = getStation(stNo);
      if (ladle is Ladle) {
        ladle.ParkPosition = parkPosition;
      }
    }


    public void orderTakeOver(string orderId, long weight) {
      Boolean orderExists = false;
      foreach (Order o in orderPool) {
        if (o.OrderId.Equals(orderId)) {
          orderExists = true;
          break;
        }
      }
      if (!orderExists) {
        Order o = new Order(orderId, weight);
        orderPool.Add(o);
        OrderRefresh = true;
      } else {
        newAlarm("order " + orderId + " already exists");
      }
    }

    public void newAlarm(string message) {
      alarmList.Add(message);
      AlarmRefresh = true;
    }

    public int getStationCount() {
      return stations.Length;
    }

    public Station getStation(int stNo) {
      if (stNo >= 0 && stNo < stations.Length) {
        return stations[stNo];
      } else {
        return null;
      }
    }

    public void takeoverAtDesulphurization(int desNo) {
      int stNo =desNo + 2;    // aus 1/2 wird 3/4
      if (stationIsValid(stNo)) {
        Station station = stations[stNo];
        if (station is Desulphurization) {
          if (station.State == States.FREE) {
            if (orderPool.Count > 0) {
              Order o = orderPool[0];   // übernimm ersten Auftrag der Liste
              orderPool.RemoveAt(0);    // lösche diesen aus der Liste
              station.Order = o;
              station.State = States.READY;
              station.Refresh = true;
              OrderRefresh = true;
            } else {
              newAlarm("Takeover at Desulphurization "+desNo+": kein Auftrag vorhanden");
            }
          } else {
            newAlarm("Takeover at Desulphurization " + desNo + ": Station ist nicht frei");
          }
        } else {
          newAlarm("Takeover at Desulphurization " + desNo + ": keine Entschwefelungsstation");
        }
      }
    }

    public void desulphurizationOnOff(int desNo, int on) {
     int stNo = desNo + 2;    // aus 1/2 wird 3/4
      if (stationIsValid(stNo)) {
        Station station = stations[stNo];
        if (station is Desulphurization) {
           if (on == States.ON) {
              if (station.State == States.READY) {
                station.State = States.BUSY;
                createDesulphurizationProcessThread((Desulphurization)station);
              } else {
                newAlarm("Entschwefelungsstation " + desNo + " ist nicht frei");
              }
            } else if (on == States.OFF) {
              if (station.State == States.BUSY) {
                stopDesulphurizationProcessThread();
                station.State = States.FINISHED;

              } else {
                newAlarm("Entschwefelungsstation " + desNo + " vorher nicht im Prozess");
              }
            }
            station.Refresh = true;
          } else {
            newAlarm("Desulphurization " + desNo + ": keine Entschwefelungsstation");
          }
		  }
    }

    public void ladleMove(int ladleNo, int dx, int dy) {
      int stNo = ladleNo + 4;   // ladleNo 1 --> stationNo 5
      Station ladle = getStation(stNo);
      if (ladle is Ladle) {
        Point c = ladle.Center;
        ladle.Center = new Point(c.X + dx, c.Y + dy);
        ladle.Move = true;
        ladle.Refresh = true;
      } else {
        newAlarm("ladle move: ladleNo " + ladleNo + " is wrong");
      }
    }

    public void setLadlePosition(int ladleNo, int x, int y) {
      int stNo = ladleNo + 4;   // ladleNo 1 --> stationNo 5
      Station ladle = getStation(stNo);
      if (ladle is Ladle) {
        Point c = ladle.Center;
        ladle.Center = new Point(x, y);
        ladle.Move = true;
        ladle.Refresh = true;
      } else {
        newAlarm("set ladle position: ladleNo " + ladleNo + " is wrong");
      }
    }

    public void converterAvailable(int convNo, int on) {
      int stNo = convNo - 1;
      if (stationIsValid(stNo)) {
        Station station = stations[stNo];
        if (station is Converter) {
          if (on == States.ON) {
            if (station.State == States.INACTIVE) {
              station.State = States.FREE;
            } else {
              newAlarm("ConverterAvailable, Converter "+convNo
                    +": activ-Setzen abgelehnt, da Station nicht inactive ist");
            }
          } else {
           if (station.State == States.FREE) {
              station.State = States.INACTIVE;
            } else {
              newAlarm("ConverterAvailable, Converter "+convNo
                    +": Inactiv-Setzen abgelehnt, da Station nicht frei ist");
            }
           }
          station.Refresh = true;
        } else {
          newAlarm("ConverterAvailable: Converter "+convNo+" ist ungültig");
        }

      }
    }

    public void transportStart() {
      Station des = null;
      for (int st = 0; st < stations.Length; st++) {
        Station station = stations[st];
        if (station is Desulphurization & station.State == States.FINISHED) {
          int distance = calculateDistance(station.Center, chgl.Center);
          if (distance < 100 && chgl.Center.X >= station.Center.X && chgl.Center.Y >= station.Center.Y) {
            des = station;
            Order o = des.Order;
            des.Order = null;
            chgl.Order = o;
            des.State = States.FREE;
            chgl.State = States.BUSY;
            des.Refresh = true;
            chgl.Refresh = true;
            break;
          }
        }
      }
      if (des == null) {
        newAlarm("TransportStart:  keine Order an einer Entschwefelungssation gefunden");
      }
    }

    public void charging(int on) {
      Station conv = null;
      for (int st = 0; st < stations.Length; st++) {
        Station station = stations[st];
        if (station is Converter) {
          if (on == States.ON && station.State == States.FREE) {
            int distance = calculateDistance(station.Center, chgl.Center);
            if (distance < 100 && chgl.Center.X <= station.Center.X && chgl.Center.Y <= station.Center.Y) {
              conv = station;
              conv.State = States.CHARGING;
              chgl.State = States.CHARGING;
              conv.Order = chgl.Order;
              conv.Refresh = true;
              chgl.Refresh = true;
              chgl.Move = true;
              break;
            }
          } else if (on == States.OFF && station.State == States.CHARGING) {
            Order o = chgl.Order;
            chgl.Order = null;
            chgl.State = States.FREE;
            conv = station;
            conv.Order = o;
            conv.State = States.READY;
            conv.Refresh = true;
            chgl.Refresh = true;
            chgl.Move = true;
            break;
          }
        }
      }
      if (conv == null) {
        if (on == States.ON) {
          newAlarm("ChargingStart:  kein Converter in der Nähe gefunden");
        } else {
          newAlarm("ChargingEnd:  kein Converter in Status CHARGING gefunden");
        }
      }
    }

    public void converterTreatment(int convNo, int on) {
      int stNo = convNo - 1;
      if (stationIsValid(stNo)) {
        Station station = stations[stNo];
        if (station is Converter) {
          if (on == States.ON && station.State == States.READY) {
            station.State = States.BUSY;
            createConverterProcessThread((Converter)station);
          } else if (on == States.OFF && station.State == States.BUSY) {
            stopConverterProcessThread();
            station.State = States.FINISHED;
          } else {
            newAlarm("ConverterTreatment: Converter " + convNo + " Status:" + station.State + "  nicht kompatibel");
          }
          station.Refresh = true;
        } else {
          newAlarm("ConverterTreatment:  Aggregatnummer " + convNo + " ist kein Converter");
        }
      } else {
        newAlarm("ConverterTreatment: falsche Aggregatnummer: " + convNo);
      }
    }

    public void tapping(int convNo, int on) {
      int stNo = convNo - 1;
      if (stationIsValid(stNo)) {
        Station station = stations[stNo];
        if (station is Converter) {
          if (on == States.ON && station.State == States.FINISHED) {
            station.State = States.TAPPING;
            createTappingProcessThread((Converter)station);
          } else if (on == States.OFF && station.State == States.TAPPING) {
            stopTappingProcessThread();
            station.State = States.FREE;
            chgl.State = States.FREE;
            station.Order = null;
          } else {
            newAlarm("Tapping: Converter " + convNo + " Status:" + station.State + "  nicht kompatibel");
          }
          station.Refresh = true;
          chgl.Move = true;
        } else {
          newAlarm("Tapping:  Aggregatnummer " + convNo + " ist kein Converter");
        }
      } else {
        newAlarm("Tapping: falsche Aggregatnummer: " + convNo);
      }
    }

    void createConverterProcessThread(Converter convStation) {
      ConverterProcessThread converterProcess = new ConverterProcessThread(convStation);
      converterControl = new Thread(new ThreadStart(converterProcess.SimulateConverterProcess));
      converterControl.IsBackground = true;
      converterControl.Start();
    }

    void createTappingProcessThread(Converter convStation) {
      TappingProcessThread converterProcess = new TappingProcessThread(convStation);
      tappingControl = new Thread(new ThreadStart(converterProcess.SimulateTappingProcess));
      tappingControl.IsBackground = true;
      tappingControl.Start();
    }

    void createDesulphurizationProcessThread(Desulphurization desStation) {
      DesulphurizationProcessThread desulphurizationProcess = new DesulphurizationProcessThread(desStation);
      desulphurizationControl = new Thread(new ThreadStart(desulphurizationProcess.SimulateDesulphurizationProcess));
      desulphurizationControl.IsBackground = true;
      desulphurizationControl.Start();
    }

    void stopConverterProcessThread() {
      converterControl.Abort();
      converterControl.Join();
    }

    void stopTappingProcessThread() {
      tappingControl.Abort();
      tappingControl.Join();
    }

    void stopDesulphurizationProcessThread() {
      desulphurizationControl.Abort();
      desulphurizationControl.Join();
    }

    public int calculateDistance(Point p1, Point p2) {
      double x = Math.Pow((p1.X - p2.X), 2);
      double y = Math.Pow((p1.Y - p2.Y), 2);
      int distance = (int)Math.Sqrt(x + y);
      return distance;
    }

    public void calcReladlingPoints(int stNo) {
      Station station = stations[stNo];
      Point p = new Point();
      p.X = station.Center.X + station.Reladling.X;
      p.Y = station.Center.Y + station.Reladling.Y;
      station.Reladling = p;
    }

    public void requestReladlingPositions() {
      Point[] positions = new Point[5];
      int i = 0;
      int p = 0;
      while (i<stations.Length && p<positions.Length) {
        if (!(stations[i] is Ladle)) {
          positions[p] = stations[i].Reladling;
          p++;
        }
        i++;
      }
      sender.SendReladlingPositions(positions);
    }

    public void requestLadlePosition(int LadleNumber)
    {
       Point position=new Point(-1,-1);
       foreach(Station s in stations)
        {
    
     //  Station s = getStation(LadleNumber);
           if ((!(s is Ladle) || s.Number != LadleNumber)) continue;
           position = s.Position;
        }
      sender.SendLadlePosition(position);
      if (position.Equals(new Point(-1,-1)))newAlarm("requestLadlePosition: Falsche LadleNo: "+LadleNumber);
    }

    public Boolean stationIsValid(int stNo) {
      if (stNo >= 0 && stNo < stations.Length) {
        return true;
      } else {
        return false;
      }
    }
  }
}
