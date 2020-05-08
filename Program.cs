using System;

namespace Labynov
{
    class Program
    {
        static void Main(string[] args)
        {
            Map plateauJeu = new Map(50, 50, 3, 50, 0);
            plateauJeu.toString();
            plateauJeu.creuserGalleries();
            plateauJeu.odeurInit();
            Console.Write("\n");
            Console.Write("\n");
            plateauJeu.toString();
            Console.Write("\n");
            string str = plateauJeu.startRantanplan();
            Console.Write(str);
            plateauJeu.toString();

        }
    }
}
