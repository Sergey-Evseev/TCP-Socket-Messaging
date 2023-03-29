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

namespace TCP_Socket_Messaging
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        TcpListener list;

        private void button1_Click(object sender, EventArgs e)
        {
            try 
            {
                //создание экземпляра класса TcpListener
                //данные о хосте и порте читаются из текстовых окон
                list = new TcpListener(IPAddress.Parse(textBox1.Text),
                    Convert.ToInt32(textBox2.Text));

                //начало прослушивания клиентов
                list.Start();
                //создание отдельного потока для чтения сообщения
                Thread thread = new Thread(new ThreadStart(ThreadFun));
                thread.IsBackground = true;
                thread.Start();
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
        void ThreadFun()
        {
            while(true)
            {
                //сервер сообщает клиенту о готовности к соединению
                //AcceptTcpClient() method of a TcpListener object
                //TcpClient object that represents the client's endpoint for the communication
                TcpClient cl = list.AcceptTcpClient();

                //чтение данных из сети в формате Unicode
                StreamReader sr = new StreamReader(cl.GetStream(), Encoding.Unicode);
                string s = sr.ReadLine();
                //добавление полученного сообщения в список
                messageList.Items.Add(s);
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
