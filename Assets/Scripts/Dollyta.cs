using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Dollyta : MonoBehaviour {

	// Use this for initialization
	private NavMeshAgent agent;
	public GameObject cualquiera;
	void Start () {
		agent = GetComponent<NavMeshAgent> ();
	}
	
	// Update is called once per frame
	void Update () {
		
		agent.SetDestination (cualquiera.transform.position);

	}
}
