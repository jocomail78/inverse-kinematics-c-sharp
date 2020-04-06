using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace IK_SE
{
    public class TestPont
    {
        public int x, y, magassag;
        public TestPont(int PontX, int PontY, int PontMagassag)
        {
            x = PontX;
            y = PontY;
            magassag = PontMagassag;
        }
        public Point getPoint()
        {
            Point p = new Point(x, y);
            return p;
        }

    }
}
