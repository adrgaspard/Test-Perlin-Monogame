using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Diagnostics;
using Microsoft.Xna.Framework;

namespace Procedural
{
    static class Noise
    {
        // Table de permutation (STUB).
        //static readonly ushort[] perm = {151, 160, 137, 91, 90, 15, 131, 13, 201, 95, 96, 53, 194, 233, 7, 225, 140, 36, 103, 30, 69, 142, 8, 99, 37, 240, 21, 10, 23, 190, 6, 148, 247, 120, 234, 75, 0, 26, 197, 62, 94, 252, 219, 203, 117, 35, 11, 32, 57, 177, 33, 88, 237, 149, 56, 87, 174, 20, 125, 136, 171, 168, 68, 175, 74, 165, 71, 134, 139, 48, 27, 166, 77, 146, 158, 231, 83, 111, 229, 122, 60, 211, 133, 230, 220, 105, 92, 41, 55, 46, 245, 40, 244, 102, 143, 54, 65, 25, 63, 161, 1, 216, 80, 73, 209, 76, 132, 187, 208, 89, 18, 169, 200, 196, 135, 130, 116, 188, 159, 86, 164, 100, 109, 198, 173, 186, 3, 64, 52, 217, 226, 250, 124, 123, 5, 202, 38, 147, 118, 126, 255, 82, 85, 212, 207, 206, 59, 227, 47, 16, 58, 17, 182, 189, 28, 42, 223, 183, 170, 213, 119, 248, 152, 2, 44, 154, 163, 70, 221, 153, 101, 155, 167, 43, 172, 9, 129, 22, 39, 253, 19, 98, 108, 110, 79, 113, 224, 232, 178, 185, 112, 104, 218, 246, 97, 228, 251, 34, 242, 193, 238, 210, 144, 12, 191, 179, 162, 241, 81, 51, 145, 235, 249, 14, 239, 107, 49, 192, 214, 31, 181, 199, 106, 157, 184, 84, 204, 176, 115, 121, 50, 45, 127, 4, 150, 254, 138, 236, 205, 93, 222, 114, 67, 29, 24, 72, 243, 141, 128, 195, 78, 66, 215, 61, 156, 180};

        // Constante qui évite un même calcul plusieurs fois.
        static readonly float unit = 1.0f / (float)Math.Sqrt(2);

        // Constante (tableau des 8 vecteurs directionnels).
        static float[,] gradient2D = { { unit, unit }, { -unit, unit }, { unit, -unit }, { -unit, -unit }, { 1, 0 }, { -1, 0 }, { 0, 1 }, { 0, -1 } };

        /// <summary>
        /// Fonction de génération de table de permutation.
        /// </summary>
        /// <param name="tailleTable">Taille de la table à renvoyer.</param>
        /// <param name="verbose">Défini si des logs sont écrites dans la sortie Debug ou non.</param>
        /// <returns>Renvoi la table de permutation générée.</returns>
        internal static ushort[] GénérerTablePermutation(Random64 random, int tailleTable, bool verbose = false)
        {
            // Déclaration des variables
            int rangTiré;
            ushort valeur;
            ushort[] table = null;
            List<ushort> possibilités;
            DateTime début = DateTime.Now;

            while (!NiveauQualité(table))
            {
                // Initialisation
                possibilités = new List<ushort>();
                table = new ushort[tailleTable];
                for (int i = 0; i < tailleTable; i++)
                {
                    possibilités.Add((ushort)i);
                }

                // Assignation des valeurs
                for (int i = tailleTable; i > 0; i--)
                {
                    rangTiré = random.Next(0, i);
                    valeur = possibilités.ElementAt(rangTiré);
                    possibilités.RemoveAt(rangTiré);
                    table[tailleTable - i] = valeur;
                }
            }

            // Annonce des résultats
            if (verbose)
            {
                DateTime fin = DateTime.Now;
                TimeSpan durée = fin - début;
                Debug.WriteLine("Table de permutation valide trouvée :");
                for (ushort i = 0; i < tailleTable; i++)
                {
                    Debug.WriteLine($"[{i}] \t-> {table[i]}");
                }
                Debug.WriteLine($"Durée du calcul : {durée.TotalMilliseconds} ms.");
            }
            return table;
        }

        /// <summary>
        /// Teste la qualité d'une table de permutation. A tester avec minimum une taille de 64 /!\.
        /// </summary>
        /// <param name="table">Table de permutation à tester.</param>
        /// <returns>Renvoi vrai si la table de permutation est de qualité suffisante.</returns>
        private static bool NiveauQualité(ushort[] table)
        {
            // Vérification que la table ne soit pas nulle.
            if (table == null)
            {
                return false;
            }

            // Initialisation
            int tailleTable = table.Length;

            // Règle 1 : Chaque index de la table ne doit pas avoir pour valeur lui même.
            for (int i = 0; i < tailleTable; i++)
            {
                if (table[i] == i)
                {
                    return false;
                }
            }

            // Règle 2 : Il ne doit pas y avoir de suite de numéro.
            int valeurPrécédente = table[tailleTable - 1];
            for (int i = 0; i < tailleTable; i++)
            {
                if (table[i] == valeurPrécédente - 1 || table[i] == valeurPrécédente + 1)
                {
                    return false;
                }
                valeurPrécédente = table[i];
            }
            if (table[tailleTable - 1] == 0)
            {
                return false;
            }

            // Règle 3 : Il ne doit pas y avoir de cycles de valeurs en fonction des index.
            List<uint> testCycle = new List<uint>();
            testCycle.Add(table[0]);
            while (testCycle.Count < tailleTable - 1)
            {
                if (testCycle.Contains(table[testCycle.Last()]))
                {
                    return false;
                }
                testCycle.Add(table[testCycle.Last()]);
            }
            if (table[testCycle.Last()] != 0)
            {
                return false;
            }

            // Règle 4 : Les différences de sauts ne doivent pas être similaires.
            int différencePrécédente = 0;
            int différenceActuelle = 0;
            int sommePrécédente = 0;
            int sommeActuelle = 0;
            for (int i = 0; i < tailleTable; i++)
            {
                sommeActuelle += table[i];
                différenceActuelle = sommeActuelle - sommePrécédente;
                if (différenceActuelle == différencePrécédente)
                {
                    return false;
                }
                sommePrécédente = sommeActuelle;
                différencePrécédente = différenceActuelle;
            }
            return true;
        }

        /// <summary>
        /// Génère un bruit de perlin 2D, pour des coordonnées et une résolution donnée.
        /// </summary>
        /// <param name="x">Coordonnée X.</param>
        /// <param name="y">Coordonnée Y.</param>
        /// <param name="résolution">Résolution (zoom).</param>
        /// <returns>Renvoi un float entre -1 et 1.</returns>
        private static float PerlinNoise2D(ushort[] tablePermutation, float x, float y, float résolution)
        {
            //--- Initialisation ---
            float tmpX, tmpY;
            int coordX, coordY, masqueX, masqueY;
            int[] valeurs = new int[4];
            float[] valeursPondérés = new float[4];
            float coefLerpX, coefLerpY, lissageX, lissageY;

            //--- On récupère les positions de la grille associée à (x,y) une fois l'adaptation faite ---
            x /= résolution;
            y /= résolution;
            coordX = (int) x;
            coordY = (int) y;

            //--- On fait masque sur les coordonnées avec un modulo 256 ---
            masqueX = coordX & 255;
            masqueY = coordY & 255;

            //--- On détermine les valeurs du gradient2D à utiliser en désordre ---
            valeurs[0] = tablePermutation[(masqueX + tablePermutation[masqueY]) % 256] % 8;
            valeurs[1] = tablePermutation[(masqueX + 1 + tablePermutation[masqueY]) % 256] % 8;
            valeurs[2] = tablePermutation[(masqueX + tablePermutation[(masqueY + 1) % 256]) % 256] % 8;
            valeurs[3] = tablePermutation[(masqueX + 1 + tablePermutation[(masqueY + 1) % 256]) % 256] % 8;

            // --- On récupère les valeurs et on les pondère ---

            tmpX = x - coordX;
            tmpY = y - coordY;
            valeursPondérés[0] = gradient2D[valeurs[0], 0] * tmpX + gradient2D[valeurs[0], 1] * tmpY;

            tmpX = x - (coordX + 1);
            tmpY = y - coordY;
            valeursPondérés[1] = gradient2D[valeurs[1], 0] * tmpX + gradient2D[valeurs[1], 1] * tmpY;

            tmpX = x - coordX;
            tmpY = y - (coordY + 1);
            valeursPondérés[2] = gradient2D[valeurs[2], 0] * tmpX + gradient2D[valeurs[2], 1] * tmpY;

            tmpX = x - (coordX + 1);
            tmpY = y - (coordY + 1);
            valeursPondérés[3] = gradient2D[valeurs[3], 0] * tmpX + gradient2D[valeurs[3], 1] * tmpY;


            //--- On applique ensuite un lissage sur les valeurs pondérés ---

            //Calcul du coefficiant d'interpolation selon X
            tmpX = x - coordX;
            coefLerpX = 3 * tmpX * tmpX - 2 * tmpX * tmpX * tmpX;

            //Lissage des valeurs 2 à 2
            lissageX = valeursPondérés[0] + coefLerpX * (valeursPondérés[1] - valeursPondérés[0]);
            lissageY = valeursPondérés[2] + coefLerpX * (valeursPondérés[3] - valeursPondérés[2]);

            //Calcul du coefficiant d'interpolation selon Y
            tmpY = y - coordY;
            coefLerpY = 3 * tmpY * tmpY - 2 * tmpY * tmpY * tmpY;

            //Lissage final des deux valeurs lissé précédemment
            return lissageX + coefLerpY * (lissageY - lissageX);
        }

        /// <summary>
        /// Génère une HeightMap qui permet de construire une carte par la suite.
        /// </summary>
        /// <param name="tablePermutation">Table de permutation à utiliser.</param>
        /// <param name="coordonnées">Coordonnées du point le plus en haut et le plus à gauche de la HeightMap.</param>
        /// <param name="longueurX">Longueur en X de la HeightMap.</param>
        /// <param name="longueurY">Longueur en Y de la HeightMap.</param>
        /// <param name="résolution">Niveau de zoom sur la HeightMap.</param>
        /// <param name="nbOctaves">Nombre d'octaves de la HeightMap (= niveau de précision).</param>
        /// <param name="persistance">Niveau de persistance des "imperfections de la HeightMap" à travers les octaves
        ///                          (compris entre 0 et 1).</param>
        /// <param name="imperfections">Fréquence des imperfections./param>
        /// <param name="normalisation">Mode de normalisation (global ou local).</param>
        /// <returns></returns>
        public static float[,] HeightMap(ushort[] tablePermutation, Vector2 coordonnées, int longueurX, int longueurY, float résolution, int nbOctaves, float persistance, float imperfections, NormalisationMode normalisation)
        {
            if (longueurX <= 0 || longueurY <= 0 || résolution <= 0 || nbOctaves <= 0)
            {
                throw new Exception("Les paramètres : longueurX, longueurY, résolution et nbOctaves doivent tous être supérieurs à 0.");
            }

            //--- Initialisation ---
            float[,] heightmap = new float[longueurX, longueurY]; // Le tableau à compléter.
            float amplitude = 1; // Paramètre propre à chaque octave, plus l'amplitude est élevée est plus les reliefs sont importants.
            float fréquence; // Paramètre propre à chaque octave, permet de faire un relief différent à chaque octave.
            float hauteur; // Valeur qui completera la heightmap.
            float hauteurMax = float.MinValue; // Utilisée pour le lissage, permet de ramener toutes les valeur au dessus de 1 à 1.
            float hauteurMin = float.MaxValue; // Utilisée pour le lissage, permet de ramener toutes les valeur en dessous de 0 à 0.
            float tmpX, tmpY; // Valeurs à calculer en fonction de la fréquence et des coordonnées qui appelera le bruit de perlin.
            float perlin; // Valeur issue du bruit de Perlin.
            float hauteurMaximumPossible = 0; // Permet de rendre les chunks cohérents entre eux.

            for(int i = 0; i < nbOctaves; i++)
            {
                hauteurMaximumPossible += amplitude;
                amplitude *= persistance;
            }

            //--- Génération des points de la map ---
            for(int y = 0; y < longueurY; y++)
            {
                for(int x = 0; x < longueurX; x++)
                {
                    // Initialisation du cycle
                    amplitude = 1;
                    fréquence = 1;
                    hauteur = 0;

                    // Calcul des valeurs de la map
                    for(int o = 0; o < nbOctaves; o++)
                    {
                        tmpX = (x + coordonnées.X) * fréquence;
                        tmpY = (y + coordonnées.Y) * fréquence;
                        perlin = PerlinNoise2D(tablePermutation, tmpX, tmpY, résolution);

                        hauteur += perlin * amplitude;
                        amplitude *= persistance;
                        fréquence *= imperfections;
                    }

                    // Redéfinission potentielle des deux valeurs extrêmes
                    if(hauteur > hauteurMax)
                    {
                        hauteurMax = hauteur;
                    }
                    if(hauteur < hauteurMin)
                    {
                        hauteurMin = hauteur;
                    }
                    heightmap[x, y] = hauteur;
                }
            }

            //--- Lissage des valeurs (elles seront toutes entre 0 et 1 après ça) ---
            if(normalisation == NormalisationMode.Local) // Mode local
            {
                float écart = hauteurMax - hauteurMin;
                for (int y = 0; y < longueurY; y++)
                {
                    for (int x = 0; x < longueurX; x++)
                    {
                        heightmap[x, y] = (heightmap[x, y] - hauteurMin) / écart;
                    }
                }
            }
            else // Mode Global
            {
                float hauteurNormalisée;
                for (int y = 0; y < longueurY; y++)
                {
                    for (int x = 0; x < longueurX; x++)
                    {
                        // Pour la valeur à droite : plus elle est grande et plus les hauteurs seront basses, plus elle est petite et plus des blocs auront atteint la valeur maximum.
                        hauteurNormalisée = (heightmap[x, y] + 1) / (2f * hauteurMaximumPossible / 1.7f);
                        heightmap[x, y] = Math.Clamp(hauteurNormalisée, 0, int.MaxValue);
                    }
                }
            }
            

            return heightmap;
        }
    }
}
