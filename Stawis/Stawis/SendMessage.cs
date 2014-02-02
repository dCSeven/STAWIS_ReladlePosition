using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Drawing;

namespace Stawis {
    public class SendMessage {
        private static int port = 8000;    
        private static IPEndPoint remoteEndpoint;
        private static UdpClient client;

        public SendMessage() {
            string IP = "127.0.0.1";
            Console.WriteLine("MSRT: Verbinden mit " + IP);
            remoteEndpoint = new IPEndPoint(IPAddress.Parse(IP), port);
            client = new UdpClient();
        }

        public void SendReladlingPositions(Point[] positions) {
          string pos = "";
          for (int i = 0; i < positions.Length; i++) {
            Point p = positions[i];
            pos += p.X + " ";
            pos += p.Y + " ";
          }
          string msg = String.Format("{0:d2} {1}", 41, pos);
          Console.WriteLine("SendDim: " + msg);
          byte[] data = Encoding.UTF8.GetBytes(msg);
          try {
              client.Send(data, data.Length, remoteEndpoint);
          } catch (Exception e) { Console.WriteLine(e.Message); }
       }

        public void SendLadlePosition(Point position)
        {
          string pos = "";
          pos += position.X + " ";
          pos += position.Y;
          string msg = String.Format("{0:d2} {1}", 51, pos);
          Console.WriteLine("SendDim: " + msg);
          byte[] data = Encoding.UTF8.GetBytes(msg);
          try {
              client.Send(data, data.Length, remoteEndpoint);
          } catch (Exception e) { Console.WriteLine(e.Message); }
       }
        
    }
}
