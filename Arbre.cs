using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace hufman
{
    class Arbre
    {
        private List<Noeud> noeuds = new List<Noeud>();
        public Noeud Root { get; set; }
        public Dictionary<char, int> Frequencies = new Dictionary<char, int>();

        public void Build(string source)
        {
            for (int i = 0; i < source.Length; i++)
            {
                if (!Frequencies.ContainsKey(source[i]))
                {
                    Frequencies.Add(source[i], 0);
                }

                Frequencies[source[i]]++;
            }

            foreach (KeyValuePair<char, int> symbol in Frequencies)
            {
                noeuds.Add(new Noeud() { Symbole = symbol.Key, Frequence = symbol.Value });
            }

            while (noeuds.Count > 1)
            {
                List<Noeud> orderednoeuds = noeuds.OrderBy(Noeud => Noeud.Frequence).ToList<Noeud>();

                if (orderednoeuds.Count >= 2)
                {
                    // Take first two items
                    List<Noeud> taken = orderednoeuds.Take(2).ToList<Noeud>();

                    // Create a parent Noeud by combining the frequencies
                    Noeud parent = new Noeud()
                    {
                        Symbole = '*',
                        Frequence = taken[0].Frequence + taken[1].Frequence,
                        Gauche = taken[0],
                        Droite = taken[1]
                    };

                    noeuds.Remove(taken[0]);
                    noeuds.Remove(taken[1]);
                    noeuds.Add(parent);
                }

                this.Root = noeuds.FirstOrDefault();

            }

        }

        public BitArray Encode(string source)
        {
            List<bool> encodedSource = new List<bool>();

            for (int i = 0; i < source.Length; i++)
            {
                List<bool> encodedSymbol = this.Root.Stockage(source[i], new List<bool>());
                encodedSource.AddRange(encodedSymbol);
            }

            BitArray bits = new BitArray(encodedSource.ToArray());

            return bits;
        }

        public string Decode(BitArray bits)
        {
            Noeud current = this.Root;
            string decoded = "";

            foreach (bool bit in bits)
            {
                if (bit)
                {
                    if (current.Droite != null)
                    {
                        current = current.Droite;
                    }
                }
                else
                {
                    if (current.Gauche != null)
                    {
                        current = current.Gauche;
                    }
                }

                if (IsLeaf(current))
                {
                    decoded += current.Symbole;
                    current = this.Root;
                }
            }

            return decoded;
        }

        public bool IsLeaf(Noeud Noeud)
        {
            return (Noeud.Gauche == null && Noeud.Droite == null);
        }
    }
}
