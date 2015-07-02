using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InvasionWar.GameEntities.Visible
{
    public class Scoreboard : Sprite2D
    {
        public Sprite2D RedPlayerImage, BluePlayerImage;
        public TextBox RedCount, BlueCount, Result;

        public Scoreboard(Sprite2D Red, Sprite2D Blue, Vector2 ScreenScaleFactor)
        {            
            this.RedPlayerImage = Red;
            this.BluePlayerImage = Blue;
            RedCount = TextBox.CreateTextBox(Red.Left+Red.Width+10, 30, ScreenScaleFactor, null, 0.6f, 0, 0, true);
            RedCount.InitTextBox();
            RedCount.SetFont("ScoreFont");
            RedCount.SetColor(Color.WhiteSmoke);

            BlueCount = TextBox.CreateTextBox(Blue.Left+Blue.Width+10, 110, ScreenScaleFactor, null, 0.6f, 0, 0, true);
            BlueCount.InitTextBox();
            BlueCount.SetFont("ScoreFont");
            BlueCount.SetColor(Color.WhiteSmoke);

            Result = TextBox.CreateTextBox(-50, 190, ScreenScaleFactor, null, 0.6f, 0, 0, true);
            Result.InitTextBox();
            Result.SetFont("WinFont");
            Result.SetColor(Color.WhiteSmoke);

            AddChild(Red);
            AddChild(Blue);
            AddChild(RedCount);
            AddChild(BlueCount);
            AddChild(Result);
        }

        public override void Update(GameTime gameTime)
        {
            var map = Global.thisGame.hexMap;
            if (map== null) return;
            RedCount.SetText("x" + Convert.ToString(map.ScoreRed));
            BlueCount.SetText("x" + Convert.ToString(map.ScoreBlue));
            Result.SetText("");
            if (map.winState == HexagonMap.WinState.Blue)
            {
                Result.SetText("Team Blue Wins!");
            }
            else if (map.winState == HexagonMap.WinState.Red)
            {
                Result.SetText("Team Red Wins!");
            }
            base.Update(gameTime);
        }
        public override void Draw(GameTime gameTime, object param)
        {
            var map = Global.thisGame.hexMap;
            if (map == null) return;
            base.Draw(gameTime, param);
        }
    }
}
