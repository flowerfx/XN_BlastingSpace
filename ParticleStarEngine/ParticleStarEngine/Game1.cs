using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Media;

namespace ParticleStarEngine
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        ParticleEngine particleEngine;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            List<Texture2D> textures = new List<Texture2D>();
            textures.Add(Content.Load<Texture2D>("circle"));
            textures.Add(Content.Load<Texture2D>("star"));
            textures.Add(Content.Load<Texture2D>("diamond"));
            particleEngine = new ParticleEngine(textures, new Vector2(400, 240));
        }

        protected override void UnloadContent()
        {
        }
        int step = 0;
        int stickCount = 0;
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            Vector2 PosEmittedLocation = new Vector2(0, 0);
            stickCount++;
           // if (stickCount % 10 == 0)
            step++;
            PosEmittedLocation.X = step;
            PosEmittedLocation.Y = (float)(100 * Math.Cos((double)((float)step / 180f) * Math.PI)) + 200;
            if (step > 800)
            {
                step = 0;
                stickCount = 0;
            }
            Random temp = new Random();
            particleEngine.EmitterLocation = new Vector2(Mouse.GetState().X, Mouse.GetState().Y);
            particleEngine.Update();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            particleEngine.Draw(spriteBatch);

            base.Draw(gameTime);
        }
    }
}
