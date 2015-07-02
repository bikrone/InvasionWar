using InvasionWar.Effects;
using InvasionWar.Helper;
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
    

    public class Sprite2D : VisibleGameEntity, IObserver
    {
        public PolygonCollider Collider;

        public void SetCollider(PolygonCollider collider) {
            this.Collider = collider;
        }

        public delegate void Del(object sender);

        private Color overlay = new Color(255, 255, 255, 255);

        public List<Storyboard> states = new List<Storyboard>();

        private float rotation = 0;

        public void SetRotation(float rotation)
        {
            this.rotation = rotation;
        }

        public float GetRotation() { return rotation; }

        public Storyboard currentState = null;

        public void ClearState()
        {
            if (currentState != null) currentState.Stop();
            states.Clear();
        }

        public void AddChildAtMid(Sprite2D sprite)
        {
            AddChild(sprite);
            sprite.Left = (Width - sprite.Width) / 2;
            sprite.Top = (Height - sprite.Height) / 2;
        }

        public void ChangeState(int i)
        {
            if (i == -1)
            {
                if (currentState != null)
                {
                    currentState.Stop();
                    currentState = null;
                }
                return;
            }

            if (currentState == null || currentState != states[i])
            {
                if (currentState != null)
                {
                    currentState.Stop();
                }
                currentState = states[i];
                currentState.Start();
            }
        }

        public void AddNewState(Storyboard a)
        {
            states.Add(a);
        }

        public void AddNewState(Animation a)
        {
            Storyboard sb = new Storyboard();
            sb.AddAnimation(a);
            states.Add(sb);
        }
        
        public Vector4 GetOverlay()
        {
            return new Vector4(overlay.A, overlay.B, overlay.G, overlay.R);
        }

        public void SetOverlay(Vector4 color)
        {
            overlay.A = (byte)color.X;
            overlay.B = (byte)color.Y;
            overlay.G = (byte)color.Z;
            overlay.R = (byte)color.W;
        }

        private bool isEnable = true;
        public bool IsEnable
        {
            get { return isEnable; }
            set { isEnable = value; }
        }

        public bool IsAvailable() { return IsEnable; }        

        public Del OnMouseClick;
        public Del OnMouseDown;
        public Del OnMouseUp;
        public Del OnMouseMove;
        public Del OnMouseLeave;

        private Vector2 scale = new Vector2(1, 1);
        public Vector2 Scale
        {
            get { return scale; }            
        }

        public Sprite2D()
        {

        }

        public Sprite2D(List<Texture2D> textures, 
            float left, float top, int width, int height, bool isReserveScale = false)
        {
            Textures = textures;
            Left = left;
            Top = top;
            if (textures == null || textures.Count == 0) return;
            if (width == 0 || isReserveScale)
                OriginalWidth = Textures[0].Width;
            else
                OriginalWidth = width;

            if (height == 0 || isReserveScale)
                OriginalHeight = Textures[0].Height;
            else
                OriginalHeight = height;

            if (isReserveScale)
            {
                scale.X = (float)width / OriginalWidth;
                scale.Y = (float)height / OriginalHeight;
            }

            Width = width;
            Height = height;

        }

        public void ReloadTexture(List<Texture2D> textures)
        {
            Textures = textures;
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

        private float _Width;

        public float Width
        {
            get { return _Width; }
            set { _Width = value; }
        }
        private float _Height;

        public float Height
        {
            get { return _Height; }
            set { _Height = value; }
        }

        private float originalWidth, originalHeight;
        public float OriginalWidth
        {
            get { return originalWidth; }
            set { originalWidth = value; }
        }
        public float OriginalHeight
        {
            get { return originalHeight; }
            set { originalHeight = value; }
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

        public void SetPosition(Vector2 position)
        {
            Left = position.X;
            Top = position.Y;
        }

        public void SetSize(Vector2 size)
        {
            var deltaX = (size.X - Width)/2.0f;
            var deltaY = (size.Y - Height)/2.0f;

            Left -= deltaX;
            Top -= deltaY;

            Width += deltaX * 2; Height += deltaY * 2;

        }

        public override void Update(GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            HandleInput();

            if (_nTextures > 1)
            {
                elapsedTimeSinceLastFrame += gameTime.ElapsedGameTime.TotalMilliseconds;
                if (elapsedTimeSinceLastFrame - (1000.0 / Fps) > -Sprite2D.eps)
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

            Left += CurrentVelocity.X * deltaTime;
            Top += CurrentVelocity.Y * deltaTime;

            base.Update(gameTime);
        }

        public float AbsoluteLeft
        {
            get
            {
                var left = Left;
                if (Parent != null && Parent is Sprite2D)
                {
                    left += ((Sprite2D)Parent).AbsoluteLeft;
                }
                return left;
            }
        }

        public float AbsoluteTop
        {
            get
            {
                var top = Top;
                if (Parent != null && Parent is Sprite2D)
                {
                    top += ((Sprite2D)Parent).AbsoluteTop;
                }
                return top;
            }
        }

        public override void Draw(GameTime gameTime, object param)
        {
            if (_Textures != null && _Textures.Count > 0)
            {
                var spriteBatch = (SpriteBatch)param;
                spriteBatch.Draw(
                _Textures[_iTexture], new Rectangle((int)(AbsoluteLeft+Width/2.0f), (int)(AbsoluteTop+Height/2.0f), (int)(Width), (int)(Height)),
                new Rectangle(0, 0, (int)OriginalWidth, (int)OriginalHeight),
                overlay, rotation, new Vector2(OriginalWidth / 2, OriginalHeight / 2), SpriteEffects.None, _Depth);
            }

            base.Draw(gameTime, param);
        }

        private int _State = 0;

        public int State
        {
            get { return _State; }
            set { _State = value; }
        }       

        public bool InMousePosition(Vector2 MousePos)
        {
            if (Collider == null || Collider.vertices.Count < 3)
            {
                float x = MousePos.X, y = MousePos.Y;
                if (x >= AbsoluteLeft && x < AbsoluteLeft + Width
                    && y >= AbsoluteTop && y < AbsoluteTop + Height)
                    return true;
                return false;
            }
            else
            {
                return Collider.Inside(Vector2.Subtract(MousePos, new Vector2(AbsoluteLeft, AbsoluteTop)));
            }
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

        public void SendMouseLeave()
        {
            if (OnMouseLeave != null)
            {
                OnMouseLeave(this);
            }
        }
    }
}
