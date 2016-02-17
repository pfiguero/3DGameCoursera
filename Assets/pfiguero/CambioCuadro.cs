using UnityEngine;
using System.Collections;

public class CambioCuadro : MonoBehaviour {

	public GameObject cuadro;
	public float delta = 3f;

	private Material m = null;
	private bool entrando = false;
	private bool saliendo = false;
	private float tIni;
	private float vIni;
	private float vFin;


	// Use this for initialization
	void Start () {
		if (cuadro != null) {
			m = cuadro.GetComponent<Renderer>().material;
			m.SetFloat ("_slideTex", 0);
		}
	}

	void Update() {
		if (entrando) {
			float val = Mathf.Lerp (vIni, vFin, (Time.time - tIni) / delta);
			m.SetFloat ("_slideTex", val);
			if( val >= 1 )
				entrando = false;
		}
		if (saliendo) {
			float val = Mathf.Lerp (vIni, vFin, (Time.time - tIni) / delta);
			m.SetFloat ("_slideTex", val);
			if( val <= 0 )
				saliendo = false;
		}
	}

	void OnTriggerEnter(Collider other) {
		if (other.gameObject.name.Equals ("TriggerCuadro")) {
			entrando = true;
			tIni = Time.time;
			vIni = 0;
			vFin = 1;
		}
	}

	void OnTriggerExit(Collider other) {
		if (other.gameObject.name.Equals ("TriggerCuadro")) {
			saliendo = true;
			tIni = Time.time;
			vIni = 1;
			vFin = 0;
		}
	}

}
