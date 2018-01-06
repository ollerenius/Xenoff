using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTargetetMarker : MonoBehaviour {

	Enemy enemy = null;

	void Start () {
		enemy = GetComponentInParent<Enemy>();
		CombatController.instance.notifyCombatStateChangeObservers += OnCombatStateChange;
		CombatController.instance.notifyOnTargetChangeListeners += OnTargetChange;
		gameObject.SetActive(false);
	}

	/// <summary>
	/// Listener method that is called whenever the current CombatState is changed in CombatController
	/// </summary>
	void OnCombatStateChange(CombatState state) {
		print("Change combat state to " + state);
		switch(state) {
			case CombatState.COMBAT:
				break;
			case CombatState.DEFAULT:
				break;
			case CombatState.ENGAGED:
				break;
			default:
				break;
		}
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
