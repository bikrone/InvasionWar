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

        public int ScoreRed, ScoreBlue;

        public enum MapState
        {
            RedTurn, BlueTurn
        }

        public enum WinState
        {
            Red, Blue, None
        }

        public enum VisualState
        {
            Transitioning, Idle, Disabled
        }

        public MapState State = MapState.RedTurn;
        public VisualState visualState = VisualState.Disabled;

        public WinState winState = WinState.None;

        public Tile[,] Tiles;
        public Gem[,] Gems;
        public GameEntity tiles = new GameEntity();
        public GameEntity gems = new GameEntity();
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
            this.nRows = height * 2;
            this.nCols = height;
            this.TileWidth = tileWidth;
            this.TileHeight = tileHeight;                     
            
            this.Left = left;
            this.Top = top;

            Tiles = new Tile[nRows, nCols];

            LoadTile(strTextures);
            this.AddChild(gems);
            this.AddChild(tiles);
        }

        public void ResetMap()
        {
            ClearAllCellState();
            ScoreBlue = ScoreRed = 3;

            gems.ClearChildren();

            Gems = new Gem[nRows, nCols];

            AddGem(4, 0, Gem.Team.Blue);
            AddGem(16, 4, Gem.Team.Blue);
            AddGem(4, 8, Gem.Team.Blue);

            AddGem(0, 4, Gem.Team.Red);
            AddGem(12, 0, Gem.Team.Red);
            AddGem(12, 8, Gem.Team.Red);

            State = MapState.RedTurn;
            winState = WinState.None;
        }

        public void StartSingle()
        {
            ResetMap();
            visualState = VisualState.Idle;
        }

        public void StartMultiple()
        {
            ResetMap();
            visualState = VisualState.Disabled;
        }

        public void AddGem(int i, int j, Gem.Team team)
        {
            Gem gem = new Gem(team, i, j);
            Gems[i, j] = gem;
            gems.AddChild(gem);
        }

        private void ClearAllCellState()
        {
            foreach (Tile tile in tiles.Children)
            {
                tile.ChangeState(Tile.CellState.None);
            }
        }

        private void SetCellState(int i, int j, Tile.CellState state)
        {
            if (i < 0 || i >= nRows || j < 0 || j >= nCols) return;
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


        private int CountRelative(Gem gem)
        {
            int count = 0;
            int[,] delta = { { -1, 1 }, { 1, 1 }, { 2, 0 }, { -2, 0 }, { -1, -1 }, { 1, -1 } };

            bool[,] isVisisted = new bool[nRows, nCols];

            for (int i = 0; i < 6; i++)
            {
                for (int t = 0; t < 6; t++)
                {
                    int ii = gem.i + delta[t, 0] + delta[i, 0];
                    int jj = gem.j + delta[t, 1] + delta[i, 1];
                    if (ii < 0 || ii >= nRows || jj < 0 || jj >= nCols) continue;
                    if (ii == gem.i && jj == gem.j) continue;
                    if (Gems[ii, jj] != null || Tiles[ii,jj]==null) continue;
                    if (!isVisisted[ii, jj])
                    {
                        ++count;
                        isVisisted[ii, jj] = true;
                    }
                }
            }

            return count;
        }

        private void AddEnemyCellToList(List<Tile> list, Gem myGem, int i, int j)
        {
            if (i < 0 || i >= nRows || j < 0 || j >= nCols) return;
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

        public void SelectCell(int i, int j, bool isRemote = false)
        {
            if (winState != WinState.None) return;
            if (!isRemote)
            {
                if (Global.thisGame.playerState != Game1.PlayerState.Both)
                {
                    if (State==MapState.RedTurn && Global.thisGame.playerState != Game1.PlayerState.Red) return;
                    if (State==MapState.BlueTurn && Global.thisGame.playerState != Game1.PlayerState.Blue) return;
                }
            }
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
                if (toFades.Count > 0)
                {
                    if (toFades[0].team == Gem.Team.Blue)
                    {
                        ScoreRed += toFades.Count;
                        ScoreBlue -= toFades.Count;
                    }
                    else
                    {
                        ScoreRed -= toFades.Count;
                        ScoreBlue += toFades.Count;
                    }
                }
                foreach (var toFade in toFades)
                {
                    gems.RemoveChild(toFade);
                }                
            }
            if (sender is Storyboard)
            {
                ((Storyboard)sender).OnCompleted -= OnOvertakeCompleted;
            }            

            if (State == MapState.BlueTurn) State = MapState.RedTurn;
            else State = MapState.BlueTurn;

            CheckWinState();
        }

        private bool isCannotMove()
        {            
            Gem.Team checkTeam = Gem.Team.Red;
            if (State == MapState.BlueTurn) checkTeam = Gem.Team.Blue;

            foreach (Gem gem in gems.Children)
            {
                if (gem.team == checkTeam && CountRelative(gem) > 0) return false;                
            }
            return true;
        }

        public void GoWinBlue()
        {
            winState = WinState.Blue;
        }

        public void GoWinRed()
        {
            winState = WinState.Red;
        }

        public void CheckWinState()
        {
            if (ScoreRed + ScoreBlue == tiles.Children.Count)
            {
                if (ScoreBlue < ScoreRed) GoWinRed();
                else GoWinBlue();
                return;
            }

            if (ScoreRed == 0)
            {
                GoWinBlue();
                return;
            }

            if (ScoreBlue == 0)
            {
                GoWinRed();
                return;
            }

            if (isCannotMove())
            {
                if (State == MapState.BlueTurn) GoWinRed();
                else GoWinBlue();
                return;
            }


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
                
                gems.AddChild(toOvertake);

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

            this.gems.AddChild(Gems[x, y]);
            if (Gems[x, y].team == Gem.Team.Blue)
            {
                ScoreBlue++;
            }
            else { ScoreRed++; }

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

        private void AddHexagonCollider(Sprite2D sprite)
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
                tiles.ClearChildren();
          
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
                    tiles.AddChild(Tiles[i,j]);
                }
                if (j < (height - 1) / 2) startRow--;
                else startRow++;
                lastRow = (height - 1) * 2 - startRow;
            }            
        }

        public override void Update(GameTime gameTime)
        {
            if (visualState == VisualState.Disabled) return;
            foreach (Gem gem in gems.Children)
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
            }
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, object param)
        {
            base.Draw(gameTime, param);
        }

        private bool IsVisible(int i, int j)
        {
            return true;
        }
    }
}
