using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pong
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            Connect();
        }
        int speedx = 3;
        int speedy = 3;

        int rightwall;
        int leftwall;
        int bottomwall;
        int topwall;

        int player1top;
        int player1bottom;
        int player1left;
        int player1right;

        int player2top;
        int player2bottom;
        int player2left;
        int player2right;

        int balltop;
        int ballbottom;
        int ballleft;
        int ballright;

        private void button1_KeyUp(object sender, KeyEventArgs e)
        {
            //if (e.KeyCode == Keys.W) 
            //{
            //    button1.Top = button1.Top - 1;
            //}
            //if (e.KeyCode == Keys.S)
            //{
            //    button1.Top = button1.Top + 1;
            //}
        }

        private void button1_KeyDown(object sender, KeyEventArgs e)
        {
            //if (e.KeyCode == Keys.W)
            //{
            //    button1.Top = button1.Top - 30;
            //}
            //if (e.KeyCode == Keys.S)
            //{
            //    button1.Top = button1.Top + 30;
            //}
        }

        private void button2_KeyDown(object sender, KeyEventArgs e)
        {
            //if (e.KeyCode == Keys.O)
            //{
            //    button2.Top = button2.Top - 30;
            //}
            //if (e.KeyCode == Keys.L)
            //{
            //    button2.Top = button2.Top + 30;
            //}
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.W)
            {
                button1.Top = button1.Top - 30;
            }
            if (e.KeyCode == Keys.S)
            {
                button1.Top = button1.Top + 30;
            }

            if (e.KeyCode == Keys.O)
            {
                button2.Top = button2.Top - 30;
            }
            if (e.KeyCode == Keys.L)
            {
                button2.Top = button2.Top + 30;
            }
        }

        private void colision()
        {
            player1top = button1.Top;
            player1bottom = button1.Top + button1.Height;
            player1left = button1.Left;
            player1right = button1.Left + button1.Width;

            player2top = button2.Top;
            player2bottom = button2.Top + button2.Height;
            player2left = button2.Left;
            player2right = button2.Left + button2.Width;

            balltop = ball.Top;
            ballbottom = ball.Top + ball.Height;
            ballleft = ball.Left;
            ballright = ball.Left + ball.Width;

            ball.Top = ball.Top + speedy;
            ball.Left = ball.Left + speedx;

            topwall = 40;
            bottomwall = this.Size.Height - ball.Height - 37;
            leftwall = 0;
            rightwall = this.Size.Width - ball.Width;

            if (ballbottom >= player1top && balltop <= player1bottom && ballleft <= player1right && ballright >= player1left)
            {
                speedx = 3;
            }

            if (ballbottom >= player2top && balltop <= player2bottom && ballleft <= player2right && ballright >= player2left)
            {
                speedx = -3;
            }

            if (ball.Top <= topwall)
            {
                speedy = 3;
            }
            if (ball.Top >= bottomwall)
            {
                speedy = -3;
            }
            if (ball.Left >= rightwall)
            {
                speedx = -3;
            }
            if (ball.Left <= leftwall)
            {
                speedx = 3;
            }
            Send();        
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            colision();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        int scorep1;
        int scorep2;
        private void reset()
        {
            ball.Top = 180;
            ball.Left = 373;
            timer1.Enabled = Enabled;
        }

        private void resetscore()
        {
            ball.Top = 180;
            ball.Left = 370;
            label3.Text = "0";
            label4.Text = "0";
            scorep1 = 0;
            scorep2 = 0;
            MessageBox.Show("Game Over");
            timer1.Enabled = false;
        }


        public void Connect()
        {
            ip = IPAddress.Parse("127.0.0.1");
            ipep = new IPEndPoint(ip, 8888);
            server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                server.Connect(ipep);
                MessageBox.Show("Connected to Server");
            }
            catch (SocketException e)
            {              
                MessageBox.Show("Can't connect to Server");
                return;
            }
        }

        public static Socket server;
        public static byte[] send = new byte[1024];
        public static byte[] receive = new byte[1024];
        IPAddress ip;
        IPEndPoint ipep;

        public void Send()
        {         
            if (ball.Left <= leftwall)
            {
                timer1.Enabled = false;
                reset();
                string i = scorep2.ToString();
                send = Encoding.ASCII.GetBytes(i);
                server.Send(send, send.Length, SocketFlags.None);
                int receivebytes = server.Receive(receive);
                string a = Encoding.ASCII.GetString(receive, 0, receivebytes);
                int b = int.Parse(a);
                scorep2 = b;
                label4.Text = scorep2.ToString();
            }
            if (ball.Left >= rightwall)
            {
                timer1.Enabled = false;
                reset();            
                string j = scorep1.ToString();
                send = Encoding.ASCII.GetBytes(j);
                server.Send(send, send.Length, SocketFlags.None);
                int receivebytes = server.Receive(receive);
                string c = Encoding.ASCII.GetString(receive, 0, receivebytes);              
                int d = int.Parse(c);
                scorep1 = d;
                label3.Text = scorep1.ToString();
            }

            if (scorep1 == 5 || scorep2 == 5)
            {
                timer1.Enabled = false;
                string x = "5";
                resetscore();
                send = Encoding.ASCII.GetBytes(x);
                server.Send(send, send.Length, SocketFlags.None);
                int receivebytes = server.Receive(receive);
                string f = Encoding.ASCII.GetString(receive,0, receivebytes);
                MessageBox.Show(f);
                timer1.Enabled = Enabled;
            }

        }

        private void menToolStripMenuItem_Click(object sender, EventArgs e)
        {        
        }

        private void stopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            timer1.Enabled=false; 
        }       

        private void quitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void resumeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            timer1.Enabled = Enabled;
        }
    }
}
