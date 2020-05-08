using System;
using System.Collections.Generic;
using System.Text;

namespace Labynov
{
   public class Map
    {
        public int sizeY { get; set; }

        public int sizeX { get; set; }
        public List<Case> sousSol { get; set; }

        public List<Dalton> mechants = new List<Dalton>();

        public bool perdu = false;
        //Variable d'aléatoire 
        public Random random = new Random();

        //On déclare une liste de directions
        public List<string> directions = new List<string>();
        //On déclare la variable qui va recevoir celle choisi
        public string directionChoisi = "";

        public Avrel avrel { get; set; }

        public Rantanplan rantanplan { get; set; }
        /**
         * 
         *         FONCTIONS INITIALISATIONS MAP
         * 
         */
        public Map()
        {//initialisation de la map par défaut
            this.sizeY = 100;
            this.sizeX = 100;
            this.sousSol = new List<Case>();
            for (int y = 0; y < 100; y++)
            {
                for (int x = 0; x < 100; x++)
                {
                    //Création des cases
                    Case newCase = new Case(x, x);
                    this.sousSol.Add(newCase);
                }
            }
            //On initialise le point de départ aux centre 
            this.sousSol[((50 * 100) + 50)].estLibre();
            //On fait descendre les méchants
            this.creerDaltons(100, 100);
            //On fait descendre Avrel
            this.creerAvrel(51, 50);
            //Et El famoso Rantanplan
            this.creerRantanplan(50, 50, 3, 50, 50);
        }
        public Map(int X, int Y, int viesRantanplan, int faimRantanplan, int obeissanceRantanplan )
        {//initialisation de la map de la taille demandé
            this.sizeY = Y;
            this.sizeX = X;
            this.sousSol = new List<Case>();
            for (int y = 0; y < this.sizeY; y++)
            {
                for (int x = 0; x < this.sizeX; x++)
                {
                    //Création des cases
                    Case newCase = new Case(x, x);
                    this.sousSol.Add(newCase);
                }
            }
            //On initialise le point de départ aux centre 
            this.sousSol[((Y/2 * X) + X/2)].seLibere();
            //On fait descendre les méchants
            this.creerDaltons(X, Y);
            //On fait descendre Avrel
            int coordX = (X / 2) + 1;
            int coordY = (Y / 2);
            this.creerAvrel(coordX, coordY);
            //Et El famoso Rantanplan
            coordX = (X / 2);
            coordY = (Y / 2);
            this.creerRantanplan(coordX, coordY, viesRantanplan, faimRantanplan, obeissanceRantanplan);
        }
        //Fontion pour générer le labirynte
        public void creuserGalleries()
        {
            this.avrel.creuse = true;
            for (int i = 0; i < this.mechants.Count; i++)
            {
                this.mechants[i].creuse = true;
                while (this.mechants[i].creuse)
                {
                    this.antiDemiTour(this.directionChoisi);
                    this.directionChoisi = this.aleaStrings(this.directions);
                    this.CreuseOuStopDalton(this.directionChoisi, this.mechants[i].x, this.mechants[i].y, i);
                }
                this.directionChoisi = "";
            }
            while (this.avrel.creuse)
            {
                this.antiDemiTour(this.directionChoisi);
                directionChoisi = this.aleaStrings(this.directions);
                this.CreuseOuStopAvrel(directionChoisi, this.avrel.x, this.avrel.y);
            }

        }
        //Visuel pour dev
        public void toString()
        {
            for (int i = 0; i < this.sousSol.Count; i++)
            {
                if (i % this.sizeX == this.sizeX - 1)
                {
                    Console.Write("\n");
                }
                else
                {
                    Console.Write(this.sousSol[i].occupePar);
                }
            }
        }
        /**
         * 
         *  FONCTIONS DE TESTS
         * 
         */
        public bool estDansSousSol(int index)
        {
            if (index < 0 || index > this.sousSol.Count)
            {
                return false;
            }
            return true;
        }
        //Petite fonction pratique pour passer de coordonnées à l'index de la liste de case
        public int convertCoordonneeToIndex(int x, int y)
        {
            return (y * this.sizeX) + x;
        }
        public bool estLibreNord(int x, int y)
        {
            //On calcul l'index de la case dans le tableau et on vérifie pour la ligne d'au dessus .
            int indexCalc = this.convertCoordonneeToIndex(x, y-1);
            if (this.estDansSousSol(indexCalc) && this.sousSol[indexCalc].estLibre())
            {
                return true;
            }
            else
            {
                return false;
            }

        }
        public bool estLibreSud(int x, int y)
        {
            //On calcul l'index de la case dans le tableau et on vérifie pour la ligne d'en dessous .
            int indexCalc = this.convertCoordonneeToIndex(x, y+1);
            if (this.estDansSousSol(indexCalc) && this.sousSol[indexCalc].estLibre())
            {
                return true;
            }
            else
            {
                return false;
            }

        }
        public bool estLibreOuest(int x, int y)
        {
            //On calcul l'index de la case dans le tableau et on vérifie pour la case suivante .
            int indexCalc = this.convertCoordonneeToIndex(x-1, y);
            if (this.estDansSousSol(indexCalc) && this.sousSol[indexCalc].estLibre())
            {
                return true;
            }
            else
            {
                return false;
            }

        }
        public bool estLibreEst(int x, int y)
        {
            //On calcul l'index de la case dans le tableau et on vérifie pour la case précédente .
            int indexCalc = this.convertCoordonneeToIndex(x+1, y);
            if (this.estDansSousSol(indexCalc) && this.sousSol[indexCalc].estLibre())
            {
                return true;
            }
            else
            {
                return false;
            }

        }
        /**
         * 
         *      Fonctions Utilitaires
         * 
         */
        public void libererCase(int x, int y)
        {
            //On calcul l'index de la case dans le tableau et on libere
            this.sousSol[this.convertCoordonneeToIndex(x, y)].seLibere();
        }
        public void occuperCase(int x, int y, string obstacle)
        {
            //On calcul l'index de la case dans le tableau et on occupe
            this.sousSol[this.convertCoordonneeToIndex(x, y)].soccupe(obstacle);
        }
        //Fonction pour faire un choix aléatoire parmis une liste de string
        public string aleaStrings(List<string> choixList)
        {
            string choix = choixList[this.random.Next(choixList.Count)];
            return choix;
        }
        //Fonction pour empécher le demi-tour 
        public void antiDemiTour(string directionPrecedente)
        {
            switch (directionPrecedente)
            {
                case "N":
                    {
                        this.directions.Clear();
                        this.directions.Add("N");
                        this.directions.Add("O");
                        this.directions.Add("E");
                    }
                    break;
                case "S":
                    {
                        this.directions.Clear();
                        this.directions.Add("O");
                        this.directions.Add("S");
                        this.directions.Add("E");
                    }
                    break;
                case "E":
                    {
                        this.directions.Clear();
                        this.directions.Add("N");
                        this.directions.Add("S");
                        this.directions.Add("E");
                    }
                    break;
                case "O":
                    {
                        this.directions.Clear();
                        this.directions.Add("N");
                        this.directions.Add("S");
                        this.directions.Add("O");
                    }
                    break;
                default:
                    {
                        this.directions.Clear();
                        this.directions.Add("N");
                        this.directions.Add("S");
                        this.directions.Add("E");
                        this.directions.Add("O");
                    }
                    break;
            }

        }


        /**
         *
         *      FONCTIONS INITIALISATION PERSONNAGES
         * 
         */
        public void creerAvrel(int x, int y)
        {
            this.avrel = new Avrel(x, y);
            this.occuperCase(x, y, this.avrel.signe);
            this.avrel.odeurDeCuisine = this.sizeX * this.sizeY;
            this.avrel.odeurDePied = this.sizeX * this.sizeY;
        }
        public void creerDalton(int x , int y, string name)
        {
            Dalton dalton = new Dalton(x, y, name);
            this.occuperCase(x, y, dalton.signe);
            dalton.odeurDePied = this.sizeX * this.sizeY;
            this.mechants.Add(dalton);
        }
        //Pour créer rapidement les méchants
        public void creerDaltons(int X, int Y)
        {
            //On fait descendre Joe
            int coordX = (X / 2);
            int coordY = (Y / 2) - 1;
            this.creerDalton(coordX, coordY, "Joe");
            //On fait descendre Jack
            coordX = (X / 2) - 1;
            coordY = (Y / 2);
            this.creerDalton(coordX, coordY, "Jack");
            //On fait descendre Williams
            coordX = (X / 2);
            coordY = (Y / 2) + 1;
            this.creerDalton(coordX, coordY, "Williams");
        }
        //On créer notre héros
        public void creerRantanplan(int x, int y, int vies, int faim , int obeissance)
        {
            this.rantanplan = new Rantanplan(x, y , vies, faim, obeissance);
            this.occuperCase(x, y, rantanplan.signe);
        }
       /**
        * 
        *       Fonctions Actions PERSONNAGE/MAP
        * 
        */

        public void CreuseOuStopDalton(string direction, int x , int y , int indexDalton)
        {
            //On vérifie la possibilité d'avancer dans cette direction
            switch (direction)
            {
                case "N":
                    {
                        // Vérification hors map Nord
                        if(y > 3)
                        {
                            //Vérification du passage Nord (pas de coup de pioche entre les personnages )
                            if (this.sousSol[this.convertCoordonneeToIndex(x, y) - this.sizeX].occupePar == "X" || this.sousSol[this.convertCoordonneeToIndex(x, y) - this.sizeX].occupePar == " " &&
                                (this.sousSol[this.convertCoordonneeToIndex(x, y) - (2 * this.sizeX)].occupePar == "X" || this.sousSol[this.convertCoordonneeToIndex(x, y) - (2 * this.sizeX)].occupePar == " "))
                            {
                                //On libere la case courante
                                this.sousSol[this.convertCoordonneeToIndex(x, y)].seLibere();
                                //On creuse au nord
                                this.sousSol[this.convertCoordonneeToIndex(x, y) - this.sizeX].seLibere();
                                this.mechants[indexDalton].allerAuNord();
                                this.sousSol[this.convertCoordonneeToIndex(x, y) - (2 * this.sizeX)].soccupe("o");
                                this.mechants[indexDalton].allerAuNord();
                            }
                            else
                            {
                                break;
                            }
                        }
                        else
                        {
                            //On prévient pourquoi on s'arrette de creuser 
                            Console.Write("2 cases du nord de la part de ");
                            Console.Write(this.mechants[indexDalton].name);
                            Console.Write("\n");
                            //On arrette de creuser
                            this.stopCreuserDalton(this.mechants[indexDalton]);
                        }
                    }
                    break;
                case "S":
                    {
                        // Vérification hors map Sud
                        if ( y < this.sizeY - 3)
                        {
                            //Vérification du passage Sud
                            if (this.sousSol[this.convertCoordonneeToIndex(x, y) + this.sizeX].occupePar == "X" || this.sousSol[this.convertCoordonneeToIndex(x, y) + this.sizeX].occupePar == " " &&
                                (this.sousSol[this.convertCoordonneeToIndex(x, y) + (2 * this.sizeX)].occupePar == "X" || this.sousSol[this.convertCoordonneeToIndex(x, y) + (2 * this.sizeX)].occupePar == " "))
                            {
                                //On libere la case courante
                                this.sousSol[this.convertCoordonneeToIndex(x, y)].seLibere();
                                //On creuse au sud
                                this.sousSol[this.convertCoordonneeToIndex(x, y) + this.sizeX].seLibere();
                                this.mechants[indexDalton].allerAuSud();
                                this.sousSol[this.convertCoordonneeToIndex(x, y) + (2 * this.sizeX)].soccupe("o");
                                this.mechants[indexDalton].allerAuSud();
                            }
                            else
                            {
                                break;
                            }
                        }
                        else
                        {
                            //On prévient pourquoi on s'arrette de creuser 
                            Console.Write("2 cases du sud de la part de ");
                            Console.Write(this.mechants[indexDalton].name);
                            Console.Write("\n");
                            //On arrette de creuser
                            this.stopCreuserDalton(this.mechants[indexDalton]);
                        }
                    }
                    break;
                case "E":
                    {
                        // Vérification hors map Est
                        if (x < this.sizeX - 3)
                        {
                            //Vérification du passage Est
                            if (this.sousSol[this.convertCoordonneeToIndex(x, y) + 1].occupePar == "X" || this.sousSol[this.convertCoordonneeToIndex(x, y) + 1].occupePar == " " &&
                                (this.sousSol[this.convertCoordonneeToIndex(x, y) + 2].occupePar == "X" || this.sousSol[this.convertCoordonneeToIndex(x, y) + 2].occupePar == " "))
                            {
                                //On libere la case courante
                                this.sousSol[this.convertCoordonneeToIndex(x, y)].seLibere();
                                //On creuse a l'est
                                this.sousSol[this.convertCoordonneeToIndex(x, y) + 1].seLibere();
                                this.mechants[indexDalton].allerAlEst();
                                this.sousSol[this.convertCoordonneeToIndex(x, y) + 2].soccupe("o");
                                this.mechants[indexDalton].allerAlEst();
                            }
                            else
                            {
                                break;
                            }
                        }
                        else
                        {
                            //On prévient pourquoi on s'arrette de creuser 
                            Console.Write("2 cases de l'Est de la part de ");
                            Console.Write(this.mechants[indexDalton].name);
                            Console.Write("\n");
                            //On arrette de creuser
                            this.stopCreuserDalton(this.mechants[indexDalton]);
                        }
                    }
                    break;
                case "O":
                    {
                        // Vérification hors map Ouest
                        if (x > 3)
                        {
                            //Vérification du passage Ouest
                            if (this.sousSol[this.convertCoordonneeToIndex(x, y) - 1].occupePar == "X" || this.sousSol[this.convertCoordonneeToIndex(x, y) - 1].occupePar == " " &&
                                (this.sousSol[this.convertCoordonneeToIndex(x, y) - 2].occupePar == "X" || this.sousSol[this.convertCoordonneeToIndex(x, y) - 2].occupePar == " "))
                            {
                                //On libere la case courante
                                this.sousSol[this.convertCoordonneeToIndex(x, y)].seLibere();
                                //On creuse a l'ouest
                                this.sousSol[this.convertCoordonneeToIndex(x, y) - 1].seLibere();
                                this.mechants[indexDalton].allerAlOuest();
                                this.sousSol[this.convertCoordonneeToIndex(x, y) - 2].soccupe("o");
                                this.mechants[indexDalton].allerAlOuest();
                            }
                            else
                            {
                                break;
                            }
                        }
                        else
                        {
                            //On prévient pourquoi on s'arrette de creuser 
                            Console.Write("2 cases de l'Ouest de la part de ");
                            Console.Write(this.mechants[indexDalton].name);
                            Console.Write("\n");
                            //On arrette de creuser
                            this.stopCreuserDalton(this.mechants[indexDalton]);
                        }
                    }
                    break;
            }
        }
        public void CreuseOuStopAvrel(string direction, int x, int y)
        {
            //On vérifie la possibilité d'avancer dans cette direction
            switch (direction)
            {
                case "N":
                    {
                        // Vérification hors map Nord
                        if (y > 3)
                        {
                            //Vérification du passage Nord (pas de coup de pioche entre les personnages )
                            if (this.sousSol[this.convertCoordonneeToIndex(x, y) - this.sizeX].occupePar == "X" || this.sousSol[this.convertCoordonneeToIndex(x, y) - this.sizeX].occupePar == " " &&
                                (this.sousSol[this.convertCoordonneeToIndex(x, y) - (2 * this.sizeX)].occupePar == "X" || this.sousSol[this.convertCoordonneeToIndex(x, y) - (2 * this.sizeX)].occupePar == " "))
                            {
                                //On libere la case courante
                                this.sousSol[this.convertCoordonneeToIndex(x, y)].seLibere();
                                this.sousSol[this.convertCoordonneeToIndex(x, y) - this.sizeX].seLibere();
                                this.avrel.allerAuNord();
                                this.sousSol[this.convertCoordonneeToIndex(x, y) - (2 * this.sizeX)].soccupe("O");
                                this.avrel.allerAuNord();
                            }
                            else
                            {
                                break;
                            }
                        }
                        else
                        {
                            Console.Write("2 cases du nord de la part d'Avrel");
                            this.avrel.creuse = false;
                        }
                    }
                    break;
                case "S":
                    {
                        // Vérification hors map Sud
                        if (y < this.sizeY - 3)
                        {
                            //Vérification du passage Sud
                            if (this.sousSol[this.convertCoordonneeToIndex(x, y) + this.sizeX].occupePar == "X" || this.sousSol[this.convertCoordonneeToIndex(x, y) + this.sizeX].occupePar == " " &&
                                (this.sousSol[this.convertCoordonneeToIndex(x, y) + (2 * this.sizeX)].occupePar == "X" || this.sousSol[this.convertCoordonneeToIndex(x, y) + (2 * this.sizeX)].occupePar == " "))
                            {
                                //On libere la case courante
                                this.sousSol[this.convertCoordonneeToIndex(x, y)].seLibere();
                                this.sousSol[this.convertCoordonneeToIndex(x, y) + this.sizeX].seLibere();
                                this.avrel.allerAuSud();
                                this.sousSol[this.convertCoordonneeToIndex(x, y) + (2 * this.sizeX)].soccupe("O");
                                this.avrel.allerAuSud();
                            }
                            else
                            {
                                break;
                            }
                        }
                        else
                        {
                            Console.Write("2 cases du sud de la part d'Avrel");
                            this.avrel.creuse = false;
                        }
                    }
                    break;
                case "E":
                    {
                        // Vérification hors map Est
                        if (x < this.sizeX -3)
                        {
                            //Vérification du passage Est
                            if (this.sousSol[this.convertCoordonneeToIndex(x, y) + 1].occupePar == "X" || this.sousSol[this.convertCoordonneeToIndex(x, y) + 1].occupePar == " " &&
                                (this.sousSol[this.convertCoordonneeToIndex(x, y) + 2].occupePar == "X" || this.sousSol[this.convertCoordonneeToIndex(x, y) + 2].occupePar == " "))
                            {
                                //On libere la case courante
                                this.sousSol[this.convertCoordonneeToIndex(x, y)].seLibere();
                                this.sousSol[this.convertCoordonneeToIndex(x, y) + 1].seLibere();
                                this.avrel.allerAlEst();
                                this.sousSol[this.convertCoordonneeToIndex(x, y) + 2].soccupe("O");
                                this.avrel.allerAlEst();
                            }
                            else
                            {
                                break;
                            }
                        }
                        else
                        {
                            Console.Write("2 cases de l'Est de la part d'Avrel");
                            this.avrel.creuse = false;
                        }
                    }
                    break;
                case "O":
                    {
                        // Vérification hors map Ouest
                        if (x > 3)
                        {
                            //Vérification du passage Ouest
                            if (this.sousSol[this.convertCoordonneeToIndex(x, y) - 1].occupePar == "X" || this.sousSol[this.convertCoordonneeToIndex(x, y) - 1].occupePar == " " &&
                                (this.sousSol[this.convertCoordonneeToIndex(x, y) - 2].occupePar == "X" || this.sousSol[this.convertCoordonneeToIndex(x, y) - 2].occupePar == " "))
                            {
                                //On libere la case courante
                                this.sousSol[this.convertCoordonneeToIndex(x, y)].seLibere();
                                this.sousSol[this.convertCoordonneeToIndex(x, y) - 1].seLibere();
                                this.avrel.allerAlOuest();
                                this.sousSol[this.convertCoordonneeToIndex(x, y) - 2].soccupe("O");
                                this.avrel.allerAlOuest();
                            }
                            else
                            {
                                break;
                            }
                        }
                        else
                        {
                            Console.Write("2 cases de l'Ouest de la part d'Avrel");
                            this.avrel.creuse = false;
                        }
                    }
                    break;
            }
        }
        //Fonction pour stopper un Dalton 
        public void stopCreuserDalton(Dalton dalton)
        {
            dalton.creuse = false;
        }
        //Fonction pour initialiser es odeurs sur la map
        public void odeurInit()
        {
            //On utilise la fonction suivante pour remplir de toute les directions this.directions
            this.antiDemiTour("");
            List<string> directionsOdeur = this.directions;
            foreach (Dalton dalton in this.mechants)
            {
                this.propagationOdeur(dalton.x, dalton.y, dalton.odeurDePied, false, directionsOdeur);
            }
            this.propagationOdeur(this.avrel.x, this.avrel.y, this.avrel.odeurDeCuisine, true, directionsOdeur);
        }
        //Fonction pour répendre l'odeur du Dalton
        public void sentLeDalton(int x, int y, int odeur)
        {
            this.sousSol[this.convertCoordonneeToIndex(x, y)].odeurDePieds = odeur;
        }
        //Fonction pour répendre l'odeur de nourriture
        public void sentLeManger(int x, int y, int odeur)
        {
            this.sousSol[this.convertCoordonneeToIndex(x, y)].odeurDeCuisine = odeur;
        }
        //Fonction de propagation de l'odeur
        public void propagationOdeur(int x, int y, int odeur, bool odeurManger, List<string> directionsOdeur)
        {
            //List<string> newDirectionsOdeur = new List<string>();
            if (odeurManger)
            {
                //Dans le cas de la nourriture d'avrel
                this.sentLeManger(x, y, odeur);
                //newDirectionsOdeur = this.continueOdeurNourriture(x, y, odeur);
                if(this.continueOdeurNourriture(x, y, odeur).Count > 0)
                {
                    foreach (string direction in this.continueOdeurNourriture(x, y, odeur))
                    {
                        switch (direction)
                        {
                            case "N":
                                {
                                    this.propagationOdeur(x, y - 1, odeur - 1, odeurManger, directionsOdeur);
                                }
                                break;
                            case "S":
                                {
                                    this.propagationOdeur(x, y + 1, odeur - 1, odeurManger, directionsOdeur);
                                }
                                break;
                            case "E":
                                {
                                    this.propagationOdeur(x + 1, y, odeur - 1, odeurManger, directionsOdeur);
                                }
                                break;
                            case "O":
                                {
                                    this.propagationOdeur(x - 1, y, odeur - 1, odeurManger, directionsOdeur);
                                }
                                break;
                            default:
                                break;
                        }
                    }
                }
                
            }
            //Dans le cas de l'odeur des pieds des dalton
            this.sentLeDalton(x, y, odeur);
            //newDirectionsOdeur = this.continueOdeurDalton(x, y, odeur);
            if (this.continueOdeurDalton(x, y, odeur).Count > 0)
            {
                foreach (string direction in this.continueOdeurDalton(x, y, odeur))
                {
                    switch (direction)
                    {
                        case "N":
                            {
                                this.propagationOdeur(x, y - 1, odeur - 1, odeurManger, directionsOdeur);
                            }
                            break;
                        case "S":
                            {
                                this.propagationOdeur(x, y + 1, odeur - 1, odeurManger, directionsOdeur);
                            }
                            break;
                        case "E":
                            {
                                this.propagationOdeur(x + 1, y, odeur - 1, odeurManger, directionsOdeur);
                            }
                            break;
                        case "O":
                            {
                                this.propagationOdeur(x - 1, y, odeur - 1, odeurManger, directionsOdeur);
                            }
                            break;
                        default:
                            break;
                    }
                }
            }
        }
        //Fonction pour savoir où propager l'odeur du dalton
        public List<string> continueOdeurDalton(int x , int y , int odeur)
        {
            List<string> directionsOdeur = new List<string>();
            //On test si là case Nord
            if (this.estLibreNord(x,y) && this.sousSol[this.convertCoordonneeToIndex(x, y - 1)].odeurDePieds < odeur-1)
            {
                directionsOdeur.Add("N");
            }
            if (this.estLibreSud(x, y) && this.sousSol[this.convertCoordonneeToIndex(x, y + 1)].odeurDePieds < odeur - 1)
            {
                directionsOdeur.Add("S");
            }
            if (this.estLibreEst(x, y) && this.sousSol[this.convertCoordonneeToIndex(x + 1 , y)].odeurDePieds < odeur - 1)
            {
                directionsOdeur.Add("E");
            }
            if (this.estLibreOuest(x, y) && this.sousSol[this.convertCoordonneeToIndex(x - 1, y)].odeurDePieds < odeur - 1)
            {
                directionsOdeur.Add("O");
            }
            return directionsOdeur;
        }
        //Fonction pour savoir où propager l'odeur de nourriture
        public List<string> continueOdeurNourriture(int x, int y, int odeur)
        {
            List<string> directionsOdeur = new List<string>();
            //On test si là case Nord
            if (this.estLibreNord(x, y) && this.sousSol[this.convertCoordonneeToIndex(x, y - 1)].odeurDeCuisine < odeur - 1)
            {
                directionsOdeur.Add("N");
            }
            if (this.estLibreSud(x, y) && this.sousSol[this.convertCoordonneeToIndex(x, y + 1)].odeurDeCuisine < odeur - 1)
            {
                directionsOdeur.Add("S");
            }
            if (this.estLibreEst(x, y) && this.sousSol[this.convertCoordonneeToIndex(x + 1, y)].odeurDeCuisine < odeur - 1)
            {
                directionsOdeur.Add("E");
            }
            if (this.estLibreOuest(x, y) && this.sousSol[this.convertCoordonneeToIndex(x - 1, y)].odeurDeCuisine < odeur - 1)
            {
                directionsOdeur.Add("O");
            }
            return directionsOdeur;
        }
        /**
         * 
         *      Fonctions IA de Rantanplan
         * 
         */
         public string startRantanplan()
        {
            //On reste dans la boucle de recherche de Rantanplan tant qu'il n'a pas trouvé Avrel et n'est pas mort
            
            while(!this.rantanplan.trouveAvrel && !this.perdu)
            {
                //On relève les direction possible de son déplacement , ainsi que la présence de dalton autour de rantanplan
                this.setDirectionPossibleRantanplan();
                if(this.rantanplan.trouveAvrel)
                {
                    return "Bien Joué Rantanplan";
                }
                if (this.perdu)
                {
                    return "Rantanplan s'est fait battre par les méchants Daltons";
                }
                //Parmis ces direction on sélectionne celles qui sente le plus fort
                this.choixDirectionPossible();
                //On empèche les demi-tour
                this.directions.Remove(this.rantanplan.provenance);
                //Maintenant l'éducation de rantanplan et son état courant interviennent dans le choix 
                if (this.suivreSonEstomac())
                {
                    this.deplacerRantanplan(this.rantanplan.choixRecordNourriture);
                }
                else
                {
                    this.deplacerRantanplan(this.rantanplan.choixRecordPied);
                }
            }
            return "Relance donc une partie";
        }
        public void setDirectionPossibleRantanplan()
        {
            this.directions.Clear();
            //On test le Nord
            if(this.estLibreNord(this.rantanplan.x, this.rantanplan.y)){
                this.directions.Add("N");
            }
            else if (this.estDansSousSol(this.convertCoordonneeToIndex(this.rantanplan.x,this.rantanplan.y - 1)))
            {
                //Si la case est dans la map , on check si il y a un dalton dessus
                if(this.sousSol[this.convertCoordonneeToIndex(this.rantanplan.x, this.rantanplan.y - 1)].occupePar == "O")
                {
                    //Si c'est Avrel c'est gagné
                    this.rantanplan.trouveAvrel = true;
                }
                else if(this.sousSol[this.convertCoordonneeToIndex(this.rantanplan.x, this.rantanplan.y - 1)].occupePar == "o")
                {
                    //Sinon c'est balo
                    this.perdu = this.rantanplan.perdreUneVieEtMeurt();
                }
            }
            //On test le sud
            if (this.estLibreSud(this.rantanplan.x, this.rantanplan.y)){
                this.directions.Add("S");
            }
            else if (this.estDansSousSol(this.convertCoordonneeToIndex(this.rantanplan.x, this.rantanplan.y + 1)))
            {
                //Si la case est dans la map , on check si il y a un dalton dessus
                if (this.sousSol[this.convertCoordonneeToIndex(this.rantanplan.x, this.rantanplan.y + 1)].occupePar == "O")
                {
                    //Si c'est Avrel c'est gagné
                    this.rantanplan.trouveAvrel = true;
                }
                else if (this.sousSol[this.convertCoordonneeToIndex(this.rantanplan.x, this.rantanplan.y + 1)].occupePar == "o")
                {
                    //Sinon c'est balo
                    this.perdu = this.rantanplan.perdreUneVieEtMeurt();
                }
            }
            //On test le sud
            if (this.estLibreEst(this.rantanplan.x, this.rantanplan.y)){
                this.directions.Add("E");
            }
            else if (this.estDansSousSol(this.convertCoordonneeToIndex(this.rantanplan.x + 1, this.rantanplan.y)))
            {
                //Si la case est dans la map , on check si il y a un dalton dessus
                if (this.sousSol[this.convertCoordonneeToIndex(this.rantanplan.x + 1, this.rantanplan.y)].occupePar == "O")
                {
                    //Si c'est Avrel c'est gagné
                    this.rantanplan.trouveAvrel = true;
                }
                else if (this.sousSol[this.convertCoordonneeToIndex(this.rantanplan.x + 1, this.rantanplan.y)].occupePar == "o")
                {
                    //Sinon c'est balo
                    this.perdu = this.rantanplan.perdreUneVieEtMeurt();
                }
            }
            //On test le sud
            if (this.estLibreOuest(this.rantanplan.x, this.rantanplan.y)){
                this.directions.Add("O");
            }
            else if (this.estDansSousSol(this.convertCoordonneeToIndex(this.rantanplan.x, this.rantanplan.y - 1)))
            {
                //Si la case est dans la map , on check si il y a un dalton dessus
                if (this.sousSol[this.convertCoordonneeToIndex(this.rantanplan.x - 1, this.rantanplan.y)].occupePar == "O")
                {
                    //Si c'est Avrel c'est gagné
                    this.rantanplan.trouveAvrel = true;
                }
                else if (this.sousSol[this.convertCoordonneeToIndex(this.rantanplan.x - 1, this.rantanplan.y)].occupePar == "o")
                {
                    //Sinon c'est balo
                    this.perdu = this.rantanplan.perdreUneVieEtMeurt();
                }
            }
        }
        public void choixDirectionPossible()
        {
            this.rantanplan.ptsRecordPied = 0;
            this.rantanplan.choixRecordPied = "";
            this.rantanplan.ptsRecordNourriture = 0;
            this.rantanplan.choixRecordNourriture = "";
            foreach (string direction in this.directions)
            {
                switch (direction)
                {
                    case "N":
                        {
                            //Si la case pue plus les pieds que les autres observé alors on note le record et la direction
                            if(this.sousSol[this.convertCoordonneeToIndex(this.rantanplan.x, this.rantanplan.y - 1)].odeurDePieds > this.rantanplan.ptsRecordPied)
                            {
                                this.rantanplan.ptsRecordPied = this.sousSol[this.convertCoordonneeToIndex(this.rantanplan.x, this.rantanplan.y - 1)].odeurDePieds;
                                this.rantanplan.choixRecordPied = direction;
                            }
                            //Si la case pue plus la bouffe que les autres observé alors on note le record et la direction
                            if (this.sousSol[this.convertCoordonneeToIndex(this.rantanplan.x, this.rantanplan.y - 1)].odeurDeCuisine > this.rantanplan.ptsRecordNourriture)
                            {
                                this.rantanplan.ptsRecordNourriture = this.sousSol[this.convertCoordonneeToIndex(this.rantanplan.x, this.rantanplan.y - 1)].odeurDeCuisine;
                                this.rantanplan.choixRecordNourriture = direction;
                            }
                        }
                        break;
                    case "S":
                        {
                            //Si la case pue plus les pieds que les autres observé alors on note le record et la direction
                            if (this.sousSol[this.convertCoordonneeToIndex(this.rantanplan.x, this.rantanplan.y + 1)].odeurDePieds > this.rantanplan.ptsRecordPied)
                            {
                                this.rantanplan.ptsRecordPied = this.sousSol[this.convertCoordonneeToIndex(this.rantanplan.x, this.rantanplan.y + 1)].odeurDePieds;
                                this.rantanplan.choixRecordPied = direction;
                            }
                            //Si la case pue plus la bouffe que les autres observé alors on note le record et la direction
                            if (this.sousSol[this.convertCoordonneeToIndex(this.rantanplan.x, this.rantanplan.y - 1)].odeurDeCuisine > this.rantanplan.ptsRecordNourriture)
                            {
                                this.rantanplan.ptsRecordNourriture = this.sousSol[this.convertCoordonneeToIndex(this.rantanplan.x, this.rantanplan.y + 1)].odeurDeCuisine;
                                this.rantanplan.choixRecordNourriture = direction;
                            }
                        }
                        break;
                    case "E":
                        {
                            //Si la case pue plus les pieds que les autres observé alors on note le record et la direction
                            if (this.sousSol[this.convertCoordonneeToIndex(this.rantanplan.x + 1, this.rantanplan.y)].odeurDePieds > this.rantanplan.ptsRecordPied)
                            {
                                this.rantanplan.ptsRecordPied = this.sousSol[this.convertCoordonneeToIndex(this.rantanplan.x +1, this.rantanplan.y)].odeurDePieds;
                                this.rantanplan.choixRecordPied = direction;
                            }
                            //Si la case pue plus la bouffe que les autres observé alors on note le record et la direction
                            if (this.sousSol[this.convertCoordonneeToIndex(this.rantanplan.x + 1, this.rantanplan.y)].odeurDeCuisine > this.rantanplan.ptsRecordNourriture)
                            {
                                this.rantanplan.ptsRecordNourriture = this.sousSol[this.convertCoordonneeToIndex(this.rantanplan.x +1, this.rantanplan.y + 1)].odeurDeCuisine;
                                this.rantanplan.choixRecordNourriture = direction;
                            }
                        }
                        break;
                    case "O":
                        {
                            //Si la case pue plus les pieds que les autres observé alors on note le record et la direction
                            if (this.sousSol[this.convertCoordonneeToIndex(this.rantanplan.x - 1, this.rantanplan.y)].odeurDePieds > this.rantanplan.ptsRecordPied)
                            {
                                this.rantanplan.ptsRecordPied = this.sousSol[this.convertCoordonneeToIndex(this.rantanplan.x -1, this.rantanplan.y)].odeurDePieds;
                                this.rantanplan.choixRecordPied = direction;
                            }
                            //Si la case pue plus la bouffe que les autres observé alors on note le record et la direction
                            if (this.sousSol[this.convertCoordonneeToIndex(this.rantanplan.x -1, this.rantanplan.y)].odeurDeCuisine > this.rantanplan.ptsRecordNourriture)
                            {
                                this.rantanplan.ptsRecordNourriture = this.sousSol[this.convertCoordonneeToIndex(this.rantanplan.x -1, this.rantanplan.y)].odeurDeCuisine;
                                this.rantanplan.choixRecordNourriture = direction;
                            }
                        }
                        break;
                }
            }
        }
        public bool suivreSonEstomac()
        {
            if(this.rantanplan.inteligent && this.rantanplan.vies < this.rantanplan.viesMax)
            {
                //S'il s'est déjà fait cogné et apprend de ses erreur il va naturellement suivre l'odeur de nourriture
                return true;
            }
            // Sinon il reste tiraillé entre son estomac et son dressage comme un bon clebar
            if(this.random.Next(this.rantanplan.obeissance + this.rantanplan.faim) >= this.rantanplan.faim){
                //On fait entrée en jeu les paramètre courant du conditionnement de rantanplan
                return false;
            }
            return true;
        }
        public void deplacerRantanplan(string direction)
        {
            switch (direction)
            {
                case "N":
                    {
                        this.sousSol[this.convertCoordonneeToIndex(this.rantanplan.x, this.rantanplan.y - 1)].soccupe("R");
                        this.rantanplan.allerAuNord();
                    }
                    break;
                case "S":
                    {
                        this.sousSol[this.convertCoordonneeToIndex(this.rantanplan.x, this.rantanplan.y + 1)].soccupe("R");
                        this.rantanplan.allerAuSud();
                    }
                    break;
                case "E":
                    {
                        this.sousSol[this.convertCoordonneeToIndex(this.rantanplan.x + 1, this.rantanplan.y)].soccupe("R");
                        this.rantanplan.allerAlEst();
                    }
                    break;
                case "O":
                    {
                        this.sousSol[this.convertCoordonneeToIndex(this.rantanplan.x - 1, this.rantanplan.y)].soccupe("R");
                        this.rantanplan.allerAlOuest();
                    }
                    break;
                default:
                    break;
            }
        }
    }
}
