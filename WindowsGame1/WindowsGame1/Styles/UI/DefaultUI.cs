using InvasionWar.Effects;
using InvasionWar.Effects.Animations;
using InvasionWar.GameEntities.Invisible.Effects.GraphFunctions;
using InvasionWar.GameEntities.Visible;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InvasionWar.Styles.UI
{
    public class DefaultUI : GameGUI
    {           

        public override void InitUI()
        {
            game.background = StaticSprite.CreateSprite(0, 0, game.ScreenScaleFactor, @"Sprite/GameUI/Background", 1.0f);
            // title
            if (game.GameTitle == null)
            {
                game.GameTitle = StaticSprite.CreateSprite(200, 182, game.ScreenScaleFactor, @"Sprite/GameUI/Title", 0.9f);
            }

            // panel
            if (game.Panel == null)
            {
                game.Panel = StaticSprite.CreateSprite(215, 315, game.ScreenScaleFactor, @"Sprite/GameUI/Panel", 0.8f);
            }

            if (game.TextBoxName == null)
            {
                game.TextBoxName = TextBox.CreateTextBox(454, 350, game.ScreenScaleFactor, @"Sprite/GameUI/TextBox", 0.7f);
                game.TextBoxName.InitTextBox();
                game.TextBoxName.SetFont("SegoeWP");
                TextBoxStyle.Assign(game.TextBoxName);
            }

            if (game.TextBoxRoom == null)
            {
                game.TextBoxRoom = TextBox.CreateTextBox(454, 420, game.ScreenScaleFactor, @"Sprite/GameUI/TextBox", 0.7f);
                game.TextBoxRoom.InitTextBox();
                game.TextBoxRoom.SetFont("SegoeWP");
                TextBoxStyle.Assign(game.TextBoxRoom);
            }

            if (game.TextPlayerName == null)
            {
                game.TextPlayerName = StaticSprite.CreateSprite(273, 360, game.ScreenScaleFactor, @"Sprite/GameUI/TextPlayerName", 0.7f);
            }

            if (game.TextRoomID == null)
            {
                game.TextRoomID = StaticSprite.CreateSprite(273, 430, game.ScreenScaleFactor, @"Sprite/GameUI/TextRoomID", 0.7f);
            }

            // btn play
            if (game.btnPlaySingle == null)
            {
                game.btnPlaySingle = StaticSprite.CreateSprite(440, 538, game.ScreenScaleFactor, @"Sprite/GameUI/btnPlay", 0.8f);

                game.btnPlaySingle.OnMouseUp += TransitionToGame;

                ButtonStyle.Assign(game.btnPlaySingle);
            }

            // small top icons
            if (game.btnShop == null)
            {
                game.btnShop = StaticSprite.CreateSprite(700, 22, game.ScreenScaleFactor, @"Sprite/GameUI/btnShop", 0.9f);
                ButtonStyle.Assign(game.btnShop);
            }

            if (game.btnSetting == null)
            {
                game.btnSetting = StaticSprite.CreateSprite(780, 22, game.ScreenScaleFactor, @"Sprite/GameUI/btnSetting", 0.9f);
                ButtonStyle.Assign(game.btnSetting);
            }

            if (game.btnHelp == null)
            {
                game.btnHelp = StaticSprite.CreateSprite(860, 22, game.ScreenScaleFactor, @"Sprite/GameUI/btnHelp", 0.9f);
                ButtonStyle.Assign(game.btnHelp);
            }

            if (game.btnExit == null)
            {
                game.btnExit = StaticSprite.CreateSprite(940, 22, game.ScreenScaleFactor, @"Sprite/GameUI/btnExit", 0.9f);
                ButtonStyle.Assign(game.btnExit);
                game.btnExit.OnMouseUp += TransitionToMenu;
            }

            base.InitUI();
        }

       
        public override void LoadScene()
        {
            base.LoadScene();
        }

        private void TransitionToGame(object sender)
        {
            if (game.CurrentGameState == Game1.GameState.Started) return;
            float distance = 700;
            float time = 1.0f;
            
            mainStoryboard.Stop();
            mainStoryboard.Clear();
            mainStoryboard.OnCompleted += StartGame;

            mainStoryboard.AddAnimation(new TranslationAnimation(mainStoryboard, game.GameTitle, time, new Vector2(game.GameTitle.Left, game.GameTitle.Top - distance), false));
            mainStoryboard.AddAnimation(new TranslationAnimation(mainStoryboard, game.Panel, time, new Vector2(game.Panel.Left, game.Panel.Top - distance), false));
            mainStoryboard.AddAnimation(new TranslationAnimation(mainStoryboard, game.TextPlayerName, time, new Vector2(game.TextPlayerName.Left, game.TextPlayerName.Top - distance), false));
            mainStoryboard.AddAnimation(new TranslationAnimation(mainStoryboard, game.TextRoomID, time, new Vector2(game.TextRoomID.Left, game.TextRoomID.Top - distance), false));
            mainStoryboard.AddAnimation(new TranslationAnimation(mainStoryboard, game.TextBoxRoom, time, new Vector2(game.TextBoxRoom.Left, game.TextBoxRoom.Top - distance), false));
            mainStoryboard.AddAnimation(new TranslationAnimation(mainStoryboard, game.TextBoxName, time, new Vector2(game.TextBoxName.Left, game.TextBoxName.Top - distance), false));

            Animation anim = new TranslationAnimation(mainStoryboard, game.btnPlaySingle, time, new Vector2(game.btnPlaySingle.Left, game.btnPlaySingle.Top + distance), false);
            anim.SetGraphFunction(new LinearGraphFunction());
            mainStoryboard.AddAnimation(anim);

            mainStoryboard.Start();

        }

        private void TransitionToMenu(object sender)
        {
            if (game.CurrentGameState == Game1.GameState.NotStarted) return;
            game.ResetGame();
            float distance = -700;
            float time = 1.0f;
            
            mainStoryboard.Stop();
            mainStoryboard.Clear();
            mainStoryboard.OnCompleted = null;

            mainStoryboard.AddAnimation(new TranslationAnimation(mainStoryboard, game.GameTitle, time, new Vector2(game.GameTitle.Left, game.GameTitle.Top - distance), false));
            mainStoryboard.AddAnimation(new TranslationAnimation(mainStoryboard, game.Panel, time, new Vector2(game.Panel.Left, game.Panel.Top - distance), false));
            mainStoryboard.AddAnimation(new TranslationAnimation(mainStoryboard, game.TextPlayerName, time, new Vector2(game.TextPlayerName.Left, game.TextPlayerName.Top - distance), false));
            mainStoryboard.AddAnimation(new TranslationAnimation(mainStoryboard, game.TextRoomID, time, new Vector2(game.TextRoomID.Left, game.TextRoomID.Top - distance), false));
            mainStoryboard.AddAnimation(new TranslationAnimation(mainStoryboard, game.TextBoxRoom, time, new Vector2(game.TextBoxRoom.Left, game.TextBoxRoom.Top - distance), false));
            mainStoryboard.AddAnimation(new TranslationAnimation(mainStoryboard, game.TextBoxName, time, new Vector2(game.TextBoxName.Left, game.TextBoxName.Top - distance), false));
           

            Animation anim = new TranslationAnimation(mainStoryboard, game.btnPlaySingle, time, new Vector2(game.btnPlaySingle.Left, game.btnPlaySingle.Top + distance), false);
            anim.SetGraphFunction(new LinearGraphFunction());
            mainStoryboard.AddAnimation(anim);

            mainStoryboard.Start();
        }

        private void StopGame(object sender, object argument)
        {            
            // game.ResetGame();
        }

        private void StartGame(object sender, object argument)
        {            
            game.StartGame();
        }

    }
}
