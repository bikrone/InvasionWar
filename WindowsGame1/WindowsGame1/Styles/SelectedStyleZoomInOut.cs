using InvasionWar.Effects;
using InvasionWar.Effects.Animations;
using InvasionWar.GameEntities.Visible;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InvasionWar.Styles
{
    public class SelectedStyleZoomInOut
    {      
        public static void Assign(Sprite2D btn)
        {
            if (btn.states.Count > 0)
            {
                btn.ChangeState(0);
                return;
            }
            Storyboard sb = new Storyboard();
            btn.ClearState();
            sb.Clear();
            var toScale = new Vector2(1.07f, 1.07f);
            var fromScale = Vector2.Divide(Vector2.One, toScale);
            sb.AddAnimation(new ScaleAnimation(sb, btn, GameSettings.GemSelectedEffectDuration, toScale, true, fromScale, true, true, true));
            btn.AddNewState(sb);
            btn.ChangeState(0);
        }

        public static void Unassign(Sprite2D btn)
        {
            btn.ChangeState(-1);
        }
    }
}
