using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Commander : MonoBehaviour {
	
	public List<CommandObject> commands;
	
	public CommandObject currentCommand;
	
	bool isSet = false;
	
	void Update () {
		
		// check if we have commands
		if(commands.Count > 0) {
			
			// set if not set
			if(!isSet) {
				commands[0].setTransform(transform);
				isSet = true;
			}
			
			// see if its still working
			if(!commands[0].doingWork()) {
				// dump and reset
				commands.RemoveAt(0);
				isSet = false;
			}
		}
	}
}