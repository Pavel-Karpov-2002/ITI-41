using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace UDPConnect
{
    public class Connect
    {
        private IPAddress remoteAddress; // хост для отправки данных
        private const int remotePort = 8032; // порт для отправки данных
        private const int localPort = 8032; // порт для прослушивания входящих подключений

        public UdpClient receiver;
        public static Connect instance = null;
        public RequestProcess RequestProcessing { get; set; }

        public ConnectType ConnectType { get; private set; }

        public Connect(string ip, ConnectType type)
        {
            remoteAddress = IPAddress.Parse(ip);
            ConnectType = type;
            receiver = new UdpClient(localPort);
            receiver.JoinMulticastGroup(remoteAddress); // Доступные подкючения от 224.0.0.0 до 239.255.255.255
            instance = this;
        }

        public void SendMessage(Message message)
        {
            IPEndPoint endPoint = new IPEndPoint(remoteAddress, remotePort);
            try
            {
                byte[] data = Encoding.Unicode.GetBytes(JsonConvert.SerializeObject(message));
                receiver.Send(data, data.Length, endPoint);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public void ReceiveMessage()
        {
            IPEndPoint remoteIp = null;

            while (true)
            {
                try
                {
                    byte[] data = receiver.Receive(ref remoteIp);
                    string message = Encoding.Unicode.GetString(data);
                    var receive = JsonConvert.DeserializeObject<Message>(message);
                    if ((receive.MessageType == MessageType.CountConnectedToServer || receive.MessageType == MessageType.PlayerSpentMines) && ConnectType == ConnectType.Server)
                        RequestProcessing.ConnectTriggersServer(receive);
                    else
                        RequestProcessing.ConnectTriggersClient(receive);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    break;
                }
            }
        }
    }

    public enum ConnectType
    {
        Server,
        Client
    }
}

