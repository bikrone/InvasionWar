using InvasionWar.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InvasionWar.GameEntities.Visible
{
    

    public class My2DSprite : VisibleGameEntity, IObserver
    {
        public delegate void Del(object sender);

        public class TransitionTask
        {
            public Vector2 toPosition;
            public float time;
            public Del callback;
            public float currentTime = 0;

            public TransitionTask(Vector2 toPosition, float time, Del callback)
            {
                this.toPosition = toPosition;
                this.time = time;
                this.callback = callback;
            }
        }

        private bool isEnable = true;
        public bool IsEnable
        {
            get { return isEnable; }
            set { isEnable = value; }
        }

        public bool IsAvailable() { return IsEnable; }

        private TransitionTask transitionTask;

        public void SetTransitionTask(Vector2 toPosition, float time, Del callback)
        {
            transitionTask = new TransitionTask(toPosition, time, callback);
        }

        public Del OnMouseClick;
        public Del OnMouseDown;
        public Del OnMouseUp;
        public Del OnMouseMove;

        private Vector2 scale = new Vector2(1,1);
        public Vector2 Scale
        {
            get { return scale; }        
        }

        public My2DSprite()
        {

        }

        public My2DSprite(List<Texture2D> textures, 
            float left, float top, int width, int height, bool isReserveScale = false)
        {
            Textures = textures;
            Left = left;
            Top = top;
            if (width == 0 || isReserveScale)
                Width = Textures[0].Width;
            else
                Width = width;

            if (height == 0 || isReserveScale)
                Height = Textures[0].Height;
            else
                Height = height;

            if (isReserveScale)
            {
                scale.X = (float)width / Width;
                scale.Y = (float)height / Height;                
            }

            presentedWidth = Width * Scale.X;
            presentedHeight = Height * Scale.Y;

        }       

        private List<Texture2D> _Textures;

        public List<Texture2D> Textures
        {
            get { return _Textures; }
            set { 
                _Textures = value;
                _nTextures = _Textures.Count;
                _iTexture = 0;
            }
        }
        private int _nTextures;

        public int NTextures
        {
            get { return _nTextures; }
            set { _nTextures = value; }
        }
        private int _iTexture;

        public int ITexture
        {
            get { return _iTexture; }
            set { _iTexture = value; }
        }
        private float _Top;

        public float Top
        {
            get { return _Top; }
            set { _Top = value; }
        }
        private float _Left;

        public float Left
        {
            get { return _Left; }
            set { _Left = value; }
        }


        private Vector2 Velocity = new Vector2(0, 0);

        public void SetMaxVelocity(float x, float y)
        {
            Velocity.X = x;
            Velocity.Y = y;
        }

        public void SetVelocity(float x, float y)
        {
            CurrentVelocity.X = x;
            CurrentVelocity.Y = y;
        }

        private Vector2 CurrentVelocity = new Vector2(0, 0);
        
        private int _Width;

        public int Width
        {
            get { return _Width; }
            set { _Width = value; }
        }
        private int _Height;

        public int Height
        {
            get { return _Height; }
            set { _Height = value; }
        }

        private float presentedWidth, presentedHeight;
        public float PresentedWidth {
            get {return presentedWidth;}
        }
        public float PresentedHeight
        {
            get { return presentedHeight; }
        }

        public float _Depth = 1;
        private float d1 = 0, d2 = 1;

        private double _fps = 24;
        public double Fps
        {
            get { return _fps; }
            set { _fps = value; }
        }

        private double elapsedTimeSinceLastFrame = 0;
        private static double eps = 2;


        void HandleInput()
        {
            //CurrentVelocity.X = 0;
            //CurrentVelocity.Y = 0;
            //if (Global.gKeyboardHelper.IsKeyDown(Keys.Right))            
            //    CurrentVelocity.X = Velocity.X;

            //if (Global.gKeyboardHelper.IsKeyDown(Keys.Left))
            //    CurrentVelocity.X = -Velocity.X;

            //if (Global.gKeyboardHelper.IsKeyDown(Keys.Up))
            //    CurrentVelocity.Y = -Velocity.Y;

            //if (Global.gKeyboardHelper.IsKeyDown(Keys.Down))
            //    CurrentVelocity.Y = Velocity.Y;    
        }

        private float Sign(float f)
        {
            if (f < 0) return -1;
            else return 1;
        }

        public override void Update(GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            HandleInput();

            if (_nTextures > 1)
            {
                elapsedTimeSinceLastFrame += gameTime.ElapsedGameTime.TotalMilliseconds;
                if (elapsedTimeSinceLastFrame - (1000.0 / Fps) > -My2DSprite.eps)
                {
                    elapsedTimeSinceLastFrame = 0;
                    _iTexture = (_iTexture + 1) % _nTextures;
                }
            }

            if (State == 1)
            {
                d1 += d2;
                if (Math.Abs(d1) == 10)
                    d2 *= -1;
            }

            if (transitionTask != null)
            {
                if (transitionTask.currentTime == 0)
                {
                    SetVelocity((transitionTask.toPosition.X - Left) / transitionTask.time, (transitionTask.toPosition.Y - Top) / transitionTask.time);                    
                }
                transitionTask.currentTime += deltaTime;
                float signX = Sign(CurrentVelocity.X), signY = Sign(CurrentVelocity.Y);
                if (this.Left * signX >= transitionTask.toPosition.X && this.Top * signY >= transitionTask.toPosition.Y * signY)
                {
                    this.Left = transitionTask.toPosition.X;
                    this.Top = transitionTask.toPosition.Y;
                    transitionTask.callback(this);
                    transitionTask = null;
                } 
            }

            Left += CurrentVelocity.X * deltaTime;
            Top += CurrentVelocity.Y * deltaTime;
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (State==0)
                spriteBatch.Draw(
                _Textures[_iTexture], new Vector2(_Left, _Top),
                new Rectangle(0, 0, _Width, _Height), 
                Color.White, 0, Vector2.Zero, Scale, SpriteEffects.None, _Depth);
            else
            {
                spriteBatch.Draw(
                _Textures[_iTexture], new Rectangle((int)(_Left-d1), (int)(_Top-d1), (int)(PresentedWidth+2*d1), (int)(PresentedHeight+2*d1)),
                new Rectangle(0, 0, _Width, _Height),
                Color.Yellow, 0, Vector2.Zero, SpriteEffects.None, _Depth);

            }
        }

        private int _State = 0;

        public int State
        {
            get { return _State; }
            set { _State = value; }
        }       

        public bool InMousePosition(Vector2 MousePos)
        {
            float x = MousePos.X, y = MousePos.Y;

            if (x >= Left && x < Left + PresentedWidth
                && y >= Top && y < Top + PresentedHeight)
                return true;
            return false;
        }

        public void SendMouseMove()
        {
            if (OnMouseMove != null)
                OnMouseMove(this);
        }

        public void SendMouseClick()
        {
            if (OnMouseClick != null)
                OnMouseClick(this);
        }

        public void SendMouseDown()
        {
            if (OnMouseDown != null)
                OnMouseDown(this);
        }

        public void SendMouseUp()
        {
            if (OnMouseUp != null)
                OnMouseUp(this);
        }

        public void AddEventHandler(ref Del a, Del b) {
            if (a == null) a = b;
            else a += b;
        }
    }
}
