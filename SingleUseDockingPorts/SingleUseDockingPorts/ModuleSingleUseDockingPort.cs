﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace SingleUseDockingPorts
{
    public class ModuleSingleUseDockingPort : ModuleDockingNode
    {
        Part ghostConnectionFrom = null;
        Part ghostConnectionTo = null;

        public override void OnAwake()
        {
            base.OnAwake();
            print("Initializing ModuleSingleUseDockingPort");
            //initialize event handler
            GameEvents.onPartCouple.Add(onPartCoupled);
        }

        public override void OnStart(StartState st)
        {
            base.OnStart(st);
            foreach (Part child in part.children)
            {
                print("hunting for children");
                //look through all the modules on the part
                foreach (PartModule module in child.Modules)
                {
                    //if the module is a single use port
                    if (module is ModuleSingleUseDockingPort)
                    {
                        //cast the module to the proper type
                        ModuleSingleUseDockingPort singlePort = module as ModuleSingleUseDockingPort;
                        //if we have the same node type
                        if (singlePort.nodeType.Equals(this.nodeType))
                        {
                            //set the needed connection parts
                            ghostConnectionFrom = part.parent;
                            ghostConnectionTo = child.children[0];
                        }
                    }
                }
            }
        }

        public override void OnFixedUpdate()
        {
            base.OnFixedUpdate();
            if (ghostConnectionFrom != null && ghostConnectionTo != null)
            {
                FixedJoint joint = ghostConnectionFrom.gameObject.AddComponent<FixedJoint>();
                joint.connectedBody = ghostConnectionTo.gameObject.rigidbody;
                ghostConnectionFrom = null;
                ghostConnectionTo = null;
            }
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
                ghostConnectionFrom = part.parent;
                ghostConnectionTo = data.to.parent;
            }
        }
    }
}
