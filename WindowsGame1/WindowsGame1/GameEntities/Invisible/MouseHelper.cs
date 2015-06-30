using InvasionWar.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InvasionWar.GameEntities.Invisible
{
    public class MouseHelper : InvisibleGameEntity, IObservedSubject
    {
        private List<IObserver> observers = new List<IObserver>();
        private MouseState CurrentState;
        private MouseState PreviousState;

        public Vector2 GetCurrentMousePosition()
        {
            return new Vector2(CurrentState.X, CurrentState.Y);
        }

        public Vector2 GetCurrentWorldMousePoistion()
        {
            return Vector2.Transform(GetCurrentMousePosition(), Global.gMainCamera.InvWVP);
        }

        public Vector2 GetMousePositionDifference()
        {
            return new Vector2(CurrentState.X - PreviousState.X,
                CurrentState.Y - PreviousState.Y);
        }

        public bool IsLeftButtonDown()
        {
            return CurrentState.LeftButton == ButtonState.Pressed;
        }

        public bool IsLeftButtonUp()
        {
            return CurrentState.LeftButton == ButtonState.Released;
        }

        public bool IsLeftButtonPressed()
        {
            return (CurrentState.LeftButton == ButtonState.Pressed)
                && (PreviousState.LeftButton == ButtonState.Released);
        }

        public bool IsLeftButtonReleased()
        {
            return (CurrentState.LeftButton == ButtonState.Released)
                && (PreviousState.LeftButton == ButtonState.Pressed);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            PreviousState = CurrentState;
            CurrentState = Mouse.GetState();
            NotifyAll();
        }

        public bool IsRightButtonReleased()
        {
            return (CurrentState.RightButton == ButtonState.Released)
                && (PreviousState.RightButton == ButtonState.Pressed);
        }

        
        public void Register(IObserver observer)
        {
            if (!observers.Contains(observer))
                observers.Add(observer);
        }

        public void Unregister(IObserver observer)
        {
            if (observers.Contains(observer))
                observers.Remove(observer);
        }

        public void NotifyAll()
        {
            Vector2 MousePos = Global.gMouseHelper.GetCurrentMousePosition();          
            Vector2 worldPositionOfMouse;
            worldPositionOfMouse = Vector2.Transform(MousePos, Global.gMainCamera.InvWVP);             
            foreach (IObserver observer in observers)
            {
                if (observer.IsAvailable())
                {
                    if (observer.InMousePosition(worldPositionOfMouse))
                    {
                        if (IsLeftButtonUp()) observer.SendMouseMove();
                        if (IsLeftButtonDown()) observer.SendMouseDown();
                        if (IsLeftButtonReleased()) observer.SendMouseUp();
                        if (IsLeftButtonPressed()) observer.SendMouseClick();
                    }
                    else
                    {
                        observer.SendMouseLeave();
                    }
                }
            }
        }
    }
}
