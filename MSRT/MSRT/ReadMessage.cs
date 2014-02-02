using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Drawing;

namespace MSRT {
    public class ReadMessage {
        int localPort = 8000;
        IPEndPoint anyIP;
        UdpClient receiver;

        public ReadMessage(UdpClient client) {
            try {
                receiver = new UdpClient(localPort);
                anyIP = new IPEndPoint(IPAddress.Any, 0);
            } catch(Exception e1) {
                Console.WriteLine("Fehler bei Erzeugen ReadMessage: "+e1.Message);
            }
        }

        public Dimension[] RequestReladlingPoints() {
          Dimension[] reladlingPoints = new Dimension[5];
            string msg = String.Format("{0:d2}", 40);
            Program.Send(msg);

            try {
                byte[] data = receiver.Receive(ref anyIP);
                string text = Encoding.UTF8.GetString(data);
                Console.WriteLine("nach GetString: " + text);
                In.OpenString(text);
                int msgNo = In.ReadInt();
                if (msgNo == 41) {
                  for (int i = 1; i < 6; i++) {
                    Dimension d = new Dimension();
                    d.Width = In.ReadInt();
                    d.Height = In.ReadInt();
                    reladlingPoints[i-1] = d;
                  }
                } else {
                    Console.WriteLine(msgNo + " ungültig;  41 erwartet");
                }
                In.Close();
            } catch (Exception e1) {
                Console.WriteLine("Ausnahme: " + e1.Message);
            }
            return reladlingPoints;
        }
    }
}
