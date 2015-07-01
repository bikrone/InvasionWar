using InvasionWar.Effects;
using InvasionWar.Effects.Animations;
using InvasionWar.Helper;
using InvasionWar.Styles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;

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

        public enum VisualState
        {
            Transitioning, Idle, Disabled
        }

        public MapState State = MapState.RedTurn;
        public VisualState visualState = VisualState.Idle;

        public class Tile
        {
            public static HexagonMap map;
            public My2DSprite sprite;
            public int i;
            public int j;

            private static string[] CellImages = { "hexa_selected", "hexa_near", "hexa_far", "Hexa" };

            public enum CellState { Selected, Near, Far, None }
            public CellState State;         

            public Tile(My2DSprite sprite, int i, int j)
            {
                State = CellState.None;        
                this.sprite = sprite;
                this.i = i;
                this.j = j;
                this.sprite.OnMouseUp += OnMouseUp;
                Global.gMouseHelper.Register(this.sprite);
            }

           
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

        private Tile selectedCell = null;

        public Vector2 GetGemVisualPosition(int i, int j) {
            Vector2 position = new Vector2();

            position.X = Tiles[i, j].sprite.Left + (Tiles[i,j].sprite.Width - Gem.Size.X * Global.thisGame.ScreenScaleFactor.X)/2.0f;
            position.Y = Tiles[i, j].sprite.Top + (Tiles[i,j].sprite.Height - Gem.Size.X * Global.thisGame.ScreenScaleFactor.Y)/2.0f;

            return position;
        }

        public HexagonMap(int left, int top, int height, int tileWidth, int tileHeight, string[] strTextures)
        {
            Gem.map = this;
            Tile.map = this;

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

            //SelectCell(4, 0);
        }

        public void AddGem(int i, int j, Gem.Team team)
        {
            Gem gem = new Gem(team, i, j);
            Gems[i, j] = gem;
            gems.Add(gem);
        }

        private void ClearAllCellState()
        {
            foreach (var tile in tiles)
            {
                tile.ChangeState(Tile.CellState.None);
            }
        }

        private void SetCellState(int i, int j, Tile.CellState state)
        {
            if (i < 0 || i >= height * 2 || j < 0 || j > height) return;
            if (Tiles[i, j] == null) return;
            if (Tiles[i,j].State == Tile.CellState.None && Gems[i,j] == null)
                Tiles[i, j].ChangeState(state);
        }

        private void MarkNearAs(int i, int j, Tile.CellState state)
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

        private void AddEnemyCellToList(List<Tile> list, Gem myGem, int i, int j)
        {
            if (i < 0 || i >= height * 2 || j < 0 || j > height) return;
            if (Tiles[i, j] != null && Gems[i,j]!=null && myGem.team!=Gems[i,j].team)
            {
                list.Add(Tiles[i, j]);
            }
        }

        private List<Tile> GetAdjacentCell(Gem gem) {
            int i = gem.i, j = gem.j;

            var list = new List<Tile>();
            AddEnemyCellToList(list, gem, i - 1, j + 1);
            AddEnemyCellToList(list, gem, i + 1, j + 1);
            AddEnemyCellToList(list, gem, i + 2, j + 0);
            AddEnemyCellToList(list, gem, i - 2, j + 0);
            AddEnemyCellToList(list, gem, i - 1, j - 1);
            AddEnemyCellToList(list, gem, i + 1, j - 1);
            return list;
        }

        public void SelectCell(int i, int j)
        {
            if (visualState == VisualState.Transitioning || visualState == VisualState.Disabled) return;
            if (Tiles[i, j].State == Tile.CellState.None)
            {                
                if (Gems[i, j] == null) return;
                
                if (Gems[i, j].team == Gem.Team.Red && State != MapState.RedTurn) return;
                if (Gems[i, j].team == Gem.Team.Blue && State != MapState.BlueTurn) return;

                ClearAllCellState();

                selectedCell = Tiles[i,j];

                Tiles[i, j].ChangeState(Tile.CellState.Selected);
                MarkNearAs(i, j, Tile.CellState.Near);
            } 
            else if (Tiles[i, j].State == Tile.CellState.Near)
            {
                if (selectedCell != null)
                {
                    ClearAllCellState();
                    DuplicateCell(selectedCell.i, selectedCell.j, i, j);
                }
            }
            else if (Tiles[i, j].State == Tile.CellState.Far)
            {
                if (selectedCell != null)
                {
                    ClearAllCellState();
                    MoveCell(selectedCell.i, selectedCell.j, i, j);
                }
            }

        }

        public void OnOvertakeCompleted(object sender, object argument)
        {
            visualState = VisualState.Idle;            

            if (argument != null)
            {
                List<Gem> toFades = (List<Gem>)argument;
                foreach (var toFade in toFades)
                {
                    gems.Remove(toFade);
                }
            }
            if (sender is Storyboard)
            {
                ((Storyboard)sender).OnCompleted -= OnOvertakeCompleted;
            }

            if (State == MapState.BlueTurn) State = MapState.RedTurn;
            else State = MapState.BlueTurn;
        }

        public void OvertakeEnemies(Gem gem)
        {
            visualState = VisualState.Transitioning;
            List<Tile> adjacents = GetAdjacentCell(gem);
            if (adjacents.Count == 0)
            {
                OnOvertakeCompleted(this, null);
                return;
            }

            Storyboard sb = new Storyboard();
            List<Gem> toFades = new List<Gem>();
            foreach (Tile adjacent in adjacents)
            {
                Gem toOvertake = new Gem(gem);
                toOvertake.i = adjacent.i;
                toOvertake.j = adjacent.j;
                
                gems.Add(toOvertake);

                Gem toFade = Gems[adjacent.i, adjacent.j];
                toFades.Add(toFade);      

                Gems[toOvertake.i, toOvertake.j] = toOvertake;

                Animation anim = new TranslationAnimation(sb, toOvertake.sprite, GameSettings.GemTranslationDuration, GetGemVisualPosition(toOvertake.i, toOvertake.j), false);
                sb.AddAnimation(anim);

                anim = new ColorAnimation(sb, toFade.sprite, GameSettings.GemTranslationDuration, new Vector4(0,0,0,0), false);
                sb.AddAnimation(anim);            
            }

            sb.argument = toFades;
            sb.Start();
            sb.OnCompleted += OnOvertakeCompleted;
        }

        public void OnMoveCellCompleted(object sender, object argument)
        {
            if (sender is Storyboard)
            {
                ((Storyboard)sender).OnCompleted -= OnMoveCellCompleted;
            }
            Gem justMoved = (Gem)argument;
            var timer = new Timer();
            timer.Interval = GameSettings.GemTransitionToOvertake*1000.0f;
            timer.Elapsed += (s, e) => {
                var t = (Timer)s;
                t.Stop();                
                OvertakeEnemies(justMoved); 
            };
            timer.Enabled = true;
            timer.Start();
        }
        

        private void DuplicateCell(int i, int j, int x, int y)
        {
            visualState = VisualState.Transitioning;
            Gems[x, y] = new Gem(Gems[i, j]);

            Gems[x, y].i = x;
            Gems[x, y].j = y;

            this.gems.Add(Gems[x, y]);

            Storyboard sb = new Storyboard();
            Animation anim = new TranslationAnimation(sb, Gems[x, y].sprite, GameSettings.GemTranslationDuration, GetGemVisualPosition(x, y), false);
            sb.AddAnimation(anim);
            sb.argument = Gems[x, y];
            sb.Start();
            sb.OnCompleted += OnMoveCellCompleted;
        }

        private void MoveCell(int i, int j, int x, int y)
        {
            visualState = VisualState.Transitioning;
            var toMove = Gems[i,j];
            toMove.i = x;
            toMove.j = y;

            Gems[i, j] = null;
            Gems[x, y] = toMove;

            Storyboard sb = new Storyboard();
            Animation anim = new TranslationAnimation(sb, toMove.sprite, GameSettings.GemTranslationDuration, GetGemVisualPosition(x, y), false);
            sb.AddAnimation(anim);
            sb.argument = Gems[x, y];
            sb.Start();
            sb.OnCompleted += OnMoveCellCompleted;
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
                    
                    float left = this.Left + j * TileWidth;
                    float top = this.Top + i * TileHeight;
                    var sprite = StaticSprite.CreateSprite(left, top, new Vector2(1, 1), @"Sprite\GameUI\" + strTextures[0], 0.5f, (int)((float)TileWidth * 4 / 3), 2 * TileHeight);

                    Tiles[i, j] = new Tile(sprite, i, j);

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

                foreach (var gem in gems)
                {
                    bool isAssign = false;
                    if ((gem.team == Gem.Team.Blue && State == MapState.BlueTurn) || (gem.team == Gem.Team.Red && State == MapState.RedTurn))
                    {
                        if (visualState == VisualState.Idle)
                        {
                            isAssign = true;
                        }
                    }

                    if (isAssign)
                    {
                        SelectedStyleZoomInOut.Assign(gem.sprite);
                    }
                    else
                    {
                        SelectedStyleZoomInOut.Unassign(gem.sprite);
                    }

                    gem.sprite.Update(gameTime);
                }
            }
        }

        public override void Draw(GameTime gameTime, object param)
        {
            int startRow = (height - 1) / 2;
            int lastRow = (height-1)*2-startRow;

            int i, j;

            foreach (var tile in tiles)
            {
                tile.sprite.Draw(gameTime, (SpriteBatch)param);
            }

            foreach (var gem in gems)
            {
                gem.sprite.Draw(gameTime, (SpriteBatch)param);
            }
        }

        private bool IsVisible(int i, int j)
        {
            return true;
        }
    }
}
