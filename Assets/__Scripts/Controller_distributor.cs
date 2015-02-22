using UnityEngine;
using System.Collections;
using InControl;

// Manages controllers. Attach this to main camera
public class Controller_distributor : MonoBehaviour{
	public static Controller_distributor S;
	int num_controllers;
	int curControllerNum;

	void Awake(){
		S = this;
		num_controllers = InputManager.Devices.Count;
		curControllerNum = 0;
	}

	void Start(){

	}

	public InputDevice GetController(){
		if (num_controllers > curControllerNum) {
			return InputManager.Devices[curControllerNum++];
		}
		return null;
	}
}
