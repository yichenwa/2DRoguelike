// Scenes Must be added to the Build Settings if being manually imported.
// The code references the build settings to find the string parameters passed. 
// So--->File--->Build Settings--->type in "name of scene created"

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class menuctrl : MonoBehaviour {

	public void LoadScene(string sceneName){

        SceneManager.LoadScene(sceneName);
	}
	
	}
