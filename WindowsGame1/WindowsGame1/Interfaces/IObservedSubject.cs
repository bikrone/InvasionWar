using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InvasionWar.Interfaces
{
    public interface IObservedSubject
    {   
        void Register(IObserver observer);
        void Unregister(IObserver observer);
        void NotifyAll();
    }
}
