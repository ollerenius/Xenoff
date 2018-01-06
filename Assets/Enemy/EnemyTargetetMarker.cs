using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTargetetMarker : MonoBehaviour {

	Enemy enemy = null;

	void Start () {
		enemy = GetComponentInParent<Enemy>();
		CombatController.instance.notifyOnTargetChangeListeners += OnTargetChange;
		gameObject.SetActive(false);
	}

	void OnTargetChange(Enemy newTarget) {
		if (gameObject != null) {
			if (newTarget == enemy) {
				gameObject.SetActive(true);
			} else {
				gameObject.SetActive(false);
			}
		}
	}
}
