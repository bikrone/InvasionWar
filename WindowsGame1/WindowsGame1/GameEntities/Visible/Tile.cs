using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InvasionWar.GameEntities.Visible
{
    public class Tile : Sprite2D
    {
        public static HexagonMap map;       
        public int i;
        public int j;

        private static string[] CellImages = { "hexa_selected", "hexa_near", "hexa_far", "Hexa" };

        public enum CellState { Selected, Near, Far, None }
        public CellState State;

        public Tile(Sprite2D sprite, int i, int j)
        {
            State = CellState.None;
            
            sprite.OnMouseUp += OnMouseUp;
            this.AddChild(sprite);
            this.i = i;
            this.j = j;
            this.sprite = sprite;
            Global.gMouseHelper.Register(sprite);
        }

        public Sprite2D sprite;       

        public void OnMouseUp(object sender)
        {
            Tile.map.OnMouseClick(i, j);
        }

        public void ChangeState(CellState state)
        {
            if (State != state)
            {
                State = state;
                switch (State)
                {
                    case CellState.Selected:
                        ((StaticSprite)sprite).ReloadTexture(@"Sprite\GameUI\" + Tile.CellImages[0]);
                        break;
                    case CellState.Near:
                        ((StaticSprite)sprite).ReloadTexture(@"Sprite\GameUI\" + Tile.CellImages[1]);
                        break;
                    case CellState.Far:
                        ((StaticSprite)sprite).ReloadTexture(@"Sprite\GameUI\" + Tile.CellImages[2]);
                        break;
                    default:
                        ((StaticSprite)sprite).ReloadTexture(@"Sprite\GameUI\" + Tile.CellImages[3]);
                        break;
                }
            }
        }        

    }        
}
