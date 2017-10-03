using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class skullname : MonoBehaviour {

	bool isShowTip;  
	public bool WindowShow = false;  
	void Start()  
	{  
		isShowTip = false;  
	}  
	void OnMouseEnter()  
	{  
		isShowTip = true;  

	}  
	void OnMouseExit()  
	{  
		isShowTip = false;  
	}  
	void OnGUI()  
	{  
		if (isShowTip)  
		{  
			GUI.Window(0, new Rect(30, 30, 200, 100), MyWindow, "Jumping Skull"); 

		}  

	}  


	void MyWindow(int WindowID)  
	{  
		GUILayout.Label("This is Jumping Skull, it will not attack you, but it will in the way, go across them.");  
	}  

}  
