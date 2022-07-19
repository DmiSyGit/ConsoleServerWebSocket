using System;
using System.Net;
using System.Net.WebSockets;
using System.Web;
using System.Text;
using System.Threading;
using System.Data.SqlClient;
using System.Data;
using System.Threading.Tasks;
using System.IO;
using System.Collections.Generic;

namespace ConsoleServerWebSocket
{
    class TimerWaiting
    {
        private WebSocket webSocket;
        private Timer timer;
        public WebSocket WebSocket { get => webSocket; set => webSocket = value; }
        public WebSocketContext SocketContext;
        public Timer Timer { get => timer; set => timer = value; }
        public TimerWaiting(WebSocket webSocket, WebSocketContext context)
        {
            SocketContext = context;
            WebSocket = webSocket;
        }
    }
    class Program
    {
        //static string connectionString = @"Data Source=DESKTOP-GK894KI\SQLEXPRESS;Initial Catalog=CatalogDataBase;Integrated Security=True;Pooling=false;";
        //static string connectionString = @"Data Source=ADMIN-ой\SQLEXPRESS;Initial Catalog=CatalogDataBase;Integrated Security=True;";
        static string connectionString = @"Data Source=WIN-R84DEUE96RB\SQLEXPRESS;Initial Catalog=CatalogDataBase;Integrated Security=True;Pooling=false;";
        static SqlConnection connection;
        static string nameTablePhoto = "PhotoProduct";
        static string address;
        static void Main(string[] args)
        {
            address = Console.ReadLine();
            ServerLaunchAsync();
            Console.ReadKey();
        }

        private static async void ServerLaunchAsync()
        {
            var httpListener = new HttpListener();
            httpListener.Prefixes.Add("http://"+address+":5000/");// для локальной сети 192.168.0.37
            //httpListener.Prefixes.Add("http://localhost:5000/");// только на компьютере
            httpListener.Start();
            Console.WriteLine("Сервер запущен успешно");
            while (true)
            {
                try
                {
                    HttpListenerContext listenerContext = await httpListener.GetContextAsync();
                    // Если обращение WebSocket
                    if (listenerContext.Request.IsWebSocketRequest)
                    {

                        WebSocketContext webSocketContext = await listenerContext.AcceptWebSocketAsync(null);
                        //WebSocket webSocket = webSocketContext.WebSocket;
                        // Сообщение в консоль о подключении
                        Console.WriteLine("(" + webSocketContext.SecWebSocketKey + ") Создано подключение");
                        WebSocketRequest(webSocketContext);


                        //await webSocket.CloseOutputAsync(WebSocketCloseStatus.Empty, null, CancellationToken.None);
                        //Console.WriteLine(" Question: "+ question);
                        // Console.WriteLine("Connection("+webSocketContext.SecWebSocketKey+"): ConnectionStatus: " + webSocket.State);

                    }
                    else
                    {
                        listenerContext.Response.StatusCode = 426;
                        listenerContext.Response.Close();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
        private static async Task WebSocketRequest(WebSocketContext context)
        {
            //await Task.Delay(10000);
            // Получаем сокет клиента из контекста запроса
            WebSocket webSocket = context.WebSocket;

            TimerCallback tm = new TimerCallback(TimerOutOfTime);

            
            // Слушаем его
            while (webSocket.State == WebSocketState.Open)
            {
                TimerWaiting timerWaiting = new TimerWaiting(webSocket, context);
                Timer timer = new Timer(tm, timerWaiting, 5000, 5000);
                timerWaiting.Timer = timer;
                // Создание буффера для принятия запроса состоящего из массива байт
                var buffer = new byte[100000];
                var segment = new ArraySegment<byte>(buffer);
                ArraySegment<byte> arraySegment;
                Console.WriteLine("(" + context.SecWebSocketKey + ") Ожидает запрос");

                // Асинхронно ожидаем данные от сокета
                try
                {
                    await webSocket.ReceiveAsync(segment, CancellationToken.None);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("(" + context.SecWebSocketKey + ") Ошибка при ожидании запроса: "+ex.Message);
                    break;
                }
                timer.Dispose();
                Console.WriteLine("(" + context.SecWebSocketKey + ") Получил запрос");

                string[] requestParameters = GetRequestParam(segment);
                //SqlConnection connectionBaseData = new SqlConnection(connectionString);
                SqlDataReader reader = null;
                try
                {
                    using (SqlConnection connectionBaseData = new SqlConnection(connectionString)) { 
                    await connectionBaseData.OpenAsync();
                    string sqlExpression;
                    string productDataString = "";
                    SqlCommand command;

                    // Формирование запроса к бд
                    switch (requestParameters[0])
                    {
                        case "GetPhoto":
                            if (requestParameters.Length == 2)
                            {
                                sqlExpression = "SELECT photo FROM " + nameTablePhoto + " WHERE idPhoto =" + requestParameters[1];
                                command = new SqlCommand(sqlExpression, connectionBaseData);

                                //Чтение и отправка данных
                                using (reader = await command.ExecuteReaderAsync())
                                {
                                    if (reader.HasRows)
                                    {
                                        while (await reader.ReadAsync())
                                        {
                                            byte[] fileByteArray = await reader.GetFieldValueAsync<byte[]>(0, CancellationToken.None);
                                            await webSocket.SendAsync(fileByteArray, WebSocketMessageType.Binary, true, CancellationToken.None);
                                        }
                                    }
                                    else
                                    {
                                        arraySegment = new ArraySegment<byte>(Encoding.UTF8.GetBytes("PhotoMissing"));
                                        await webSocket.SendAsync(arraySegment, WebSocketMessageType.Text, true, CancellationToken.None);
                                    }
                                }
                                await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "", CancellationToken.None);
                                //reader.Close();
                            }
                            break;
                        case "GetProducts":
                            string searchParam = "";
                            if (requestParameters.Length >= 5)
                            {
                                searchParam = string.Format("WHERE (nameProduct LIKE '%{0}%') " +
                                " OR(descriptionProduct LIKE '%{0}%') " +
                                " OR(sizes LIKE '%{0}%') " +
                                " OR(nameCategory LIKE '%{0}%') ", requestParameters[4]);
                            }
                            if (requestParameters[3] != "NoKey" && searchParam != "")
                            {
                                searchParam += " AND (KeyShop = '" + requestParameters[3] + "') ";
                            }
                            else if (requestParameters[3] != "NoKey" && searchParam == "")
                            {
                                searchParam += " WHERE (KeyShop = '" + requestParameters[3] + "') ";
                            }
                            if (requestParameters[2] == "NewProd")
                            {
                                sqlExpression = "SELECT idProduct, nameProduct, descriptionProduct, idPhoto, CAST(price AS VARCHAR) FROM ProductsSearchView " +
                                    searchParam +
                                            "ORDER BY idProduct DESC OFFSET " + requestParameters[1] + " ROWS FETCH NEXT 8 ROWS ONLY";
                            }
                            else if (requestParameters[2] == "Cheapest")
                            {
                                sqlExpression = "SELECT idProduct, nameProduct, descriptionProduct, idPhoto, CAST(price AS VARCHAR) FROM ProductsSearchView " +
                                    searchParam +
                                            "ORDER BY price OFFSET " + requestParameters[1] + " ROWS FETCH NEXT 8 ROWS ONLY";
                            }
                            else
                            {
                                sqlExpression = "SELECT idProduct, nameProduct, descriptionProduct, idPhoto, CAST(price AS VARCHAR) FROM ProductsSearchView " +
                                    searchParam +
                                            "ORDER BY price DESC OFFSET " + requestParameters[1] + " ROWS FETCH NEXT 8 ROWS ONLY";
                            }
                            command = new SqlCommand(sqlExpression, connectionBaseData);
                            //Чтение и отправка данных
                            using (reader = await command.ExecuteReaderAsync())
                            {
                                if (reader.HasRows)
                                {
                                    while (await reader.ReadAsync())
                                    {
                                        productDataString = "";
                                        productDataString += (reader.IsDBNull(0) ? "null" : reader[0].ToString()) + "%";//idProduct
                                        productDataString += (reader.IsDBNull(1) ? "Не указано" : reader[1].ToString()) + "%";//nameProduct
                                        productDataString += (reader.IsDBNull(2) ? "Не указано" : reader[2].ToString()) + "%";//descriptionProduct
                                        productDataString += (reader.IsDBNull(3) ? "null" : reader[3].ToString()) + "%";//idPhotoMain
                                        productDataString += (reader.IsDBNull(4) ? "Не указано" : reader[4].ToString()) + "%";//price
                                        arraySegment = new ArraySegment<byte>(Encoding.UTF8.GetBytes(productDataString));
                                        await webSocket.SendAsync(arraySegment, WebSocketMessageType.Text, false, CancellationToken.None);
                                    }
                                    arraySegment = new ArraySegment<byte>(Encoding.UTF8.GetBytes("EndMessage"));
                                    await webSocket.SendAsync(arraySegment, WebSocketMessageType.Text, true, CancellationToken.None);
                                }
                                else
                                {
                                    arraySegment = new ArraySegment<byte>(Encoding.UTF8.GetBytes("ErrorProducts"));
                                    await webSocket.SendAsync(arraySegment, WebSocketMessageType.Text, true, CancellationToken.None);
                                }
                            }
                            await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "", CancellationToken.None);
                            //reader.Close();
                            break;
                        case "GetMoreInfoProduct":
                            string CanEdit = "LockEdit%";
                            if (requestParameters[2] != "NoKey")
                            {
                                string sqlExpressionCanEdit = "SELECT nameProduct " +
                                            "FROM ProductsSearchView " +
                                            "WHERE (idProduct = '" + requestParameters[1] + "') AND (KeyShop = '" + requestParameters[2] + "');";
                                SqlCommand commandCanEdit = new SqlCommand(sqlExpressionCanEdit, connectionBaseData);
                                reader = await commandCanEdit.ExecuteReaderAsync();
                                if (reader.HasRows)
                                {
                                    CanEdit = "AllowedEdit%";
                                }
                                reader.Close();
                            }
                            string sqlExpressionInfo = "SELECT nameProduct, " +
                                            "descriptionProduct, " +
                                            "sizes, " +
                                            "addressShop, " +
                                            "CAST(price AS VARCHAR), " +
                                            "feedback, " +
                                            "categoryId " +
                                            "FROM ProductsSearchView " +
                                            "WHERE idProduct = " + requestParameters[1];

                            string sqlExpressionPhoto = "SELECT idPhoto " +
                                            "FROM PhotoProduct " +
                                            "WHERE idProduct = " + requestParameters[1];
                            SqlCommand commandInfo = new SqlCommand(sqlExpressionInfo, connectionBaseData);
                            SqlCommand commandPhoto = new SqlCommand(sqlExpressionPhoto, connectionBaseData);
                            //Чтение и отправка данных
                            reader = await commandInfo.ExecuteReaderAsync();
                            productDataString = "";
                            if (reader.HasRows)
                            {
                                while (await reader.ReadAsync())
                                {
                                    productDataString += (reader.IsDBNull(0) ? "Не указано" : reader[0].ToString()) + "%";//nameProduct
                                    productDataString += (reader.IsDBNull(1) ? "Не указано" : reader[1].ToString()) + "%";//descriptionProduct
                                    productDataString += (reader.IsDBNull(2) ? "Не указано" : reader[2].ToString()) + "%";//sizes
                                    productDataString += (reader.IsDBNull(3) ? "Не указано" : reader[3].ToString()) + "%";//addressShop
                                    productDataString += (reader.IsDBNull(4) ? "Не указано" : reader[4].ToString()) + "%";//price
                                    productDataString += (reader.IsDBNull(5) ? "Не указано" : reader[5].ToString()) + "%";//feedback
                                    productDataString += (reader.IsDBNull(6) ? "Не указано" : reader[6].ToString()) + "%";//categoryId
                                    productDataString += CanEdit;//Разрешено ли редавктирование
                                }
                                reader.Close();
                                reader = await commandPhoto.ExecuteReaderAsync();
                                if (reader.HasRows)
                                {
                                    while (await reader.ReadAsync())
                                    {
                                        productDataString += (reader.IsDBNull(0) ? "Не указано" : reader[0].ToString()) + "%";//idPhoto
                                    }
                                    reader.Close();
                                }
                                arraySegment = new ArraySegment<byte>(Encoding.UTF8.GetBytes(productDataString));
                                await webSocket.SendAsync(arraySegment, WebSocketMessageType.Text, false, CancellationToken.None);

                                arraySegment = new ArraySegment<byte>(Encoding.UTF8.GetBytes("EndMessage"));
                                await webSocket.SendAsync(arraySegment, WebSocketMessageType.Text, true, CancellationToken.None);
                            }
                            else
                            {
                                arraySegment = new ArraySegment<byte>(Encoding.UTF8.GetBytes("ErrorProducts"));
                                await webSocket.SendAsync(arraySegment, WebSocketMessageType.Text, true, CancellationToken.None);
                            }
                            await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "", CancellationToken.None);
                            break;
                        case "AddUpdateProduct":
                            string sqlExpressionCanInsert = "SELECT nameShop " +
                                            "FROM Shop " +
                                            $"WHERE (passwordKeyShop = '{requestParameters[2]}');";
                            SqlCommand commandCanInsert = new SqlCommand(sqlExpressionCanInsert, connectionBaseData);
                            reader = await commandCanInsert.ExecuteReaderAsync();
                            if (reader.HasRows)
                            {
                                await reader.CloseAsync();
                                try
                                {
                                    var bufferImage = new byte[5242880];
                                    var segmentImage = new ArraySegment<byte>(bufferImage);
                                    List<byte> allBytes = new List<byte>();
                                    int countImagesDownloaded = 0;
                                    SqlCommand commandAddUpdate;
                                    string sqlExpressionAddUpdate;
                                    string idProduct = "";
                                    if (requestParameters[1] == "0")
                                    {
                                        sqlExpressionAddUpdate = "INSERT INTO Product (KeyShop, nameProduct, descriptionProduct, sizes, categoryId, price) " +
                                                                "VALUES(@KeyShop ,@nameProduct, @descriptionProduct, @sizes, @categoryId, @price);" +
                                                                " SELECT SCOPE_IDENTITY();";
                                        commandAddUpdate = new SqlCommand(sqlExpressionAddUpdate, connectionBaseData);
                                        commandAddUpdate.Parameters.Add("@KeyShop", SqlDbType.NVarChar).Value = requestParameters[2];//Ключ магазина
                                        commandAddUpdate.Parameters.Add("@nameProduct", SqlDbType.NVarChar).Value = requestParameters[4];//Название
                                        commandAddUpdate.Parameters.Add("@descriptionProduct", SqlDbType.NVarChar).Value = requestParameters[5];//Описание
                                        commandAddUpdate.Parameters.Add("@sizes", SqlDbType.NVarChar).Value = requestParameters[6];//Размеры
                                        commandAddUpdate.Parameters.Add("@categoryId", SqlDbType.Int).Value = requestParameters[7];//Категория
                                        commandAddUpdate.Parameters.Add("@price", SqlDbType.Money).Value = requestParameters[8];//Цена
                                        idProduct = commandAddUpdate.ExecuteScalar().ToString();
                                    }
                                    else if (requestParameters[1] == "1")
                                    {
                                        sqlExpressionAddUpdate = $"UPDATE Product SET nameProduct='{requestParameters[4]}', " +
                                            $" descriptionProduct='{requestParameters[5]}', " +
                                            $" sizes='{requestParameters[6]}', " +
                                            $" categoryId='{requestParameters[7]}', " +
                                            $" price=@price " +
                                            $" WHERE KeyShop='{requestParameters[2]}' AND idProduct='{requestParameters[3]}'";
                                        commandAddUpdate = new SqlCommand(sqlExpressionAddUpdate, connectionBaseData);
                                        commandAddUpdate.Parameters.Add("@price", SqlDbType.Money).Value = requestParameters[8];//Цена
                                        await commandAddUpdate.ExecuteNonQueryAsync();
                                        string sqlExpressionDeletePhoto = "DELETE FROM PhotoProduct " +
                                                                  $" WHERE idProduct = '{requestParameters[3]}';";
                                        SqlCommand commandDelete = new SqlCommand(sqlExpressionDeletePhoto, connectionBaseData);
                                        await commandDelete.ExecuteNonQueryAsync();
                                        idProduct = requestParameters[3];
                                    }
                                    while (countImagesDownloaded < Convert.ToInt32(requestParameters[9]))
                                    {
                                        allBytes.Clear();
                                        while (true)
                                        {
                                            WebSocketReceiveResult response = await webSocket.ReceiveAsync(segmentImage, CancellationToken.None);
                                            for (int i = 0; i < response.Count; i++)
                                            {
                                                allBytes.Add(segmentImage.Array[i]);
                                            }
                                            if (response.EndOfMessage)
                                            {
                                                break;
                                            }
                                        }
                                        //File.WriteAllBytes("C:\\Users\\ADMIN\\Desktop\\МАЗ\\photo.jpg", allBytes.ToArray());
                                        sqlExpressionAddUpdate = "INSERT INTO PhotoProduct (photo, idProduct, isMainPhoto) VALUES(@photoByte, @idProd, @isMainPhoto); SELECT SCOPE_IDENTITY();";
                                        commandAddUpdate = new SqlCommand(sqlExpressionAddUpdate, connectionBaseData);
                                        commandAddUpdate.Parameters.Add("@photoByte", SqlDbType.VarBinary).Value = allBytes.ToArray();
                                        commandAddUpdate.Parameters.Add("@idProd", SqlDbType.Int).Value = idProduct;
                                        if (countImagesDownloaded == 0)
                                        {
                                            commandAddUpdate.Parameters.Add("@isMainPhoto", SqlDbType.Bit).Value = 1;
                                        }
                                        else
                                        {
                                            commandAddUpdate.Parameters.Add("@isMainPhoto", SqlDbType.Bit).Value = 0;
                                        }
                                        await commandAddUpdate.ExecuteScalarAsync();
                                        countImagesDownloaded++;
                                    }
                                    arraySegment = new ArraySegment<byte>(Encoding.UTF8.GetBytes("ProductAdditionCompleted"));
                                    await webSocket.SendAsync(arraySegment, WebSocketMessageType.Text, true, CancellationToken.None);
                                }
                                catch (Exception msg)
                                {
                                    string message = msg.Message;
                                    arraySegment = new ArraySegment<byte>(Encoding.UTF8.GetBytes("ProductAdditionError"));
                                    //Ошибка при загрузке фото
                                }
                            }

                            await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "", CancellationToken.None);
                            break;
                        case "LogIn":
                            if (requestParameters.Length == 2)
                            {
                                sqlExpression = "SELECT nameShop FROM Shop WHERE passwordKeyShop = '" + requestParameters[1] + "'";
                                command = new SqlCommand(sqlExpression, connectionBaseData);

                                //Чтение и отправка данных
                                reader = await command.ExecuteReaderAsync();
                                if (reader.HasRows)
                                {
                                    arraySegment = new ArraySegment<byte>(Encoding.UTF8.GetBytes("Correctly"));
                                    await webSocket.SendAsync(arraySegment, WebSocketMessageType.Text, true, CancellationToken.None);
                                }
                                else
                                {
                                    arraySegment = new ArraySegment<byte>(Encoding.UTF8.GetBytes("Incorrectly"));
                                    await webSocket.SendAsync(arraySegment, WebSocketMessageType.Text, true, CancellationToken.None);
                                }
                                reader.Close();
                            }
                            await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "", CancellationToken.None);
                            break;
                        case "DeleteProduct":
                            string sqlExpressionCanDelete = "SELECT nameProduct " +
                                            "FROM ProductsSearchView " +
                                            "WHERE (idProduct = '" + requestParameters[1] + "') AND (KeyShop = '" + requestParameters[2] + "');";
                            SqlCommand commandCanDelete = new SqlCommand(sqlExpressionCanDelete, connectionBaseData);
                            reader = await commandCanDelete.ExecuteReaderAsync();
                            if (reader.HasRows)
                            {
                                reader.Close();
                                string sqlExpressionDeletePhoto = "DELETE FROM PhotoProduct " +
                                                                  " WHERE idProduct = '" + requestParameters[1] + "';";
                                SqlCommand commandDelete = new SqlCommand(sqlExpressionDeletePhoto, connectionBaseData);
                                await commandDelete.ExecuteNonQueryAsync();

                                string sqlExpressionDeleteProduct = "DELETE FROM Product " +
                                                                  " WHERE idProduct = '" + requestParameters[1] + "';";
                                commandDelete = new SqlCommand(sqlExpressionDeleteProduct, connectionBaseData);
                                await commandDelete.ExecuteNonQueryAsync();

                                arraySegment = new ArraySegment<byte>(Encoding.UTF8.GetBytes("ProductRemoved"));
                                await webSocket.SendAsync(arraySegment, WebSocketMessageType.Text, true, CancellationToken.None);
                            }
                            else
                            {
                                arraySegment = new ArraySegment<byte>(Encoding.UTF8.GetBytes("FailedRemoved"));
                                await webSocket.SendAsync(arraySegment, WebSocketMessageType.Text, true, CancellationToken.None);
                            }
                            await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "", CancellationToken.None);
                            break;
                    }
                }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("(" + context.SecWebSocketKey + ") Ошибка: " + ex.Message);
                    arraySegment = new ArraySegment<byte>(Encoding.UTF8.GetBytes("Error"));
                    await webSocket.SendAsync(arraySegment, WebSocketMessageType.Text, true, CancellationToken.None);
                }
                finally
                { 
                    //await connectionBaseData.CloseAsync();
                    //await connectionBaseData.DisposeAsync();
                    //Console.WriteLine($"({context.SecWebSocketKey})___________________________________{connectionBaseData.State}");
                }
            }
        }
        private static void TimerOutOfTime(object obj)
        {
            if (((TimerWaiting)obj).WebSocket.State == WebSocketState.Open)
            {
                WebSocket webSocket = ((TimerWaiting)obj).WebSocket;
                webSocket.CloseOutputAsync(WebSocketCloseStatus.Empty, null, CancellationToken.None);
                Console.WriteLine("(" + ((TimerWaiting)obj).SocketContext.SecWebSocketKey + ") Истекло время ожидания");
            }
            ((TimerWaiting)obj).Timer.Dispose();
        }

        private static string[] GetRequestParam(ArraySegment<byte> segment)
        {
            // Получение строки из принятых байт и удаление пустых символов в конце строки
            string request = Encoding.UTF8.GetString(segment.Array).TrimEnd(' ', '\0');

            // Разделение строки запроса на части
            return request.Split('%', StringSplitOptions.RemoveEmptyEntries);
        }
    }
}
