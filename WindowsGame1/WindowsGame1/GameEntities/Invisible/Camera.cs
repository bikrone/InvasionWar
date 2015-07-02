using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InvasionWar.GameEntities.Invisible
{
    public class Camera : InvisibleGameEntity
    {
        private Matrix _World = Matrix.Identity;

        public Matrix World
        {
            get { return _World; }
            set { _World = value; }
        }
        private Matrix _View = Matrix.Identity;

        public Matrix View
        {
            get { return _View; }
            set { _View = value; }
        }
        private Matrix _Projection = Matrix.Identity;

        public Matrix Projection
        {
            get { return _Projection; }
            set { _Projection = value; }
        }

        // World Coordinates ====> WVP ====> Screen Coordinates
        public Matrix WVP
        {
            get
            {
                return _World * _View * _Projection;
            }
        }

        // Screen Coordinates ====> INvWVP ====> World Coordinates
        public Matrix InvWVP
        {
            get
            {
                return Matrix.Invert(WVP);
            }
        }

        float dx = 0, dy = 0;

        float translationVelocity = 500;

        public float TranslationVelocity
        {
            get { return translationVelocity; }
            set { translationVelocity = value; }
        }

        public override void Update(GameTime gameTime)
        {
            float deltaTranslation = TranslationVelocity*(float)gameTime.ElapsedGameTime.TotalSeconds;
            //if (Global.gKeyboardHelper.IsKeyDown(Keys.A))
            //{
            //    dx += deltaTranslation;
            //}
            //if (Global.gKeyboardHelper.IsKeyDown(Keys.D)) dx -= deltaTranslation;
            //if (Global.gKeyboardHelper.IsKeyDown(Keys.W)) dy += deltaTranslation;
            //if (Global.gKeyboardHelper.IsKeyDown(Keys.S)) dy -= deltaTranslation;            

            //View = Matrix.CreateTranslation(dx, dy, 0);     
        }
    }
}
