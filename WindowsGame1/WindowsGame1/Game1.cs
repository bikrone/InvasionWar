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
using InvasionWar.GameEntities.Visible;
using InvasionWar.GameEntities.Invisible;
using InvasionWar.GameEntities;
using System.Timers;

namespace InvasionWar
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {

        public enum GameState
        {
            NotStarted, Started, Lost, Won
        }

        Timer gameStateChanged = new Timer();        

        public GameState CurrentGameState = GameState.NotStarted;
        public GameState NextGameState = GameState.NotStarted;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public Vector2 OriginScreenSize;
        public Vector2 ScreenSize;
        public Vector2 ScreenScaleFactor;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);

            graphics.IsFullScreen = false;
            OriginScreenSize = new Vector2(1024, 768);
            ScreenSize = new Vector2(768, 576);

            ScreenScaleFactor = new Vector2();
            ScreenScaleFactor.X = ScreenSize.X / OriginScreenSize.X;
            ScreenScaleFactor.Y = ScreenSize.Y / OriginScreenSize.Y;

            graphics.PreferredBackBufferWidth = 768;
            graphics.PreferredBackBufferHeight = 576;            

            Content.RootDirectory = "Content";

            Global.thisGame = this;
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

        My2DSprite btnPlay, GameTitle, Panel, TextPlayerName, TextRoomID, TextBoxName, TextBoxRoom;

        List<My2DSprite> sprites;

        public void LoadScene() {
            sprites.Clear();

            switch (CurrentGameState)
            {
                case GameState.NotStarted:
                    // background                    
                    sprites.Add(StaticSprite.CreateSprite(0, 0, ScreenScaleFactor, @"Sprite/GameUI/Background", 1.0f));

                    // title
                    if (GameTitle == null)
                    {
                        GameTitle = StaticSprite.CreateSprite(200, 182, ScreenScaleFactor, @"Sprite/GameUI/Title", 0.9f);
                    }
                    sprites.Add(GameTitle);

                    // panel
                    if (Panel==null) {
                        Panel = StaticSprite.CreateSprite(215, 315, ScreenScaleFactor, @"Sprite/GameUI/Panel", 0.8f);
                    }
                    sprites.Add(Panel);

                    if (TextPlayerName== null){
                        TextPlayerName = StaticSprite.CreateSprite(273, 360, ScreenScaleFactor, @"Sprite/GameUI/TextPlayerName", 0.8f);
                    }
                    sprites.Add(TextPlayerName);

                    if (TextRoomID == null) {
                        TextRoomID = StaticSprite.CreateSprite(273, 430, ScreenScaleFactor, @"Sprite/GameUI/TextRoomID", 0.8f);
                    }
                    sprites.Add(TextRoomID);
                    if (TextBoxName == null) {
                        TextBoxName =StaticSprite.CreateSprite(454, 350, ScreenScaleFactor, @"Sprite/GameUI/TextBox", 0.8f);
                    }
                    sprites.Add(TextBoxName);

                    if (TextBoxRoom == null) {
                        TextBoxRoom = StaticSprite.CreateSprite(454, 420, ScreenScaleFactor, @"Sprite/GameUI/TextBox", 0.8f);
                    }
                    sprites.Add(TextBoxRoom);

                    // btn play
                    if (btnPlay == null)
                    {
                        btnPlay = StaticSprite.CreateSprite(440, 538, ScreenScaleFactor, @"Sprite/GameUI/btnPlay", 0.8f);
                        btnPlay.OnMouseClick += TransitionNotStartedToStarted;
                        Global.gMouseHelper.Register(btnPlay);
                    }

                    sprites.Add(btnPlay);

                    // small top icons
                    sprites.Add(StaticSprite.CreateSprite(700, 22, ScreenScaleFactor, @"Sprite/GameUI/btnShop", 0.9f));
                    sprites.Add(StaticSprite.CreateSprite(780, 22, ScreenScaleFactor, @"Sprite/GameUI/btnSetting", 0.9f));
                    sprites.Add(StaticSprite.CreateSprite(860, 22, ScreenScaleFactor, @"Sprite/GameUI/btnHelp", 0.9f));
                    sprites.Add(StaticSprite.CreateSprite(940, 22, ScreenScaleFactor, @"Sprite/GameUI/btnExit", 0.9f));

                    break;

                case GameState.Started:
                    // background                    
                    sprites.Add(StaticSprite.CreateSprite(0, 0, ScreenScaleFactor, @"Sprite/GameUI/Background", 1.0f));

                    // small top icons
                    sprites.Add(StaticSprite.CreateSprite(700, 22, ScreenScaleFactor, @"Sprite/GameUI/btnShop", 0.9f));
                    sprites.Add(StaticSprite.CreateSprite(780, 22, ScreenScaleFactor, @"Sprite/GameUI/btnSetting", 0.9f));
                    sprites.Add(StaticSprite.CreateSprite(860, 22, ScreenScaleFactor, @"Sprite/GameUI/btnHelp", 0.9f));
                    sprites.Add(StaticSprite.CreateSprite(940, 22, ScreenScaleFactor, @"Sprite/GameUI/btnExit", 0.9f));
                    break;
            }
            

        }

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

            LoadScene();
            
            //string[] strTextures = {"Water", "Grass", "Highland", "Snow"};
            //int[,] MapData = new int[10, 20];
            //Random r = new Random();

            //for (int i=0; i<10; i++) {
            //    for (int j = 0; j < 20; j++)
            //    {
            //        MapData[i, j] = r.Next() % 4;
            //    }
            //}
            
            //map = new TilingMap(10, 20, 100, 100, strTextures, MapData);
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
                       

            base.Update(gameTime);
        }

        private void SelectSprite(int idx)
        {
            for (int i = 0; i < sprites.Count; i++)
                sprites[i].State = (idx == i) ? 1 : 0;
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
            for (int i = 0; i < sprites.Count; i++)
                sprites[i].Draw(gameTime, this.spriteBatch);
            spriteBatch.End();
            base.Draw(gameTime);
        }

        private void TransitionNotStartedToStarted(object sender)
        {
            float speed = 700;
            float time = 1.5f;
            NextGameState = GameState.Started;
            GameTitle.SetTransitionTask(new Vector2(GameTitle.Left, GameTitle.Top - speed), time, TransitionCompleted);

            Panel.SetTransitionTask(new Vector2(Panel.Left, Panel.Top - speed), time, TransitionCompleted);
            TextPlayerName.SetTransitionTask(new Vector2(TextPlayerName.Left, TextPlayerName.Top - speed), time, TransitionCompleted);
            TextRoomID.SetTransitionTask(new Vector2(TextRoomID.Left, TextRoomID.Top - speed), time, TransitionCompleted);
            TextBoxRoom.SetTransitionTask(new Vector2(TextBoxRoom.Left, TextBoxRoom.Top - speed), time, TransitionCompleted);
            TextBoxName.SetTransitionTask(new Vector2(TextBoxName.Left, TextBoxName.Top - speed), time, TransitionCompleted);

            btnPlay.SetTransitionTask(new Vector2(btnPlay.Left, btnPlay.Top + speed), time, TransitionCompleted);
        }

        private void TransitionStartedToNotStarted(object sender)
        {
            
        }

        private void TransitionCompleted(object sender)
        {
            CurrentGameState = NextGameState;
            LoadScene();
        }
    
    }
}
