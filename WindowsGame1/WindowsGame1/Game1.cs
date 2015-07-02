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
using InvasionWar.Effects;
using InvasionWar.Effects.Animations;
using InvasionWar.Styles;
using InvasionWar.GameEntities.Invisible.Effects.GraphFunctions;
using InvasionWar.Styles.UI;
using Quobject.SocketIoClientDotNet.Client;

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

        public enum ConnectionState
        {
            NotConnected, RoomFull, DuplicateID, OtherDisconnected, Connected, Waiting
        }

        public enum TurnState
        {
            Ready, Waiting
        }

        public TurnState turnState = TurnState.Ready;

        public ConnectionState connectionState = ConnectionState.NotConnected;

        public enum PlayerState
        {
            Red, Blue, Both
        }

        public enum GameMode
        {
            Single, Multiplayer
        }

        public GameMode gameMode = GameMode.Single;

        Timer gameStateChanged = new Timer();        

        public GameState CurrentGameState = GameState.NotStarted;
        public GameState NextGameState = GameState.NotStarted;

        public PlayerState playerState = PlayerState.Both;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public Vector2 OriginScreenSize;
        public Vector2 ScreenSize;
        public Vector2 ScreenScaleFactor;

        public HexagonMap hexMap;

        public Socket HexagonServer;
        public bool ServerReady;

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
            HexagonServer = IO.Socket(GameSettings.HexagonServer);
            
            HexagonServer.On(Socket.EVENT_CONNECT, () =>
            {
                ServerReady = true;
            });

            HexagonServer.On("sendMove", (data) =>
            {
                if (hexMap != null)
                {
                    int i,j;
                    string[] d = ((string)data).Split(',');
                    i = Convert.ToInt32(d[0]);
                    j = Convert.ToInt32(d[1]);
                    hexMap.SelectCell(i, j, true);
                }
            });

            HexagonServer.On("registerResult", (data) =>
            {
                var result = Convert.ToInt32(data);
                switch (result)
                {
                    case -1:
                        connectionState = ConnectionState.DuplicateID;
                        break;
                    case 0:
                        connectionState = ConnectionState.RoomFull;
                        break;
                    case 1:
                        connectionState = ConnectionState.Connected;
                        playerState = PlayerState.Blue;
                        turnState = TurnState.Waiting;                        
                        break;
                    case 2:
                        connectionState = ConnectionState.Waiting;
                        playerState = PlayerState.Red;
                        turnState = TurnState.Ready;                        
                        break;
                }                
            });

            HexagonServer.On("otherQuit", (data) =>
            {
                var username = (string)data;
                connectionState = ConnectionState.OtherDisconnected;
            });

            HexagonServer.On("otherJoin", (data) =>
            {
                var username = (string)data;
                connectionState = ConnectionState.Connected;
            });
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

        public Sprite2D btnPlaySingle, GameTitle, Panel, TextPlayerName, TextRoomID, btnShop, btnHelp, btnSetting, btnExit, btnPlayMulti, background, btnSound;
        public Sprite2D scoreBoard;
        public TextBox TextBoxName, TextBoxRoom;

        public List<Sprite2D> sprites = new List<Sprite2D>();

        

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
            sprites = new List<Sprite2D>();

            new ModernUI().ApplyUI(this);
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
            if (sprites == null) return;
            for (int i = 0; i < sprites.Count; i++)
                if (sprites[i] != null) 
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

            if (hexMap != null)
            {
                hexMap.Update(gameTime);
            }

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
            {                
                if (sprites[i] != null)
                    sprites[i].Draw(gameTime, this.spriteBatch);
            }

            if (hexMap != null)
                hexMap.Draw(gameTime, spriteBatch);
            
            base.Draw(gameTime);
            spriteBatch.End();
           
        }

     
        private void TransitionStartedToNotStarted(object sender)
        {
            
        }

        public void StartGameSingle()
        {
            Global.UnfocusAllTextBox();
            gameMode = GameMode.Single;
            CurrentGameState = GameState.Started;
            if (hexMap == null)
            {
                string[] textures = new string[3];
                textures[0] = "Hexa";
                textures[1] = "hexa_near";
                textures[2] = "hexa_far";
                hexMap = new HexagonMap((int)(375.0f * ScreenScaleFactor.X), (int)(110.0f * ScreenScaleFactor.Y), 9, 45, 26, textures);
            }
            hexMap.StartSingle();
        }

        public void StartGameMultiplayer()
        {
            Global.UnfocusAllTextBox();
            gameMode = GameMode.Multiplayer;
            CurrentGameState = GameState.Started;
            if (hexMap == null)
            {
                string[] textures = new string[3];
                textures[0] = "Hexa";
                textures[1] = "hexa_near";
                textures[2] = "hexa_far";
                hexMap = new HexagonMap((int)(375.0f * ScreenScaleFactor.X), (int)(110.0f * ScreenScaleFactor.Y), 9, 45, 26, textures);
            }

            var username = TextBoxName.GetText();
            var roomid = TextBoxRoom.GetText();
            if (ServerReady) {
                HexagonServer.Emit("register", username +"|"+ roomid);
            }

            hexMap.StartMultiplayer();
        }

        public void ResetGame()
        {
            CurrentGameState = GameState.NotStarted;
            if (gameMode == GameMode.Multiplayer)
            {
                if (connectionState == ConnectionState.Waiting || connectionState == ConnectionState.Connected)
                {
                    HexagonServer.Emit("unregister");
                }

                connectionState = ConnectionState.NotConnected;
            }            

            hexMap = null;
        }
    
    }
}
