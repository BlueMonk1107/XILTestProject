using LifeCycle;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hot.Mgr
{
    public class LifeCycleMgr : LifeCycleMgrBase<LifeCycleMgr>
    {
        protected override LifeCycleAddConfigBase GetConfig()
        {
            return new LifeCycleAddConfig();
        }
    }
}
