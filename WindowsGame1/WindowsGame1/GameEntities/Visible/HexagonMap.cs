using InvasionWar.Helper;
using InvasionWar.Styles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InvasionWar.GameEntities.Visible
{
    public class HexagonMap : VisibleGameEntity
    {
        public int Left = 0;
        public int Top = 0;
        public int nRows, nCols;
        public int height;
        public int TileWidth, TileHeight;
        public int MapWidth, MapHeight;

        public enum MapState
        {
            RedTurn, BlueTurn
        }

        public MapState State = MapState.RedTurn;

        public class Tile
        {
            public My2DSprite sprite;
            public int i;
            public int j;

            private static string[] CellImages = { "hexa_selected", "hexa_near", "hexa_far", "Hexa" };

            public enum CellState { Selected, Near, Far, None }
            public CellState State;

            public Tile()
            {
                State = CellState.None;
            }

            public Tile(My2DSprite sprite, int i, int j)
            {
                this.sprite = sprite;
                this.i = i;
                this.j = j;
            }

            public void ChangeState(CellState state)
            {
                if (State != state)
                {
                    State = state;
                    switch (State)
                    {
                        case CellState.Selected:
                            ((StaticSprite)sprite).ReloadTexture(@"Sprite\GameUI\"+Tile.CellImages[0]);
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

        public Tile[,] Tiles;
        public Gem[,] Gems;
        public List<Tile> tiles;
        public List<Gem> gems;
        public int[,] MapData;

        public Vector2 GetGemVisualPosition(int i, int j) {
            Vector2 position = new Vector2();

            position.X = Tiles[i, j].sprite.Left + (Tiles[i,j].sprite.Width - Gem.Size.X * Global.thisGame.ScreenScaleFactor.X)/2.0f;
            position.Y = Tiles[i, j].sprite.Top + (Tiles[i,j].sprite.Height - Gem.Size.X * Global.thisGame.ScreenScaleFactor.Y)/2.0f;

            return position;
        }

        public HexagonMap(int left, int top, int height, int tileWidth, int tileHeight, string[] strTextures)
        {
            this.height = height;
            this.nRows = height*2-1;
            this.nCols = height;
            this.TileWidth = tileWidth;
            this.TileHeight = tileHeight;
         
            this.MapData = new int[height*2, height+1];
            Tiles = new Tile[height * 2, height + 1];
            Gems = new Gem[height * 2, height + 1];
            this.Left = left;
            this.Top = top;

            gems = new List<Gem>();            

            LoadTile(strTextures);

            AddGem(4, 0, Gem.Team.Blue);
            AddGem(16, 4, Gem.Team.Blue);
            AddGem(4, 8, Gem.Team.Blue);

            AddGem(0, 4, Gem.Team.Red);
            AddGem(12, 0, Gem.Team.Red);
            AddGem(12, 8, Gem.Team.Red);

            Gem.map = this;

            //SelectCell(4, 0);
        }

        public void AddGem(int i, int j, Gem.Team team)
        {
            Gem gem = new Gem(team, GetGemVisualPosition(i, j), i, j);
            Gems[i, j] = gem;
            gems.Add(gem);
        }

        public void ClearAllCellState()
        {
            foreach (var tile in tiles)
            {
                tile.ChangeState(Tile.CellState.None);
            }
        }

        public void SetCellState(int i, int j, Tile.CellState state)
        {
            if (i < 0 || i >= height * 2 || j < 0 || j > height) return;
            if (Tiles[i, j] == null) return;
            if (Tiles[i,j].State == Tile.CellState.None)
                Tiles[i, j].ChangeState(state);
        }

        public void MarkNearAs(int i, int j, Tile.CellState state)
        {            
            SetCellState(i - 1, j + 1, state);
            SetCellState(i + 1, j + 1, state);
            SetCellState(i + 2, j + 0, state);
            SetCellState(i - 2, j + 0, state);
            SetCellState(i - 1, j - 1, state);
            SetCellState(i + 1, j - 1, state);

            if (state == Tile.CellState.Near)
            {
                MarkNearAs(i - 1, j + 1, Tile.CellState.Far);
                MarkNearAs(i + 1, j + 1, Tile.CellState.Far);
                MarkNearAs(i + 2, j + 0, Tile.CellState.Far);
                MarkNearAs(i - 2, j + 0, Tile.CellState.Far);
                MarkNearAs(i - 1, j - 1, Tile.CellState.Far);
                MarkNearAs(i + 1, j - 1, Tile.CellState.Far);
            }
        }

        public void SelectCell(int i, int j)
        {
            if (Tiles[i, j].State == Tile.CellState.None)
            {                
                if (Gems[i, j] == null) return;
                
                if (Gems[i, j].team == Gem.Team.Red && State != MapState.RedTurn) return;
                if (Gems[i, j].team == Gem.Team.Blue && State != MapState.BlueTurn) return;

                ClearAllCellState();

                Tiles[i, j].ChangeState(Tile.CellState.Selected);
                MarkNearAs(i, j, Tile.CellState.Near);
            } 
            else if (Tiles[i, j].State == Tile.CellState.Near)
            {

            }

        }

        public void OnMouseClick(int i, int j)
        {
            SelectCell(i, j);
        }

        private void AddHexagonCollider(My2DSprite sprite)
        {
            PolygonCollider collider = new PolygonCollider();
            collider.AddVertex(new Vector2(0, sprite.Height / 2.0f));
            collider.AddVertex(new Vector2(sprite.Width/4.0f, 0));
            collider.AddVertex(new Vector2(0.75f*sprite.Width, 0));
            collider.AddVertex(new Vector2(sprite.Width, sprite.Height / 2.0f));
            collider.AddVertex(new Vector2(0.75f * sprite.Width, sprite.Height));
            collider.AddVertex(new Vector2(sprite.Width / 4.0f, sprite.Height));

            sprite.SetCollider(collider);
        }

        private void LoadTile(string[] strTextures)
        {
            if (tiles != null)
                tiles.Clear();
            else tiles = new List<Tile>();
            int nTextures = strTextures.Length;

            int startRow = (height - 1) / 2;
            int lastRow = (height - 1) * 2 - startRow;

            for (int j = 0; j < nCols; j++)
            {
                for (int i = startRow; i <= lastRow; i += 2)
                {
                    Tiles[i, j] = new Tile();
                    float left = this.Left + j * TileWidth;
                    float top = this.Top + i * TileHeight;
                    Tiles[i, j].sprite = StaticSprite.CreateSprite(left, top, new Vector2(1, 1), @"Sprite\GameUI\" + strTextures[0], 0.5f, (int)((float)TileWidth * 4 / 3), 2 * TileHeight);
                    Tiles[i, j].i = i;
                    Tiles[i, j].j = j;
                    AddHexagonCollider(Tiles[i, j].sprite);
                    CellStyle.Assign(Tiles[i,j].sprite);
                    tiles.Add(Tiles[i,j]);
                }
                if (j < (height - 1) / 2) startRow--;
                else startRow++;
                lastRow = (height - 1) * 2 - startRow;
            }            
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (tiles != null)
            {
                foreach (var tile in tiles)
                    tile.sprite.Update(gameTime);
            }
        }

        public override void Draw(GameTime gameTime, object param)
        {
            int startRow = (height - 1) / 2;
            int lastRow = (height-1)*2-startRow;

            int i, j;

            foreach (var tile in tiles)
            {
                i = tile.i; j = tile.j;
                if (IsVisible(i, j))
                {
                    DrawTile(i, j, gameTime, (SpriteBatch)param);
                }
                if (j < (height - 1) / 2) startRow--;
                else startRow++;
                lastRow = (height - 1) * 2 - startRow;
            }

            foreach (var gem in gems)
            {
                gem.sprite.Draw(gameTime, (SpriteBatch)param);
            }
        }

        private void DrawTile(int i, int j, GameTime gameTime, SpriteBatch spriteBatch)
        {           
            Tiles[i,j].sprite.Draw(gameTime, spriteBatch);
        }

        private bool IsVisible(int i, int j)
        {
            return true;
        }
    }
}
