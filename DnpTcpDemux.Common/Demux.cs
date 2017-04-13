using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DnpTcpDemux
{
    public class Demux : IDisposable
    {
        public int PortNumber { get; set; }
        public Dictionary<int, TcpClient> EndPoints { get; private set; }

        public void AddEndPoint(int dnpAddress, TcpClient tcpClient)
        {
            EndPoints.Add(10, tcpClient);
        }

        private TcpListener _tcpListener;
        private TcpClient _mainClient;

        public Demux(int listenPort)
        {
            PortNumber = listenPort;
        }

        public void Start()
        {
            _tcpListener = new TcpListener(IPAddress.Any, PortNumber);
            Thread listenThread = new Thread(unused => ListenForSource(_tcpListener));
            listenThread.Start();
            ConnectEndPoints();
        }

        public void Stop()
        {

        }

        
        public void ConnectEndPoints()
        {
            foreach (var kvp in EndPoints)
            {
                Thread listenThread = new Thread(unused => HandleClientComms(kvp.Value));
                listenThread.Start();
            }
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        private void ReconnectSource(TcpListener listener)
        {
            
        }

        public void ListenForSource(TcpListener listener)
        {
            listener.Start();
            Console.WriteLine("Listening for client... " + listener.Server.LocalEndPoint.ToString());
            //blocks until a client has connected to the server
            _mainClient = listener.AcceptTcpClient();
            Console.WriteLine("Client connected, stopped listening on " + listener.LocalEndpoint.ToString());
            HandleServerComms(_mainClient);
        }


        public void HandleClientComms(TcpClient client)
        {
            
            NetworkStream clientStream = client.GetStream();

            byte[] message = new byte[4096];
            int bytesRead;

            while (true)
            {
                bytesRead = 0;

                try
                {
                    //blocks until a client sends a message
                    bytesRead = clientStream.Read(message, 0, 4096);
                }
                catch
                {
                    //client has disconnected or socket error
                    Console.WriteLine("Client disconnected or socket error on " + client.Client.LocalEndPoint.ToString());
                    break;
                }

                if (bytesRead == 0)
                {
                    //the client has disconnected from the server
                    Console.WriteLine("Client disconnected on " + client.Client.LocalEndPoint.ToString());
                    break;
                }

                    if (_mainClient.Connected)
                    {
                        NetworkStream stream = _mainClient.GetStream();
                        stream.Write(message, 0, bytesRead);
                        stream.Flush();
                    }
                
            }

            client.Close();
            //ReconnectClient(listener);
        }

        public void HandleServerComms(TcpClient client)
        {
            NetworkStream clientStream = client.GetStream();

            byte[] message = new byte[4096];
            int bytesRead;

            while (true)
            {
                bytesRead = 0;

                try
                {
                    //blocks until a client sends a message
                    bytesRead = clientStream.Read(message, 0, 4096);
                }
                catch
                {
                    //client has disconnected or socket error
                    Console.WriteLine("Client disconnected or socket error on " + client.Client.LocalEndPoint.ToString());
                    break;
                }

                if (bytesRead == 0)
                {
                    //the client has disconnected from the server
                    Console.WriteLine("Client disconnected on " + client.Client.LocalEndPoint.ToString());
                    break;
                }


                //TODO: check the dnp address and dispatch to the correct TcpListener
                if (_mainClient.Connected)
                {
                    if (bytesRead >= 10)
                    {
                        if (message[0] == 0x05 && message[1] == 0x64)
                        {
                            int dnpAddress = message[4] << 8 & message[5];
                            if (EndPoints.ContainsKey(dnpAddress))
                            {
                                NetworkStream stream = EndPoints[dnpAddress].GetStream();
                                stream.Write(message, 0, bytesRead);
                                stream.Flush();
                            }
                        }
                    }
                    
                }

            }

            client.Close();
            //ReconnectClient(listener);
        }
    }
}
