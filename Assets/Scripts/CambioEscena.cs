using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class CambioEscena : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter() {
		Invoke ("Cambiar", 2);
	}

	void OnTriggerExit() {
		CancelInvoke ();
	}

	void Cambiar () {
		SceneManager.LoadScene ("room-pfiguero");
	}
}
