using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows;

namespace clientHW
{
    public partial class MainWindow : Window
    {
        public Socket client;
        public string host;
        public int port;
        public string portStr;
        public string username;
        public string message;
        public int connect;
        private byte[] result;
        public int receiveNumber;
        public string recStr;

        public MainWindow()
        {
            InitializeComponent();
        }
        private void connectBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                host = ipContent.Text;
                portStr = portContent.Text;
                port = int.Parse(portStr);
                username = userContent.Text;

                IPAddress ip = IPAddress.Parse(host);
                client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                client.Connect(new IPEndPoint(ip, port));

                Thread receiveThread = new Thread(ReceiveMessage);
                receiveThread.Start();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
                messageAll.Text += "Connect Fail\n";
            }
        }

        private void ReceiveMessage()
        {
            ClientData clientData = new ClientData(host, port, username, "", 1);
            string jsonString = System.Text.Json.JsonSerializer.Serialize(clientData);
            client.Send(Encoding.ASCII.GetBytes(jsonString));
            Console.WriteLine("Client第一次傳訊 "+jsonString);   //print

            while (true)
            {
                try
                {
                    result = new byte[1024];
                    receiveNumber = client.Receive(result);
                    recStr = Encoding.ASCII.GetString(result, 0, receiveNumber);

                    if (receiveNumber > 0)
                    {
                        Console.WriteLine("Client持續收訊 "+recStr);   //print
                        ClientData clientData2 = JsonConvert.DeserializeObject<ClientData>(recStr);
                        message = clientData2.clientMsg;
                        connect = clientData2.clientConn;
                   
                        messageAll.Dispatcher.BeginInvoke(new Action(() => { messageAll.Text += message; }), null);

                        if (connect == 0)
                        {
                            client.Shutdown(SocketShutdown.Both);
                            client.Close();
                            Console.WriteLine("client斷線");   //print
                            break;
                        }
                    }
                }
                catch (Exception ex)
                {
                    //exception close()
                    client.Shutdown(SocketShutdown.Both);
                    client.Close();
                    break;
                }
            }
        }

        private void sendBtn_Click(object sender, RoutedEventArgs e)
        {
            if(connect==1)
            {
                host = ipContent.Text;
                portStr = portContent.Text;
                port = int.Parse(portStr);
                username = userContent.Text;
                message = messageContent.Text;

                ClientData clientData = new ClientData(host, port, username, message, 1);
                string jsonString = System.Text.Json.JsonSerializer.Serialize(clientData);

                try
                {
                    Console.WriteLine("Client第二次傳訊 " + jsonString);   //print
                    //send message to server
                    client.Send(Encoding.ASCII.GetBytes(jsonString));
                    messageContent.Text = "";
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    messageAll.Text += "send Fail\n";
                }
            }
        }
        private void disconnectBtn_Click(object sender, RoutedEventArgs e)
        {
            ClientData clientData = new ClientData("",0,"",username+" is disconnected...\n",0);
            string jsonString = System.Text.Json.JsonSerializer.Serialize(clientData);
            client.Send(Encoding.ASCII.GetBytes(jsonString));
        }
    }
}
