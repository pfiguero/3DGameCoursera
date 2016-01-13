using UnityEngine;
using System.Collections;

public class MoveToClosest : MonoBehaviour {
	GameObject p1;
	GameObject p2;
	GameObject pClose;
	public float tiempoSubir=5f;
	public float tiempoRotar=3f;
	private float startTime;
	private Vector3 posIni;
	private bool startAnim;
	private Rigidbody rgb;
	private float fracComplete;

	// Use this for initialization
	void Start () {
		rgb = GetComponent<Rigidbody> ();
		// Hacer los enlaces a los objetos. Debería hacerse solo una vez...
		p1 = GameObject.Find("BenchSpecial/p1");
		p2 = GameObject.Find("BenchSpecial/p2");
		if (Vector3.Distance (transform.position, p1.transform.position) < Vector3.Distance (transform.position, p2.transform.position)) {
			pClose = p1;
		} else {
			pClose = p2;
		}
		startAnim = false;
	}

	void OnTriggerEnter(Collider other) {
		startTime = Time.time;
		posIni = transform.position;
		startAnim = true;
	}

	void FixedUpdate() {
		if (fracComplete >= 1.0f) {
			ResetRigidbody ();
			enabled = false;
		}
	}

	void ResetRigidbody() {
		rgb.velocity = Vector3.zero;
		rgb.angularVelocity = Vector3.zero;
		rgb.drag = 0f;
		rgb.inertiaTensorRotation = Quaternion.identity;
		rgb.ResetInertiaTensor ();
	}

	// Update is called once per frame
	void Update () {
		if (startAnim) {
			fracComplete = (Time.time - startTime) / tiempoSubir;
			transform.position = Vector3.Slerp (posIni, pClose.transform.position, fracComplete);
		}
	}
}
