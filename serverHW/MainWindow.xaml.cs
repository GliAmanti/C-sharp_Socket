using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows;
using Newtonsoft.Json;

namespace serverHW
{
    public partial class MainWindow : Window
    {
        public Socket server;
        public string host;
        public string portStr;
        public int port;
        //public Socket connection;
        //int client_Num;
        List<Socket> sockets = new List<Socket>();
        public string username;
        public string message;
        public bool flag;


        public MainWindow()
        {
            InitializeComponent();
        }

        private void start_Click(object sender, RoutedEventArgs e)
        {
            host = ipContent.Text;
            portStr = portContent.Text;
            port = int.Parse(portStr);

            if (server == null)
            {
                messageAll.Text = "Server Start\n";
                IPAddress ip = IPAddress.Parse(host);
                server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                server.Bind(new IPEndPoint(ip, port));
                flag = true;
                server.Listen(10);

                Thread thread = new Thread(Listen);
                thread.Start();
            }
        }

        private void Listen()
        {
            while (flag)
            {
               try
                {
                    Socket client = server.Accept();
                    sockets.Add(client);
                    Thread receive = new Thread(ReceiveMsg);
                    Console.WriteLine("accept");   //print
                    receive.Start(client);
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
        }

        private void ReceiveMsg(object client)
        {
            bool first = true;
            Socket connection = (Socket)client;
            byte[] result = new byte[1024];
            int receive_num = connection.Receive(result);
            string receive_str = Encoding.ASCII.GetString(result, 0, receive_num);

            while (true)
            {
                try
                {
                    if (receive_num > 0 && first)
                    {
                        first = false;
                        Console.WriteLine("Server第一次收訊 " + receive_str);   //print
                        ClientData clientData = JsonConvert.DeserializeObject<ClientData>(receive_str);
                        username = clientData.clientName;

                        ClientData clientData2 = new ClientData("", 0, "", "Server: Welcome " + username + "\n", 1);
                        string jsonString = System.Text.Json.JsonSerializer.Serialize(clientData2);

                        messageAll.Dispatcher.BeginInvoke(new Action(() => { messageAll.Text += username + " connect\n"; }), null);

                        for (int i = 0; i < sockets.Count; i++)
                        {
                            if (sockets[i] != null)
                            {
                                sockets[i].Send(Encoding.ASCII.GetBytes(jsonString));
                                Console.WriteLine("送出" + jsonString);   //print
                            }
                        }
                    }
                    else if(receive_num > 0 && !first)
                    {
                        result = new byte[1024];
                        receive_num = connection.Receive(result);  //receive message from client
                        receive_str = Encoding.ASCII.GetString(result, 0, receive_num);
                        if (receive_num > 0)
                        {
                            Console.WriteLine("Server第二次收訊 " + receive_str);
                            ClientData clientData = JsonConvert.DeserializeObject<ClientData>(receive_str);
                            username = clientData.clientName;
                            message = clientData.clientMsg;
                            int connect = clientData.clientConn;

                            if (connect == 1)
                            {
                                ClientData clientData2 = new ClientData("", 0, "", username + " : " + message + "\n", 1);
                                string jsonString = System.Text.Json.JsonSerializer.Serialize(clientData2);
                                ClientData clientData3 = new ClientData("", 0, "", "You send : " + message + "\n", 1);
                                string jsonString2 = System.Text.Json.JsonSerializer.Serialize(clientData3);

                                for (int i = 0; i < sockets.Count; i++)   //resend message to client
                                {
                                    if (sockets[i] != null)
                                    {
                                        if (sockets[i].Equals(client))
                                        {
                                            sockets[i].Send(Encoding.ASCII.GetBytes(jsonString2));
                                            Console.WriteLine("送出" + jsonString2);   //print
                                        }
                                        else
                                        {
                                            sockets[i].Send(Encoding.ASCII.GetBytes(jsonString));
                                            Console.WriteLine("送出" + jsonString);   //print
                                        }
                                    }
                                }
                                messageAll.Dispatcher.BeginInvoke(new Action(() => { messageAll.Text += username + " : " + message + "\n"; }), null);
                            }
                            else
                            {
                                Console.WriteLine("收到client斷訊通知");   //print
                                ClientData clientData4 = new ClientData("", 0, "", message, 1);
                                string jsonString3 = System.Text.Json.JsonSerializer.Serialize(clientData4);
                                ClientData clientData5 = new ClientData("", 0, "", "You are disconnected...\n", 0);
                                string jsonString4 = System.Text.Json.JsonSerializer.Serialize(clientData5);

                                for (int i = 0; i < sockets.Count; i++)
                                {
                                    if (sockets[i] != null)
                                    {
                                        if (sockets[i].Equals(client))
                                        {
                                            sockets[i].Send(Encoding.ASCII.GetBytes(jsonString4));
                                            Console.WriteLine("送出" + jsonString4);   //print
                                            sockets[i] = null;
                                        }
                                        else
                                        {
                                            sockets[i].Send(Encoding.ASCII.GetBytes(jsonString3));
                                            Console.WriteLine("送出" + jsonString3);   //print
                                        }
                                    }
                                }
                                messageAll.Dispatcher.BeginInvoke(new Action(() => { messageAll.Text += message; }), null);
                                //sockets.Remove((Socket)client);
                            }
                        }
                    }
                }
                catch (Exception e)   //exception close()
                {
                    Console.WriteLine(e);
                    //connection.Shutdown(SocketShutdown.Both);
                    connection.Close();
                    break;
                }
            }
        }

        private void sendBtn_Click(object sender, RoutedEventArgs e)
        {
            if (flag)
            {
                message = messageContent.Text;
                ClientData clientData = new ClientData("", 0, "", "Server : " + message+"\n", 1);
                string jsonString = System.Text.Json.JsonSerializer.Serialize(clientData);

                try
                {
                    for (int i = 0; i < sockets.Count; i++)
                    {
                        if (sockets[i] != null)
                        {
                            sockets[i].Send(Encoding.ASCII.GetBytes(jsonString));
                            Console.WriteLine("送出" + jsonString);   //print
                        }
                    }
                    messageContent.Text = "";
                    messageAll.Dispatcher.BeginInvoke(new Action(() => { messageAll.Text += "You send : " + message + "\n"; }), null);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    messageAll.Text += "send Fail\n";
                }
            }
        }
        private void stopBtn_Click(object sender, RoutedEventArgs e)
        {
            if(flag)
            {
                flag = false;
                ClientData clientData = new ClientData("", 0, "", "Server is disconnected...\n", 0);
                string jsonString = System.Text.Json.JsonSerializer.Serialize(clientData);

                for (int i = 0; i < sockets.Count; i++)
                {
                    if (sockets[i] != null)
                    {
                        sockets[i].Send(Encoding.ASCII.GetBytes(jsonString));
                    }
                }
                messageAll.Dispatcher.BeginInvoke(new Action(() => { messageAll.Text += "Server is disconnected...\n"; }), null);
                //connection.Shutdown(SocketShutdown.Both);
                sockets.Clear();
                server.Close();
                server = null;
                Console.WriteLine("server斷線\n");
            }
        }
    }
}
