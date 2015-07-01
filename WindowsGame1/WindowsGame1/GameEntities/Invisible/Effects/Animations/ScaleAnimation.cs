using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using InvasionWar.GameEntities.Visible;
using InvasionWar.Helper;
using InvasionWar.GameEntities.Invisible.Effects.GraphFunctions;

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

        private Vector2 totalDistance;

        public override void Update(GameTime gameTime)
        {
            if (!isStarted) return;

            // UtilityHelper.ApplyVelocity(ref currentSize, UtilityHelper.MultiplyVector(velocity, Sign), gameTime);
            Vector2 deltaDistance = graphFunction.ApplyVelocity(CurrentTime, CurrentTime.Add(gameTime.ElapsedGameTime), Duration, totalDistance);

            CurrentTime = CurrentTime.Add(gameTime.ElapsedGameTime);            
            deltaDistance = Vector2.Multiply(deltaDistance, Sign);

            currentSize = Vector2.Add(currentSize, deltaDistance);

            currentSize.X = MathHelper.Clamp(currentSize.X, Math.Min(fromSize.X, toSize.X), Math.Max(fromSize.X, toSize.X));
            currentSize.Y = MathHelper.Clamp(currentSize.Y, Math.Min(fromSize.Y, toSize.Y), Math.Max(fromSize.Y, toSize.Y));

            sprite.SetSize(currentSize);

            if (isLoop)
            {
                int completed = 0;
                if (currentSize.X == fromSize.X || currentSize.X == toSize.X)
                {
                    Sign.X *= -1;
                    completed++;
                }
                if (currentSize.Y == fromSize.Y || currentSize.Y == toSize.Y)
                {
                    Sign.Y *= -1;
                    completed++;
                }
                if (completed == 2)
                {
              //      CurrentTime = TimeSpan.Zero;
                }
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
            fromSize.X = (int)fromSize.X; fromSize.Y = (int)fromSize.Y;
            toSize = UtilityHelper.MultiplyVector(this.originalSize, this.toScale);
            toSize.X = (int)toSize.X; toSize.Y = (int)toSize.Y;

            if (duration == 0)
            {
                sprite.SetSize(this.toSize);
                if (!isInfinite) this.Stop();
                return;
            }                 

            velocity = UtilityHelper.CalculateVelocity(this.fromSize, this.toSize, duration);

            Sign = UtilityHelper.CalculateSign(this.fromSize, this.toSize);

            currentSize = this.fromSize;

            if (isAnimatedFromOrigin)
            {
                currentSize = this.originalSize;
                Sign = UtilityHelper.CalculateSign(currentSize, this.fromSize);
            }
            else
            {
                sprite.SetSize(this.currentSize);
            }

            CurrentTime = TimeSpan.Zero;
            Duration = TimeSpan.FromSeconds(duration);
            totalDistance = UtilityHelper.VectorAbs(Vector2.Subtract(toSize, fromSize));

            if (graphFunction == null)
                graphFunction = new ConstantGraphFunction(duration);
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
