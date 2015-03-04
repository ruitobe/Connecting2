using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DrawLine : MonoBehaviour
{
	public List<Transform> waypoints = new List<Transform>();
	public float rate = 1;

	public void MoveToWaypoint() {			

		StartCoroutine ("move");
	}

	public void ClearPath() {

		waypoints.Clear();
	}

	IEnumerator move() {

		for (int i = 0; i < waypoints.Count; i++) {

			iTween.MoveTo(this.gameObject, waypoints[i].position, rate);
			yield return new WaitForSeconds(rate);
		}
		waypoints.Clear();
	}
}
