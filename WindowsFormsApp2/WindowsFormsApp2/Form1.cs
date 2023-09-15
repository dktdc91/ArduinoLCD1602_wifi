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

namespace WindowsFormsApp2
{
    public partial class Form1 : Form
    {
        private String text1;
        private String text2;
        private String ipAddress;
        private String text3;
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
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            text2 = textBox2.Text;
        }

        private async Task ConnectAsync(string server, string message, int port = 80)
        {
            try
            {
                using (TcpClient client = new TcpClient())
                {
                    var result = client.BeginConnect(server, port, null, null);
                    var success = result.AsyncWaitHandle.WaitOne(TimeSpan.FromSeconds(5));

                    if (!success)
                    {
                        MessageBox.Show("Failed to connect. Check connection and try again.");
                        throw new Exception("Failed to connect.");
                    }

                    using (NetworkStream stream = client.GetStream())
                    {
                        byte[] data = Encoding.UTF8.GetBytes(message);

                        await stream.WriteAsync(data, 0, data.Length);
                        Console.WriteLine("Sent: {0}", message);

                        data = new byte[256];
                        string responseData = string.Empty;

                        int bytes = await stream.ReadAsync(data, 0, data.Length);
                        responseData = Encoding.UTF8.GetString(data, 0, bytes);
                    
                        Console.WriteLine("Received: {0}", responseData);
                        
                        UpdateUI(responseData);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: {0}", e);
                // Log your exception (consider using a logging framework)
            }
        }

        private void UpdateUI(string response)
        {
            // Ensure to invoke UI changes on the UI thread
            if (textBox4.InvokeRequired)
            {
                textBox4.Invoke(new Action<string>(UpdateUI), response);
                return;
            }

            textBox4.Text = response;
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
            try {
                await ConnectAsync(ipAddress, message);
            } catch (Exception ex) {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
        }
    }
}
