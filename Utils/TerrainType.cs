using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Procedural
{
    public struct TerrainType
    {
        public string Nom { get; private set; }
        public float Hauteur { get; private set; }
        public Color Couleur { get; private set; }

        public TerrainType(string nom, float hauteur, Color couleur)
        {
            Nom = nom;
            Hauteur = hauteur;
            Couleur = couleur;
        }
    }
}
