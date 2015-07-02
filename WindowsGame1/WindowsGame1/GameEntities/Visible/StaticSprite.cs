using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using InvasionWar.GameEntities.Visible;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace InvasionWar.GameEntities.Visible
{
    public class StaticSprite : Sprite2D
    {
        public StaticSprite(List<Texture2D> textures,
            float left, float top, int width, int height, bool isReserveScale = false)
            : base(textures, left, top, width, height, isReserveScale)
        {

        }

        public void ReloadTexture(string path)
        {
            List<Texture2D> textures = new List<Texture2D>();
            textures.Add(Global.thisGame.Content.Load<Texture2D>(path));
            base.ReloadTexture(textures);
        }

        public static StaticSprite CreateSprite(float left, float top, int width, int height, String path, float depth = 1.0f)
        {          
            List<Texture2D> textures = new List<Texture2D>();
            textures.Add(Global.thisGame.Content.Load<Texture2D>(path));
            StaticSprite temp = new StaticSprite(textures, left, top, width, height);
            temp._Depth = depth;
            return temp;
        }
        

        public static StaticSprite CreateSprite(float left, float top, Vector2 ScreenScaleFactor, String path, float depth = 1.0f, int width = 0, int height = 0)
        {
            List<Texture2D> textures = new List<Texture2D>();
            Texture2D newTexture = Global.thisGame.Content.Load<Texture2D>(path);
            textures.Add(newTexture);

            if (width == 0)
                width = newTexture.Width;        

            if (height == 0 )
                height = newTexture.Height;            

            top = top * ScreenScaleFactor.Y;
            left = left * ScreenScaleFactor.X;

            width = (int)((float)width * ScreenScaleFactor.X);
            height = (int)((float)height * ScreenScaleFactor.Y);

            StaticSprite temp = new StaticSprite(textures, left, top, width, height, true);
            temp._Depth = depth;
            return temp;
        }
    }
}
