using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Procedural
{
    /// <summary>
    /// Cette classe sert à faire un random basé sur une seed long.
    /// Il ne doit pas y avoir plus d'une occurence d'un objet de cette classe.
    /// </summary>
    public class Random64
    {
        private Random r0;
        private Random r1;

        public string Template { get; private set; }
        private byte templateIterator; 

        /// <summary>
        /// Construit un générateur de nombre aléatoire basé sur la classe Random,
        /// son but est de fonctionner en fonction d'une seed longue.
        /// </summary>
        /// <param name="seed">Seed qui va déterminer le générateur.</param>
        public Random64(long seed)
        {
            // Initialiser les paramètres
            StringBuilder templateBuilder = new StringBuilder();
            templateIterator = 63;

            // Déterminer les deux randoms en fonction de la seed
            bool positive = seed == Math.Abs(seed);
            seed = Math.Abs(seed);
            if(positive)
            {
                r0 = new Random((int)(seed % int.MaxValue));
                r1 = new Random((int)((seed / int.MaxValue) % int.MaxValue));
            }
            else
            {
                r1 = new Random((int)(seed % int.MaxValue));
                r0 = new Random((int)((seed / int.MaxValue) % int.MaxValue));
            }

            // Calculer le template d'utilisation des randoms.
            long tmp = (long)(new BigInteger(r1.Next() * r0.Next() * r1.Next() * r0.Next() * r1.Next()) % long.MaxValue);
            byte pos = 63;
            for(byte i = 0; i < 64; i++)
            {
                if ((tmp & (1 << i)) != 0)
                {
                    templateBuilder.Append('1');
                }
                else
                {
                    templateBuilder.Append('0');
                }
                pos--;
            }
            Template = templateBuilder.ToString();
        }

        /// <summary>
        /// Méthode permettant de retourner le random à utiliser tout en parcourant le template.
        /// </summary>
        /// <returns>Renvoi le random à utiliser.</returns>
        private Random ChoisirRandom()
        {
            if(templateIterator == 63)
            { 
                templateIterator = 0;
            }
            else
            {
                templateIterator++;
            }
            if (Template.ElementAt(templateIterator) == '0')
            {
                return r0;
            }
            return r1;
        }

        /// <summary>
        /// Permet de générer un entier aléatoire non négatif signé sur 32 bits.
        /// </summary>
        /// <returns>Renvoi l'entier aléatoire.</returns>
        public int Next()
        {
            return Next(0, int.MaxValue);
        }

        /// <summary>
        ///  Permet de générer un entier aléatoire non négatif signé sur 32 bits, compris entre deux valeurs.
        /// </summary>
        /// <param name="minValue">Valeur minimum de l'entier.</param>
        /// <param name="maxValue">Valeur maximum de l'entier.</param>
        /// <returns>Renvoi l'entier aléatoire.</returns>
        public int Next(int minValue, int maxValue)
        {
            return ChoisirRandom().Next(minValue, maxValue);
        }
    }
}
