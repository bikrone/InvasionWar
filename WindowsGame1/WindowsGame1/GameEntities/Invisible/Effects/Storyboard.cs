using InvasionWar.GameEntities.Invisible;
using InvasionWar.GameEntities.Visible;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InvasionWar.Effects
{
    public class Storyboard : InvisibleGameEntity
    {
        public delegate void Callback(object sender);        

        public bool isStarted = false;
        public bool isCompleted = false;

        private int completedCount = 0;

        public void Start()
        {
            isStarted = true;
            isCompleted = false;
            completedCount = 0;
            foreach (var animation in animations)
            {
                animation.Start();
            }
            if (!Global.storyboards.Contains(this))
                Global.storyboards.Add(this);
        }

        public Callback OnCompleted;

        public List<Animation> animations = new List<Animation>();

        public void AddAnimation(Animation animation)
        {
            this.Stop();
            animations.Add(animation);
        }

        public void RemoveAnimation(Animation animation)
        {
            this.Stop();
            animations.Remove(animation);
        }

        public void Clear()
        {
            animations.Clear();
        }

        public Storyboard()
        {           
        }

        public Storyboard(Callback OnCompleted)
        {            
            this.OnCompleted += OnCompleted;        
        }

        public void SendCompleted()
        {
            if (this.animations.Count == 7)
            {
                completedCount++;
                completedCount--;
            }
            completedCount++;
            if (completedCount >= this.animations.Count)
            {
                Stop();
            }
        }

        public void Stop()
        {
            if (!isStarted) return;
            isStarted = false;
            isCompleted = true;
            foreach (var animation in animations)
            {                
                animation.Stop();
            }
            if (OnCompleted != null) OnCompleted(this);
        }

        public override void Update(GameTime gameTime)
        {
            foreach (var animation in animations)
            {
                animation.Update(gameTime);
            }
        }
    }
}
