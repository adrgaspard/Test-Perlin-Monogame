using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Procedural
{
    public static class Painter
    {
        public static Texture2D CouleurMap2Texture(GraphicsDevice graphicsDevice, Color[] couleurMap, int longueurX, int longueurY)
        {
            Texture2D texture = new Texture2D(graphicsDevice, longueurX, longueurY);
            texture.SetData(couleurMap);
            return texture;
        }

        public static Texture2D HeightMap2Texture(GraphicsDevice graphicsDevice, float[,] heightMap, int longueurX, int longueurY)
        {
            Color[] couleurMap = new Color[longueurX * longueurY];
            for(int y = 0; y < longueurY; y++)
            {
                for(int x = 0; x < longueurX; x++)
                {
                    couleurMap[y * longueurX + x] = Color.Lerp(Color.Black, Color.White, heightMap[x, y]);
                }
            }
            return CouleurMap2Texture(graphicsDevice, couleurMap, longueurX, longueurY);
        }
    }
}
