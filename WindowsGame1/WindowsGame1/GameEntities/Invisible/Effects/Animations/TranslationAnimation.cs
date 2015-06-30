using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using InvasionWar.GameEntities.Visible;
using InvasionWar.Helper;

namespace InvasionWar.Effects.Animations
{
    public class TranslationAnimation : Animation
    {
        
        public Vector2 toPosition;
        public Vector2 fromPosition;

        public Vector2 originalPosition;

        private Vector2 currentPosition;

        private Vector2 velocity = new Vector2();

        private Vector2 Sign = new Vector2(1,1);

        public override void Update(GameTime gameTime)
        {
            if (!isStarted) return;

            UtilityHelper.ApplyVelocity(ref currentPosition, UtilityHelper.MultiplyVector(velocity, Sign), gameTime);

            currentPosition.X = MathHelper.Clamp(currentPosition.X, Math.Min(fromPosition.X, toPosition.X), Math.Max(fromPosition.X, toPosition.X));            
            currentPosition.Y = MathHelper.Clamp(currentPosition.Y, Math.Min(fromPosition.Y, toPosition.Y), Math.Max(fromPosition.Y, toPosition.Y));

            sprite.SetPosition(currentPosition);

            if (isLoop)
            {
                if (currentPosition.X == fromPosition.X || currentPosition.X == toPosition.X)
                {
                    Sign.X *= -1;
                }
                if (currentPosition.Y == fromPosition.Y || currentPosition.Y == toPosition.Y)
                {
                    Sign.Y *= -1;
                }
            }
            else
            {
                if (currentPosition.X == toPosition.X && currentPosition.Y == toPosition.Y)
                {
                    if (!isInfinite)
                        this.Stop();
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
                sprite.SetPosition(this.originalPosition);
            }

            storyboard.SendCompleted();
        }
        public override void Start()
        {
            isStarted = true;
            this.originalPosition = new Vector2(sprite.Left, sprite.Top);

            if (duration == 0)
            {
                sprite.SetPosition(this.toPosition);
                if (!isInfinite) this.Stop();
                return;
            }

            if (isFromNull) fromPosition = this.originalPosition;

            velocity = UtilityHelper.CalculateVelocity(this.fromPosition, this.toPosition, duration);

            currentPosition = this.fromPosition;

            if (isAnimatedFromOrigin)
            {
                currentPosition = this.originalPosition;
                Sign.X = -1; Sign.Y = -1;
            }
            else
            {
                sprite.SetPosition(this.currentPosition);
            }                        
        }

        public TranslationAnimation(Storyboard storyboard, My2DSprite sprite, float duration, Vector2 toPosition, bool isReserveProperty = true, Vector2? fromPosition = null, bool isAnimatedFromOrigin = false, bool isLoop = false, bool isInfinite = false)
        {
            this.isInfinite = isInfinite;
            this.isLoop = isLoop;
            this.storyboard = storyboard;
            this.sprite = sprite;
            this.duration = duration;
            this.isReserveProperty = isReserveProperty;            
            this.toPosition = toPosition;

            isFromNull = fromPosition == null;

            if (!isFromNull)
            {
                this.fromPosition = fromPosition.Value;
            }            
            
        }
    }
}
