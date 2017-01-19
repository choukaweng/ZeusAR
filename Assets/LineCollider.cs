using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineCollider : MonoBehaviour {

    private GameObject lightning_object;
    private LineRenderer line;
    private Vector3 startPos;
    private Vector3 endPos;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void addColliderToLine()
    {
        Debug.Log("Ping!");
        BoxCollider boxcol = new GameObject(this.tag).AddComponent<BoxCollider>();
        //boxcol.transform.parent = this.transform;
        //startPos = 
        //Debug.Log("StartPos: " + startPos);
    }
}
