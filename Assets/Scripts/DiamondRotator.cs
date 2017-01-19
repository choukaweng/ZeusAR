using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiamondRotator : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
       //transform.rotation = transform.rotation * Quaternion.Euler(0, 0, 100 * Time.deltaTime);
        if(gameObject.CompareTag("Heart") || gameObject.CompareTag("Energy"))
        {
            transform.localRotation = transform.rotation * Quaternion.Euler(0, 100 * Time.deltaTime, 0);
        }
        else
        {
            transform.localRotation = transform.rotation * Quaternion.Euler(0, 0, 100 * Time.deltaTime);
        }
    }
}
