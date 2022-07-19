using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.IO;
using System.Net.WebSockets;
using System.Threading;

namespace photoWindowsFormsApp
{
    public partial class Form1 : Form
    {
        static string connectionString = @"Data Source=DESKTOP-GK894KI\SQLEXPRESS;Initial Catalog=CatalogDataBase;Integrated Security=True;";
        SqlConnection connection;
        static ImageList imgList;
        static ListView listView;
                                                                                                        //static ClientWebSocket webSocketClient;
        List<Task> tasks = new List<Task>();
        static CancellationTokenSource cts = new CancellationTokenSource();

        public Form1()
        {
            connection = new SqlConnection(connectionString);
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            imgList = imageList1;
            listView = listView1;
        }
        private static async Task ClientLaunchAsync(string msg)
        {
            ClientWebSocket webSocketClient = new ClientWebSocket();
            await webSocketClient.ConnectAsync(new Uri("ws://localhost:5000"), CancellationToken.None);


            var arraySegment = new ArraySegment<byte>(Encoding.UTF8.GetBytes(msg));
            await webSocketClient.SendAsync(arraySegment, WebSocketMessageType.Text, true, cts.Token);
            //await webSocketClient.CloseOutputAsync(WebSocketCloseStatus.Empty, null, CancellationToken.None);
            int count =0;

            while (webSocketClient.State == WebSocketState.Open)
            {
                byte[] buffer = new byte[5242880];
                var segment = new ArraySegment<byte>(buffer);

                WebSocketReceiveResult resp;
                if (msg == "getProducts")
                {
                    resp = await webSocketClient.ReceiveAsync(segment, CancellationToken.None);
                    string response = Encoding.UTF8.GetString(segment.Array).TrimEnd(' ', '\0');
                    string[] responseParameters = response.Split('%', StringSplitOptions.RemoveEmptyEntries);
                    listView.Items.Add(responseParameters[0], responseParameters[1]);
                    ClientLaunchAsync("getphoto%"+ responseParameters[1]);
                }
                else
                {

                    resp = await webSocketClient.ReceiveAsync(segment, cts.Token);
                    string response = Encoding.UTF8.GetString(segment.Array).TrimEnd(' ', '\0');
                    if (response != "" && response != "photoMissing")
                    {
                        string[] photoParameters = msg.Split('%', StringSplitOptions.RemoveEmptyEntries);
                        imgList.Images.Add(photoParameters[1], Image.FromStream(new MemoryStream(segment.Array)));
                        //listView.Items.Add("text").ImageIndex = imgList.Images.Count - 1;
                    }
                    else
                    {
                        listView.Items.Add("Error");
                    }

                }
                if (resp.EndOfMessage)
                {
                    break;
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            if (connection.State == ConnectionState.Closed)
            {
                try
                {
                    connection.Open();
                    button1.Text = "CloseCon";

                    string sqlExpression = "SELECT nameCategory FROM Categories";
                    SqlCommand command = new SqlCommand(sqlExpression, connection);

                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows) // если есть данные
                    {
                        while (reader.Read()) // построчно считываем данные
                        {
                            CategoryCb.Items.Add(reader[0].ToString());
                        }
                    }
                    reader.Close();
                }
                catch
                {
                    MessageBox.Show("Error");
                }
            }
            else
            {
                connection.Close();
                button1.Text = "OpenCon";
            }
            

        }

        private void button2_Click(object sender, EventArgs e)
        {
            imageList1.Images.Clear();
            listView1.Items.Clear();
            ClientLaunchAsync("getProducts");
            //cts.Cancel();


            /*
            string sqlExpression = "SELECT photo FROM photoTableTest";
            //string sqlExpression = "SELECT photo FROM photoTableTest WHERE idPhoto = @idPhoto";
            SqlCommand command = new SqlCommand(sqlExpression, connection);
            command.Parameters.Add("@idPhoto", SqlDbType.Int).Value = 1;
            //byte[] fileByteArray = (byte[])command.ExecuteScalar();
           // pictureBox1.Image = Image.FromStream(new MemoryStream(fileByteArray));

            SqlDataReader reader = command.ExecuteReader();
            if (reader.HasRows) // если есть данные
            {
                while (reader.Read()) // построчно считываем данные
                {
                    byte[] fileByteArray = (byte[])reader.GetValue(0);
                    imageList1.Images.Add(Image.FromStream(new MemoryStream(fileByteArray)));
                    listView1.Items.Add(imageList1.Images.Count.ToString()).ImageIndex = imageList1.Images.Count-1;
                }
            }
            reader.Close();


            //OleDbCommand dbCommand = dbConn.CreateCommand();
            //dbCommand.CommandText = "SELECT BINARY_FILE FROM TEST_BINARY_FILES WHERE ID_FILE = ?";

            //dbCommand.Parameters.Add("idFile", OleDbType.Integer).Value = dbFileID;

            //byte[] fileByteArray = (byte[])dbCommand.ExecuteScalar();
            */
        }

        private void AddBtn_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
            string path = openFileDialog1.FileName;
            //this.Text = openFileDialog1.FileName;
            //Image img = Image.FromFile(Text);
            //System.IO.MemoryStream ms = new System.IO.MemoryStream();
            //img.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);

            byte[] fileByteArray = File.ReadAllBytes(path);

            string query = "Insert into photoTableTest (photo) values(@photoByte); SELECT SCOPE_IDENTITY();";
            SqlCommand comm = new SqlCommand(query, connection);
            comm.Parameters.Add("@photoByte", SqlDbType.VarBinary).Value = fileByteArray;
            MessageBox.Show(comm.ExecuteScalar().ToString());
        }

        private void getProducts_Click(object sender, EventArgs e)
        {
            string query = "Insert into Product (nameProduct, descriptionProduct, sizes, addressMagazin, categoryId, price, idPhotoMain, idPhotoSecond, idPhotoThird) " +
                "values(@nameProduct, @descriptionProduct, @sizes, @addressMagazin, @categoryId, @price, @idPhotoMain, @idPhotoSecond, @idPhotoThird);" +
                " SELECT SCOPE_IDENTITY();";
            SqlCommand comm = new SqlCommand(query, connection);
            comm.Parameters.Add("@nameProduct", SqlDbType.NVarChar).Value = nameProductTb.Text;
            comm.Parameters.Add("@descriptionProduct", SqlDbType.NVarChar).Value = descriptionProductTb.Text;
            comm.Parameters.Add("@sizes", SqlDbType.NVarChar).Value = sizesTb.Text;
            comm.Parameters.Add("@addressMagazin", SqlDbType.NVarChar).Value = addressMagazinTb.Text;
            comm.Parameters.Add("@categoryId", SqlDbType.Int).Value = CategoryCb.SelectedIndex+1;
            comm.Parameters.Add("@price", SqlDbType.NVarChar).Value = priceTb.Text;
            if (int.TryParse(idPhotoMainTb.Text, out var num1))
            {
                comm.Parameters.Add("@idPhotoMain", SqlDbType.Int).Value = Convert.ToInt32(idPhotoMainTb.Text);
            }
            else
            {
                comm.Parameters.Add("@idPhotoMain", SqlDbType.Int).Value = DBNull.Value;
            }
            if (int.TryParse(idPhotoSecondTb.Text, out var num2))
            {
                comm.Parameters.Add("@idPhotoSecond", SqlDbType.Int).Value = Convert.ToInt32(idPhotoSecondTb.Text);
            }
            else
            {
                comm.Parameters.Add("@idPhotoSecond", SqlDbType.Int).Value = DBNull.Value;
            }
            if (int.TryParse(idPhotoThirdTb.Text, out var num3))
            {
                comm.Parameters.Add("@idPhotoThird", SqlDbType.Int).Value = Convert.ToInt32(idPhotoThirdTb.Text);
            }
            else
            {
                comm.Parameters.Add("@idPhotoThird", SqlDbType.Int).Value = DBNull.Value;
            }
            MessageBox.Show(comm.ExecuteScalar().ToString());
        }

        private void statusBtn_Click(object sender, EventArgs e)
        {
           // statusBtn.Text = webSocketClient.State.ToString();
        }

        private static async void ClientSendAsync(ClientWebSocket webSocketClient)
        {
            var arraySegment = new ArraySegment<byte>(Encoding.UTF8.GetBytes("getphoto%2"));
            await webSocketClient.SendAsync(arraySegment, WebSocketMessageType.Text, true, CancellationToken.None);
            var buffer = new byte[5242880];
            var segment = new ArraySegment<byte>(buffer);


            while (webSocketClient.State == WebSocketState.Open)
            {
                WebSocketReceiveResult resp;
                resp = await webSocketClient.ReceiveAsync(segment, CancellationToken.None);
                string response = Encoding.UTF8.GetString(segment.Array).TrimEnd(' ', '\0');
                if (response != "" && response != "photoMissing")
                {
                    imgList.Images.Add(Image.FromStream(new MemoryStream(segment.Array)));
                    listView.Items.Add("text").ImageIndex = imgList.Images.Count - 1;
                }
                if (resp.EndOfMessage)
                {
                    break;
                }
            }
        }

        private void sendBtn_Click(object sender, EventArgs e)
        {
            //webSocketClient.ConnectAsync(new Uri("ws://localhost:5000"), CancellationToken.None);
        }

        private void CategoryCb_SelectedIndexChanged(object sender, EventArgs e)
        {
            CategoryLbl.Text = "Категория: " + CategoryCb.SelectedIndex+1;
        }

        private void testBtn_Click(object sender, EventArgs e)
        {
            try
            {
                while (true)
                {
                    connection.OpenAsync();
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
    }
}
