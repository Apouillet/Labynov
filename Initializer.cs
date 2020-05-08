using System;
using System.Collections.Generic;
using System.Text;

namespace Labynov
{

    public class Initializer
    {
        public int viesRantanplan { get; set; }
        public int niveauDeDressage { get; set; }
        public int niveauDeFaim { get; set; }

        public Initializer()
        {

        }
        public Map startMenu()
        {
            Console.Clear();
            Console.WriteLine("Entrez le nombre de vies: \n");
            try
            {
                this.viesRantanplan = Convert.ToInt32(Console.ReadLine());
                if (this.viesRantanplan <= 1)
                {
                    //on gere le cas de la division par 0
                    this.viesRantanplan = 1;
                    Console.WriteLine("Rantanplan part donc avec 1 vie .");
                }
                else
                {
                    Console.WriteLine("Rantanplan part donc avec {0} vies .",
                                  this.viesRantanplan);
                }
            }
            catch (FormatException)
            {
                this.viesRantanplan = 3;
                Console.WriteLine("La valeurs par défaut du nombre de vies est 3.");
            }
            Console.WriteLine("Entrez le niveau de dressage:");
            try
            {
                this.niveauDeDressage = Convert.ToInt32(Console.ReadLine());
                Console.WriteLine("Rantanplan à un dressage de niveau {0} !",
                                  this.niveauDeDressage);
            }
            catch (FormatException)
            {
                this.niveauDeDressage = 0;
                Console.WriteLine("La valeurs par défaut du niveau de dressage est 0.");
            }
            Console.WriteLine("Entrez le niveau de faim:");
            try
            {
                this.niveauDeFaim = Convert.ToInt32(Console.ReadLine());
                Console.WriteLine("Rantanplan à une faim de niveau {0} !",
                                  this.niveauDeFaim);
            }
            catch (FormatException)
            {
                this.niveauDeFaim = 50;
                Console.WriteLine("La valeurs par défaut du niveau de faim est 50.");
            }
            return new Map(50, 50, this.viesRantanplan, this.niveauDeFaim, this.niveauDeDressage);
        }
    }
}
