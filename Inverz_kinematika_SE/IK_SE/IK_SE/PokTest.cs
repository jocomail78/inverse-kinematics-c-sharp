using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace IK_SE
{
    public class PokTest
    {
        public TestPont t1, t2, t3, t4, t5, t6, t7;
        public Point t1p, t2p, t3p, t4p, t5p, t6p;
        //private Point t1v, t2v, t3v, t4v, t5v, t6v;
        public Pen myPen, whitePen, blackPen;
        public Lab balFelsoLab, balKozepsoLab, balAlsoLab, jobbFelsoLab, jobbKozepsoLab, jobbAlsoLab;

        public PokTest(TestPont balfelso, TestPont balkozepso, TestPont balalso, TestPont jobbfelso, TestPont jobbkozepso, TestPont jobbalso, TestPont kozepe)
        {
            t1 = balfelso;
            t2 = balkozepso;
            t3 = balalso;
            t4 = jobbfelso;
            t5 = jobbkozepso;
            t6 = jobbalso;
            t7 = kozepe;
            myPen = new Pen(Color.Red, 4);
            whitePen = new Pen(Color.White, 4);
            blackPen = new Pen(Color.Black, 4);
        }
        public void PokLabak(Point balfelso, Point balkozepso, Point balalso, Point jobbfelso, Point jobbkozepso, Point jobbalso)
        {
            t1p = balfelso;
            t2p = balkozepso;
            t3p = balalso;
            t4p = jobbfelso;
            t5p = jobbkozepso;
            t6p = jobbalso;
        }
        public void eltolas(int le, int jobbra)
        {
            t1.x += jobbra;
            t1.y += le;
            t2.x += jobbra;
            t2.y += le;
            t3.x += jobbra;
            t3.y += le;
            t4.x += jobbra;
            t4.y += le;
            t5.x += jobbra;
            t5.y += le;
            t6.x += jobbra;
            t6.y += le;
            t7.x += jobbra;
            t7.y += le;
        }
        public void magassag_valtoztatas(int fel)
        {
            t1.magassag += fel;
            t2.magassag += fel;
            t3.magassag += fel;
            t4.magassag += fel;
            t5.magassag += fel;
            t6.magassag += fel;
            t7.magassag += fel;
        }
        public void PokKirajzol(PokTest pt, System.Drawing.Graphics pg)
        {
            Rajzol(pg, whitePen,whitePen);
            LabVegpontokKiszamolasa(pt);
            Rajzol(pg, myPen,blackPen);
        }
        private void LabVegpontokKiszamolasa(PokTest pt)
        {
            t1p = new Point();
            t1p.X = t1.x + pt.balFelsoLab.felulrol.lax - pt.balFelsoLab.felulrol.lfx;
            t1p.Y = t1.y + pt.balFelsoLab.felulrol.lay - pt.balFelsoLab.felulrol.lfy;

            t2p = new Point();
            t2p.X = t2.x + pt.balKozepsoLab.felulrol.lax - pt.balKozepsoLab.felulrol.lfx;
            t2p.Y = t2.y + pt.balKozepsoLab.felulrol.lay - pt.balKozepsoLab.felulrol.lfy;

            t3p = new Point();
            t3p.X = t3.x + pt.balAlsoLab.felulrol.lax - pt.balAlsoLab.felulrol.lfx;
            t3p.Y = t3.y + pt.balAlsoLab.felulrol.lay - pt.balAlsoLab.felulrol.lfy;

            t4p = new Point();
            t4p.X = t4.x + pt.jobbFelsoLab.felulrol.lax - pt.jobbFelsoLab.felulrol.lfx;
            t4p.Y = t4.y + pt.jobbFelsoLab.felulrol.lay - pt.jobbFelsoLab.felulrol.lfy;

            t5p = new Point();
            t5p.X = t5.x + pt.jobbKozepsoLab.felulrol.lax - pt.jobbKozepsoLab.felulrol.lfx;
            t5p.Y = t5.y + pt.jobbKozepsoLab.felulrol.lay - pt.jobbKozepsoLab.felulrol.lfy;

            t6p = new Point();
            t6p.X = t6.x + pt.jobbAlsoLab.felulrol.lax - pt.jobbAlsoLab.felulrol.lfx;
            t6p.Y = t6.y + pt.jobbAlsoLab.felulrol.lay - pt.jobbAlsoLab.felulrol.lfy;
        }
        private void Rajzol(System.Drawing.Graphics pg, System.Drawing.Pen pen,Pen bckPen)
        {

            //korvonal megrajzolasa
            pg.DrawLine(pen, t1.getPoint(), t2.getPoint());
            pg.DrawLine(pen, t2.getPoint(), t3.getPoint());
            pg.DrawLine(pen, t3.getPoint(), t4.getPoint());
            pg.DrawLine(pen, t4.getPoint(), t5.getPoint());
            pg.DrawLine(pen, t5.getPoint(), t6.getPoint());
            pg.DrawLine(pen, t6.getPoint(), t1.getPoint());
            pg.DrawLine(pen, t1.getPoint(), t4.getPoint());
            pg.DrawLine(pen, t3.getPoint(), t6.getPoint());

            //kozeppont osszekotese
            pg.DrawLine(pen, t7.getPoint(), t1.getPoint());
            pg.DrawLine(pen, t7.getPoint(), t2.getPoint());
            pg.DrawLine(pen, t7.getPoint(), t3.getPoint());
            pg.DrawLine(pen, t7.getPoint(), t4.getPoint());
            pg.DrawLine(pen, t7.getPoint(), t5.getPoint());
            pg.DrawLine(pen, t7.getPoint(), t6.getPoint());

            ////labak megrajzolasa
            //pg.DrawLine(pen, t1.getPoint(), t1p);
            //pg.DrawLine(pen, t2.getPoint(), t2p);
            //pg.DrawLine(pen, t3.getPoint(), t3p);
            //pg.DrawLine(pen, t4.getPoint(), t4p);
            //pg.DrawLine(pen, t5.getPoint(), t5p);
            //pg.DrawLine(pen, t6.getPoint(), t6p);

            //kis korok rajzolasa a testhez
            int meret = 5;
            pg.DrawEllipse(bckPen, t1.x - meret, t1.y - meret, meret * 2, meret * 2);
            pg.DrawEllipse(bckPen, t2.x - meret, t2.y - meret, meret * 2, meret * 2);
            pg.DrawEllipse(bckPen, t3.x - meret, t3.y - meret, meret * 2, meret * 2);
            pg.DrawEllipse(bckPen, t4.x - meret, t4.y - meret, meret * 2, meret * 2);
            pg.DrawEllipse(bckPen, t5.x - meret, t5.y - meret, meret * 2, meret * 2);
            pg.DrawEllipse(bckPen, t6.x - meret, t6.y - meret, meret * 2, meret * 2);
            pg.DrawEllipse(bckPen, t7.x - meret, t7.y - meret, meret * 2, meret * 2);     
    
            //labak rajzolasa
            try
            {
                pg.DrawLine(pen, t1.getPoint(), t1p);
                pg.DrawEllipse(bckPen, t1p.X - meret, t1p.Y - meret, meret * 2, meret * 2);
                pg.DrawLine(pen, t2.getPoint(), t2p);
                pg.DrawEllipse(bckPen, t2p.X - meret, t2p.Y - meret, meret * 2, meret * 2);
                pg.DrawLine(pen, t3.getPoint(), t3p);
                pg.DrawEllipse(bckPen, t3p.X - meret, t3p.Y - meret, meret * 2, meret * 2);
                pg.DrawLine(pen, t4.getPoint(), t4p);
                pg.DrawEllipse(bckPen, t4p.X - meret, t4p.Y - meret, meret * 2, meret * 2);
                pg.DrawLine(pen, t5.getPoint(), t5p);
                pg.DrawEllipse(bckPen, t5p.X - meret, t5p.Y - meret, meret * 2, meret * 2);
                pg.DrawLine(pen, t6.getPoint(), t6p);
                pg.DrawEllipse(bckPen, t6p.X - meret, t6p.Y - meret, meret * 2, meret * 2);
            }
            catch
            {
                //hibakezeles
            }
        }

    }
}
