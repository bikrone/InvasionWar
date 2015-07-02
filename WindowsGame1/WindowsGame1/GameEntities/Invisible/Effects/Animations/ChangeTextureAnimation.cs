using InvasionWar.Effects;
using InvasionWar.GameEntities.Visible;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InvasionWar.GameEntities.Invisible.Effects.Animations
{
    public class ChangeTextureAnimation: Animation
    {
        private List<Texture2D> original;
        private List<Texture2D> to;
        public ChangeTextureAnimation(Storyboard sb, Sprite2D sprite, List<Texture2D> to, bool isInfinite = true, bool isReserveProperty = false)
        {
            this.isReserveProperty = isReserveProperty;
            this.to = to;
            this.sprite = sprite;
            this.original = sprite.Textures;
            this.isInfinite = isInfinite;
            this.storyboard = sb;
        }

        public override void Start() {
            if (!isStarted)
            {
                isStarted = true;
                sprite.Textures = this.to;
                if (!isInfinite) this.Stop();
            }
        }

        public override void Stop()
        {
            if (isStarted)
            {
                isStarted = false;
                if (isReserveProperty)
                {
                    sprite.Textures = this.original;
                    storyboard.SendCompleted();
                }
            }
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            
        }
    }
}
