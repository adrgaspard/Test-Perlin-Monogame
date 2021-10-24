using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Procedural
{
    public class TestProcedural : Game
    {
        // Initialisation de quelques variables;
        Random64 r64;
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Générateur générateur;


        // Variables modifiables
        int imagePerlin_largeur = 200;
        int imagePerlin_hauteur = 200;
        float résolution = 100f;
        int nbOctaves = 4;
        float persistance = 0.5f;
        float imperfections = 2;


        public TestProcedural()
        {
            r64 = new Random64((long)(Randomizer.Random.NextDouble() * long.MaxValue));
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            
        }

        protected override void Initialize()
        {
            base.Initialize();
            générateur = new Générateur(graphics.GraphicsDevice, (long)(Randomizer.Random.NextDouble() * long.MaxValue), imagePerlin_largeur, imagePerlin_hauteur, résolution, DrawMode.CouleurMap, NormalisationMode.Global);
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            // Dessiner l'image
            spriteBatch.Begin();

            spriteBatch.Draw(générateur.GénérerMap(new Vector2(0, 0)), new Vector2(0, 0));
            spriteBatch.Draw(générateur.GénérerMap(new Vector2(200, 0)), new Vector2(202, 0));
            spriteBatch.Draw(générateur.GénérerMap(new Vector2(400, 0)), new Vector2(404, 0));
            spriteBatch.Draw(générateur.GénérerMap(new Vector2(0, 200)), new Vector2(0, 202));
            spriteBatch.Draw(générateur.GénérerMap(new Vector2(200, 200)), new Vector2(202, 202));
            spriteBatch.Draw(générateur.GénérerMap(new Vector2(400, 200)), new Vector2(404, 202));

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
