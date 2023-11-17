using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Timers;
using System.Web;

namespace DIALScanner
{

    class Program
    {
        static void Main(string[] args)
        {
            Console.SetWindowSize(230, 40);
            UPnPDevDiscovery disc = new UPnPDevDiscovery();
            disc.FindDevices();
            Console.ReadLine();
        }
    }

    public class UPnPDevDiscovery
    {
        //https://stackoverflow.com/questions/37671144/ssdp-discovery-of-upnp-devices-using-multicast-sockets
        private const string searchRequest = @"M-SEARCH * HTTP/1.1
HOST: {0}:{1}
MAN: ""ssdp:discover""
MX: {2}
ST: {3}

";
        private const string MulticastIP = "239.255.255.250";
        private const int multicastPort = 1900;
        private const int multicastTTL = 1;
        private const int MaxResultSize = 8096;
        private const string DefaultDeviceType = "urn:dial-multiscreen-org:service:dial:1";
        private int searchTimeOut = 2; //Seconds
        private Socket socket;
        private SocketAsyncEventArgs sendEvent;
        private List<string> Locations = new List<string>();
        private List<string> ApplicationURLs = new List<string>();
        public void FindDevices()
        {
            string request = string.Format(searchRequest, MulticastIP, multicastPort, this.searchTimeOut, DefaultDeviceType);
            Console.WriteLine("Sending: \n" + request);
            byte[] multiCastData = Encoding.UTF8.GetBytes(request);
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            socket.SendBufferSize = multiCastData.Length;
            sendEvent = new SocketAsyncEventArgs();
            sendEvent.RemoteEndPoint = new IPEndPoint(IPAddress.Parse(MulticastIP), multicastPort);
            sendEvent.SetBuffer(multiCastData, 0, multiCastData.Length);
            sendEvent.Completed += OnSocketSendEventCompleted;

            Timer t = new Timer(TimeSpan.FromSeconds(this.searchTimeOut + 1).TotalMilliseconds);
            t.Elapsed += (e, s) =>
            {
                try
                {
                    socket.Dispose();
                    //socket = null;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    t.Stop();
                }
            };

            // Kick off the initial Send
            socket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.MulticastInterface, IPAddress.Parse(MulticastIP).GetAddressBytes());
            socket.SendToAsync(sendEvent);
            t.Start();
        }

        private void AnalyzeResult(string result)
        {
            using (var reader = new System.IO.StringReader(result))
            {

                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    if (line.StartsWith("Location:") | line.StartsWith("LOCATION:"))
                    {
                        string loc = line.Split(' ')[1];
                        Console.WriteLine("Found location  : " + loc);
                        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(loc);
                        request.Method = "GET";
                        request.ServicePoint.Expect100Continue = false;
                        WebResponse response = request.GetResponse();
                        StreamReader readStream = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                        Console.BackgroundColor = ConsoleColor.Blue;
                        Console.WriteLine("Found DIAL location  : " + response.Headers["Application-URL"]);
                        //use HTTP, HTTPS will create a problem for CORS
                        Console.WriteLine("Tester URL  :\r\nhttp://tester.dialstranger.com/dialtester-single.html?uri=" +  response.Headers["Application-URL"]);
                        Console.ResetColor();
                        Console.WriteLine("Device Details:");
                        Console.WriteLine(readStream.ReadToEnd().Replace("\r\n", " "));
                    }

                }
            }
        }

        private void OnSocketSendEventCompleted(object sender, SocketAsyncEventArgs e)
        {
            if (e.SocketError != SocketError.Success)
            {
                Console.WriteLine("SocketError: " + e.SocketError);
                return;
            }

            switch (e.LastOperation)
            {
                case SocketAsyncOperation.SendTo:
                    Console.WriteLine("M-SEARCH sending completed");
                    // When the initial multicast is done, get ready to receive responses
                    e.RemoteEndPoint = new IPEndPoint(IPAddress.Any, 0);
                    byte[] receiveBuffer = new byte[MaxResultSize];
                    socket.ReceiveBufferSize = receiveBuffer.Length;
                    e.SetBuffer(receiveBuffer, 0, MaxResultSize);
                    Console.WriteLine("Waiting for response");
                    socket.ReceiveFromAsync(e);
                    break;
                case SocketAsyncOperation.ReceiveFrom:
                    Console.WriteLine("Received:");
                    // Got a response, so decode it
                    string result = Encoding.UTF8.GetString(e.Buffer, 0, e.BytesTransferred);
                    if (result.StartsWith("HTTP/1.1 200 OK", StringComparison.InvariantCultureIgnoreCase))
                    {
                        Console.WriteLine(result);
                        AnalyzeResult(result);
                    }
                    else
                        Console.WriteLine("INVALID SEARCH RESPONSE\n" + result);

                    if (socket != null)// and kick off another read
                        socket.ReceiveFromAsync(e);
                    break;
                default:
                    Console.WriteLine("***" + e.LastOperation);
                    break;
            }
        }
    }
}
