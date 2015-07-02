using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InvasionWar.GameEntities.Visible
{
    public class Gem : Sprite2D
    {
        public static HexagonMap map;
        public static Vector2 Size = new Vector2(41, 41);
        private static string[] SpritePath = { @"Sprite\GameUI\Player1", @"Sprite\GameUI\Player2" };
        public Sprite2D sprite;
        public int i;
        public int j;
       
        public enum Team { Red, Blue }
        public Team team;

        public Gem(Team team, int i, int j)
        {
            int width = (int)(Gem.Size.X * Global.thisGame.ScreenScaleFactor.X);
            int height = (int)(Gem.Size.Y * Global.thisGame.ScreenScaleFactor.Y);
            var visualPosition = Gem.map.GetGemVisualPosition(i, j);
            if (team == Team.Red)
            {
                sprite = StaticSprite.CreateSprite(visualPosition.X, visualPosition.Y, new Vector2(1,1), SpritePath[0], 0.1f, width, height);
            }
            else
            {
                sprite = StaticSprite.CreateSprite(visualPosition.X, visualPosition.Y, new Vector2(1, 1), SpritePath[1], 0.1f, width, height);
            }
            this.team = team;
            this.i = i;
            this.j = j;
            this.AddChild(sprite);
        }

        public Gem(Gem gem2)
        {
            this.team = gem2.team;
            this.i = gem2.i;
            this.j = gem2.j;
            var visualPosition = Gem.map.GetGemVisualPosition(i, j);
            int width = (int)(Gem.Size.X * Global.thisGame.ScreenScaleFactor.X);
            int height = (int)(Gem.Size.Y * Global.thisGame.ScreenScaleFactor.Y);
            if (team == Team.Red)
            {
                sprite = StaticSprite.CreateSprite(visualPosition.X, visualPosition.Y, new Vector2(1, 1), SpritePath[0], 0.1f, width, height);
            }
            else
            {
                sprite = StaticSprite.CreateSprite(visualPosition.X, visualPosition.Y, new Vector2(1, 1), SpritePath[1], 0.1f, width, height);
            }
            this.AddChild(sprite);
        }

        public void ChangeTeam()
        {
            if (team == Team.Red)
            {
                team = Team.Blue;
                ((StaticSprite)sprite).ReloadTexture(SpritePath[1]);
            }
            else
            {
                team = Team.Red;
                ((StaticSprite)sprite).ReloadTexture(SpritePath[0]);
            }
        }
        
    }        
}
