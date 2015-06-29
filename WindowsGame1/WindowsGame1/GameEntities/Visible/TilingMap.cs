using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace InvasionWar.GameEntities.Visible
{
    public class TilingMap : VisibleGameEntity
    {
        public int nRows, nCols;
        public int TileWidth, TileHeight;
        public int MapWidth, MapHeight;

        public My2DSprite[] Tiles;
        public int[,] MapData;

        public TilingMap(int nRows, int nCols, int tileWidth, int tileHeight, string[] strTextures, int[,] mapData)
        {
            this.nRows = nRows;
            this.nCols = nCols;
            this.TileWidth = tileWidth;
            this.TileHeight = tileHeight;
            this.MapWidth = this.TileWidth * this.nCols;
            this.MapHeight = this.TileHeight * this.nRows;
            this.MapData = mapData;

            LoadTile(strTextures);
        }

        private void LoadTile(string[] strTextures)
        {
            int nTextures = strTextures.Length;
            Tiles = new My2DSprite[nTextures];

            for (int i = 0; i < nTextures; i++)
            {
                Tiles[i] = new My2DSprite(LoadTextures(strTextures[i]), 0, 0, TileWidth, TileHeight);
            }
        }

        private List<Texture2D> LoadTextures(string strTexture)
        {
            List<Texture2D> result = new List<Texture2D>();
            result.Add(Global.Content.Load<Texture2D>(@"Terrain\"+strTexture));
            return result;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            for (int i = 0; i < Tiles.Length; i++)
                Tiles[i].Update(gameTime);
        }

        public override void Draw(GameTime gameTime, object param)
        {
            for (int i = 0; i < nRows; i++)
            {
                for (int j = 0; j < nCols; j++)
                {
                    if (IsVisible(i, j))
                    {
                        DrawTile(i, j, gameTime, (SpriteBatch)param);
                    }
                }
            }
        }

        private void DrawTile(int i, int j, GameTime gameTime, SpriteBatch spriteBatch)
        {
            // Where to draw
            float left = j * TileWidth;
            float top = i * TileHeight;

            // What to draw
            int TileTypeID = MapData[i, j];
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
