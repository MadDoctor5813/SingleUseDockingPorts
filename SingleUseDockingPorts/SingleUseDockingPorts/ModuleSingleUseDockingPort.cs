using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace SingleUseDockingPorts
{
    public class ModuleClampableDockingPort : ModuleDockingNode
    {
        public bool clamped = false;

        public override void OnAwake()
        {
            base.OnAwake();
        }

        [KSPEvent(guiName = "Clamp", guiActiveUnfocused = true, externalToEVAOnly = true, guiActive = false, unfocusedRange = 4)]
        public void clamp()
        {
            if (!clamped)
            {
                Part dockedPort = getDockedPort();
                if (dockedPort != null)
                {
                    ModuleClampableDockingPort clampableModule = getDockedPort().Modules.GetModules<ModuleClampableDockingPort>()[0];
                    //create the joint between the two parts on the non-docking end of both ports   
                    FixedJoint joint = part.findAttachNode("bottom").attachedPart.gameObject.AddComponent<FixedJoint>();
                    joint.connectedBody = dockedPort.findAttachNode("bottom").attachedPart.rigidbody;
                    clamped = true;
                    clampableModule.clamped = true;
                    this.disableEventsAfterClamp();
                    clampableModule.disableEventsAfterClamp();
                    ScreenMessages.PostScreenMessage("Port clamped.", 2f, ScreenMessageStyle.UPPER_CENTER);
                }
            }
            else
            {
                ScreenMessages.PostScreenMessage("Already clamped. You should never see this.", 2f, ScreenMessageStyle.UPPER_CENTER);
            }

        }

        public void disableEventsAfterClamp()
        {
            Events["clamp"].active = false;
            Events["Decouple"].active = false;
            Events["undock"].active = false;
            Events["undocksamevessel"].active = false;
        }

        private Part getDockedPort()
        {
            Part dockedPart = part.findAttachNode("top").attachedPart;
            foreach (PartModule module in dockedPart.Modules)
            {
                if ((module as ModuleClampableDockingPort).nodeType == this.nodeType)
                {
                    return dockedPart;
                }
            }
            return null;
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
