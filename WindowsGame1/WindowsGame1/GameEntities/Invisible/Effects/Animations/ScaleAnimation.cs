using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using InvasionWar.GameEntities.Visible;
using InvasionWar.Helper;

namespace InvasionWar.Effects.Animations
{
    public class ScaleAnimation : Animation
    {
        public Vector2 toSize;
        public Vector2 fromSize;

        public Vector2 fromScale, toScale;

        public Vector2 originalSize;

        private Vector2 currentSize;

        private Vector2 velocity = new Vector2();

        private Vector2 Sign = new Vector2(1, 1);

        public override void Update(GameTime gameTime)
        {
            if (!isStarted) return;

            UtilityHelper.ApplyVelocity(ref currentSize, UtilityHelper.MultiplyVector(velocity, Sign), gameTime);

            currentSize.X = MathHelper.Clamp(currentSize.X, Math.Min(fromSize.X, toSize.X), Math.Max(fromSize.X, toSize.X));
            currentSize.Y = MathHelper.Clamp(currentSize.Y, Math.Min(fromSize.Y, toSize.Y), Math.Max(fromSize.Y, toSize.Y));

            sprite.SetSize(currentSize);

            if (isLoop)
            {
                if (currentSize.X == fromSize.X || currentSize.X == toSize.X) Sign.X *= -1;
                if (currentSize.Y == fromSize.Y || currentSize.Y == toSize.Y) Sign.Y *= -1;
            }
            else
            {
                if (currentSize.X == toSize.X && currentSize.Y == toSize.Y)
                {
                    if (!isInfinite) this.Stop();
                    return;
                }
            }            
        }

        public override void Stop()
        {
            if (!isStarted) return;

            isStarted = false;
            if (isReserveProperty)
            {
                sprite.SetSize(this.originalSize);
            }

            storyboard.SendCompleted();
        }

        public override void Start()
        {
            isStarted = true;            

            this.originalSize = new Vector2(sprite.Width, sprite.Height);

            if (isFromNull) fromScale = new Vector2(1, 1);

            fromSize = UtilityHelper.MultiplyVector(this.originalSize, this.fromScale);
            toSize = UtilityHelper.MultiplyVector(this.originalSize, this.toScale);   

            if (duration == 0)
            {
                sprite.SetSize(this.toSize);
                if (!isInfinite) this.Stop();
                return;
            }                 

            velocity = UtilityHelper.CalculateVelocity(this.fromSize, this.toSize, duration);

            currentSize = this.fromSize;

            if (isAnimatedFromOrigin)
            {
                Sign.X = -1; Sign.Y = -1;
                currentSize = this.originalSize;
            }
            else
            {
                sprite.SetSize(this.currentSize);
            }
        }


        public ScaleAnimation(Storyboard storyboard, My2DSprite sprite, float duration, Vector2 toScale, bool isReserveProperty = true, Vector2? fromScale = null, bool isAnimatedFromOrigin = false, bool isLoop = false, bool isInfinite = false)
        {
            this.isInfinite = isInfinite;
            this.isLoop = isLoop;
            this.storyboard = storyboard;
            this.sprite = sprite;
            this.duration = duration;
            this.isReserveProperty = isReserveProperty;
            this.toScale = toScale;
           
            isFromNull = fromScale == null;
            if (!isFromNull)
            {
                this.fromScale = fromScale.Value;
            }                                               
        }
    }
}
