using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace MSRT {
  public class Program {
    private static int port = 8001;
    private static IPEndPoint remoteEndpoint;
    private static UdpClient client;
    private static int delay = 0;
    static Dimension[] reladlingPoints;
    private static ReadMessage readMessage;

    static void Main(string[] args) {
      Console.WriteLine("Verbinden mit 127.0.0.1");
      string IP = "127.0.0.1";

      remoteEndpoint = new IPEndPoint(IPAddress.Parse(IP), port);
      client = new UdpClient();
      readMessage = new ReadMessage(client);

      In.Open("SimForStawis.txt");
      if (!In.Done) {
        Console.WriteLine("Fehler bei Open msrtScript");
        return;
      }

      reladlingPoints = readMessage.RequestReladlingPoints();

      String text = In.ReadLine();
      while (In.Done) {
        Boolean stopScript = false;
        if (!text.Equals("") && !text.Substring(0,2).Equals("//")) {
          In.OpenString(text);
          String cmd = In.ReadWord();
          Console.WriteLine("cmd: " + cmd);
 
          switch (cmd) {
            case "delayinloop": DoDelayInLoop(); break;
            case "delayonestep": DoDelayOneStep(); break;
            case "setladle": DoSetLadlePosition(); break;
            case "move": DoMove(); break;
            case "order": DoOrder(); break;
            case "desulphready": DoDesulphready(); break;
            case "desulphstart": DoDesulphStart(); break;
            case "desulphend": DoDesulphEnd(); break;
            case "available": DoAvailable(); break;
            case "transport": DoTransport(); break;
            case "charging": DoCharging(); break;
            case "converterstart": DoConverterStart(); break;
            case "converterend": DoConverterEnd(); break;
            case "tappingstart": DoTappingStart(); break;
            case "movetodesulphreladling": DoMoveToDesulphReladling(); break;
            case "movetoconverterreladling": DoMoveToConverterReladling(); break;
            case "tappingend": DoTappingEnd(); break;
            case "break": stopScript = true; break;
            default: break;// keine Aktion;
          }
          In.Close();
        }
        if (stopScript) {
          break;
        }
        Thread.Sleep(delay);
        text = In.ReadLine();
      }
    }
    static void DoTappingStart() {
      int station = In.ReadInt();
      string msg = String.Format("{0:d2} {1:d1} {2:d1}", 17, station, 1);
      Send(msg);
    }
    static void DoTappingEnd() {
      int station = In.ReadInt();
      string msg = String.Format("{0:d2} {1:d1} {2:d1}", 17, station, 0);
      Send(msg);
    }
    static void DoConverterStart() {
      int station = In.ReadInt();
      string msg = String.Format("{0:d2} {1:d1} {2:d1}", 16, station, 1);
      Send(msg);
    }
    static void DoConverterEnd() {
      int station = In.ReadInt();
      string msg = String.Format("{0:d2} {1:d1} {2:d1}", 16, station, 0);
      Send(msg);
    }
    static void DoCharging() {
      string flag = In.ReadWord();
      int val = (flag == "on" ? 1 : 0);
      string msg = String.Format("{0:d2} {1:d1}", 15, val);
      Send(msg);
    }
    static void DoTransport() {
      string msg = String.Format("{0:d2}", 13);
      Send(msg);
    }
    static void DoAvailable() {
      int station = In.ReadInt();
      string flag = In.ReadWord();
      int val = (flag == "yes" ? 1 : 0);
      string msg = String.Format("{0:d2} {1:d1} {2:d1}", 2, station, val);
      Send(msg);
    }
    static void DoDesulphStart() {
      int station = In.ReadInt();
      string msg = String.Format("{0:d2} {1:d1} {2:d1}", 12, station, 1);
      Send(msg);
    }
    static void DoDesulphEnd() {
      int station = In.ReadInt();
      string msg = String.Format("{0:d2} {1:d1} {2:d1}", 12, station, 0);
      Send(msg);
    }
    static void DoDesulphready() {
      int station = In.ReadInt();
      string msg
          = String.Format("{0:d2} {1:d1}", 11, station);
      Send(msg);
    }
    static void DoOrder() {
      string orderId = In.ReadWord();
      long weight = In.ReadLong();
      string msg
          = String.Format("{0:d2} {1} {2:d6}", 1, orderId, weight);
      Send(msg);
    }
    static void DoSetLadlePosition() {
      int ladleNo = In.ReadInt();
      int x = In.ReadInt();
      int y = In.ReadInt();
      string msg
          = String.Format("{0:d2} {1:d2} {2:d4} {3:d4}", 18, ladleNo, x, y);
      Send(msg);
    }

    static void DoMove() {
      int ladleNo = In.ReadInt();
      int count = In.ReadInt();
      int dx = In.ReadInt();
      int dy = In.ReadInt();
      for (int i = 0; i < count; i++) {
         string msg
        = String.Format("{0:d2} {1:d2} {2:d4} {3:d4}", 14, ladleNo, dx, dy);
        Send(msg);
        Thread.Sleep(delay);
      }
    }
    

    static void DoMoveToDesulphReladling() {
      int ladleNo = In.ReadInt();
      int desNo = In.ReadInt();
      int stationNo = desNo + 2;
      Vector destPoint = (Vector) reladlingPoints[stationNo];
      Vector startingPoint = (Vector) readMessage.RequestLadlePosition(ladleNo);
      Vector vec = destPoint - startingPoint;
      int count = (int) Math.Ceiling(vec.Len());
      string msg;
      int xpart, ypart;
      float x, y;
      x = y = ypart = xpart = 0;
      for (int i = 0; i < count - 1; i++)
      {
          x += vec.x / count;
          y += vec.y / count;
          xpart = (int)Math.Floor(x);
          ypart = (int)Math.Floor(y);

          msg = String.Format("{0:d2} {1:d2} {2:d4} {3:d4}", 14, ladleNo, xpart, ypart);
          Send(msg);
          x = x - xpart;
          y = y - ypart;
          Thread.Sleep(delay);
      }
      msg = String.Format("{0:d2} {1:d2} {2:d4} {3:d4}", 18, ladleNo, destPoint.x,destPoint.y);
      Send(msg);
    }

    static void DoMoveToConverterReladling() {
      int ladleNo = In.ReadInt();
      int convNo = In.ReadInt();
      int stationNo = convNo -1;
      Vector destPoint = (Vector) reladlingPoints[stationNo];
      Vector startingPoint = (Vector)readMessage.RequestLadlePosition(ladleNo);

      Vector vec = destPoint - startingPoint;
      int count = (int)Math.Ceiling(vec.Len());
      string msg;
      int xpart, ypart;
      float x, y;
      x = y = ypart = xpart = 0;
      for (int i = 0; i < count - 1; i++)
      {
          x += vec.x/count;
          y += vec.y / count;
          xpart = (int) Math.Floor(x);
          ypart = (int) Math.Floor(y);
          
          msg = String.Format("{0:d2} {1:d2} {2:d4} {3:d4}", 14, ladleNo, xpart ,ypart);
          Send(msg);
          x = x - xpart;
          y = y - ypart;
          Thread.Sleep(delay);
      }
      msg = String.Format("{0:d2} {1:d2} {2:d4} {3:d4}", 18, ladleNo, destPoint.x,destPoint.y);
      Send(msg);
    }

    static void DoDelayInLoop() {
      delay = In.ReadInt();
    }

    static void DoDelayOneStep() {
      int wait = In.ReadInt();
      Thread.Sleep(wait);
    }


    public static void Send(string msg) {
      byte[] data = Encoding.UTF8.GetBytes(msg);
      try {
        client.Send(data, data.Length, remoteEndpoint);
      } catch (Exception e) { }
    }
  }
}

