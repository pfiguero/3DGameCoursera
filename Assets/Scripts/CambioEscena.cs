using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class CambioEscena : MonoBehaviour {

	private GameManager gameManager;
	private AudioSource sonidoCasaPfiguero1;

	// Use this for initialization
	void Start () {
		GameObject g = GameObject.Find ("GameManager");
		if (g != null)
			gameManager = g.GetComponent<GameManager> ();
		g = GameObject.Find ("casaPfiguero1");
		if (g != null)
			sonidoCasaPfiguero1 = g.GetComponent<AudioSource> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider other) {
		if( other.gameObject.name.Equals("carpetTrigger") )
			Invoke ("Cambiar", 2);
	}

	void OnTriggerExit(Collider other) {
		if( other.gameObject.name.Equals("carpetTrigger") )
			CancelInvoke ();
	}

	void Cambiar () {
		gameManager.offsetSong = sonidoCasaPfiguero1.time;
		SceneManager.LoadScene ("room-pfiguero");
	}

}
