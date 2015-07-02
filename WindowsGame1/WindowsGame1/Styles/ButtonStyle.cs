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
    public class ButtonStyle
    {
        public static void btn_OnMouseMove(object sender)
        {
            var sendr = (Sprite2D)sender;
            if (sendr == null) return;

            sendr.ChangeState(1);
        }

        public static void btn_OnMouseLeave(object sender)
        {
            var sendr = (Sprite2D)sender;
            if (sendr == null) return;
            if (sendr.states[1].animations.Contains(sendr.states[2].animations[0]))
            {
                sendr.states[1].RemoveAnimation(sendr.states[2].animations[0]);
            }
            sendr.ChangeState(0);
        }

        public static void btn_OnMouseDown(object sender)
        {
            var sendr = (Sprite2D)sender;
            if (sendr == null) return;

            if (!sendr.states[1].animations.Contains(sendr.states[2].animations[0]))
            {
                sendr.states[1].AddAnimation(sendr.states[2].animations[0]);
                sendr.states[1].Start();
            }
        }

        public static void btn_OnMouseUp(object sender)
        {
            var sendr = (Sprite2D)sender;
            if (sendr.states[1].animations.Contains(sendr.states[2].animations[0]))
            {
                sendr.states[1].RemoveAnimation(sendr.states[2].animations[0]);
                sendr.states[1].Start();
            }
        }

        public static void Assign(Sprite2D btn)
        {
            Global.gMouseHelper.Register(btn);
            btn.ClearState();
            Storyboard sb = new Storyboard();
            sb.AddAnimation(new ColorAnimation(sb, btn, 0.3f, new Vector4(255, 255, 255, 255), false));

            btn.AddNewState(sb);

            sb = new Storyboard();
            sb.AddAnimation(new ColorAnimation(sb, btn, 0.3f, new Vector4(255, 0, 255, 255), false));

            btn.AddNewState(sb);

            sb = new Storyboard();
            sb.AddAnimation(new ScaleAnimation(sb, btn, 0.1f, new Vector2(0.95f, 0.95f), true, null, false, false, true));

            btn.AddNewState(sb);

            btn.OnMouseMove += btn_OnMouseMove;
            btn.OnMouseLeave += btn_OnMouseLeave;
            btn.OnMouseDown += btn_OnMouseDown;
            btn.OnMouseUp += btn_OnMouseUp;
        }
    }
}
