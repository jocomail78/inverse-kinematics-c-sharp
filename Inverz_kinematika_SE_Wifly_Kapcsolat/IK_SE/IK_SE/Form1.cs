using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace IK_SE
{
    public partial class Form1 : Form
    {
        public PokTest pt;
        Graphics pBody;
        Graphics pBalFelsoOldalrol,pBalFelsoFelulrol;
        Graphics pBalKozepsoOldalrol, pBalKozepsoFelulrol;
        Graphics pBalAlsoOldalrol, pBalAlsoFelulrol;

        Graphics pJobbFelsoOldalrol, pJobbFelsoFelulrol;
        Graphics pJobbKozepsoOldalrol, pJobbKozepsoFelulrol;
        Graphics pJobbAlsoOldalrol, pJobbAlsoFelulrol;
        public int magassag = 10;
        public int L1,L2;
        public Point BalKezdoOldalrol, BalKezdoFelulrol,JobbKezdoOldalrol, JobbKezdoFelulrol;
        private bool connected;
        TcpClient WiFly_address;
        NetworkStream WiFly_stream;
        recv_thread WiFly_recv;
        Thread WiFly_thread;
        private delegate void PrintOutDelegate(string message);

        public Form1()
        {
            InitializeComponent();
            pt = new PokTest(new TestPont(115,100,magassag), new TestPont(100,200,magassag),new TestPont(115,300,magassag),new TestPont(205,100,magassag),new TestPont(220,200,magassag),new TestPont(205,300,magassag),new TestPont(160,200,magassag));
            pBody = p_body.CreateGraphics();
            L1 = 50;
            L2 = 75;
            //Grafika hozzarendelese a panelekhez. Bal oldal
            pBalFelsoOldalrol = panel_bal_felso_oldalrol.CreateGraphics();
            pBalFelsoFelulrol = panel_bal_felso_felulrol.CreateGraphics();
            pBalKozepsoOldalrol = panel_bal_kozepso_oldalrol.CreateGraphics();
            pBalKozepsoFelulrol = panel_bal_kozepso_felulrol.CreateGraphics();
            pBalAlsoOldalrol = panel_bal_also_oldalrol.CreateGraphics();
            pBalAlsoFelulrol = panel_bal_also_felulrol.CreateGraphics();

            //Grafika hozzarendelese a panelekhez. Jobb oldal
            pJobbFelsoOldalrol = panel_jobb_felso_oldalrol.CreateGraphics();
            pJobbFelsoFelulrol = panel_jobb_felso_felulrol.CreateGraphics();
            pJobbKozepsoOldalrol = panel_jobb_kozepso_oldalrol.CreateGraphics();
            pJobbKozepsoFelulrol = panel_jobb_kozepso_felulrol.CreateGraphics();
            pJobbAlsoOldalrol = panel_jobb_also_oldalrol.CreateGraphics();
            pJobbAlsoFelulrol = panel_jobb_also_felulrol.CreateGraphics();

            BalKezdoOldalrol = new Point(150, 50);
            BalKezdoFelulrol = new Point(150, 75);
            pt.balFelsoLab = new Lab(new Nezet(BalKezdoOldalrol.X, BalKezdoOldalrol.Y, BalKezdoOldalrol.X - L1, BalKezdoOldalrol.Y, BalKezdoOldalrol.X - L1, BalKezdoOldalrol.Y + L2), new Nezet(BalKezdoFelulrol.X, BalKezdoFelulrol.Y, BalKezdoFelulrol.X - L1, BalKezdoFelulrol.Y, BalKezdoFelulrol.X - L1, BalKezdoFelulrol.Y));
            pt.balKozepsoLab = new Lab(new Nezet(BalKezdoOldalrol.X, BalKezdoOldalrol.Y, BalKezdoOldalrol.X - L1, BalKezdoOldalrol.Y, BalKezdoOldalrol.X - L1, BalKezdoOldalrol.Y + L2), new Nezet(BalKezdoFelulrol.X, BalKezdoFelulrol.Y, BalKezdoFelulrol.X - L1, BalKezdoFelulrol.Y, BalKezdoFelulrol.X - L1, BalKezdoFelulrol.Y));
            pt.balAlsoLab = new Lab(new Nezet(BalKezdoOldalrol.X, BalKezdoOldalrol.Y, BalKezdoOldalrol.X - L1, BalKezdoOldalrol.Y, BalKezdoOldalrol.X - L1, BalKezdoOldalrol.Y + L2), new Nezet(BalKezdoFelulrol.X, BalKezdoFelulrol.Y, BalKezdoFelulrol.X - L1, BalKezdoFelulrol.Y, BalKezdoFelulrol.X - L1, BalKezdoFelulrol.Y));

            JobbKezdoOldalrol = new Point(0, 50);
            JobbKezdoFelulrol = new Point(0, 75);
            pt.jobbFelsoLab = new Lab(new Nezet(JobbKezdoOldalrol.X, JobbKezdoOldalrol.Y, JobbKezdoOldalrol.X + L1, JobbKezdoOldalrol.Y, JobbKezdoOldalrol.X + L1, JobbKezdoOldalrol.Y + L2), new Nezet(JobbKezdoFelulrol.X, JobbKezdoFelulrol.Y, JobbKezdoFelulrol.X + L1, JobbKezdoFelulrol.Y, JobbKezdoFelulrol.X + L1, JobbKezdoFelulrol.Y));
            pt.jobbKozepsoLab = new Lab(new Nezet(JobbKezdoOldalrol.X, JobbKezdoOldalrol.Y, JobbKezdoOldalrol.X + L1, JobbKezdoOldalrol.Y, JobbKezdoOldalrol.X + L1, JobbKezdoOldalrol.Y + L2), new Nezet(JobbKezdoFelulrol.X, JobbKezdoFelulrol.Y, JobbKezdoFelulrol.X + L1, JobbKezdoFelulrol.Y, JobbKezdoFelulrol.X + L1, JobbKezdoFelulrol.Y));
            pt.jobbAlsoLab = new Lab(new Nezet(JobbKezdoOldalrol.X, JobbKezdoOldalrol.Y, JobbKezdoOldalrol.X + L1, JobbKezdoOldalrol.Y, JobbKezdoOldalrol.X + L1, JobbKezdoOldalrol.Y + L2), new Nezet(JobbKezdoFelulrol.X, JobbKezdoFelulrol.Y, JobbKezdoFelulrol.X + L1, JobbKezdoFelulrol.Y, JobbKezdoFelulrol.X + L1, JobbKezdoFelulrol.Y));

            pt.balFelsoLab.oldalrol.t1 = tb_BalFelsoTeta1;
            pt.balFelsoLab.oldalrol.t2 = tb_BalFelsoTeta2;
            pt.balFelsoLab.oldalrol.t3 = tb_BalFelsoTeta3;
            pt.balFelsoLab.oldalrol.t4 = tb_BalFelso_OldalnezetX;
            pt.balFelsoLab.oldalrol.t5 = tb_BalFelso_OldalnezetY;

            pt.balFelsoLab.felulrol.t1 = tb_BalFelsoTeta1;
            pt.balFelsoLab.felulrol.t2 = tb_BalFelsoTeta2;
            pt.balFelsoLab.felulrol.t3 = tb_BalFelsoTeta3;
            pt.balFelsoLab.felulrol.t4 = tb_BalFelso_FelulnezetX;
            pt.balFelsoLab.felulrol.t5 = tb_BalFelso_FelulnezetY;

            pt.balKozepsoLab.oldalrol.t1 = tb_BalKozepsoTeta1;
            pt.balKozepsoLab.oldalrol.t2 = tb_BalKozepsoTeta2;
            pt.balKozepsoLab.oldalrol.t3 = tb_BalKozepsoTeta3;
            pt.balKozepsoLab.oldalrol.t4 = tb_BalKozepso_OldalnezetX;
            pt.balKozepsoLab.oldalrol.t5 = tb_BalKozepso_OldalnezetY;

            pt.balKozepsoLab.felulrol.t1 = tb_BalKozepsoTeta1;
            pt.balKozepsoLab.felulrol.t2 = tb_BalKozepsoTeta2;
            pt.balKozepsoLab.felulrol.t3 = tb_BalKozepsoTeta3;
            pt.balKozepsoLab.felulrol.t4 = tb_BalKozepso_FelulnezetX;
            pt.balKozepsoLab.felulrol.t5 = tb_BalKozepso_FelulnezetY;

            pt.balAlsoLab.felulrol.t1 = tb_BalAlsoTeta1;
            pt.balAlsoLab.felulrol.t2 = tb_BalAlsoTeta2;
            pt.balAlsoLab.felulrol.t3 = tb_BalAlsoTeta3;
            pt.balAlsoLab.felulrol.t4 = tb_BalAlso_FelulnezetX;
            pt.balAlsoLab.felulrol.t5 = tb_BalAlso_FelulnezetY;

            pt.balAlsoLab.oldalrol.t1 = tb_BalAlsoTeta1;
            pt.balAlsoLab.oldalrol.t2 = tb_BalAlsoTeta2;
            pt.balAlsoLab.oldalrol.t3 = tb_BalAlsoTeta3;
            pt.balAlsoLab.oldalrol.t4 = tb_BalAlso_OldalnezetX;
            pt.balAlsoLab.oldalrol.t5 = tb_BalAlso_OldalnezetY;

            //pt.balFelsoLab.oldalrol.eltolas1 = Math.PI;
            pt.balFelsoLab.oldalrol.eltolas2 = -Math.PI/2;
            pt.balFelsoLab.felulrol.eltolas3 = Math.PI;
            pt.balFelsoLab.oldalrol.baloldali = true;
            pt.balFelsoLab.felulrol.baloldali = true;
            pt.balFelsoLab.oldalrol.parja = pt.balFelsoLab.felulrol;
            pt.balFelsoLab.felulrol.parja = pt.balFelsoLab.oldalrol;

            pt.balKozepsoLab.felulrol.parja = pt.balKozepsoLab.oldalrol;
            pt.balKozepsoLab.oldalrol.parja = pt.balKozepsoLab.felulrol;
            pt.balKozepsoLab.oldalrol.eltolas2 = -Math.PI / 2;
            pt.balKozepsoLab.felulrol.eltolas3 = Math.PI;
            pt.balKozepsoLab.felulrol.baloldali = true;
            pt.balKozepsoLab.oldalrol.baloldali = true;

            pt.balAlsoLab.felulrol.parja = pt.balAlsoLab.oldalrol;
            pt.balAlsoLab.oldalrol.parja = pt.balAlsoLab.felulrol;
            pt.balAlsoLab.oldalrol.eltolas2 = -Math.PI / 2;
            pt.balAlsoLab.felulrol.eltolas3 = Math.PI;
            pt.balAlsoLab.felulrol.baloldali = true;
            pt.balAlsoLab.oldalrol.baloldali = true;


            pt.jobbFelsoLab.oldalrol.parja = pt.jobbFelsoLab.felulrol;
            pt.jobbFelsoLab.felulrol.parja = pt.jobbFelsoLab.oldalrol;

            pt.jobbFelsoLab.oldalrol.t1 = tb_JobbFelsoTeta1;
            pt.jobbFelsoLab.oldalrol.t2 = tb_JobbFelsoTeta2;
            pt.jobbFelsoLab.oldalrol.t3 = tb_JobbFelsoTeta3;
            pt.jobbFelsoLab.oldalrol.t4 = tb_JobbFelso_OldalnezetX;
            pt.jobbFelsoLab.oldalrol.t5 = tb_JobbFelso_OldalnezetY;

            pt.jobbFelsoLab.felulrol.t1 = tb_JobbFelsoTeta1;
            pt.jobbFelsoLab.felulrol.t2 = tb_JobbFelsoTeta2;
            pt.jobbFelsoLab.felulrol.t3 = tb_JobbFelsoTeta3;
            pt.jobbFelsoLab.felulrol.t4 = tb_JobbFelso_FelulnezetX;
            pt.jobbFelsoLab.felulrol.t5 = tb_JobbFelso_FelulnezetY;

            pt.jobbFelsoLab.oldalrol.eltolas2 = -Math.PI/2;

            pt.jobbKozepsoLab.oldalrol.parja = pt.jobbKozepsoLab.felulrol;
            pt.jobbKozepsoLab.felulrol.parja = pt.jobbKozepsoLab.oldalrol;

            pt.jobbKozepsoLab.oldalrol.t1 = tb_JobbKozepsoTeta1;
            pt.jobbKozepsoLab.oldalrol.t2 = tb_JobbKozepsoTeta2;
            pt.jobbKozepsoLab.oldalrol.t3 = tb_JobbKozepsoTeta3;
            pt.jobbKozepsoLab.oldalrol.t4 = tb_JobbKozepso_OldalnezetX;
            pt.jobbKozepsoLab.oldalrol.t5 = tb_JobbKozepso_OldalnezetY;

            pt.jobbKozepsoLab.felulrol.t1 = tb_JobbKozepsoTeta1;
            pt.jobbKozepsoLab.felulrol.t2 = tb_JobbKozepsoTeta2;
            pt.jobbKozepsoLab.felulrol.t3 = tb_JobbKozepsoTeta3;
            pt.jobbKozepsoLab.felulrol.t4 = tb_JobbKozepso_FelulnezetX;
            pt.jobbKozepsoLab.felulrol.t5 = tb_JobbKozepso_FelulnezetY;

            pt.jobbKozepsoLab.oldalrol.eltolas2 = -Math.PI / 2;

            pt.jobbAlsoLab.oldalrol.parja = pt.jobbAlsoLab.felulrol;
            pt.jobbAlsoLab.felulrol.parja = pt.jobbAlsoLab.oldalrol;

            pt.jobbAlsoLab.oldalrol.t1 = tb_JobbAlsoTeta1;
            pt.jobbAlsoLab.oldalrol.t2 = tb_JobbAlsoTeta2;
            pt.jobbAlsoLab.oldalrol.t3 = tb_JobbAlsoTeta3;
            pt.jobbAlsoLab.oldalrol.t4 = tb_JobbAlso_OldalnezetX;
            pt.jobbAlsoLab.oldalrol.t5 = tb_JobbAlso_OldalnezetY;

            pt.jobbAlsoLab.felulrol.t1 = tb_JobbAlsoTeta1;
            pt.jobbAlsoLab.felulrol.t2 = tb_JobbAlsoTeta2;
            pt.jobbAlsoLab.felulrol.t3 = tb_JobbAlsoTeta3;
            pt.jobbAlsoLab.felulrol.t4 = tb_JobbAlso_FelulnezetX;
            pt.jobbAlsoLab.felulrol.t5 = tb_JobbAlso_FelulnezetY;

            pt.jobbAlsoLab.oldalrol.eltolas2 = -Math.PI / 2;

        }

        public void PrintOut(string input)
        {
            if (tb_message_received.InvokeRequired)
            {
                // This is a worker thread so delegate the task.
                tb_message_received.Invoke(new PrintOutDelegate(this.PrintOut), input);
            }
            else
            {
                // This is the UI thread so perform the task.
                tb_message_received.Text += input;
                tb_message_received.SelectionStart = tb_message_received.Text.Length;
                tb_message_received.ScrollToCaret();
                tb_message_received.Refresh();
            }


        }

        private void button49_Click(object sender, EventArgs e)
        {
            pt.PokKirajzol(pt,pBody);
            pt.balFelsoLab.oldalrol.LabKirajzol(pBalFelsoOldalrol);
            pt.balFelsoLab.felulrol.LabKirajzol(pBalFelsoFelulrol);
            pt.balKozepsoLab.oldalrol.LabKirajzol(pBalKozepsoOldalrol);
            pt.balKozepsoLab.felulrol.LabKirajzol(pBalKozepsoFelulrol);
            pt.balAlsoLab.oldalrol.LabKirajzol(pBalAlsoOldalrol);
            pt.balAlsoLab.felulrol.LabKirajzol(pBalAlsoFelulrol);

            pt.jobbFelsoLab.oldalrol.LabKirajzol(pJobbFelsoOldalrol);
            pt.jobbFelsoLab.felulrol.LabKirajzol(pJobbFelsoFelulrol);
            pt.jobbKozepsoLab.oldalrol.LabKirajzol(pJobbKozepsoOldalrol);
            pt.jobbKozepsoLab.felulrol.LabKirajzol(pJobbKozepsoFelulrol);
            pt.jobbAlsoLab.oldalrol.LabKirajzol(pJobbAlsoOldalrol);
            pt.jobbAlsoLab.felulrol.LabKirajzol(pJobbAlsoFelulrol);

            tb_eredmenyek.Text = "A sorrend BF, BK, BL, JF, JK, JL.\r\nElso Fent, Masodik Kozepen, Harmadik Lent.\r\n";
        }

        private void panel_bal_felso_oldalrol_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left) //le van-e nyomva az eger?
            {
                pt.balFelsoLab.felulrol.LabTorol(pBalFelsoFelulrol);
                pt.balFelsoLab.oldalrol.LabTorol(pBalFelsoOldalrol);

                pt.balFelsoLab.oldalrol.lax = e.X;  //frissit
                pt.balFelsoLab.oldalrol.lay = e.Y;

                pt.balFelsoLab.oldalrol.Kiszamol_OldalNezet();

                pt.balFelsoLab.oldalrol.LabKirajzol(pBalFelsoOldalrol);
                pt.balFelsoLab.felulrol.LabKirajzol(pBalFelsoFelulrol);
                pt.PokKirajzol(pt, pBody);

            }
        }

        private void panel_bal_felso_felulrol_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left) //le van-e nyomva az eger?
            {
                pt.balFelsoLab.felulrol.LabTorol(pBalFelsoFelulrol);
                pt.balFelsoLab.oldalrol.LabTorol(pBalFelsoOldalrol);

                pt.balFelsoLab.felulrol.lax = e.X;  //frissit
                pt.balFelsoLab.felulrol.lay =panel_bal_felso_felulrol.Height - e.Y;
                
                pt.balFelsoLab.felulrol.Kiszamol_FelulNezet();

                pt.balFelsoLab.oldalrol.LabKirajzol(pBalFelsoOldalrol);
                pt.balFelsoLab.felulrol.LabKirajzol(pBalFelsoFelulrol);
                pt.PokKirajzol(pt, pBody);
            }
        }

        private void panel_bal_kozepso_oldalrol_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left) //le van-e nyomva az eger?
            {
                pt.balKozepsoLab.felulrol.LabTorol(pBalKozepsoFelulrol);
                pt.balKozepsoLab.oldalrol.LabTorol(pBalKozepsoOldalrol);

                pt.balKozepsoLab.oldalrol.lax = e.X;  //frissit
                pt.balKozepsoLab.oldalrol.lay = e.Y;

                pt.balKozepsoLab.oldalrol.Kiszamol_OldalNezet();

                pt.balKozepsoLab.oldalrol.LabKirajzol(pBalKozepsoOldalrol);
                pt.balKozepsoLab.felulrol.LabKirajzol(pBalKozepsoFelulrol);
                pt.PokKirajzol(pt, pBody);
            }
        }

        private void panel_bal_kozepso_felulrol_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left) //le van-e nyomva az eger?
            {
                pt.balKozepsoLab.felulrol.LabTorol(pBalKozepsoFelulrol);
                pt.balKozepsoLab.oldalrol.LabTorol(pBalKozepsoOldalrol);

                pt.balKozepsoLab.felulrol.lax = e.X;  //frissit
                pt.balKozepsoLab.felulrol.lay = panel_bal_kozepso_felulrol.Height - e.Y;
                
                pt.balKozepsoLab.felulrol.Kiszamol_FelulNezet();

                pt.balKozepsoLab.oldalrol.LabKirajzol(pBalKozepsoOldalrol);
                pt.balKozepsoLab.felulrol.LabKirajzol(pBalKozepsoFelulrol);
                pt.PokKirajzol(pt, pBody);
            }
        }


        private void panel_bal_also_oldalrol_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left) //le van-e nyomva az eger?
            {
                pt.balAlsoLab.felulrol.LabTorol(pBalAlsoFelulrol);
                pt.balAlsoLab.oldalrol.LabTorol(pBalAlsoOldalrol);

                pt.balAlsoLab.oldalrol.lax = e.X;  //frissit
                pt.balAlsoLab.oldalrol.lay = e.Y;

                pt.balAlsoLab.oldalrol.Kiszamol_OldalNezet();

                pt.balAlsoLab.oldalrol.LabKirajzol(pBalAlsoOldalrol);
                pt.balAlsoLab.felulrol.LabKirajzol(pBalAlsoFelulrol);
                pt.PokKirajzol(pt, pBody);
            }
        }

        private void panel_bal_also_felulrol_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left) //le van-e nyomva az eger?
            {
                pt.balAlsoLab.felulrol.LabTorol(pBalAlsoFelulrol);
                pt.balAlsoLab.oldalrol.LabTorol(pBalAlsoOldalrol);

                pt.balAlsoLab.felulrol.lax = e.X;  //frissit
                pt.balAlsoLab.felulrol.lay = panel_bal_also_felulrol.Height - e.Y;

                pt.balAlsoLab.felulrol.Kiszamol_FelulNezet();

                pt.balAlsoLab.oldalrol.LabKirajzol(pBalAlsoOldalrol);
                pt.balAlsoLab.felulrol.LabKirajzol(pBalAlsoFelulrol);
                pt.PokKirajzol(pt, pBody);
            }
        }

        private void panel_jobb_felso_felulrol_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left) //le van-e nyomva az eger?
            {
                pt.jobbFelsoLab.felulrol.LabTorol(pJobbFelsoFelulrol);
                pt.jobbFelsoLab.oldalrol.LabTorol(pJobbFelsoOldalrol);

                pt.jobbFelsoLab.felulrol.lax = e.X;  //frissit
                pt.jobbFelsoLab.felulrol.lay = e.Y;
                
                pt.jobbFelsoLab.felulrol.Kiszamol_FelulNezet();

                pt.jobbFelsoLab.oldalrol.LabKirajzol(pJobbFelsoOldalrol);
                pt.jobbFelsoLab.felulrol.LabKirajzol(pJobbFelsoFelulrol);
                pt.PokKirajzol(pt, pBody);
            }
        }

        private void panel_jobb_felso_oldalrol_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left) //le van-e nyomva az eger?
            {
                pt.jobbFelsoLab.felulrol.LabTorol(pJobbFelsoFelulrol);
                pt.jobbFelsoLab.oldalrol.LabTorol(pJobbFelsoOldalrol);

                pt.jobbFelsoLab.oldalrol.lax = e.X;  //frissit
                pt.jobbFelsoLab.oldalrol.lay = e.Y;
                
                pt.jobbFelsoLab.oldalrol.Kiszamol_OldalNezet();

                pt.jobbFelsoLab.oldalrol.LabKirajzol(pJobbFelsoOldalrol);
                pt.jobbFelsoLab.felulrol.LabKirajzol(pJobbFelsoFelulrol);
                pt.PokKirajzol(pt, pBody);
            }
        }

        private void panel_jobb_kozepso_felulrol_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left) //le van-e nyomva az eger?
            {
                pt.jobbKozepsoLab.felulrol.LabTorol(pJobbKozepsoFelulrol);
                pt.jobbKozepsoLab.oldalrol.LabTorol(pJobbKozepsoOldalrol);

                pt.jobbKozepsoLab.felulrol.lax = e.X;  //frissit
                pt.jobbKozepsoLab.felulrol.lay = e.Y;
                
                pt.jobbKozepsoLab.felulrol.Kiszamol_FelulNezet();

                pt.jobbKozepsoLab.oldalrol.LabKirajzol(pJobbKozepsoOldalrol);
                pt.jobbKozepsoLab.felulrol.LabKirajzol(pJobbKozepsoFelulrol);
                pt.PokKirajzol(pt, pBody);
            }
        }

        private void panel_jobb_kozepso_oldalrol_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left) //le van-e nyomva az eger?
            {
                pt.jobbKozepsoLab.felulrol.LabTorol(pJobbKozepsoFelulrol);
                pt.jobbKozepsoLab.oldalrol.LabTorol(pJobbKozepsoOldalrol);

                pt.jobbKozepsoLab.oldalrol.lax = e.X;  //frissit
                pt.jobbKozepsoLab.oldalrol.lay = e.Y;
                
                pt.jobbKozepsoLab.oldalrol.Kiszamol_OldalNezet();

                pt.jobbKozepsoLab.oldalrol.LabKirajzol(pJobbKozepsoOldalrol);
                pt.jobbKozepsoLab.felulrol.LabKirajzol(pJobbKozepsoFelulrol);
                pt.PokKirajzol(pt, pBody);
            }
        }

        private void panel_jobb_also_felulrol_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left) //le van-e nyomva az eger?
            {
                pt.jobbAlsoLab.felulrol.LabTorol(pJobbAlsoFelulrol);
                pt.jobbAlsoLab.oldalrol.LabTorol(pJobbAlsoOldalrol);

                pt.jobbAlsoLab.felulrol.lax = e.X;  //frissit
                pt.jobbAlsoLab.felulrol.lay = e.Y;
                
                pt.jobbAlsoLab.felulrol.Kiszamol_FelulNezet();

                pt.jobbAlsoLab.oldalrol.LabKirajzol(pJobbAlsoOldalrol);
                pt.jobbAlsoLab.felulrol.LabKirajzol(pJobbAlsoFelulrol);
                pt.PokKirajzol(pt, pBody);
            }
        }

        private void panel_jobb_also_oldalrol_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left) //le van-e nyomva az eger?
            {
                pt.jobbAlsoLab.felulrol.LabTorol(pJobbAlsoFelulrol);
                pt.jobbAlsoLab.oldalrol.LabTorol(pJobbAlsoOldalrol);

                pt.jobbAlsoLab.oldalrol.lax = e.X;  //frissit
                pt.jobbAlsoLab.oldalrol.lay = e.Y;
                
                pt.jobbAlsoLab.oldalrol.Kiszamol_OldalNezet();

                pt.jobbAlsoLab.oldalrol.LabKirajzol(pJobbAlsoOldalrol);
                pt.jobbAlsoLab.felulrol.LabKirajzol(pJobbAlsoFelulrol);
                pt.PokKirajzol(pt, pBody);
            }
        }
        //Bal felso oldalnezetbol. Gombok
        private void button1_Click(object sender, EventArgs e)
        {
            pt.balFelsoLab.felulrol.LabTorol(pBalFelsoFelulrol);
            pt.balFelsoLab.oldalrol.LabTorol(pBalFelsoOldalrol);

            pt.balFelsoLab.oldalrol.lay -= 1;

            pt.balFelsoLab.oldalrol.Kiszamol_OldalNezet();

            pt.balFelsoLab.oldalrol.LabKirajzol(pBalFelsoOldalrol);
            pt.balFelsoLab.felulrol.LabKirajzol(pBalFelsoFelulrol);
            pt.PokKirajzol(pt, pBody);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            pt.balFelsoLab.felulrol.LabTorol(pBalFelsoFelulrol);
            pt.balFelsoLab.oldalrol.LabTorol(pBalFelsoOldalrol);

            pt.balFelsoLab.oldalrol.lay += 1;

            pt.balFelsoLab.oldalrol.Kiszamol_OldalNezet();

            pt.balFelsoLab.oldalrol.LabKirajzol(pBalFelsoOldalrol);
            pt.balFelsoLab.felulrol.LabKirajzol(pBalFelsoFelulrol);
            pt.PokKirajzol(pt, pBody);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            pt.balFelsoLab.felulrol.LabTorol(pBalFelsoFelulrol);
            pt.balFelsoLab.oldalrol.LabTorol(pBalFelsoOldalrol);

            pt.balFelsoLab.oldalrol.lax += 1;

            pt.balFelsoLab.oldalrol.Kiszamol_OldalNezet();

            pt.balFelsoLab.oldalrol.LabKirajzol(pBalFelsoOldalrol);
            pt.balFelsoLab.felulrol.LabKirajzol(pBalFelsoFelulrol);
            pt.PokKirajzol(pt, pBody);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            pt.balFelsoLab.felulrol.LabTorol(pBalFelsoFelulrol);
            pt.balFelsoLab.oldalrol.LabTorol(pBalFelsoOldalrol);

            pt.balFelsoLab.oldalrol.lax -= 1;

            pt.balFelsoLab.oldalrol.Kiszamol_OldalNezet();

            pt.balFelsoLab.oldalrol.LabKirajzol(pBalFelsoOldalrol);
            pt.balFelsoLab.felulrol.LabKirajzol(pBalFelsoFelulrol);
            pt.PokKirajzol(pt, pBody);
        }
        //Bal felso Felulnezetbol gombok...
        private void button8_Click(object sender, EventArgs e)
        {
            pt.balFelsoLab.felulrol.LabTorol(pBalFelsoFelulrol);
            pt.balFelsoLab.oldalrol.LabTorol(pBalFelsoOldalrol);

            pt.balFelsoLab.felulrol.lay = panel_bal_felso_felulrol.Height - pt.balFelsoLab.felulrol.lay +5;

            pt.balFelsoLab.felulrol.Kiszamol_FelulNezet();

            pt.balFelsoLab.oldalrol.LabKirajzol(pBalFelsoOldalrol);
            pt.balFelsoLab.felulrol.LabKirajzol(pBalFelsoFelulrol);
            pt.PokKirajzol(pt, pBody);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            pt.balFelsoLab.felulrol.LabTorol(pBalFelsoFelulrol);
            pt.balFelsoLab.oldalrol.LabTorol(pBalFelsoOldalrol);

            pt.balFelsoLab.felulrol.lax += 5;  //frissit
            pt.balFelsoLab.felulrol.lay = panel_bal_felso_felulrol.Height - pt.balFelsoLab.felulrol.lay;

            pt.balFelsoLab.felulrol.Kiszamol_FelulNezet();

            pt.balFelsoLab.oldalrol.LabKirajzol(pBalFelsoOldalrol);
            pt.balFelsoLab.felulrol.LabKirajzol(pBalFelsoFelulrol);
            pt.PokKirajzol(pt, pBody);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            pt.balFelsoLab.felulrol.LabTorol(pBalFelsoFelulrol);
            pt.balFelsoLab.oldalrol.LabTorol(pBalFelsoOldalrol);

            pt.balFelsoLab.felulrol.lax -= 5;  //frissit
            pt.balFelsoLab.felulrol.lay = panel_bal_felso_felulrol.Height - pt.balFelsoLab.felulrol.lay;

            pt.balFelsoLab.felulrol.Kiszamol_FelulNezet();

            pt.balFelsoLab.oldalrol.LabKirajzol(pBalFelsoOldalrol);
            pt.balFelsoLab.felulrol.LabKirajzol(pBalFelsoFelulrol);
            pt.PokKirajzol(pt, pBody);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            pt.balFelsoLab.felulrol.LabTorol(pBalFelsoFelulrol);
            pt.balFelsoLab.oldalrol.LabTorol(pBalFelsoOldalrol);

            pt.balFelsoLab.felulrol.lay = panel_bal_felso_felulrol.Height - pt.balFelsoLab.felulrol.lay - 5;

            pt.balFelsoLab.felulrol.Kiszamol_FelulNezet();

            pt.balFelsoLab.oldalrol.LabKirajzol(pBalFelsoOldalrol);
            pt.balFelsoLab.felulrol.LabKirajzol(pBalFelsoFelulrol);
            pt.PokKirajzol(pt, pBody);
        }

        private void button12_Click(object sender, EventArgs e)
        {
            pt.balKozepsoLab.felulrol.LabTorol(pBalKozepsoFelulrol);
            pt.balKozepsoLab.oldalrol.LabTorol(pBalKozepsoOldalrol);

            pt.balKozepsoLab.oldalrol.lay -= 1;  //frissit

            pt.balKozepsoLab.oldalrol.Kiszamol_OldalNezet();

            pt.balKozepsoLab.oldalrol.LabKirajzol(pBalKozepsoOldalrol);
            pt.balKozepsoLab.felulrol.LabKirajzol(pBalKozepsoFelulrol);
            pt.PokKirajzol(pt, pBody);
        }

        private void button11_Click(object sender, EventArgs e)
        {
            pt.balKozepsoLab.felulrol.LabTorol(pBalKozepsoFelulrol);
            pt.balKozepsoLab.oldalrol.LabTorol(pBalKozepsoOldalrol);

            pt.balKozepsoLab.oldalrol.lay += 1;  //frissit

            pt.balKozepsoLab.oldalrol.Kiszamol_OldalNezet();

            pt.balKozepsoLab.oldalrol.LabKirajzol(pBalKozepsoOldalrol);
            pt.balKozepsoLab.felulrol.LabKirajzol(pBalKozepsoFelulrol);
            pt.PokKirajzol(pt, pBody);
        }

        private void button9_Click(object sender, EventArgs e)
        {
            pt.balKozepsoLab.felulrol.LabTorol(pBalKozepsoFelulrol);
            pt.balKozepsoLab.oldalrol.LabTorol(pBalKozepsoOldalrol);

            pt.balKozepsoLab.oldalrol.lax -= 1;

            pt.balKozepsoLab.oldalrol.Kiszamol_OldalNezet();

            pt.balKozepsoLab.oldalrol.LabKirajzol(pBalKozepsoOldalrol);
            pt.balKozepsoLab.felulrol.LabKirajzol(pBalKozepsoFelulrol);
            pt.PokKirajzol(pt, pBody);
        }

        private void button10_Click(object sender, EventArgs e)
        {
            pt.balKozepsoLab.felulrol.LabTorol(pBalKozepsoFelulrol);
            pt.balKozepsoLab.oldalrol.LabTorol(pBalKozepsoOldalrol);

            pt.balKozepsoLab.oldalrol.lax += 1;

            pt.balKozepsoLab.oldalrol.Kiszamol_OldalNezet();

            pt.balKozepsoLab.oldalrol.LabKirajzol(pBalKozepsoOldalrol);
            pt.balKozepsoLab.felulrol.LabKirajzol(pBalKozepsoFelulrol);
            pt.PokKirajzol(pt, pBody);
        }

        private void button16_Click(object sender, EventArgs e)
        {
            pt.balKozepsoLab.felulrol.LabTorol(pBalKozepsoFelulrol);
            pt.balKozepsoLab.oldalrol.LabTorol(pBalKozepsoOldalrol);


            pt.balKozepsoLab.felulrol.lay = panel_bal_kozepso_felulrol.Height - pt.balKozepsoLab.felulrol.lay + 5;

            pt.balKozepsoLab.felulrol.Kiszamol_FelulNezet();

            pt.balKozepsoLab.oldalrol.LabKirajzol(pBalKozepsoOldalrol);
            pt.balKozepsoLab.felulrol.LabKirajzol(pBalKozepsoFelulrol);
            pt.PokKirajzol(pt, pBody);
        }

        private void button15_Click(object sender, EventArgs e)
        {
            pt.balKozepsoLab.felulrol.LabTorol(pBalKozepsoFelulrol);
            pt.balKozepsoLab.oldalrol.LabTorol(pBalKozepsoOldalrol);


            pt.balKozepsoLab.felulrol.lay = panel_bal_kozepso_felulrol.Height - pt.balKozepsoLab.felulrol.lay - 5;

            pt.balKozepsoLab.felulrol.Kiszamol_FelulNezet();

            pt.balKozepsoLab.oldalrol.LabKirajzol(pBalKozepsoOldalrol);
            pt.balKozepsoLab.felulrol.LabKirajzol(pBalKozepsoFelulrol);
            pt.PokKirajzol(pt, pBody);
        }

        private void button14_Click(object sender, EventArgs e)
        {
            pt.balKozepsoLab.felulrol.LabTorol(pBalKozepsoFelulrol);
            pt.balKozepsoLab.oldalrol.LabTorol(pBalKozepsoOldalrol);

            pt.balKozepsoLab.felulrol.lax += 5;  //frissit
            pt.balKozepsoLab.felulrol.lay = panel_bal_kozepso_felulrol.Height - pt.balKozepsoLab.felulrol.lay;

            pt.balKozepsoLab.felulrol.Kiszamol_FelulNezet();

            pt.balKozepsoLab.oldalrol.LabKirajzol(pBalKozepsoOldalrol);
            pt.balKozepsoLab.felulrol.LabKirajzol(pBalKozepsoFelulrol);
            pt.PokKirajzol(pt, pBody);
        }

        private void button13_Click(object sender, EventArgs e)
        {
            pt.balKozepsoLab.felulrol.LabTorol(pBalKozepsoFelulrol);
            pt.balKozepsoLab.oldalrol.LabTorol(pBalKozepsoOldalrol);

            pt.balKozepsoLab.felulrol.lax -= 5;  //frissit
            pt.balKozepsoLab.felulrol.lay = panel_bal_kozepso_felulrol.Height - pt.balKozepsoLab.felulrol.lay;

            pt.balKozepsoLab.felulrol.Kiszamol_FelulNezet();

            pt.balKozepsoLab.oldalrol.LabKirajzol(pBalKozepsoOldalrol);
            pt.balKozepsoLab.felulrol.LabKirajzol(pBalKozepsoFelulrol);
            pt.PokKirajzol(pt, pBody);
        }
        //Bal also Labhoz tartozo gombok...
        private void button20_Click(object sender, EventArgs e)
        {
            pt.balAlsoLab.felulrol.LabTorol(pBalAlsoFelulrol);
            pt.balAlsoLab.oldalrol.LabTorol(pBalAlsoOldalrol);

            pt.balAlsoLab.oldalrol.lay -= 1;

            pt.balAlsoLab.oldalrol.Kiszamol_OldalNezet();

            pt.balAlsoLab.oldalrol.LabKirajzol(pBalAlsoOldalrol);
            pt.balAlsoLab.felulrol.LabKirajzol(pBalAlsoFelulrol);
            pt.PokKirajzol(pt, pBody);
        }

        private void button19_Click(object sender, EventArgs e)
        {
            pt.balAlsoLab.felulrol.LabTorol(pBalAlsoFelulrol);
            pt.balAlsoLab.oldalrol.LabTorol(pBalAlsoOldalrol);

            pt.balAlsoLab.oldalrol.lay += 1;

            pt.balAlsoLab.oldalrol.Kiszamol_OldalNezet();

            pt.balAlsoLab.oldalrol.LabKirajzol(pBalAlsoOldalrol);
            pt.balAlsoLab.felulrol.LabKirajzol(pBalAlsoFelulrol);
            pt.PokKirajzol(pt, pBody);
        }

        private void button18_Click(object sender, EventArgs e)
        {
            pt.balAlsoLab.felulrol.LabTorol(pBalAlsoFelulrol);
            pt.balAlsoLab.oldalrol.LabTorol(pBalAlsoOldalrol);

            pt.balAlsoLab.oldalrol.lax += 1;  //frissit

            pt.balAlsoLab.oldalrol.Kiszamol_OldalNezet();

            pt.balAlsoLab.oldalrol.LabKirajzol(pBalAlsoOldalrol);
            pt.balAlsoLab.felulrol.LabKirajzol(pBalAlsoFelulrol);
            pt.PokKirajzol(pt, pBody);
        }

        private void button17_Click(object sender, EventArgs e)
        {
            pt.balAlsoLab.felulrol.LabTorol(pBalAlsoFelulrol);
            pt.balAlsoLab.oldalrol.LabTorol(pBalAlsoOldalrol);

            pt.balAlsoLab.oldalrol.lax -= 1;  //frissit

            pt.balAlsoLab.oldalrol.Kiszamol_OldalNezet();

            pt.balAlsoLab.oldalrol.LabKirajzol(pBalAlsoOldalrol);
            pt.balAlsoLab.felulrol.LabKirajzol(pBalAlsoFelulrol);
            pt.PokKirajzol(pt, pBody);
        }
        //Bal also Lab Felulnezetben
        private void button24_Click(object sender, EventArgs e)
        {
            pt.balAlsoLab.felulrol.LabTorol(pBalAlsoFelulrol);
            pt.balAlsoLab.oldalrol.LabTorol(pBalAlsoOldalrol);

            pt.balAlsoLab.felulrol.lay = panel_bal_also_felulrol.Height - pt.balAlsoLab.felulrol.lay + 5;

            pt.balAlsoLab.felulrol.Kiszamol_FelulNezet();

            pt.balAlsoLab.oldalrol.LabKirajzol(pBalAlsoOldalrol);
            pt.balAlsoLab.felulrol.LabKirajzol(pBalAlsoFelulrol);
            pt.PokKirajzol(pt, pBody);
        }

        private void button23_Click(object sender, EventArgs e)
        {
            pt.balAlsoLab.felulrol.LabTorol(pBalAlsoFelulrol);
            pt.balAlsoLab.oldalrol.LabTorol(pBalAlsoOldalrol);

            pt.balAlsoLab.felulrol.lay = panel_bal_also_felulrol.Height - pt.balAlsoLab.felulrol.lay - 5;

            pt.balAlsoLab.felulrol.Kiszamol_FelulNezet();

            pt.balAlsoLab.oldalrol.LabKirajzol(pBalAlsoOldalrol);
            pt.balAlsoLab.felulrol.LabKirajzol(pBalAlsoFelulrol);
            pt.PokKirajzol(pt, pBody);
        }

        private void button22_Click(object sender, EventArgs e)
        {
            pt.balAlsoLab.felulrol.LabTorol(pBalAlsoFelulrol);
            pt.balAlsoLab.oldalrol.LabTorol(pBalAlsoOldalrol);

            pt.balAlsoLab.felulrol.lax += 5;  //frissit
            pt.balAlsoLab.felulrol.lay = panel_bal_also_felulrol.Height - pt.balAlsoLab.felulrol.lay;

            pt.balAlsoLab.felulrol.Kiszamol_FelulNezet();

            pt.balAlsoLab.oldalrol.LabKirajzol(pBalAlsoOldalrol);
            pt.balAlsoLab.felulrol.LabKirajzol(pBalAlsoFelulrol);
            pt.PokKirajzol(pt, pBody);
        }

        private void button21_Click(object sender, EventArgs e)
        {
            pt.balAlsoLab.felulrol.LabTorol(pBalAlsoFelulrol);
            pt.balAlsoLab.oldalrol.LabTorol(pBalAlsoOldalrol);

            pt.balAlsoLab.felulrol.lax -= 5;  //frissit
            pt.balAlsoLab.felulrol.lay = panel_bal_also_felulrol.Height - pt.balAlsoLab.felulrol.lay;

            pt.balAlsoLab.felulrol.Kiszamol_FelulNezet();

            pt.balAlsoLab.oldalrol.LabKirajzol(pBalAlsoOldalrol);
            pt.balAlsoLab.felulrol.LabKirajzol(pBalAlsoFelulrol);
            pt.PokKirajzol(pt, pBody);
        }

        //JObb felso Lab Gombjai
        private void button32_Click(object sender, EventArgs e)
        {
            pt.jobbFelsoLab.felulrol.LabTorol(pJobbFelsoFelulrol);
            pt.jobbFelsoLab.oldalrol.LabTorol(pJobbFelsoOldalrol);

            pt.jobbFelsoLab.oldalrol.lay -= 1;

            pt.jobbFelsoLab.oldalrol.Kiszamol_OldalNezet();

            pt.jobbFelsoLab.oldalrol.LabKirajzol(pJobbFelsoOldalrol);
            pt.jobbFelsoLab.felulrol.LabKirajzol(pJobbFelsoFelulrol);
            pt.PokKirajzol(pt, pBody);
        }

        private void button31_Click(object sender, EventArgs e)
        {
            pt.jobbFelsoLab.felulrol.LabTorol(pJobbFelsoFelulrol);
            pt.jobbFelsoLab.oldalrol.LabTorol(pJobbFelsoOldalrol);

            pt.jobbFelsoLab.oldalrol.lay += 1;

            pt.jobbFelsoLab.oldalrol.Kiszamol_OldalNezet();

            pt.jobbFelsoLab.oldalrol.LabKirajzol(pJobbFelsoOldalrol);
            pt.jobbFelsoLab.felulrol.LabKirajzol(pJobbFelsoFelulrol);
            pt.PokKirajzol(pt, pBody);
        }

        private void button30_Click(object sender, EventArgs e)
        {
            pt.jobbFelsoLab.felulrol.LabTorol(pJobbFelsoFelulrol);
            pt.jobbFelsoLab.oldalrol.LabTorol(pJobbFelsoOldalrol);

            pt.jobbFelsoLab.oldalrol.lax += 1;  //frissit

            pt.jobbFelsoLab.oldalrol.Kiszamol_OldalNezet();

            pt.jobbFelsoLab.oldalrol.LabKirajzol(pJobbFelsoOldalrol);
            pt.jobbFelsoLab.felulrol.LabKirajzol(pJobbFelsoFelulrol);
            pt.PokKirajzol(pt, pBody);
        }

        private void button29_Click(object sender, EventArgs e)
        {
            pt.jobbFelsoLab.felulrol.LabTorol(pJobbFelsoFelulrol);
            pt.jobbFelsoLab.oldalrol.LabTorol(pJobbFelsoOldalrol);

            pt.jobbFelsoLab.oldalrol.lax -= 1;  //frissit

            pt.jobbFelsoLab.oldalrol.Kiszamol_OldalNezet();

            pt.jobbFelsoLab.oldalrol.LabKirajzol(pJobbFelsoOldalrol);
            pt.jobbFelsoLab.felulrol.LabKirajzol(pJobbFelsoFelulrol);
            pt.PokKirajzol(pt, pBody);
        }

        private void button28_Click(object sender, EventArgs e)
        {
            pt.jobbFelsoLab.felulrol.LabTorol(pJobbFelsoFelulrol);
            pt.jobbFelsoLab.oldalrol.LabTorol(pJobbFelsoOldalrol);

            pt.jobbFelsoLab.felulrol.lay -= 5;

            pt.jobbFelsoLab.felulrol.Kiszamol_FelulNezet();

            pt.jobbFelsoLab.oldalrol.LabKirajzol(pJobbFelsoOldalrol);
            pt.jobbFelsoLab.felulrol.LabKirajzol(pJobbFelsoFelulrol);
            pt.PokKirajzol(pt, pBody);
        }

        private void button27_Click(object sender, EventArgs e)
        {
            pt.jobbFelsoLab.felulrol.LabTorol(pJobbFelsoFelulrol);
            pt.jobbFelsoLab.oldalrol.LabTorol(pJobbFelsoOldalrol);

            pt.jobbFelsoLab.felulrol.lay += 5;

            pt.jobbFelsoLab.felulrol.Kiszamol_FelulNezet();

            pt.jobbFelsoLab.oldalrol.LabKirajzol(pJobbFelsoOldalrol);
            pt.jobbFelsoLab.felulrol.LabKirajzol(pJobbFelsoFelulrol);
            pt.PokKirajzol(pt, pBody);
        }

        private void button26_Click(object sender, EventArgs e)
        {
            pt.jobbFelsoLab.felulrol.LabTorol(pJobbFelsoFelulrol);
            pt.jobbFelsoLab.oldalrol.LabTorol(pJobbFelsoOldalrol);

            pt.jobbFelsoLab.felulrol.lax += 5;  //frissit

            pt.jobbFelsoLab.felulrol.Kiszamol_FelulNezet();

            pt.jobbFelsoLab.oldalrol.LabKirajzol(pJobbFelsoOldalrol);
            pt.jobbFelsoLab.felulrol.LabKirajzol(pJobbFelsoFelulrol);
            pt.PokKirajzol(pt, pBody);
        }

        private void button25_Click(object sender, EventArgs e)
        {
            pt.jobbFelsoLab.felulrol.LabTorol(pJobbFelsoFelulrol);
            pt.jobbFelsoLab.oldalrol.LabTorol(pJobbFelsoOldalrol);

            pt.jobbFelsoLab.felulrol.lax -= 5;  //frissit

            pt.jobbFelsoLab.felulrol.Kiszamol_FelulNezet();

            pt.jobbFelsoLab.oldalrol.LabKirajzol(pJobbFelsoOldalrol);
            pt.jobbFelsoLab.felulrol.LabKirajzol(pJobbFelsoFelulrol);
            pt.PokKirajzol(pt, pBody);
        }

        //Jobb Kozepso Lab Gombjai
        private void button40_Click(object sender, EventArgs e)
        {
            pt.jobbKozepsoLab.felulrol.LabTorol(pJobbKozepsoFelulrol);
            pt.jobbKozepsoLab.oldalrol.LabTorol(pJobbKozepsoOldalrol);

            pt.jobbKozepsoLab.oldalrol.lay -= 1;

            pt.jobbKozepsoLab.oldalrol.Kiszamol_OldalNezet();

            pt.jobbKozepsoLab.oldalrol.LabKirajzol(pJobbKozepsoOldalrol);
            pt.jobbKozepsoLab.felulrol.LabKirajzol(pJobbKozepsoFelulrol);
            pt.PokKirajzol(pt, pBody);
        }

        private void button39_Click(object sender, EventArgs e)
        {
            pt.jobbKozepsoLab.felulrol.LabTorol(pJobbKozepsoFelulrol);
            pt.jobbKozepsoLab.oldalrol.LabTorol(pJobbKozepsoOldalrol);

            pt.jobbKozepsoLab.oldalrol.lay += 1;

            pt.jobbKozepsoLab.oldalrol.Kiszamol_OldalNezet();

            pt.jobbKozepsoLab.oldalrol.LabKirajzol(pJobbKozepsoOldalrol);
            pt.jobbKozepsoLab.felulrol.LabKirajzol(pJobbKozepsoFelulrol);
            pt.PokKirajzol(pt, pBody);
        }

        private void button38_Click(object sender, EventArgs e)
        {
            pt.jobbKozepsoLab.felulrol.LabTorol(pJobbKozepsoFelulrol);
            pt.jobbKozepsoLab.oldalrol.LabTorol(pJobbKozepsoOldalrol);

            pt.jobbKozepsoLab.oldalrol.lax += 1;  //frissit

            pt.jobbKozepsoLab.oldalrol.Kiszamol_OldalNezet();

            pt.jobbKozepsoLab.oldalrol.LabKirajzol(pJobbKozepsoOldalrol);
            pt.jobbKozepsoLab.felulrol.LabKirajzol(pJobbKozepsoFelulrol);
            pt.PokKirajzol(pt, pBody);
        }

        private void button37_Click(object sender, EventArgs e)
        {
            pt.jobbKozepsoLab.felulrol.LabTorol(pJobbKozepsoFelulrol);
            pt.jobbKozepsoLab.oldalrol.LabTorol(pJobbKozepsoOldalrol);

            pt.jobbKozepsoLab.oldalrol.lax -= 1;  //frissit

            pt.jobbKozepsoLab.oldalrol.Kiszamol_OldalNezet();

            pt.jobbKozepsoLab.oldalrol.LabKirajzol(pJobbKozepsoOldalrol);
            pt.jobbKozepsoLab.felulrol.LabKirajzol(pJobbKozepsoFelulrol);
            pt.PokKirajzol(pt, pBody);
        }

        private void button36_Click(object sender, EventArgs e)
        {
            pt.jobbKozepsoLab.felulrol.LabTorol(pJobbKozepsoFelulrol);
            pt.jobbKozepsoLab.oldalrol.LabTorol(pJobbKozepsoOldalrol);

            pt.jobbKozepsoLab.felulrol.lay -= 5;

            pt.jobbKozepsoLab.felulrol.Kiszamol_FelulNezet();

            pt.jobbKozepsoLab.oldalrol.LabKirajzol(pJobbKozepsoOldalrol);
            pt.jobbKozepsoLab.felulrol.LabKirajzol(pJobbKozepsoFelulrol);
            pt.PokKirajzol(pt, pBody);
        }

        private void button35_Click(object sender, EventArgs e)
        {
            pt.jobbKozepsoLab.felulrol.LabTorol(pJobbKozepsoFelulrol);
            pt.jobbKozepsoLab.oldalrol.LabTorol(pJobbKozepsoOldalrol);

            pt.jobbKozepsoLab.felulrol.lay += 5;

            pt.jobbKozepsoLab.felulrol.Kiszamol_FelulNezet();

            pt.jobbKozepsoLab.oldalrol.LabKirajzol(pJobbKozepsoOldalrol);
            pt.jobbKozepsoLab.felulrol.LabKirajzol(pJobbKozepsoFelulrol);
            pt.PokKirajzol(pt, pBody);
        }

        private void button34_Click(object sender, EventArgs e)
        {
            pt.jobbKozepsoLab.felulrol.LabTorol(pJobbKozepsoFelulrol);
            pt.jobbKozepsoLab.oldalrol.LabTorol(pJobbKozepsoOldalrol);

            pt.jobbKozepsoLab.felulrol.lax += 5;  //frissit

            pt.jobbKozepsoLab.felulrol.Kiszamol_FelulNezet();

            pt.jobbKozepsoLab.oldalrol.LabKirajzol(pJobbKozepsoOldalrol);
            pt.jobbKozepsoLab.felulrol.LabKirajzol(pJobbKozepsoFelulrol);
            pt.PokKirajzol(pt, pBody);
        }

        private void button33_Click(object sender, EventArgs e)
        {
            pt.jobbKozepsoLab.felulrol.LabTorol(pJobbKozepsoFelulrol);
            pt.jobbKozepsoLab.oldalrol.LabTorol(pJobbKozepsoOldalrol);

            pt.jobbKozepsoLab.felulrol.lax -= 5;  //frissit

            pt.jobbKozepsoLab.felulrol.Kiszamol_FelulNezet();

            pt.jobbKozepsoLab.oldalrol.LabKirajzol(pJobbKozepsoOldalrol);
            pt.jobbKozepsoLab.felulrol.LabKirajzol(pJobbKozepsoFelulrol);
            pt.PokKirajzol(pt, pBody);
        }
        //Jobb also Lab Gombjai
        private void button48_Click(object sender, EventArgs e)
        {
            pt.jobbAlsoLab.felulrol.LabTorol(pJobbAlsoFelulrol);
            pt.jobbAlsoLab.oldalrol.LabTorol(pJobbAlsoOldalrol);

            pt.jobbAlsoLab.oldalrol.lay -= 1;

            pt.jobbAlsoLab.oldalrol.Kiszamol_OldalNezet();

            pt.jobbAlsoLab.oldalrol.LabKirajzol(pJobbAlsoOldalrol);
            pt.jobbAlsoLab.felulrol.LabKirajzol(pJobbAlsoFelulrol);
            pt.PokKirajzol(pt, pBody);
        }

        private void button47_Click(object sender, EventArgs e)
        {
            pt.jobbAlsoLab.felulrol.LabTorol(pJobbAlsoFelulrol);
            pt.jobbAlsoLab.oldalrol.LabTorol(pJobbAlsoOldalrol);


            pt.jobbAlsoLab.oldalrol.lay += 1;

            pt.jobbAlsoLab.oldalrol.Kiszamol_OldalNezet();

            pt.jobbAlsoLab.oldalrol.LabKirajzol(pJobbAlsoOldalrol);
            pt.jobbAlsoLab.felulrol.LabKirajzol(pJobbAlsoFelulrol);
            pt.PokKirajzol(pt, pBody);
        }

        private void button46_Click(object sender, EventArgs e)
        {
            pt.jobbAlsoLab.felulrol.LabTorol(pJobbAlsoFelulrol);
            pt.jobbAlsoLab.oldalrol.LabTorol(pJobbAlsoOldalrol);

            pt.jobbAlsoLab.oldalrol.lax += 1;  //frissit

            pt.jobbAlsoLab.oldalrol.Kiszamol_OldalNezet();

            pt.jobbAlsoLab.oldalrol.LabKirajzol(pJobbAlsoOldalrol);
            pt.jobbAlsoLab.felulrol.LabKirajzol(pJobbAlsoFelulrol);
            pt.PokKirajzol(pt, pBody);
        }

        private void button45_Click(object sender, EventArgs e)
        {
            pt.jobbAlsoLab.felulrol.LabTorol(pJobbAlsoFelulrol);
            pt.jobbAlsoLab.oldalrol.LabTorol(pJobbAlsoOldalrol);

            pt.jobbAlsoLab.oldalrol.lax -= 1;  //frissit

            pt.jobbAlsoLab.oldalrol.Kiszamol_OldalNezet();

            pt.jobbAlsoLab.oldalrol.LabKirajzol(pJobbAlsoOldalrol);
            pt.jobbAlsoLab.felulrol.LabKirajzol(pJobbAlsoFelulrol);
            pt.PokKirajzol(pt, pBody);
        }

        private void button44_Click(object sender, EventArgs e)
        {
            pt.jobbAlsoLab.felulrol.LabTorol(pJobbAlsoFelulrol);
            pt.jobbAlsoLab.oldalrol.LabTorol(pJobbAlsoOldalrol);

            pt.jobbAlsoLab.felulrol.lay -= 5;

            pt.jobbAlsoLab.felulrol.Kiszamol_FelulNezet();

            pt.jobbAlsoLab.oldalrol.LabKirajzol(pJobbAlsoOldalrol);
            pt.jobbAlsoLab.felulrol.LabKirajzol(pJobbAlsoFelulrol);
            pt.PokKirajzol(pt, pBody);
        }

        private void button43_Click(object sender, EventArgs e)
        {
            pt.jobbAlsoLab.felulrol.LabTorol(pJobbAlsoFelulrol);
            pt.jobbAlsoLab.oldalrol.LabTorol(pJobbAlsoOldalrol);

            pt.jobbAlsoLab.felulrol.lay += 5;

            pt.jobbAlsoLab.felulrol.Kiszamol_FelulNezet();

            pt.jobbAlsoLab.oldalrol.LabKirajzol(pJobbAlsoOldalrol);
            pt.jobbAlsoLab.felulrol.LabKirajzol(pJobbAlsoFelulrol);
            pt.PokKirajzol(pt, pBody);
        }

        private void button42_Click(object sender, EventArgs e)
        {
            pt.jobbAlsoLab.felulrol.LabTorol(pJobbAlsoFelulrol);
            pt.jobbAlsoLab.oldalrol.LabTorol(pJobbAlsoOldalrol);

            pt.jobbAlsoLab.felulrol.lax += 5;  //frissit

            pt.jobbAlsoLab.felulrol.Kiszamol_FelulNezet();

            pt.jobbAlsoLab.oldalrol.LabKirajzol(pJobbAlsoOldalrol);
            pt.jobbAlsoLab.felulrol.LabKirajzol(pJobbAlsoFelulrol);
            pt.PokKirajzol(pt, pBody);
        }

        private void button41_Click(object sender, EventArgs e)
        {
            pt.jobbAlsoLab.felulrol.LabTorol(pJobbAlsoFelulrol);
            pt.jobbAlsoLab.oldalrol.LabTorol(pJobbAlsoOldalrol);

            pt.jobbAlsoLab.felulrol.lax -= 5;  //frissit

            pt.jobbAlsoLab.felulrol.Kiszamol_FelulNezet();

            pt.jobbAlsoLab.oldalrol.LabKirajzol(pJobbAlsoOldalrol);
            pt.jobbAlsoLab.felulrol.LabKirajzol(pJobbAlsoFelulrol);
            pt.PokKirajzol(pt, pBody);
        }

        private void button53_Click(object sender, EventArgs e)
        {
            button1_Click(sender, e);
            button12_Click(sender, e);
            button20_Click(sender, e);
            button32_Click(sender, e);
            button40_Click(sender, e);
            button48_Click(sender, e);
        }

        private void button52_Click(object sender, EventArgs e)
        {
            button2_Click(sender, e);
            button11_Click(sender, e);
            button19_Click(sender, e);
            button31_Click(sender, e);
            button39_Click(sender, e);
            button47_Click(sender, e);
        }

        private void button50_Click(object sender, EventArgs e)
        {
            button3_Click(sender, e);
            button10_Click(sender, e);
            button18_Click(sender, e);
            button30_Click(sender, e);
            button38_Click(sender, e);
            button46_Click(sender, e);
        }

        private void button51_Click(object sender, EventArgs e)
        {
            button4_Click(sender, e);
            button9_Click(sender, e);
            button17_Click(sender, e);
            button29_Click(sender, e);
            button37_Click(sender, e);
            button45_Click(sender, e);
        }

        private void button55_Click(object sender, EventArgs e)
        {
            button7_Click(sender, e);
            button15_Click(sender, e);
            button23_Click(sender, e);
            button27_Click(sender, e);
            button35_Click(sender, e);
            button43_Click(sender, e);
        }

        private void button54_Click(object sender, EventArgs e)
        {
            button8_Click(sender, e);
            button16_Click(sender, e);
            button24_Click(sender, e);
            button28_Click(sender, e);
            button36_Click(sender, e);
            button44_Click(sender, e);
        }

        public void Set_Value_Felulrol(Nezet n,Graphics hova, Graphics hova_a_parjat, int x, int y)
        {
            n.LabTorol(hova);
            n.parja.LabTorol(hova_a_parjat);

            n.lax = x;  //frissit
            n.lay = y;

            n.Kiszamol_FelulNezet();

            n.parja.LabKirajzol(hova_a_parjat);
            n.LabKirajzol(hova);
            pt.PokKirajzol(pt, pBody);
        }

        private void b_Set_Value_Click(object sender, EventArgs e)
        {
            try
            {
                int x = Convert.ToInt32(tb_X.Text);
                int y = Convert.ToInt32(tb_Y.Text);
                y += 75;
                Set_Value_Felulrol(pt.balFelsoLab.felulrol, pBalFelsoFelulrol, pBalFelsoOldalrol, 150-x, y);
                Set_Value_Felulrol(pt.balKozepsoLab.felulrol, pBalKozepsoFelulrol, pBalKozepsoOldalrol, 150-x, y);
                Set_Value_Felulrol(pt.balAlsoLab.felulrol, pBalAlsoFelulrol, pBalAlsoOldalrol, 150-x, y);
                Set_Value_Felulrol(pt.jobbFelsoLab.felulrol, pJobbFelsoFelulrol, pJobbFelsoOldalrol, x, y);
                Set_Value_Felulrol(pt.jobbKozepsoLab.felulrol, pJobbKozepsoFelulrol, pJobbKozepsoOldalrol, x, y);
                Set_Value_Felulrol(pt.jobbAlsoLab.felulrol, pJobbAlsoFelulrol, pJobbAlsoOldalrol, x, y);

            }
            catch
            {
                tb_X.Text = "";
                tb_Y.Text = "";
            }
        }

        private int pwm(double val)
        {
            return (int)(((val + 90)* 255 ) / 180);
        }

        private void b_eredmenyek_Click(object sender, EventArgs e)
        {
            tb_eredmenyek.Text+= "\r\n"+
                                 pwm(pt.balFelsoLab.felulrol.teta3szog) + " " +
                                 pwm(pt.balFelsoLab.oldalrol.teta2szog) + " " +
                                 pwm(pt.balFelsoLab.oldalrol.teta1szog) + " " +
                                 pwm(pt.balKozepsoLab.felulrol.teta3szog) + " " +
                                 pwm(pt.balKozepsoLab.oldalrol.teta2szog) + " " +
                                 pwm(pt.balKozepsoLab.oldalrol.teta1szog) + " " +
                                 pwm(pt.balAlsoLab.felulrol.teta3szog) + " " +
                                 pwm(pt.balAlsoLab.oldalrol.teta2szog) + " " +
                                 pwm(pt.balAlsoLab.oldalrol.teta1szog) + " " +
                                 pwm(pt.jobbFelsoLab.felulrol.teta3szog) + " " +
                                 pwm(pt.jobbFelsoLab.oldalrol.teta2szog) + " " +
                                 pwm(pt.jobbFelsoLab.oldalrol.teta1szog) + " " +
                                 pwm(pt.jobbKozepsoLab.felulrol.teta3szog) + " " +
                                 pwm(pt.jobbKozepsoLab.oldalrol.teta2szog) + " " +
                                 pwm(pt.jobbKozepsoLab.oldalrol.teta1szog) + " " +
                                 pwm(pt.jobbAlsoLab.felulrol.teta3szog) + " " +
                                 pwm(pt.jobbAlsoLab.oldalrol.teta2szog) + " " +
                                 pwm(pt.jobbAlsoLab.oldalrol.teta1szog);
            tb_eredmenyek.SelectionStart = tb_eredmenyek.Text.Length;
            tb_eredmenyek.ScrollToCaret();
            tb_eredmenyek.Refresh();

        }

        private void b_torol_Click(object sender, EventArgs e)
        {
            if (tb_eredmenyek.Text.Length > 0)
            {
                DialogResult result = MessageBox.Show("Are you sure, you want to delete the results?", "TextBox Not Empty", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == System.Windows.Forms.DialogResult.Yes)
                {                    

                    tb_eredmenyek.Text = "";

                }
                
            }
        }

        private void b_mentes_Click(object sender, EventArgs e)
        {
            // Create new SaveFileDialog object
            SaveFileDialog DialogSave = new SaveFileDialog();

            // Default file extension
            DialogSave.DefaultExt = "txt";

            // Available file extensions
            DialogSave.Filter = "Text file (*.txt)|*.txt|XML file (*.xml)|*.xml|All files (*.*)|*.*";

            // Adds a extension if the user does not
            DialogSave.AddExtension = true;

            // Restores the selected directory, next time
            DialogSave.RestoreDirectory = true;

            // Dialog title
            DialogSave.Title = "Where do you want to save the file?";

            // Startup directory
            DialogSave.InitialDirectory = @"C:/";

            // Show the dialog and process the result
            if (DialogSave.ShowDialog() == DialogResult.OK)
            {
                MessageBox.Show("You selected the file: " + DialogSave.FileName);
                // create a writer and open the file
                TextWriter tw = new StreamWriter(DialogSave.FileName);

                // write a line of text to the file
                tw.WriteLine(tb_eredmenyek.Text);

                // close the stream
                tw.Close();
                MessageBox.Show("File saved succesfull. :D");

            }
            else
            {
                MessageBox.Show("You hit cancel or closed the dialog.");
            }

            DialogSave.Dispose();
            DialogSave = null;
        }

        private void b_connect_Click(object sender, EventArgs e)
        {
            if (connected == false)
            {
                this.Size = new Size(1300, 700);
                this.MinimumSize = new Size(1300, 700);
                this.MaximumSize = new Size(1300, 700);
                b_connect.Text = "Hide";
                connected = true;
            }
            else
            {
                this.Size = new Size(1000, 700);
                this.MinimumSize = new Size(1000, 700);
                this.MaximumSize = new Size(1000, 700);
                b_connect.Text = "Show";
                connected = false;
            }

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Size = new Size(1000, 700);
            this.MinimumSize = new Size(1000, 700);
            this.MaximumSize = new Size(1000, 700);
            b_connect.Text = "Show";
        }

        private void b_connect_wifly_Click(object sender, EventArgs e)
        {
            WiFly_address = new TcpClient("169.254.1.1", 2000);
            WiFly_stream = WiFly_address.GetStream();

            WiFly_recv = new recv_thread();
            WiFly_recv.ns = WiFly_stream;
            WiFly_recv.WiFly_address = WiFly_address;
            WiFly_recv.form = this;
            WiFly_thread = new Thread(new ThreadStart(WiFly_recv.recv));
            WiFly_thread.Start();
            b_Send.Enabled = true;
        }

        private void b_Send_Click(object sender, EventArgs e)
        {
            WiFly_stream.Write(Encoding.ASCII.GetBytes(tb_message_tosend.Text), 0, tb_message_tosend.Text.Length);
            WiFly_stream.Flush();
            tb_message_sent.Text += tb_message_tosend.Text + "\r\n";
            tb_message_tosend.Text = "";
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                WiFly_stream.Close();
                WiFly_thread.Abort();
            }
            catch
            {
 
            }
        }
    }
}
