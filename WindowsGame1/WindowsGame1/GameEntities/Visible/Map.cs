using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InvasionWar.GameEntities.Visible
{
    public class Map : VisibleGameEntity
    {
        public int nRows, nCols;
        public int FragmentWidth, FragmentHeight;
        public int MapWidth, MapHeight;

        public Sprite2D[,] Fragments;

        public Map(int nRows, int nCols, int fragmentWidth, int fragmentHeight, string strTexturePrefix)
        {
            this.nRows = nRows;
            this.nCols = nCols;
            this.FragmentWidth = fragmentWidth;
            this.FragmentHeight = fragmentHeight;
            this.MapWidth = this.FragmentWidth * this.nCols;
            this.MapHeight = this.FragmentHeight * this.nRows;
            LoadFragments(strTexturePrefix);
        }

        private void LoadFragments(string strTexturePrefix)
        {
            Fragments = new Sprite2D[nRows, nCols];

            for (int i=0; i<nRows; i++)
                for (int j=0; j<nCols; j++)
                {
                    Fragments[i, j] = LoadFragment(strTexturePrefix, i, j);
                }
        }

        private Sprite2D LoadFragment(string strTexturePrefix, int i, int j)
        {
            Sprite2D result;

            result = new Sprite2D(LoadTexture(strTexturePrefix + i.ToString("00") + "_" + j.ToString("00")),
                j * FragmentWidth, i*FragmentHeight,
                FragmentWidth, FragmentHeight);

            return result;
        }

        private List<Texture2D> LoadTexture(string strTexture)
        {
            List<Texture2D> result = new List<Texture2D>();
            result.Add(Global.Content.Load<Texture2D>(strTexture));
            return result;
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            base.Update(gameTime);
            for (int i=0; i<nRows; i++)
                for (int j=0; j<nCols; j++)
                    Fragments[i,j].Update(gameTime);
        }

        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime, object param)
        {
            for (int i = 0; i < nRows; i++)
            {
                for (int j = 0; j < nCols; j++)
                {
                    if (IsVisible(i, j))
                        Fragments[i, j].Draw(gameTime, (SpriteBatch)param);
                }
            }
        }

        private bool IsVisible(int i, int j)
        {
            float Wleft, Wright, Wtop, Wbottom;
            float Sleft, Sright, Stop, Sbottom;

            Wleft = j * FragmentWidth;
            Wright = (j + 1) * FragmentWidth - 1;
            Wtop = i * FragmentHeight;
            Wbottom = (i + 1) * FragmentHeight - 1;

            // world to screen
            Vector3 WtopLeft = new Vector3(Wleft, Wtop, 1);
            Vector3 WbottomRight = new Vector3(Wright, Wbottom, 1);
            Vector3 StopLeft, SbottomRight;
            StopLeft = Vector3.Transform(WtopLeft, Global.gMainCamera.WVP);
            SbottomRight = Vector3.Transform(WbottomRight, Global.gMainCamera.WVP);

            Sleft = Math.Min(StopLeft.X, SbottomRight.X);
            Sright = Math.Max(StopLeft.X, SbottomRight.X);
            Stop = Math.Min(StopLeft.Y, SbottomRight.Y);
            Sbottom = Math.Max(StopLeft.Y, SbottomRight.Y);

            if (Sright < 0) return false;
            if (Sleft > FragmentWidth) return false;
            if (Sbottom < 0) return false;
            if (Stop > FragmentHeight) return false;
            return true;
        }
    }
}

