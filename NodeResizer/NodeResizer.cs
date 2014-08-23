using KSP;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace NodeResizer
{
    public class ResizableNode
    {
        public string name; // Name of attachment node. e.g. top, bottom, etc.
        public int newsize = 1;

        public ResizableNode(string name,int newsize)
        {
            this.name = name;
            this.newsize = newsize;
        }
    }

    public class ModuleNodeResizer : PartModule
    {
        //public List<string> attachNodeNames;
        public List<ResizableNode> attachNodes;

        // Next two lines are only for debugging.
        //[KSPField(isPersistant = false, guiActive = true, guiName = "New Joint Count", guiUnits = "", guiFormat = "F2")]
        //public string newJointCount;

        public void Start()
        {
            GameEvents.onVesselGoOffRails.Add(OnVesselOffRails);
        }

        private void OnVesselOffRails(Vessel v)
        {
            // try/catch because wow, OnVesselOffRails(v) can fire with bad v.
			// And it will also fire multiple times for the same off rails event. (only one valid)
            try
            {
#if DEBUG
                print("*NodeResizer* " + this.vessel.name + " went off-rails.");
#endif
            }
            catch (Exception e)
            {
                // Don't care if we log this. Shouldn't be getting NullRef on this anyway.
            }
        }

        public override void OnAwake()
        {
            base.OnAwake();
            attachNodes = new List<ResizableNode>();
        }


        public override void OnLoad(ConfigNode node)
        {
            base.OnLoad(node);
            foreach (ConfigNode n in node.GetNodes("AttachNode"))
            {
                if (n.HasValue("name") && n.HasValue("newsize"))
                {
                    string name;
                    int newsize;
                    int.TryParse(n.GetValue("newsize"), out newsize);
                    name = n.GetValue("name");
                    print("NodeResizer.OnLoad(" + name + ", " + newsize + ")");

                    attachNodes.Add(new ResizableNode(name, newsize));
                }
            }

        }
        public override void OnStart(PartModule.StartState state)
        {
            base.OnStart(state);

            print("*NodeResizer* OnStart()");
            UpdateNodes();
        }
        private void UpdateNodes()
        {
            if (HighLogic.LoadedSceneIsFlight)
            {
                print("*NodeResizer* LoadedSceneIsFlight = true");
                if (attachNodes.Count == 0 && part.partInfo != null)
                {
                    if (part.partInfo.partPrefab.Modules.Contains("ModuleNodeResizer"))
                    {
                        attachNodes = ((ModuleNodeResizer)part.partInfo.partPrefab.Modules["ModuleNodeResizer"]).attachNodes;
                    }
                }

                foreach (ResizableNode resizableNode in attachNodes)
                {
                    try
                    {
                        this.part.findAttachNode(resizableNode.name).size = resizableNode.newsize;
                    }
                    catch (Exception e)
                    {
                        print("*NodeResizer* caught exception " + e.ToString() + " while resizing node " + resizableNode.name);
                    }
                }
                //this.part.ResetJoints(); // Not necessary. KSP automagically resets them when they are resized.
            }
        }
    }
}
