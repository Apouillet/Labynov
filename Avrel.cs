using System;
using System.Collections.Generic;
using System.Text;

namespace Labynov
{
    public class Avrel : Dalton
    {
        public int odeurDeCuisine { get; set; }
        public Avrel(int x, int y) : base(x, y)
        {
            this.name = "Avrel";
            this.signe = "O";
        }
    }
}
