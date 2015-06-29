using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using InvasionWar.GameEntities.Visible;
using InvasionWar.GameEntities.Invisible;

namespace InvasionWar
{
    class Global
    {
        public static MouseHelper gMouseHelper = new MouseHelper();
        public static KeyboardHelper gKeyboardHelper = new KeyboardHelper();
        public static Camera gMainCamera = new Camera();
        public static ContentManager Content;

        public static Game1 thisGame;

        internal static void UpdateAll(Microsoft.Xna.Framework.GameTime gameTime)
        {
            gMouseHelper.Update(gameTime);
            gKeyboardHelper.Update(gameTime);
            gMainCamera.Update(gameTime);
        }
    }
}
