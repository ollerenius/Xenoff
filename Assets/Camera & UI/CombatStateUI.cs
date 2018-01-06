using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Script used for writing the current combat state to the screen, for debugging purposes.
/// Attatch it to a text field on a canvas.
/// </summary>
public class CombatStateUI : MonoBehaviour {

	Text combatStateText = null;
	
	void Start () {
		combatStateText = GetComponent<Text>();
		combatStateText.text = CombatState.DEFAULT.ToString();
		CombatController.instance.notifyCombatStateChangeObservers += OnCombatStateChange;
	}

	void OnCombatStateChange(CombatState state) {
		combatStateText.text = state.ToString();
	}
}
