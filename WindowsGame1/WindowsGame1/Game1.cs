using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace WindowsGame1
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        TilingMap map;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);

            graphics.IsFullScreen = false;
            graphics.PreferredBackBufferWidth = 768;
            graphics.PreferredBackBufferHeight = 576;

            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        List<My2DSprite> sprites;


        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            Global.Content = this.Content; // export this one to every component
            this.IsMouseVisible = true;
            sprites = new List<My2DSprite>();
            CreateAnAngel(100.0f, 200.0f);
            // CreateAnIsland(0.0f, 0.0f);

            string[] strTextures = {"Water", "Grass", "Highland", "Snow"};
            int[,] MapData = new int[10, 20];
            Random r = new Random();

            for (int i=0; i<10; i++) {
                for (int j = 0; j < 20; j++)
                {
                    MapData[i, j] = r.Next() % 4;
                }
            }

            // map = new Map(3, 5, 800, 600, @"Map\MapFragment");
            map = new TilingMap(10, 20, 100, 100, strTextures, MapData);
        }

        private void CreateAnIsland(float left, float top)
        {
            My2DSprite temp;
            List<Texture2D> textures = new List<Texture2D>();
            textures.Add(this.Content.Load<Texture2D>(@"Sprite\Island\Lush_Island"));
            temp = new My2DSprite(textures, left, top, 0, 0);
            temp._Depth = 1;
            sprites.Add(temp);
        }

        private void CreateAnAngel(float left, float top)
        {
            My2DSprite temp;
            List<Texture2D> textures = new List<Texture2D>();
            for (int i = 1; i <= 15; i++ )
                textures.Add(this.Content.Load<Texture2D>(@"Sprite\Unit\Angel"+i.ToString("00")));
            temp = new My2DSprite(textures, left, top, 150, 150, true);
            temp._Depth = 0.5f;
            temp.SetVelocity(150, 150);
            temp.Fps = 50;
            sprites.Add(temp);            
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        private void UpdateSprites(GameTime gameTime)
        {            
            for (int i = 0; i < sprites.Count; i++)
                sprites[i].Update(gameTime);
        }

        
        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();
            
            Global.UpdateAll(gameTime);

            UpdateSprites(gameTime);

            map.Update(gameTime);                 

            base.Update(gameTime);
        }

        private void SelectSprite(int idx)
        {
            for (int i = 0; i < sprites.Count; i++)
                sprites[i].State = (idx == i) ? 1 : 0;
        }

        private int FindSelectedSprite(float x, float y)
        {
            Vector2 worldPosition = Global.gMouseHelper.GetCurrentWorldMousePoistion();
            for (int i = 0; i < sprites.Count; i++)
                if (sprites[i].IsSelected(worldPosition.X, worldPosition.Y)) 
                    return i;
            return -1;
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin(SpriteSortMode.BackToFront, 
                BlendState.AlphaBlend, 
                null,  
                DepthStencilState.None, 
                RasterizerState.CullNone, 
                null, 
                Global.gMainCamera.WVP );

            // TODO: Add your drawing code here
            map.Draw(gameTime, spriteBatch);
            for (int i = 0; i < sprites.Count; i++)
                sprites[i].Draw(gameTime, this.spriteBatch);
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
