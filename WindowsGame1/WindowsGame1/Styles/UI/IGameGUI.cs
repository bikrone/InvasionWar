using InvasionWar.Effects;
using InvasionWar.GameEntities.Visible;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InvasionWar.Styles.UI
{
    public class GameGUI
    {
        protected Storyboard mainStoryboard = new Storyboard();
        public Game1 game;
        public virtual void ApplyUI(Game1 game) {
            this.game = game;
            InitUI();
        }

        public virtual void InitUI()
        {
            LoadScene();
        }

        public virtual void LoadScene()
        {
            // background                    
            game.sprites.Add(game.background);
            game.sprites.Add(game.GameTitle);
            game.sprites.Add(game.Panel);
            game.sprites.Add(game.TextBoxName);
            game.sprites.Add(game.TextBoxRoom);
            game.sprites.Add(game.TextPlayerName);
            game.sprites.Add(game.TextRoomID);
            game.sprites.Add(game.btnPlaySingle);
            game.sprites.Add(game.btnShop);
            game.sprites.Add(game.btnSetting);
            game.sprites.Add(game.btnHelp);
            game.sprites.Add(game.btnExit);
            game.sprites.Add(game.btnPlayMulti);
            game.sprites.Add(game.btnSound);
            game.sprites.Add(game.scoreBoard);
        }
    }
}
