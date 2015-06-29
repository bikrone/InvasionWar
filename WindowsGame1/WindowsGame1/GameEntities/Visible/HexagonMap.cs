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

        public My2DSprite[] Tiles;
        public int[,] MapData;

        public HexagonMap(int left, int top, int height, int tileWidth, int tileHeight, string[] strTextures)
        {
            this.height = height;
            this.nRows = height*2-1;
            this.nCols = height;
            this.TileWidth = tileWidth;
            this.TileHeight = tileHeight;
         
            this.MapData = new int[height*2, height+1];
            this.Left = left;
            this.Top = top;

            LoadTile(strTextures);
        }

        private void LoadTile(string[] strTextures)
        {
            int nTextures = strTextures.Length;
            Tiles = new My2DSprite[nTextures];

            for (int i = 0; i < nTextures; i++)
            {
                Tiles[i] = StaticSprite.CreateSprite(0, 0, new Vector2(1,1),  @"Sprite\GameUI\"+strTextures[i], 0.5f, (int)((float)TileWidth*4/3), 2*TileHeight);
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            for (int i = 0; i < Tiles.Length; i++)
                Tiles[i].Update(gameTime);
        }

        public override void Draw(GameTime gameTime, object param)
        {
            int startRow = (height - 1) / 2;
            int lastRow = (height-1)*2-startRow;
            
            for (int j = 0; j < nCols; j++)
            {
                for (int i = startRow; i <= lastRow; i += 2)
                {                    
                    if (IsVisible(i, j))
                    {
                        DrawTile(i, j, gameTime, (SpriteBatch)param);
                    }
                }
                if (j < (height - 1) / 2) startRow--;
                else startRow++;
                lastRow = (height - 1) * 2 - startRow;
            }
        }

        private void DrawTile(int i, int j, GameTime gameTime, SpriteBatch spriteBatch)
        {
            // Where to draw
            float left = this.Left + j * TileWidth;
            float top = this.Top + i * TileHeight;

            // What to draw
            int TileTypeID = 0;
            Tiles[TileTypeID].Left = left;
            Tiles[TileTypeID].Top = top;
            Tiles[TileTypeID].Draw(gameTime, spriteBatch);
        }

        private bool IsVisible(int i, int j)
        {
            return true;
        }
    }
}
