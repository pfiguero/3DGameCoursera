using UnityEngine;
using System.Collections;

public class BallController : MonoBehaviour {

	public delegate void FondoOlla();
	public static event FondoOlla OnFondoOlla;

	public delegate void BolaDestruida();
	public static event BolaDestruida OnBolaDestruida;

	public GameObject generadorRampa;

	private Rigidbody rgb;

	// Use this for initialization
	void Start () {
		rgb = GetComponent<Rigidbody> ();
		generadorRampa = GameObject.Find ("GeneradorRampa");
	}

	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider other) {
		if (other.gameObject.name.Contains ("FondoOlla")) {
			gameObject.transform.position = generadorRampa.transform.position;
			ResetRigidbody ();
			if (OnFondoOlla != null)
				OnFondoOlla ();
		}
		if (other.gameObject.name.Contains ("phonograph")) {
			Destroy (gameObject);
			if (OnBolaDestruida != null)
				OnBolaDestruida ();
		}
	}

	void ResetRigidbody() {
		rgb.velocity = Vector3.zero;
		rgb.angularVelocity = Vector3.zero;
		rgb.drag = 0f;
		rgb.inertiaTensorRotation = Quaternion.identity;
		rgb.ResetInertiaTensor ();
	}

}
