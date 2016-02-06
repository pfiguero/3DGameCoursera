using UnityEngine;
using System.Collections;

public class BallController : MonoBehaviour {

	public delegate void FondoOlla();
	public static event FondoOlla OnFondoOlla;

	public delegate void BolaDestruida();
	public static event BolaDestruida OnBolaDestruida;

	public GameObject generadorRampa;

	public float tDespuesOlla = 5f;

	private Rigidbody rgb;
	private SphereCollider col;
	private Material m;
	private bool floatingToGeneradorRampa = false;
	private float tIni;
	Vector3 pIni;

	// Use this for initialization
	void Start () {
		rgb = GetComponent<Rigidbody> ();
		col = GetComponent<SphereCollider> ();
		m = GetComponent<Renderer>().material;
		generadorRampa = GameObject.Find ("GeneradorRampa");
	}

	// Update is called once per frame
	void Update () {
		if (floatingToGeneradorRampa) {
			float t = (Time.time - tIni) / tDespuesOlla;
			if (t < 1f) {
				Vector3 p = Vector3.Lerp (pIni, generadorRampa.transform.position, t);
				transform.position = p;
			} else {
				floatingToGeneradorRampa = false;
				MakeGhost ( false );
				ResetRigidbody ();
			}
		}
	}

	void OnTriggerEnter(Collider other) {
		if (other.gameObject.name.Contains ("FondoOlla") && !floatingToGeneradorRampa) {
			//ResetRigidbody ();
			MakeGhost ( true );
			floatingToGeneradorRampa = true;
			tIni = Time.time;
			pIni = transform.position;
			//gameObject.transform.position = generadorRampa.transform.position;
			if (OnFondoOlla != null)
				OnFondoOlla ();
		}
		if (other.gameObject.name.Contains ("phonograph")) {
			if (OnBolaDestruida != null)
				OnBolaDestruida ();
			Destroy (gameObject);
		}
	}

	void ResetRigidbody() {
		rgb.velocity = Vector3.zero;
		rgb.angularVelocity = Vector3.zero;
		rgb.drag = 0f;
		rgb.inertiaTensorRotation = Quaternion.identity;
		rgb.ResetInertiaTensor ();
	}

	void MakeGhost( bool state ) {
		if (state) {
			rgb.isKinematic = true;
			col.enabled = false;
			Color c = m.color;
			c.a = 30 / 256f;
			m.color = c;
		} else {
			rgb.isKinematic = false;
			col.enabled = true;
			Color c = m.color;
			c.a = 1f;
			m.color = c;
		}
	}
}
