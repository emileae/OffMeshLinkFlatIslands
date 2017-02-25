using UnityEngine;
using System.Collections;

public class NMAgent : MonoBehaviour {

	public bool goToTarget = false;
	public GameObject target = null;
	public Vector3 targetPosition;
	private NavMeshAgent agent = null;
	private float originalAgentRadius;

	private OffMeshLinkData _currLink;
	public bool traversingLink = false;

	public float timeTaken = 0f;
	public float totalTime = 1f;

	public float offMeshLinkTolerance = 1.0f;
	private Vector3 omlHitPosition;


	// Use this for initialization
	void Start () {
		agent = GetComponent<NavMeshAgent>();
		originalAgentRadius = agent.radius;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (goToTarget) {
//			if (target) {
//				agent.SetDestination (target.transform.position);
//			} else if (targetPosition) {
				agent.SetDestination(targetPosition);
//			}
		}

		if (agent.isOnOffMeshLink) {
			Debug.Log ("On off mesh link...");

			if (!traversingLink) {
				traversingLink = true;

				_currLink = agent.currentOffMeshLinkData;

				// allow agent to get close to the wall
				agent.radius = 0f;

				omlHitPosition = transform.position;

			}

//			Debug.Log("_currLink.startPos.x: " + _currLink.startPos.x);
//			Debug.Log("transform.position.x: " + transform.position.x);
//			Debug.Log("travA: " + (transform.position.x > _currLink.startPos.x - offMeshLinkTolerance));
//			Debug.Log("travB: " + (transform.position.x < _currLink.startPos.x + offMeshLinkTolerance));

//			if (transform.position.x > _currLink.startPos.x - offMeshLinkTolerance && transform.position.x < _currLink.startPos.x + offMeshLinkTolerance) {

			float tlerp = 0.01f;

			var newPos = Vector3.Lerp (_currLink.startPos, _currLink.endPos, timeTaken);

			// First get xPos correct
//			Vector3 newXPos = new Vector3 (Mathf.Lerp (omlHitPosition.x, _currLink.startPos.x, timeTaken), transform.position.y, transform.position.z);

			timeTaken += tlerp;

			transform.position = newPos;

			// Second get yPos correct
//			if (transform.position.x > _currLink.startPos.x - offMeshLinkTolerance && transform.position.x < _currLink.startPos.x + offMeshLinkTolerance) {
//				omlHitPosition = Vector3.zero;
//				timeTaken = 0;
//				Vector3 newYPos = new Vector3 (transform.position.x, Mathf.Lerp (_currLink.startPos.y, _currLink.endPos.y, timeTaken), transform.position.z);
//				timeTaken += tlerp;
//
//				transform.position = newYPos;
//			}

//			Debug.Log ("newPos: " + newPos);

				if (_currLink.startPos.y > _currLink.endPos.y) {
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
				} else if (_currLink.startPos.y < _currLink.endPos.y) {
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
