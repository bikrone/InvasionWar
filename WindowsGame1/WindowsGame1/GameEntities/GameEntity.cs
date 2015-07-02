using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InvasionWar.GameEntities
{
    public class GameEntity
    {
        private List<GameEntity> children;
        public List<GameEntity> Children
        {
            get
            {             
                return children;
            }
            set
            {
                children = value;
            }
        }

        public GameEntity Parent { get; set; }
        
        public virtual void Update(GameTime gameTime)
        {
            if (Children != null)
            {
                while (true)
                {
                    try
                    {
                        foreach (var child in children)
                        {
                            child.Update(gameTime);
                        }
                        break;
                    }
                    catch
                    {
                        continue;
                    }
                }
            }
        }

        public void ClearChildren()
        {
            if (Children == null) return;
            foreach (var child in Children)
            {
                child.Parent = null;
            }
            Children.Clear();
        }

        public virtual void Draw(GameTime gameTime, object param)
        {
            if (Children != null)
            {
                while (true)
                {
                    try
                    {
                        foreach (var child in children)
                        {
                            child.Draw(gameTime, param);
                        }
                        break;
                    }
                    catch
                    {
                        continue;
                    }
                }
            }
        }

        public virtual void AddChild(GameEntity entity)
        {
            entity.Parent = this;
            if (Children == null) Children = new List<GameEntity>();
            Children.Add(entity);
        }

        public virtual void RemoveChild(GameEntity entity)
        {
            entity.Parent = null;
            if (Children == null || (!Children.Contains(entity))) return;
            Children.Remove(entity);
        }
    }
}
