using UnityEngine;
using System.Collections;
using InControl;

public class menuAdvance : MonoBehaviour {
	public bool check = false;
	public string nextScene = "";
	//public InputManager man;

	// Use this for initialization
	void Start () {
		check = false;
	}
	
	// Update is called once per frame
	void Update () {
		for (int i = 0;i < InputManager.Devices.Count && !check;++i){
			if(InputManager.Devices[i].AnyButton){
				print("Fail");
				return;
			}
		}
		check = true;
		for (int i = 0;i < InputManager.Devices.Count && check;++i){
			if(InputManager.Devices[i].AnyButton){
				print("Sucess");
				Application.LoadLevel(nextScene);
			}
		}
	}
}
