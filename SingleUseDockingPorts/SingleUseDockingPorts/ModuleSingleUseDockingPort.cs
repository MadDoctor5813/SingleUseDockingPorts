using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SingleUseDockingPorts
{
    public class ModuleSingleUseDockingPort : ModuleDockingNode
    {
        public override void OnAwake()
        {
            print("Initializing ModuleSingleUseDockingPort");
        }

        public new void Undock()
        {
            ScreenMessages.PostScreenMessage("This port is single-use only and cannot be decoupled", 2f, ScreenMessageStyle.UPPER_CENTER);
        }

        public new void Decouple()
        {
            ScreenMessages.PostScreenMessage("This port is single-use only and cannot be decoupled.", 2f, ScreenMessageStyle.UPPER_CENTER);
        }

    }
}
