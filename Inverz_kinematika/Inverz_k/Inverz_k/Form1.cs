using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Inverz_k
{
    public partial class Form1 : Form
    {
        System.Drawing.Pen whitePen; //Pen - ezzel rajzoljuk a vonalakat
        System.Drawing.Pen myPen;    // azert kell ketto belole, hogy le is tudjuk torolni az addigi rajzot.
        System.Drawing.Brush myBrush;
        System.Drawing.Graphics pg,pg2; // ez a ket panel, amire rajzolunk
        int x0, y0, x1, y1, x2, y2; // a robotkarunk harom pontja. 
        int L1, L2;                 // a robotkar karjainak hossza
        int[] a,b,c,d,e,f;          // ezekben tarolom a kirajzolt objektum koordinatait
        double teta1, teta2, teta3; // ezekben taroljuk a szervok elfordulasi szoget radianban
        double teta1szog, teta2szog, teta3szog;  // itt meg a szogeket fokokban
        double valosteta1, valosteta2, valosteta3; // erre azert van szukseg, mert lehet, hogy alapbol eltolas van a szogben
        double alfa; // ez a szog, amit a robotkar vegpontja a kezdopont sikjaval bezar.
        double atfogo; // ez meg a kezdopont-vegpontot osszekoto szakasz hossza
        public Form1()
        {
            InitializeComponent();
            whitePen = new Pen(System.Drawing.Color.White,4); // feher ceruza. Ezzel torlunk
            myPen = new Pen(System.Drawing.Color.Red,4);      // piros ceruza. Ezzel rajzolunk
            myBrush = new SolidBrush(System.Drawing.Color.Black); // ez nemtudom mire van :-S
            a = new int[2]; //
            b = new int[2]; //
            c = new int[2]; //itt megmondjuk, hogy a pontoknak ket komponensuk van. Egy x es egy Y 
            d = new int[2]; // vagy felulnezetbol egy X es egy Z
            e = new int[2]; // vagy valami hasonlo
            f = new int[2]; //
            pg = panel1.CreateGraphics(); //bal oldali panel. erre rajzolunk
            pg2 = panel2.CreateGraphics(); // jobb oldali panel
            L1 = 100; // femur hossza. 
            L2 = 150; // tibia hossza (ask google)
            x0 = 0;   // kezdopont X
            y0 = panel1.Height / 3; // kezdopont Y
            x1 = x0 + L1;
            y1 = y0;
            x2 = x1;
            y2 = y1 + L2;
            a[0] = x0;      //beallitjuk a koordinatatengely x koordinatajat
            a[1] = y0;      //beallitjuk a koordinatatengely y koordinatajat
            b[0] = x1;
            b[1] = y1;
            c[0] = x2;
            c[1] = y2;
            d[0] = 0;
            d[1] = panel2.Height / 2;
            e[0] = b[0];
            e[1] = d[1];
            f[0] = c[0];
            f[1] = e[1];
//            k_transz(x0, y0);
        }
        private void k_transz(int x, int y) // transzformaciora azert van szukseg, mert a kepletem
        {                                   //ugy szol, hogy az alap a 0,0 pontban helyezkedik el
            a[0] -= x;      //beallitjuk a koordinatatengely x koordinatajat
            a[1] -= y;      //beallitjuk a koordinatatengely y koordinatajat
            b[0] -= x;
            b[1] -= y;
            c[0] -= x;
            c[1] -= y;
        }

        private void draw(Pen p)
        {
            try
            {
                k_transz(x0, y0); //transzponalunk
                calculate();      // szamolunk
                k_transz((-x0), (-y0)); // visszatranszponalunk

                //itt jon a sok sok rajzolas. Vonalak, korok, miegymas

                pg.DrawEllipse(p, a[0] - 5, a[1] - 5, 10, 10);
                pg.DrawLine(p, a[0], a[1], b[0], b[1]);
                pg.DrawEllipse(p, b[0] - 5, b[1] - 5, 10, 10);
                pg.DrawLine(p, b[0], b[1], c[0], c[1]);
                pg.DrawEllipse(p, c[0] - 5, c[1] - 5, 10, 10);

                pg2.DrawEllipse(p, d[0] - 5, d[1] - 5, 10, 10);
                pg2.DrawLine(p, d[0], d[1], e[0], e[1]);
                pg2.DrawEllipse(p, e[0] - 5, e[1] - 5, 10, 10);
                pg2.DrawLine(p, e[0], e[1], f[0], f[1]);
                pg2.DrawEllipse(p, f[0] - 5, f[1] - 5, 10, 10);

            }
            catch
            { 
                //hibakezeles. Nincs.
            }
        }

        private void calculate()
        { 
            //itt szamoljuk ki, hogy az egyes reszek milyen szogben kell alljanak.
            //elso ket sor hetedikes matek
            atfogo = Math.Sqrt(c[0]*c[0]+c[1]*c[1]);
            alfa = Math.Atan(c[1]/(double)c[0]);

            //koszinusz tetel, MIT OCV-ben megtalalhato a leirasa.
            teta1 = alfa - Math.Acos((double)(c[0] * c[0] + c[1] * c[1] + L1 * L1 - L2 * L2) / (double)(2 * L1 * Math.Sqrt(c[0]*c[0]+c[1]*c[1])));
            teta2 = (double)Math.PI - Math.Acos((double)(L1 * L1 + L2 * L2 - c[0] * c[0] - c[1] * c[1]) / (double)(2 * L1 * L2));

            //atalakitas radianbol fokba
            teta1szog = teta1 * 180 / (double)Math.PI;
            teta2szog = teta2 * 180 / (double)Math.PI;

            //eltolasok kiszamitasa
            valosteta1 = teta1szog;
            valosteta2 = teta2szog - 90;

            //eredmeny kiiratasa
            tb_teta1.Text = valosteta1.ToString();
            tb_teta2.Text = valosteta2.ToString();

            //pontok poziciojanak kiszamitasa
            b[0] = a[0] + (int)(Math.Cos(teta1) * L1);
            b[1] = a[1] + (int)(Math.Sin(teta1) * L1);

            e[0] = d[0] + (int)(Math.Cos(teta3) * b[0]);
            e[1] = d[1] + (int)(Math.Sin(teta3) * b[0]);

            f[0] = d[0] + (int)(Math.Cos(teta3) * c[0]);
            f[1] = d[1] + (int)(Math.Sin(teta3) * c[0]);

        }

        //itt kovetkezik egy par gomb esemeny. Ha kattintunk a felfele vagy lefele vagy
        //jobbra-balra nyilakra, akkor a megfelelo erteket noveljuk, majd ujrarajzoljuk 
        private void b_fel_Click(object sender, EventArgs e)
        {
            draw(whitePen); //letoroljuk a rajzlapot
            c[1] -= 1;      //frissitjuk az erteket
            draw(myPen);    //ujrarajzoljuk
        }

        private void b_le_Click(object sender, EventArgs e)
        {
            draw(whitePen);
            c[1] += 1;
            draw(myPen);
        }

        private void b_balra_Click(object sender, EventArgs e)
        {
            draw(whitePen);
            c[0] -= 1; 
            draw(myPen);
        }

        private void b_jobbra_Click(object sender, EventArgs e)
        {
            draw(whitePen);
            c[0] += 1;
            draw(myPen);
        }

        //egeresemeny. Ha lenyomott egergombot huzom a panel folott, akkor jon ez. 
        //ez a bal oldali rajzlapon
        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left) //le van-e nyomva az eger?
            {
                draw(whitePen);//torol

                c[0] = e.X;  //frissit
                c[1] = e.Y;

                draw(myPen);   //rajzol
            }
        }

        //egeresemeny jobb oldali rajzlapon
        private void panel2_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                draw(whitePen);

                teta3 = Math.Atan((double)(e.Y-d[1]) / (double)(e.X-d[0])); // szamolunk
                c[0] = (int)Math.Sqrt((e.X)*(e.X)+(e.Y-d[1])*(e.Y-d[1])); //frissitunk
                teta3szog = teta3 * 180 / (double)Math.PI;
                tb_teta3.Text = teta3szog.ToString();
                label3.Text = (c[0]).ToString();
                draw(myPen);//rajzolunk
            }
        }

        //megint egy par gomb, ezek a jobboldaliak. 
        //itt van egy kis kerekitesi hiba, vagyis ha felmegyek es utana le, nem jutok vissza a 
        //kiindulopontba. Ez a tizedesresz elhanyagolasa miatt van. 
        private void b_fel2_Click(object sender, EventArgs e)
        {
            draw(whitePen);
            f[1] -= 10;
            c[0] = (int)Math.Sqrt((f[0]) * (f[0]) + (f[1] - d[1]) * (f[1] - d[1]));//itt a kerekites.
            teta3 = Math.Atan((double)(f[1] - d[1]) / (double)(f[0] - d[0]));
            draw(myPen);
        }

        private void b_le2_Click(object sender, EventArgs e)
        {
            draw(whitePen);
            f[1] += 10;
            c[0] = (int)Math.Sqrt((f[0]) * (f[0]) + (f[1] - d[1]) * (f[1] - d[1]));
            teta3 = Math.Atan((double)(f[1] - d[1]) / (double)(f[0] - d[0]));
            draw(myPen);
        }

        private void b_balra2_Click(object sender, EventArgs e)
        {
            draw(whitePen);
            f[0] -= 10;
            c[0] = (int)Math.Sqrt((f[0]) * (f[0]) + (f[1] - d[1]) * (f[1] - d[1]));
            teta3 = Math.Atan((double)(f[1] - d[1]) / (double)(f[0] - d[0]));
            draw(myPen);
        }

        private void b_jobbra2_Click(object sender, EventArgs e)
        {
            draw(whitePen);
            f[0] += 10;
            c[0] = (int)Math.Sqrt((f[0]) * (f[0]) + (f[1] - d[1]) * (f[1] - d[1]));
            teta3 = Math.Atan((double)(f[1] - d[1]) / (double)(f[0] - d[0]));
            draw(myPen);
        }
    }
}
