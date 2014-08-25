Node Resizer is intended for modders to use in their parts pack, 
though anyone could use it if they thought they needed it.

Purpose: Allow the use of tiny attachment nodes in the VAB/SPH for easier snapping in situations where 
multiple larger nodes overlap. (which makes it harder to control which one is snapped to)

Usage:
	// A node you’re having trouble attaching because its desired size engulfs nearby nodes. Set to size 0.
	node_stack_top = 0.0, 0.125, 0.0, 0.0, 1.0, 0.0, 0 

	MODULE
	{
		name = ModuleNodeResizer
		// Must have one AttachNode for each node you want to resize
		AttachNode 
		{
			// Name of the node (node_stack_top in this case)
			name = top
			// Desired node size after launch: Recommend 1 / 1.25m diameter
			newsize = 2
		}
	}
