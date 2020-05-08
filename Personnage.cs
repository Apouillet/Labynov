using System;
using System.Collections.Generic;
using System.Text;

namespace Labynov
{
    public class Personnage
    {
        public int x { get; set; }
        public int y { get; set; }

        public string signe { get; set; }

        public string name { get; set; }

        public string provenance { get; set; }

        public void allerAuNord()
        {
            this.y -= 1;
            this.provenance = "S";
        }
        public void allerAuSud()
        {
            this.y += 1;
            this.provenance = "N";
        }
        public void allerAlEst()
        {
            this.x += 1;
            this.provenance = "O";
        }
        public void allerAlOuest()
        {
            this.x -= 1;
            this.provenance = "E";
        }
        public Personnage(int x, int y, string name )
        {
            this.name = name;
            this.y = y;
            this.x = x;
        }

        public Personnage(int x, int y)
        {
            this.y = y;
            this.x = x;

        }
    }
}
