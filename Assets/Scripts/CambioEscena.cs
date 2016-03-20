using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class CambioEscena : MonoBehaviour {

	public string nombreEscena;

	public delegate void CerrarEscena();
	public static event CerrarEscena OnCerrarEscena;

	// Use this for initialization
	void Start () {
		if (nombreEscena == null) {
			Debug.Log ("CambioEscena. No se ha asignado una escena destino. Suponiendo pfiguero");
			nombreEscena = "room-pfiguero";
		}
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
		if (OnCerrarEscena != null ) {
			OnCerrarEscena ();
		}
		SceneManager.LoadScene (nombreEscena);
	}

}
