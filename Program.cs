using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.IO;

namespace hufman
{
    class Program
    {
        public static string val = "";
        public static BitArray encoded;
        static void Main(string[] args)
        {
            string fichier;
            bool k = true;
            Arbre arbre = new Arbre();
            while (k == true )
            {
                Console.Write("\n1.Entrer le texte\n2.Entrer l'adresse complet du fichier texte\n3.Compresser\n4.Decompresser\n5.Quiter\nFaites votre choix : ");
                int choix = int.Parse(Console.ReadLine());
                if(choix == 1)
                {
                    Console.WriteLine("\nSVP Entrer une chaine de caractère : ");
                    string input = Console.ReadLine();
                    arbre.Build(input);
                    encoded = arbre.Encode(input);
                    val = input;
                }
                if(choix == 2)
                {
                    List<string> lignes;
                    lignes = new List<string>();
                    Console.Write("\nSVP Entrer le chemain d'accès du fichier : ");
                    string fichiers = Console.ReadLine();
                    Console.Write("Entrer le nom du fichier : ");
                    string nom = Console.ReadLine();
                    fichier = fichiers+nom+ ".txt";
                    if (!File.Exists(fichier))
                    {
                        Console.WriteLine("\nLe fichier n'existe pas");
                    }
                    string fichier1 = fichier + "Compresser.txt";
                    StreamReader lecture = File.OpenText(fichier);
                    
                    while(lecture.Peek() > 0)
                    {
                        lignes.Add(lecture.ReadLine());
                    } 
                    lecture.Close();
                    StreamWriter ecritureCompresser = File.CreateText(fichier1);
                    for (int i=0; i<lignes.Count; i++)
                    {
                        arbre.Build(lignes[i]);
                        encoded = arbre.Encode(lignes[i]);
                        foreach (bool bit in encoded)
                        {
                            ecritureCompresser.Write((bit ? 1 : 0) + "");
                        }
                        ecritureCompresser.WriteLine("");
                    }
                    ecritureCompresser.Close();
                }
                
                if (choix == 3)
                {
                    if(val != "")
                    {
                        Console.Write("Compresser : ");
                        foreach (bool bit in encoded)
                        {
                            Console.Write((bit ? 1 : 0) + "");
                        }
                        Console.Write("\n");
                    }
                    else
                    {
                        Console.WriteLine("Veillez entre un texte à compresser");
                    }
                }
                if (choix == 4)
                {
                    if (val != "")
                    {
                        // Decompreser
                        string decoded = arbre.Decode(encoded);

                        Console.WriteLine("Decompresser : " + decoded + '\n');
                    }
                    else
                    {
                        Console.WriteLine("Veillez compresser un texte pour pouvoir le decompresser");
                    }
                    
                }
                if(choix == 5)
                {
                    Console.Write("Vouler-vous vraiment quiter (y/n) : ");
                    char c = char.Parse(Console.ReadLine());
                    if(c == 'y' || c == 'Y')
                    {
                        k = false;
                    }
                    else if(c == 'n' || c == 'N')
                    {
                        k = true;
                    }
                }
            }
            

            Console.ReadLine();
        }
    }
}
