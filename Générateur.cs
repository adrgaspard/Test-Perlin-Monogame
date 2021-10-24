using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Procedural
{
    public class Générateur
    {
        public int LongueurX { get; private set; }
        public int LongueurY { get; private set; }
        public float Résolution { get; private set; }
        public int NbOctaves { get; private set; }
        public float Persistance { get; private set; }
        public int Imperfections { get; private set; }
        public long Seed { get; private set; }
        public ushort[] TablePermutation { get; private set; }
        private GraphicsDevice GraphicsDevice { get; set; }
        private TerrainType[] Régions { get; set; }
        public DrawMode ModeDessin { get; private set; }
        public NormalisationMode ModeNormalisation { get; private set; }

        private Random64 R64 { get; set; }

        public Générateur(GraphicsDevice graphicsDevice, long seed, int longueurX, int longueurY, float résolution, DrawMode modeDessin, NormalisationMode normalisation)
        {
            GraphicsDevice = graphicsDevice;
            R64 = new Random64(seed);
            Seed = seed;
            LongueurX = longueurX;
            LongueurY = longueurY;
            Résolution = résolution;
            NbOctaves = 4;
            Persistance = 0.5f;
            Imperfections = 2;
            Régions = new TerrainType[]
            {
                new TerrainType("Océan profond", 0f, Color.DarkSlateBlue),
                new TerrainType("Océan", 0.30f, Color.CornflowerBlue),
                new TerrainType("Terres maritimes", 0.40f, Color.LightYellow),
                new TerrainType("Plaine", 0.45f, Color.ForestGreen),
                new TerrainType("Terres réculées", 0.55f, Color.DarkGreen),
                new TerrainType("Montagne", 0.65f, Color.DarkGray),
                new TerrainType("Haute montagne", 0.80f, Color.DimGray),
                new TerrainType("Sommet", 0.95f, Color.White),
            };
            ModeDessin = modeDessin;
            ModeNormalisation = normalisation;
            TablePermutation = Noise.GénérerTablePermutation(R64, 256);
        }

        public Texture2D GénérerMap(Vector2 coordonnées)
        {
            float[,] heightmap = Noise.HeightMap(TablePermutation, coordonnées, LongueurX, LongueurY, Résolution, NbOctaves, Persistance, Imperfections, NormalisationMode.Global);
            Color[] couleurmap = new Color[LongueurX * LongueurY];

            if (ModeDessin == DrawMode.CouleurMap)
            {
                for (int y = 0; y < LongueurY; y++)
                {
                    for (int x = 0; x < LongueurX; x++)
                    {
                        float hauteurActuelle = heightmap[x, y];
                        for (int i = 0; i < Régions.Length; i++)
                        {
                            if (hauteurActuelle >= Régions[i].Hauteur)
                            {
                                couleurmap[y * LongueurX + x] = Régions[i].Couleur;
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                }
                return Painter.CouleurMap2Texture(GraphicsDevice, couleurmap, LongueurX, LongueurY);
            }
            else if (ModeDessin == DrawMode.HeightMap)
            {
                return Painter.HeightMap2Texture(GraphicsDevice, heightmap, LongueurX, LongueurY);
            }
            throw new NotImplementedException("Le mode de dessin choisi n'est pas implémenté");
        }
    }
}