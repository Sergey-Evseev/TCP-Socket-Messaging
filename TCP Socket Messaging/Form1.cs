using System;
using System.Threading;
using System.IO;
using System.Net.Sockets;
using System.Net;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;
using System.Xml.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Runtime.InteropServices;

namespace TCP_Socket_Messaging
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        TcpListener list; //SERVER

        private async void button1_Click(object sender, EventArgs e)
        {
            try 
            {
                //создание экземпляра класса TcpListener
                //данные о хосте и порте читаются из текстовых окон
                list = new TcpListener(IPAddress.Parse(textBox1.Text),
                    Convert.ToInt32(textBox2.Text));

                //============================================
                //захардкоренный прием с сетевых адресов
                //IPAddress ipAddress = IPAddress.Parse("10.9.0.18");
                //int port = 8080;
                //list = new TcpListener(ipAddress, port);
                //============================================

                //начало прослушивания клиентов
                list.Start();

                //ThreadFun method is called using the await keyword to asynchronously
                //wait for its completion. This allows the UI thread to continue
                //executing while the server is listening for incoming connections
                //and handling requests asynchronously:
                MessageBox.Show("Server Listening");
                await ThreadFun();
            }
            catch (SocketException sockEx)
            {
                MessageBox.Show("Socket Error: " + sockEx.Message);
            }
            catch (Exception Ex)
            {
                MessageBox.Show("Error: " + Ex.Message);
            }
        }
        async Task ThreadFun()
        {
            while(true)
            {
                //сервер сообщает клиенту о готовности к соединению
                //AcceptTcpClient() method of a TcpListener object
                //TcpClient object that represents the client's endpoint for the communication
                TcpClient cl = await list.AcceptTcpClientAsync();
                //- AcceptTcpClientAsync method is used to asynchronously wait
                //for an incoming connection

                //чтение данных из сети в формате Unicode
                StreamReader sr = new StreamReader(cl.GetStream(), Encoding.Unicode);
                string s =await sr.ReadLineAsync();
                //-ReadLineAsync method is used to read data
                //from the network stream asynchronously
                //await keyword is used to asynchronously wait for the completion
                //of these operations.

                //добавление полученного сообщения в конец списка
                messageList.Invoke((MethodInvoker)delegate 
                {
                    messageList.Items.Add(s);
                    messageList.TopIndex = messageList.Items.Count - 1; 
                    // set the TopIndex to the last item in the list
                    // (allways scrolled down)
                });


                cl.Close();

                //при получении сообщения EXIT завершить приложение
                if(s.ToUpper() == "EXIT")
                {
                    list.Stop();
                    this.Close();
                }

            }
        }
        //If the object TcpListener is not null, it calls its Stop() method
        //to stop listening for incoming connections.
        //application ensures that any pending connections are properly closed
        //and resources are freed up
        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (list != null) list.Stop();
        }
    }//end of Form1
}
