using System;
using System.Net;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Data.SqlClient;

namespace ConsoleAppClientWebSocket
{
    class Program
    {
        static ClientWebSocket webSocketClient;
        static void Main(string[] args)
        {
            string connectionString = @"Data Source=DESKTOP-GK894KI\SQLEXPRESS;Initial Catalog=CatalogDataBase;Integrated Security=True;";












            //while(true)
            //{
            //    Console.WriteLine("Введите действие (1, 2, 3)");
            //    string m = Console.ReadLine();
            //    //Console.WriteLine(int.TryParse(m, out number).ToString());
            //    //if (int.TryParse(m, out number))
            //    if(webSocketClient == null)
            //    {
            //        Console.WriteLine("Соединение с сервером открыто.");
            //        ClientLaunchAsync(m);
            //    }
            //    else if(webSocketClient.State == WebSocketState.Open)
            //    {
            //        ClientSendAsync(m);
            //    }
                
            //}
        }
        //private static async void ClientLaunchAsync(string text)
        //{
        //    webSocketClient = new ClientWebSocket();
        //    await webSocketClient.ConnectAsync(new Uri("ws://localhost:5000"), CancellationToken.None);
        //    ClientSendAsync(text);
        //}
        //private static async void ClientSendAsync(string m)
        //{
        //    var arraySegment = new ArraySegment<byte>(Encoding.UTF8.GetBytes(m));
        //    await webSocketClient.SendAsync(arraySegment, WebSocketMessageType.Text, true, CancellationToken.None);
        //    var buffer = new byte[1000];
        //    var segment = new ArraySegment<byte>(buffer);
        //    await webSocketClient.ReceiveAsync(segment, CancellationToken.None);
        //    Console.WriteLine(Encoding.UTF8.GetString(segment.Array).TrimEnd(' ', '\0'));
        //}
    }
}
