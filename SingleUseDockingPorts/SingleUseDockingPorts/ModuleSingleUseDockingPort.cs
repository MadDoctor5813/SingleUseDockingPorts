using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace SingleUseDockingPorts
{
    public class ModuleClampableDockingPort : ModuleDockingNode
    {
        bool clamped = false;


        Part ghostConnectionFrom = null;
        Part ghostConnectionTo = null;

        FixedJoint clampJoint;

        public override void OnAwake()
        {
            base.OnAwake();
        }

        [KSPEvent(guiName = "Clamp", externalToEVAOnly = true)]
        public void clamp()
        {
            if (this.isDockedToPort() != null)
            {
                print("This port is docked to " + this.isDockedToPort().name);
            }
            if (this.isCoupledToPort() != null)
            {
                print("This port is coupled to " + this.isCoupledToPort().name);
            }
        }

        public override void OnFixedUpdate()
        {
            base.OnFixedUpdate();
            if (ghostConnectionFrom != null && ghostConnectionTo != null)
            {
                print("Making connection between " + ghostConnectionFrom.name + " and " + ghostConnectionTo.name);
                clampJoint = ghostConnectionFrom.gameObject.AddComponent<FixedJoint>();
                clampJoint.connectedBody = ghostConnectionTo.gameObject.rigidbody;
                ghostConnectionFrom = null;
                ghostConnectionTo = null;
            }
        }

        //checks if this port is coupled to a port, i.e, it's had a port attached in the VAB. Returns the docked part or null if there isn't one.
        private Part isCoupledToPort()
        {
            List<ModuleClampableDockingPort> ports = this.vessel.FindPartModulesImplementing<ModuleClampableDockingPort>();
            foreach (ModuleClampableDockingPort port in ports)
            {
                //if we are this part's parent, or vice versa
                if (this.part.parent == port.part || port.part.parent == this.part)
                {
                    return port.part;
                }
            }
            return null;
        }

        //checks if this port has been docked to a port in the usual way. Returns the docked part or null if there isn't one.
        private Part isDockedToPort()
        {
            return vessel[dockedPartUId];
        }

        public new void Undock()
        {
            if (clamped)
            {
                ScreenMessages.PostScreenMessage("This port is clamped and cannot be undocked", 2f, ScreenMessageStyle.UPPER_CENTER);
            }
            else
            {
                base.Undock();
            }
        }

        public new void Decouple()
        {
            if (clamped)
            {
                ScreenMessages.PostScreenMessage("This port is clamped and cannot be decoupled.", 2f, ScreenMessageStyle.UPPER_CENTER);
            }
            else
            {
                base.Decouple();
            }
        }
    }
}
