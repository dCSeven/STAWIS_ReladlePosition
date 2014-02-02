using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;

namespace Stawis {
  class ReceiveDataThread {
    Model model;
    int localPort = 8001;


    public ReceiveDataThread(Model model) {
      this.model = model;
    }

    public void ReceiveData() {
      UdpClient client = new UdpClient(localPort);
      IPEndPoint remoteEndPoint = new IPEndPoint(IPAddress.Any, 0);
  
      while ((Thread.CurrentThread.ThreadState
                & ThreadState.Running) == ThreadState.Running) {
        try {
           // Bytes empfangen.
           IPEndPoint anyIP = new IPEndPoint(IPAddress.Any, 0);
           byte[] data = client.Receive(ref anyIP);
               // Bytes mit der UTF8-Kodierung in das Textformat kodieren.
               string text = Encoding.UTF8.GetString(data);
               // Den abgerufenen Text anzeigen.
 //              model.newAlarm(text);
              In.OpenString(text);
              int msgNo = In.ReadInt();
              switch (msgNo) {
                case 1: newOrder(); break;
                case 2: converterAvailable(); break;
                case 11: desulphArrival(); break;
                case 12: desulphurization(); break;
                case 13: transportStart(); break;
                case 14: ladleMove(); break;
                case 15: charging(); break;
                case 16: converterTreatment(); break;
                case 17: tapping(); break;
                case 18: setLadlePosition(); break;
                case 40: model.requestReladlingPositions(); break;
                case 50: model.requestLadlePosition(In.ReadInt()); break;
                default:
                  break;
              }

        } catch (Exception err) {
           Console.WriteLine(err.ToString());
        }
      }
    }

    void newOrder() {
      string orderId = In.ReadWord();
      long weight = In.ReadLong();
      model.orderTakeOver(orderId, weight);
    }

    void converterAvailable() {
      int convNo = In.ReadInt();
      int on = In.ReadInt();
      model.converterAvailable(convNo, on);
    }

    void desulphArrival() {
      int desNo = In.ReadInt();
      model.takeoverAtDesulphurization(desNo);
    }

    void desulphurization() {
      int desNo = In.ReadInt();
      int on = In.ReadInt();
      model.desulphurizationOnOff(desNo,on);
    }

    void transportStart() {
      model.transportStart();
    }

    void setLadlePosition() {
      int ladleNo = In.ReadInt();
      int x = In.ReadInt();
      int y = In.ReadInt();
      model.setLadlePosition(ladleNo, x, y);
    }

    void ladleMove() {
      int ladleNo = In.ReadInt();
      int dx = In.ReadInt();
      int dy = In.ReadInt();
      model.ladleMove(ladleNo, dx, dy);
    }

    void charging() {
      int on = In.ReadInt();
      model.charging(on);
    }

    void converterTreatment() {
      int convNo = In.ReadInt();
      int on = In.ReadInt();
      model.converterTreatment(convNo, on);
    }

    void tapping() {
      int convNo = In.ReadInt();
      int on = In.ReadInt();
      model.tapping(convNo, on);
    }

  }
}
