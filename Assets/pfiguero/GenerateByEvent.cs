using UnityEngine;
using System.Collections;

public class GenerateByEvent : MonoBehaviour {

	public GameObject obj;

	void OnEnable()
	{
		DieByContact.OnDying += GenerarObj;
	}


	void OnDisable()
	{
		DieByContact.OnDying -= GenerarObj;
	}

	void GenerarObj () {
		Instantiate (obj);
		obj.transform.position = transform.position;
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
