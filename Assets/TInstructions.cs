using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class TInstructions : MonoBehaviour {
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey(KeyCode.Return))
            SceneManager.LoadScene("TTova");
	}
}
