using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsGame1
{
    public class KeyboardHelper : InvisibleGameEntity
    {
        private KeyboardState CurrentState;
        private KeyboardState PreviousState;
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            PreviousState = CurrentState;
            CurrentState = Keyboard.GetState();
        }

        public bool IsKeyDown(Keys key)
        {
            return CurrentState.IsKeyDown(key);
        }

        public bool IsKeyUp(Keys key)
        {
            return CurrentState.IsKeyUp(key);
        }

        public bool IsKeyPressed(Keys key)
        {
            return (CurrentState.IsKeyDown(key)) 
                && (PreviousState.IsKeyUp(key));
        }

        public bool IsKeyReleased(Keys key)
        {
            return (CurrentState.IsKeyUp(key))
                && (PreviousState.IsKeyDown(key));
        }

    }
}
