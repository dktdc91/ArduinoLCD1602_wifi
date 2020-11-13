using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.IO;
using System.Threading;

namespace WindowsFormsApp2
{
    public partial class Form1 : Form
    {
        private String text1;
        private String text2;
        private String ipAddress;
        private String text3;
        private bool seamless = false;
        public Form1()
        {
      
            InitializeComponent();
            textBox4.Enabled = false;
            textBox4.Text = "Waiting ...";
            textBox3.Text = "192.168.137.14";

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            text1 = textBox1.Text;
            if (seamless)
            {
                String message = "<_UL1[";
                message += text1;
                message += "]>";
                var autoEvent = new AutoResetEvent(false);
                Connect(ipAddress, message);
                autoEvent.WaitOne(300);
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            text2 = textBox2.Text;
            if (seamless)
            {
                String message = "<_UL2[";
                message += text2;
                message += "]>";
                var autoEvent = new AutoResetEvent(false);
                Connect(ipAddress, message);
                autoEvent.WaitOne(300);
            }
        }


        private void Connect(String server, String message)
        {
            try
            {
                // Create a TcpClient.
                // Note, for this client to work you need to have a TcpServer
                // connected to the same address as specified by the server, port
                // combination.
                Int32 port = 80;
                TcpClient client = new TcpClient();
                var result = client.BeginConnect(server, port, null, null);
                var success = result.AsyncWaitHandle.WaitOne(TimeSpan.FromSeconds(1));

                if (!success)
                {
                    MessageBox.Show("Failed to connet. Check connection and try again.");
                    throw new Exception("Failed to connect.");
                }


                Byte[] data = System.Text.Encoding.UTF8.GetBytes(message);

                // Get a client stream for reading and writing.
                //  Stream stream = client.GetStream();

                NetworkStream stream = client.GetStream();

                if(stream == null)
                {
                    return;
                }

                // Send the message to the connected TcpServer.
                stream.Write(data, 0, data.Length);

                Console.WriteLine("Sent: {0}", message);

                // Receive the TcpServer.response.

                // Buffer to store the response bytes.
                data = new Byte[256];
               //
                // String to store the response ASCII representation.
                String responseData = String.Empty;


                // Read the first batch of the TcpServer response bytes.
                Int32 bytes = stream.Read(data, 0, data.Length);
                responseData = System.Text.Encoding.UTF8.GetString(data, 0, bytes);
               // if(responseData != "") { }
               Console.WriteLine("Received: {0}", responseData);
               textBox4.Text = responseData;
               textBox4.Update();
                

                // Close everything.
                stream.Close();
                client.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: {0}", e);
            }

            //Console.WriteLine("\n Press Enter to continue...");
            //Console.Read();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            String message = "<_UL1[";
            message += text1;
            message += "]>";
            Connect(ipAddress, message);
        }


        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            ipAddress = textBox3.Text;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Connect(ipAddress, "<Reset>");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            String message = "<_UL2[";
            message += text2;
            message += "]>";
            Connect(ipAddress, message);
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            seamless = !seamless;
        }

    }
}
