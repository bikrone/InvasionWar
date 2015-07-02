using InvasionWar.GameEntities.Invisible.Effects.GraphFunctions;
using InvasionWar.GameEntities.Visible;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InvasionWar.Effects
{
    public abstract class Animation
    {
        public bool isFromNull = true;
        public bool isAnimatedFromOrigin;
        public float duration = 0.0f;
        public bool isLoop = false;
        public bool isInfinite = false;
        public bool isReserveProperty = true;        

        protected bool isStarted = false;

        public TimeSpan Duration;
        public TimeSpan CurrentTime;

        public GraphFunction graphFunction;

        public void SetGraphFunction(GraphFunction function)
        {
            this.graphFunction = function;
        }

        public virtual void Start()
        {
            isStarted = true;
        }

        public Sprite2D sprite { get; set; }

        public Storyboard storyboard;

        public Animation()
        {

        }        

        public Animation(Storyboard storyboard, Sprite2D sprite)
        {
            this.storyboard = storyboard;
            this.sprite = sprite;
        }

        public abstract void Update(GameTime gameTime);

        public abstract void Stop();
    }
}
