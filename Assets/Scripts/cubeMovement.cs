using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class cubeMovement : MonoBehaviour {

   
    NavMeshAgent nav;
    public GameObject target;


	// Use this for initialization
	void Start () {
		
	}
	
    void Awake()
    {
        
        
        nav = GetComponent<NavMeshAgent>();
    }

	// Update is called once per frame
	void Update ()
    {
		if(target != null)
        {
            nav.SetDestination(target.transform.position);
        }
	}
}
