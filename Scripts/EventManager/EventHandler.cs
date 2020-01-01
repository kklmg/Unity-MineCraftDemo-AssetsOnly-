using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.EventManager
{
    public interface IEventHandler
    {
        bool Handle(IEvent _event);
    }
}
