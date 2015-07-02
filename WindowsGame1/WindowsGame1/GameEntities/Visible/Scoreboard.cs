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
        public TextBox RedCount, BlueCount, Result, Notify;

        private Color blue = new Color(46, 142, 206);
        private Color red = new Color(206, 46, 62);

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

            Notify = TextBox.CreateTextBox(-70, -60, ScreenScaleFactor, null, 0.6f, 0, 0, true);
            Notify.InitTextBox();
            Notify.SetFont("WinFont");
            Notify.SetColor(blue);

            AddChild(Red);
            AddChild(Blue);
            AddChild(RedCount);
            AddChild(BlueCount);
            AddChild(Result);
            AddChild(Notify);
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

            Notify.SetText("");
            if (Global.thisGame.gameMode == Game1.GameMode.Multiplayer)
            {
                var connectionState = Global.thisGame.connectionState;
                var turnState = Global.thisGame.turnState;

                switch (connectionState)
                {
                    case Game1.ConnectionState.Connected:
                        if (turnState == Game1.TurnState.Ready)
                        {
                            Notify.SetText("Your turn...");
                            Notify.SetColor(blue);
                        }
                        else
                        {
                            Notify.SetText("Their turn...");
                            Notify.SetColor(red);
                        }
                        break;
                    case Game1.ConnectionState.DuplicateID:
                        Notify.SetText("Username exists");
                        Notify.SetColor(red);
                        break;
                    case Game1.ConnectionState.OtherDisconnected:
                        Notify.SetText("The other quit!");
                        Notify.SetColor(red);
                        break;
                    case Game1.ConnectionState.NotConnected:
                        Notify.SetText("Connecting...");
                        Notify.SetColor(red);
                        break;
                    case Game1.ConnectionState.RoomFull:
                        Notify.SetText("Room is full!");
                        Notify.SetColor(red);
                        break;
                    case Game1.ConnectionState.Waiting:
                        Notify.SetText("Waiting for others!");
                        Notify.SetColor(red);
                        break;                        
                }
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
