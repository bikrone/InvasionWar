using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using InvasionWar.GameEntities.Visible;
using InvasionWar.Helper;

namespace InvasionWar.Effects.Animations
{
    public class ColorAnimation : Animation
    {                
        public Vector4 toColor;        
        public Vector4 fromColor;

        public Vector4 originalColor;

        private Vector4 currentColor;

        private Vector4 velocity = new Vector4();

        private Vector4 Sign = new Vector4(1, 1, 1, 1);

        public override void Update(GameTime gameTime)
        {
            if (!isStarted) return;

            UtilityHelper.ApplyVelocity(ref currentColor, Vector4.Multiply(velocity, Sign), gameTime);

            currentColor.X = MathHelper.Clamp(currentColor.X, Math.Min(fromColor.X, toColor.X), Math.Max(fromColor.X, toColor.X));
            currentColor.Y = MathHelper.Clamp(currentColor.Y, Math.Min(fromColor.Y, toColor.Y), Math.Max(fromColor.Y, toColor.Y));
            currentColor.Z = MathHelper.Clamp(currentColor.Z, Math.Min(fromColor.Z, toColor.Z), Math.Max(fromColor.Z, toColor.Z));
            currentColor.W = MathHelper.Clamp(currentColor.W, Math.Min(fromColor.W, toColor.W), Math.Max(fromColor.W, toColor.W));

            sprite.SetOverlay(currentColor);

            if (isLoop)
            {
                if (currentColor.X == fromColor.X || currentColor.X == toColor.X)
                {
                    Sign.X *= -1;
                }
                if (currentColor.Y == fromColor.Y || currentColor.Y == toColor.Y)
                {
                    Sign.Y *= -1;
                }
                if (currentColor.Z == fromColor.Z || currentColor.Z == toColor.Z)
                {
                    Sign.Z *= -1;
                }
                if (currentColor.W == fromColor.W || currentColor.W == toColor.W)
                {
                    Sign.W *= -1;
                }
            }
            else
            {
                if (currentColor.X == toColor.X && currentColor.Y == toColor.Y && currentColor.Z == toColor.Z && currentColor.W == toColor.W)
                {
                    if (!isInfinite) this.Stop();
                    return;
                }
            }           
        }

        public override void Start() {
            isStarted = true;
            if (duration == 0)
            {
                sprite.SetOverlay(this.toColor);
                if (!isInfinite) this.Stop();
                return;
            }

            this.originalColor = sprite.GetOverlay();
            if (isFromNull) this.fromColor = this.originalColor;    

            velocity = UtilityHelper.CalculateVelocity(this.fromColor, this.toColor, duration);

            currentColor = this.fromColor;

            if (isAnimatedFromOrigin)
            {
                currentColor = this.originalColor;
                Sign.X = -1; Sign.Y = -1;
            }
            else
            {
                sprite.SetOverlay(this.currentColor);
            }
        }

        public override void Stop()
        {
            if (!isStarted) return;

            isStarted = false;
            if (isReserveProperty)
            {
                sprite.SetOverlay(this.originalColor);
            }

            storyboard.SendCompleted();
        }


        public ColorAnimation(Storyboard storyboard, My2DSprite sprite, float duration, Vector4 toColor, bool isReserveProperty = true, Vector4? fromColor = null, bool isAnimatedFromOrigin = false, bool isLoop = false, bool isInfinite = false)
        {
            this.isInfinite = isInfinite;
            this.isAnimatedFromOrigin = isAnimatedFromOrigin;
            this.isLoop = isLoop;
            this.storyboard = storyboard;
            this.sprite = sprite;
            this.duration = duration;
            this.isReserveProperty = isReserveProperty;            
            this.toColor = toColor;

            if (!isFromNull)
            {
                this.fromColor = fromColor.Value;
            }

            isFromNull = fromColor == null;                                       
        }
    }
}
