using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Core.Services.Updater
{
    public interface IProjectUpdater
    {
        event Action UpdateCalled;
        event Action FixedUpdateCalled;
        event Action LateUpdateCalled;
    }
}
