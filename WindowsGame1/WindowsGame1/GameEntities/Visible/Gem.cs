using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InvasionWar.GameEntities.Visible
{
    public class Gem
    {
        public static HexagonMap map;
        public static Vector2 Size = new Vector2(41, 41);
        private static string[] SpritePath = { @"Sprite\GameUI\Player1", @"Sprite\GameUI\Player2" };
        public My2DSprite sprite;
        public int i;
        public int j;

        public enum Team { Red, Blue }
        public Team team;

        public Gem(Team team, Vector2 visualPosition, int i, int j)
        {
            int width = (int)(Gem.Size.X * Global.thisGame.ScreenScaleFactor.X);
            int height = (int)(Gem.Size.Y * Global.thisGame.ScreenScaleFactor.Y);
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

            this.sprite.OnMouseUp += OnMouseUp;
            Global.gMouseHelper.Register(this.sprite);
        }

        public void OnMouseUp(object sender)
        {
            Gem.map.OnMouseClick(i, j);
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
