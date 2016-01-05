using UnityEngine;
using System.Collections;

public class ObjectGenerator : MonoBehaviour {
	public float secs=10.0f;
	public GameObject obj;

	// Use this for initialization
	void Start () {
		InvokeRepeating ("GenerarObj", secs, secs);
	}
	
	// Update is called once per frame
	void GenerarObj () {
		Instantiate (obj);
		obj.transform.position = transform.position;
	}
}
