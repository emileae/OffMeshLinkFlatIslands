using UnityEngine;
using System.Collections;

public class NMAgent2 : MonoBehaviour {

	public bool goToTarget = false;
	public GameObject target = null;
	private NavMeshAgent agent = null;
	private float originalAgentRadius;

	private OffMeshLinkData _currLink;
	public bool traversingLink = false;

	public float timeTaken = 0f;
	public float totalTime = 1f;

	public bool movingDown;
	public bool movingUp;
	public float offMeshLinkTolerance = 1.0f;
	private Vector3 omlHitPosition;

	private Bounds bounds;


	// Use this for initialization
	void Start () {
		agent = GetComponent<NavMeshAgent>();
		originalAgentRadius = agent.radius;

		bounds = GetComponent<CapsuleCollider>().bounds;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (goToTarget) {
			agent.SetDestination (target.transform.position);
		}

		if (agent.isOnOffMeshLink) {
			Debug.Log ("On off mesh link...");

			if (!traversingLink) {
				traversingLink = true;
				_currLink = agent.currentOffMeshLinkData;
				// allow agent to get close to the wall
				agent.radius = 0f;
				omlHitPosition = transform.position;
				if (_currLink.startPos.y > _currLink.endPos.y) {
					movingDown = true;
					movingUp = false;
				}
				if (_currLink.startPos.y > _currLink.endPos.y) {
					movingDown = false;
					movingUp = true;
				}
			}

			float tlerp = 0.01f;

			// move to the correct x coord.
			if (movingUp) {
				transform.position = new Vector3 (_currLink.startPos.x, transform.position.y, transform.position.z);
			} else if (movingDown) {
				transform.position = new Vector3 (_currLink.endPos.x, transform.position.y, transform.position.z);
			}

			Vector3 newYPos = new Vector3 (transform.position.x, Mathf.Lerp (_currLink.startPos.y, _currLink.endPos.y, timeTaken), transform.position.z);
			timeTaken += tlerp;

			transform.position = newYPos;


//			Debug.Log ("newPos: " + newPos);

				if (movingDown) {
					Debug.Log ("Move down");
					if (timeTaken >= totalTime || transform.position.y <= _currLink.endPos.y) {
						transform.position = _currLink.endPos;
						traversingLink = false;
						//Tell unity we have traversed the link
						agent.CompleteOffMeshLink ();
						// restore the original radius
						agent.radius = originalAgentRadius;
						//Resume normal navmesh behaviour
						agent.Resume ();
						timeTaken = 0f;
					}
				} else if (movingUp) {
					Debug.Log ("Move up");
					if (transform.position.y >= _currLink.endPos.y) {
						transform.position = _currLink.endPos;
						traversingLink = false;
						//Tell unity we have traversed the link
						agent.CompleteOffMeshLink ();
						// restore the original radius
						agent.radius = originalAgentRadius;
						//Resume normal navmesh behaviour
						agent.Resume ();
						timeTaken = 0f;
					}
				}
//			}
		}
	}
}
