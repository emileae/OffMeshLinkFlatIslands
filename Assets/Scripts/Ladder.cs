using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class Ladder : MonoBehaviour {

	private OffMeshLink oml;
	private Bounds bounds;

	public Transform ladderBottom;
	public Transform ladderTop;

	public GameObject npc;
	private Bounds npcBounds;

	// Use this for initialization
	void Start () {
		oml = GetComponent<OffMeshLink>();
		bounds = GetComponent<BoxCollider>().bounds;

		// TODO: move some of the measurement stuff out of individual scripts into blackboard... like NPC bounds
		npcBounds = npc.GetComponent<CapsuleCollider>().bounds;

		ladderBottom.position = new Vector3(bounds.center.x, bounds.min.y, bounds.center.z);
		ladderTop.position = new Vector3(bounds.center.x, bounds.max.y - npcBounds.extents.y, bounds.center.z);

		oml.startTransform = ladderBottom;
		oml.endTransform = ladderTop;

	}

	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter (Collider col)
	{
		GameObject go = col.gameObject;

		Debug.Log("Entered trigger");

		if (go.tag == "Player") {
			PlayerController playerScript = go.GetComponent<PlayerController>();
			playerScript.onLadder = true;
		}
	}

	void OnTriggerExit (Collider col)
	{
		GameObject go = col.gameObject;

		if (go.tag == "Player") {
			PlayerController playerScript = go.GetComponent<PlayerController>();
			playerScript.onLadder = false;
		}
	}


}
