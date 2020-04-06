using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace IK_SE
{
    public class Nezet
    {
        public int lfx, lfy;//lab-felso-x, lab-felso-y
        public int lkx, lky;//lab-kozepso
        public int lax, lay;//lab-also
        //public static int L1 = 50;
        //public static int L2 = 75;
        public static int L1 = 35;
        public static int L2 = 50;
        public double teta1, teta2, teta3;//szervok szogei
        public double teta1szog, teta2szog, teta3szog;
        public double eltolas1, eltolas2, eltolas3; // szervok pozicionalasabol adodik
        public double alfa;//az a szog, amit a vegpont es kezdopont osszekotese a kezdopont sikjaval bezar.
        public int rajzx, rajzy;
        public TextBox t1, t2, t3, t4, t5;
        public bool baloldali;
        public Nezet parja;
        public Graphics testRajz;
        private Pen myPen, whitePen, blackPen;


        public Nezet(int lab_felso_x, int lab_felso_y, int lab_kozepso_x, int lab_kozepso_y, int lab_also_x, int lab_also_y)
        {
            lfx = lab_felso_x;
            lfy = lab_felso_y;
            lkx = lab_kozepso_x;
            lky = lab_kozepso_y;
            lax = lab_also_x;
            lay = lab_also_y;

            rajzx = lfx;
            rajzy = lfy;

            //L1 = 50;//felkar, vagyis felso es kozepso kozott
            //L2 = 75;//alkar,  vagyis kozepso es also kozott

            L1 = 35;//felkar, vagyis felso es kozepso kozott
            L2 = 50;//alkar,  vagyis kozepso es also kozott

            myPen = new Pen(Color.Red, 4);
            whitePen = new Pen(Color.White, 4);
            blackPen = new Pen(Color.Black, 4);
        }
        public void Transzponal(int x, int y)
        {
            lfx -= x;
            lfy -= y;
            lkx -= x;
            lky -= y;
            lax -= x;
            lay -= y; 
        }

        public void Tukroz(int x)
        {
            lfx = x - lfx;
            lkx = x - lkx;
            lax = x - lax;
        }

        public void VisszaTukroz(int x)
        {
            lfx = x - lfx;
            lkx = x - lkx;
            lax = x - lax;
        }

        public void Kiszamol_OldalNezet()
        {
            if (baloldali)
            {
                Tukroz(rajzx);
                Transzponal(0, rajzy); //eltoljuk a kart az origoba.

                alfa = Math.Atan(lay / (double)lax);

                teta1 = alfa - Math.Acos((double)(lax * lax + lay * lay + L1 * L1 - L2 * L2) / (double)(2 * L1 * Math.Sqrt(lax * lax + lay * lay)));
                teta2 = (double)Math.PI - Math.Acos((double)(L1 * L1 + L2 * L2 - lax * lax - lay * lay) / (double)(2 * L1 * L2));

                teta1szog = teta1 * 180 / (double)Math.PI;
                teta2szog = (teta2 + eltolas2) * 180 / (double)Math.PI;


                try
                {
                    t1.Text = teta1szog.ToString();
                    t2.Text = teta2szog.ToString();

                    t4.Text = lax.ToString();
                    t5.Text = lay.ToString();
                }
                catch
                {
                    //
                }

                lkx = lfx + (int)(Math.Cos(teta1+eltolas1) * L1);
                lky = lfy + (int)(Math.Sin(teta1+eltolas1) * L1);

                parja.lkx = parja.lfx + (int)(Math.Cos(parja.teta3+parja.eltolas3) * lkx);
                parja.lky = parja.lfy + (int)(Math.Sin(parja.teta3+parja.eltolas3) * lkx);

                parja.lax = parja.lfx + (int)(Math.Cos(parja.teta3+parja.eltolas3) * lax);
                parja.lay = parja.lfy + (int)(Math.Sin(parja.teta3+parja.eltolas3) * lax);

                Transzponal(0, -rajzy);
                VisszaTukroz(rajzx);

            }
            else
            {
                Transzponal(rajzx, rajzy); //eltoljuk a kart az origoba.

                alfa = Math.Atan(lay / (double)lax);

                teta1 = alfa - Math.Acos((double)(lax * lax + lay * lay + L1 * L1 - L2 * L2) / (double)(2 * L1 * Math.Sqrt(lax * lax + lay * lay)));
                teta2 = (double)Math.PI - Math.Acos((double)(L1 * L1 + L2 * L2 - lax * lax - lay * lay) / (double)(2 * L1 * L2));

                teta1szog = teta1 * 180 / (double)Math.PI;
                teta2szog = (teta2 + eltolas2) * 180 / (double)Math.PI;


                try
                {
                    t1.Text = teta1szog.ToString();
                    t2.Text = teta2szog.ToString();

                    t4.Text = lax.ToString();
                    t5.Text = lay.ToString();
                }
                catch
                {
                    //
                }

                lkx = lfx + (int)(Math.Cos(teta1) * L1);
                lky = lfy + (int)(Math.Sin(teta1) * L1);

                parja.lkx = parja.lfx + (int)(Math.Cos(parja.teta3) * lkx);
                parja.lky = parja.lfy + (int)(Math.Sin(parja.teta3) * lkx);

                parja.lax = parja.lfx + (int)(Math.Cos(parja.teta3) * lax);
                parja.lay = parja.lfy + (int)(Math.Sin(parja.teta3) * lax);

                Transzponal(-rajzx, -rajzy);
            }
        }
        public void Kiszamol_FelulNezet()
        {
            if (baloldali)
            {
                Tukroz(rajzx);
                Transzponal(0, rajzy);
                teta3 = Math.Atan((double)(lay) / (double)(lax));
                teta3szog = (teta3) * 180 / (double)Math.PI;
                try
                {
                    t3.Text = teta3szog.ToString();

                    t4.Text = lax.ToString();
                    t5.Text = lay.ToString();
                }
                catch
                {
                    //
                }
                //VisszaTukroz(rajzx);
                //Transzponal(0, -rajzy);
                parja.Tukroz(parja.rajzx);
                parja.Transzponal(0, parja.rajzy);

                parja.lax = (int)Math.Sqrt((lax) * (lax) + (lay) * (lay));

                
                //parja.Kiszamol_OldalNezet();
                lkx = lfx + (int)(Math.Cos(teta3+eltolas3) * parja.lkx);
                lky = lfy + (int)(Math.Sin(teta3+eltolas3) * parja.lkx);
                lax = lfx + (int)(Math.Cos(teta3+eltolas3) * parja.lax);
                lay = lfy + (int)(Math.Sin(teta3+eltolas3) * parja.lax);
                parja.Transzponal(0, -parja.rajzy);
                parja.VisszaTukroz(parja.rajzx);
                Transzponal(0, -rajzy);
                VisszaTukroz(rajzx);
                parja.Kiszamol_OldalNezet();


            }
            else
            {
                Transzponal(rajzx, rajzy);
                teta3 = Math.Atan((double)(lay) / (double)(lax));
                teta3szog = (teta3) * 180 / (double)Math.PI;
                try
                {
                    t3.Text = teta3szog.ToString();

                    t4.Text = lax.ToString();
                    t5.Text = lay.ToString();
                }
                catch
                {
                    //
                }

                parja.lax = (int)Math.Sqrt((lax) * (lax) + (lay) * (lay));
                
                parja.Transzponal(parja.rajzx, parja.rajzy);

                lkx = lfx + (int)(Math.Cos(teta3) * parja.lkx);
                lky = lfy + (int)(Math.Sin(teta3) * parja.lkx);
                lax = lfx + (int)(Math.Cos(teta3) * parja.lax);
                lay = lfy + (int)(Math.Sin(teta3) * parja.lax);
                parja.Transzponal(-parja.rajzx, -parja.rajzy);
                Transzponal(-rajzx, -rajzy);
                parja.Kiszamol_OldalNezet();
                //itt folytatni
            }
        }
        public void Mozgat(int x, int y)
        {
            lax += x;
            lay += y;
        }
        public void LabTorol(Graphics pg)
        {
            Rajzol(pg, whitePen, whitePen); 
        }
        public void LabKirajzol(Graphics pg)
        {
            Rajzol(pg, myPen, blackPen);
        }
        private void Rajzol(Graphics pg, Pen pen, Pen bckPen)
        {
            try
            {
                pg.DrawLine(pen, lfx, lfy, lkx, lky);
                pg.DrawLine(pen, lkx, lky, lax, lay);

                int meret = 3;

                pg.DrawEllipse(pen, lfx - meret, lfy - meret, meret * 2, meret * 2);
                pg.DrawEllipse(pen, lkx - meret, lky - meret, meret * 2, meret * 2);
                pg.DrawEllipse(bckPen, lax - meret, lay - meret, meret * 2, meret * 2);
            }
            catch
            {
                //hibakezeles.
            }
        }
    }
}
