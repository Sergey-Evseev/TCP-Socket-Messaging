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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrackBar;

namespace Client
{
    public partial class Form1 : Form
    {

        TcpClient client; //CLIENT
        public Form1()
        {
            InitializeComponent();
            textBox3.KeyPress += new KeyPressEventHandler(textBox3_KeyPress);

            // Set the form's border style to FixedSingle
            this.FormBorderStyle = FormBorderStyle.FixedSingle;

            // Set the form's minimum and maximum size to the same value
            this.MinimumSize = new Size(459, 155);
            this.MaximumSize = new Size(459, 155);
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            await SendMessage();
        }

        private async void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                e.Handled = true; // To prevent beep sound on Enter key press
                await SendMessage();
            }
        }

        private async Task SendMessage()
        {
            try
            {
                //The TcpClient class is a .NET Framework class used to create a client-side connection
                //to a server using TCP. After creating an instance of TcpClient, you can use
                //its Connect() method to the specified IP address and port number.
                client = new TcpClient();

                //установка соединения с использованием данных IP и номера порта
                await client.ConnectAsync(IPAddress.Parse(textBox1.Text),
                    Convert.ToInt32(textBox2.Text));
                //получение сетевого потока
                NetworkStream nstream = client.GetStream();
                //преобразование строки сообщения в массив байт
                byte[] barray = Encoding.Unicode.GetBytes(textBox3.Text);
                //запись в сетевой поток всего массива
                await nstream.WriteAsync(barray, 0, barray.Length); //byte array, offset, number of bytes to write to stream
                //закрытие клиента
                client.Close();
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

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (client != null) client.Close();
        }
    }//end of Form1
}
