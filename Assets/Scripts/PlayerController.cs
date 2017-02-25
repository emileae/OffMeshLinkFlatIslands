using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour {

	private CharacterController controller;
	public float gravity = -30.0f;
	private float gravityModifier = 1;
	public float speed = 3.0f;

	public bool onLadder = false;

	// temporary
	// TODO: implement blackboard NPC calling
	public NMAgent ai;

	// Use this for initialization
	void Start () {
		controller = GetComponent<CharacterController>();
	}
	
	// Update is called once per frame
	void Update ()
	{

		float inputH = Input.GetAxisRaw ("Horizontal");
		float inputV = Input.GetAxisRaw ("Vertical");

		if (!onLadder) {
			inputV = 0.0f;
			gravityModifier = 1;
		} else {
			gravityModifier = 0;
		}

		Vector3 direction = new Vector3 (inputH, inputV, 0) + Vector3.up * gravity * gravityModifier * Time.deltaTime;

		controller.Move (direction * speed * Time.deltaTime);


		if (Input.GetButtonDown ("Fire3")) {
			Debug.Log("Call AIssss");
			NavMeshHit hit;
			Debug.Log(NavMesh.SamplePosition(transform.position, out hit, 10.0f, NavMesh.AllAreas));
			if (NavMesh.SamplePosition(transform.position, out hit, 10.0f, NavMesh.AllAreas)) {
				Vector3 result = hit.position;
				ai.targetPosition = result;
				Debug.Log("Result: " + result);
				ai.goToTarget = true;
			}
		}

	}
}
