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
    public class ModernUI : GameGUI
    {

        public override void InitUI()
        {
            game.background = StaticSprite.CreateSprite(0, 0, game.ScreenScaleFactor, @"Sprite/GameUI/Background_Black", 1.0f);

            // title
            if (game.GameTitle == null)
            {
                game.GameTitle = StaticSprite.CreateSprite(303, 39, game.ScreenScaleFactor, @"Sprite/GameUI/Black_Logo", 0.9f);
            }

            // panel
            if (game.Panel == null)
            {
                game.Panel = StaticSprite.CreateSprite(255, 356, game.ScreenScaleFactor, @"Sprite/GameUI/Black_Panel", 0.8f);
            }

            if (game.TextBoxName == null)
            {
                game.TextBoxName = TextBox.CreateTextBox(445, 380, game.ScreenScaleFactor, @"Sprite/GameUI/Black_TextBox", 0.7f);
                game.TextBoxName.InitTextBox();
                game.TextBoxName.SetFont("Black_SegoeWP");
                game.TextBoxName.SetColor(Color.WhiteSmoke);
                TextBoxStyle.Assign(game.TextBoxName);
            }

            if (game.TextBoxRoom == null)
            {
                game.TextBoxRoom = TextBox.CreateTextBox(445, 433, game.ScreenScaleFactor, @"Sprite/GameUI/Black_TextBox", 0.7f);
                game.TextBoxRoom.InitTextBox();
                game.TextBoxRoom.SetFont("Black_SegoeWP");
                game.TextBoxRoom.SetColor(Color.WhiteSmoke);
                TextBoxStyle.Assign(game.TextBoxRoom);
            }

            if (game.TextPlayerName == null)
            {
                game.TextPlayerName = StaticSprite.CreateSprite(295, 384, game.ScreenScaleFactor, @"Sprite/GameUI/Black_TextPlayerName", 0.7f);
            }

            if (game.TextRoomID == null)
            {
                game.TextRoomID = StaticSprite.CreateSprite(295, 437, game.ScreenScaleFactor, @"Sprite/GameUI/Black_TextRoomID", 0.7f);
            }

            // btn play
            if (game.btnPlaySingle == null)
            {
                game.btnPlaySingle = StaticSprite.CreateSprite(394, 610, game.ScreenScaleFactor, @"Sprite/GameUI/Black_btnRound", 0.8f);                
                game.btnPlaySingle.AddChildAtMid(StaticSprite.CreateSprite(0, 0, game.ScreenScaleFactor, @"Sprite/GameUI/Black_btnSinglePlay", 0.7f));
                game.btnPlaySingle.OnMouseUp += TransitionToGame;

                ModernButtonStyle.Assign(game.btnPlaySingle);
            }

            if (game.btnPlayMulti == null)
            {
                game.btnPlayMulti = StaticSprite.CreateSprite(394, 554, game.ScreenScaleFactor, @"Sprite/GameUI/Black_btnRound", 0.8f);
                game.btnPlayMulti.AddChildAtMid(StaticSprite.CreateSprite(0, 0, game.ScreenScaleFactor, @"Sprite/GameUI/Black_btnMultiPlay", 0.7f));
                game.btnPlayMulti.OnMouseUp += TransitionToGame;

                ModernButtonStyle.Assign(game.btnPlayMulti);
            }

            if (game.btnSetting == null)
            {
                game.btnSetting = StaticSprite.CreateSprite(394, 666, game.ScreenScaleFactor, @"Sprite/GameUI/Black_btnRound", 0.8f);
                game.btnSetting.AddChildAtMid(StaticSprite.CreateSprite(0, 0, game.ScreenScaleFactor, @"Sprite/GameUI/Black_btnSetting", 0.7f));
                
                ModernButtonStyle.Assign(game.btnSetting);
            }

            // small top icons
            if (game.btnShop == null)
            {
                game.btnShop = StaticSprite.CreateSprite(882, 24, game.ScreenScaleFactor, @"Sprite/GameUI/Black_btnShop", 0.9f);
                ButtonStyle.Assign(game.btnShop);
            }

            // small top icons
            if (game.btnSound == null)
            {
                game.btnSound = StaticSprite.CreateSprite(934, 24, game.ScreenScaleFactor, @"Sprite/GameUI/Black_audioOn", 0.9f);
                ButtonStyle.Assign(game.btnSound);
            }

            if (game.scoreBoard == null)
            {
                var Red = StaticSprite.CreateSprite(0, 30, game.ScreenScaleFactor, @"Sprite/GameUI/Player1", 0.6f);
                var Blue = StaticSprite.CreateSprite(0, 110, game.ScreenScaleFactor, @"Sprite/GameUI/Player2", 0.6f);
                game.scoreBoard = new Scoreboard(Red, Blue, game.ScreenScaleFactor);
                game.scoreBoard.Top = 280 * game.ScreenScaleFactor.Y;
                game.scoreBoard.Left = 90 * game.ScreenScaleFactor.X;
            }


            //if (game.btnHelp == null)
            //{
            //    game.btnHelp = StaticSprite.CreateSprite(860, 22, game.ScreenScaleFactor, @"Sprite/GameUI/btnHelp", 0.9f);
            //    ButtonStyle.Assign(game.btnHelp);
            //}


            if (game.btnExit == null)
            {
                game.btnExit = StaticSprite.CreateSprite(39, 670+300, game.ScreenScaleFactor, @"Sprite/GameUI/Black_btnRound", 0.8f);
                game.btnExit.AddChildAtMid(StaticSprite.CreateSprite(0, 0, game.ScreenScaleFactor, @"Sprite/GameUI/Black_btnExit", 0.7f));

                game.btnExit.OnMouseUp += TransitionToMenu;
                ModernButtonStyle.Assign(game.btnExit);
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

            mainStoryboard.Stop();
            mainStoryboard.Clear();
            mainStoryboard.OnCompleted += StartGame;

            float distance = 700;
            Vector2 scale = new Vector2(0.9f, 0.9f);
            Vector2 logoDistance = new Vector2(-200, -10);
            Vector2 fromLogo = new Vector2(game.GameTitle.Left, game.GameTitle.Top);
            Vector2 toLogo = Vector2.Add(fromLogo, logoDistance);
            float time = 1.0f;

            Animation anim;
            anim = new TranslationAnimation(mainStoryboard, game.GameTitle, time, toLogo, false);
            anim.SetGraphFunction(new LinearGraphFunction());
            mainStoryboard.AddAnimation(anim);
            mainStoryboard.AddAnimation(new TranslationAnimation(mainStoryboard, game.Panel, time, new Vector2(game.Panel.Left, game.Panel.Top - distance), false));
            mainStoryboard.AddAnimation(new TranslationAnimation(mainStoryboard, game.TextPlayerName, time, new Vector2(game.TextPlayerName.Left, game.TextPlayerName.Top - distance), false));
            mainStoryboard.AddAnimation(new TranslationAnimation(mainStoryboard, game.TextRoomID, time, new Vector2(game.TextRoomID.Left, game.TextRoomID.Top - distance), false));
            mainStoryboard.AddAnimation(new TranslationAnimation(mainStoryboard, game.TextBoxRoom, time, new Vector2(game.TextBoxRoom.Left, game.TextBoxRoom.Top - distance), false));
            mainStoryboard.AddAnimation(new TranslationAnimation(mainStoryboard, game.TextBoxName, time, new Vector2(game.TextBoxName.Left, game.TextBoxName.Top - distance), false));

            anim = new TranslationAnimation(mainStoryboard, game.btnPlaySingle, time, new Vector2(game.btnPlaySingle.Left, game.btnPlaySingle.Top + distance), false);
            anim.SetGraphFunction(new LinearGraphFunction());
            mainStoryboard.AddAnimation(anim);

            anim = new TranslationAnimation(mainStoryboard, game.btnPlayMulti, time, new Vector2(game.btnPlayMulti.Left, game.btnPlayMulti.Top + distance), false);
            anim.SetGraphFunction(new LinearGraphFunction());
            mainStoryboard.AddAnimation(anim);

            anim = new TranslationAnimation(mainStoryboard, game.btnSetting, time, new Vector2(game.btnSetting.Left, game.btnSetting.Top + distance), false);
            anim.SetGraphFunction(new LinearGraphFunction());
            mainStoryboard.AddAnimation(anim);

            anim = new TranslationAnimation(mainStoryboard, game.btnExit, time, new Vector2(game.btnExit.Left, game.btnExit.Top - 230), false);
            anim.SetGraphFunction(new LinearGraphFunction());
            mainStoryboard.AddAnimation(anim);

            mainStoryboard.Start();

        }

        private void TransitionToMenu(object sender)
        {
            if (game.CurrentGameState == Game1.GameState.NotStarted) return;
            game.ResetGame();

            mainStoryboard.Stop();
            mainStoryboard.Clear();
            mainStoryboard.OnCompleted = null;

            float distance = -700;
            Vector2 scale = new Vector2(0.9f, 0.9f);
            Vector2 logoDistance = new Vector2(200, 10);
            Vector2 fromLogo = new Vector2(game.GameTitle.Left, game.GameTitle.Top);
            Vector2 toLogo = Vector2.Add(fromLogo, logoDistance);
            float time = 1.0f;

            Animation anim;
            anim = new TranslationAnimation(mainStoryboard, game.GameTitle, time, toLogo, false);
            anim.SetGraphFunction(new LinearGraphFunction());
            mainStoryboard.AddAnimation(anim);
            mainStoryboard.AddAnimation(new TranslationAnimation(mainStoryboard, game.Panel, time, new Vector2(game.Panel.Left, game.Panel.Top - distance), false));
            mainStoryboard.AddAnimation(new TranslationAnimation(mainStoryboard, game.TextPlayerName, time, new Vector2(game.TextPlayerName.Left, game.TextPlayerName.Top - distance), false));
            mainStoryboard.AddAnimation(new TranslationAnimation(mainStoryboard, game.TextRoomID, time, new Vector2(game.TextRoomID.Left, game.TextRoomID.Top - distance), false));
            mainStoryboard.AddAnimation(new TranslationAnimation(mainStoryboard, game.TextBoxRoom, time, new Vector2(game.TextBoxRoom.Left, game.TextBoxRoom.Top - distance), false));
            mainStoryboard.AddAnimation(new TranslationAnimation(mainStoryboard, game.TextBoxName, time, new Vector2(game.TextBoxName.Left, game.TextBoxName.Top - distance), false));

            anim = new TranslationAnimation(mainStoryboard, game.btnPlaySingle, time, new Vector2(game.btnPlaySingle.Left, game.btnPlaySingle.Top + distance), false);
            anim.SetGraphFunction(new LinearGraphFunction());
            mainStoryboard.AddAnimation(anim);

            anim = new TranslationAnimation(mainStoryboard, game.btnPlayMulti, time, new Vector2(game.btnPlayMulti.Left, game.btnPlayMulti.Top + distance), false);
            anim.SetGraphFunction(new LinearGraphFunction());
            mainStoryboard.AddAnimation(anim);

            anim = new TranslationAnimation(mainStoryboard, game.btnSetting, time, new Vector2(game.btnSetting.Left, game.btnSetting.Top + distance), false);
            anim.SetGraphFunction(new LinearGraphFunction());
            mainStoryboard.AddAnimation(anim);

            anim = new TranslationAnimation(mainStoryboard, game.btnExit, time, new Vector2(game.btnExit.Left, game.btnExit.Top + 230), false);
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
