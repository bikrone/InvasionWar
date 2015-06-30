using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using InvasionWar.GameEntities.Visible;
using InvasionWar.GameEntities.Invisible;
using InvasionWar.Effects;

namespace InvasionWar
{
    class Global
    {
        public static MouseHelper gMouseHelper = new MouseHelper();
        public static KeyboardHelper gKeyboardHelper = new KeyboardHelper();
        public static Camera gMainCamera = new Camera();
        public static ContentManager Content;

        public static List<Storyboard> storyboards = new List<Storyboard>();

        public static Game1 thisGame;

        internal static void UpdateAll(Microsoft.Xna.Framework.GameTime gameTime)
        {
            gMouseHelper.Update(gameTime);
            gKeyboardHelper.Update(gameTime);
            gMainCamera.Update(gameTime);
            List<Storyboard> completed = new List<Storyboard>();
            foreach (var storyboard in storyboards)
            {
                if (storyboard.isStarted) storyboard.Update(gameTime);
                if (storyboard.isCompleted) completed.Add(storyboard);
            }
            foreach (var storyboard in completed)
            {
                storyboards.Remove(storyboard);
            }
        }
    }
}
