using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InvasionWar.Interfaces
{
    
    public interface IObserver
    {
        void SendMouseMove();
        void SendMouseClick();
        void SendMouseDown();
        void SendMouseUp();
        void SendMouseLeave();

        bool InMousePosition(Vector2 MousePos);
        bool IsAvailable();
    }
}
