using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace SingleUseDockingPorts
{
    public class ModuleSingleUseDockingPort : ModuleDockingNode
    {

        public override void OnAwake()
        {
            print("Initializing ModuleSingleUseDockingPort");
            //initialize event handler
            GameEvents.onPartCouple.Add(onPartCoupled);
        }

        public void OnDestroy()
        {
            //deregister event handlers
            GameEvents.onPartCouple.Remove(onPartCoupled);
        }

        public new void Undock()
        {
            ScreenMessages.PostScreenMessage("This port is single-use only and cannot be undocked", 2f, ScreenMessageStyle.UPPER_CENTER);
        }

        public new void Decouple()
        {
            ScreenMessages.PostScreenMessage("This port is single-use only and cannot be decoupled.", 2f, ScreenMessageStyle.UPPER_CENTER);
        }

        private void onPartCoupled(GameEvents.FromToAction<Part, Part> data)
        {
            if (data.from == part)
            {
                //create a new joint between to the two port's parents supplement the existing one?
                part.parent.gameObject.AddComponent<FixedJoint>();
                FixedJoint joint = part.parent.gameObject.GetComponent<FixedJoint>();
                joint.connectedBody = data.to.parent.gameObject.rigidbody;
            }
        }
    }
}
