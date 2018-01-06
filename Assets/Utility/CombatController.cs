using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CombatState {
	DEFAULT, ENGAGED, COMBAT
}

/// <summary>
/// A state controller for the game's combat.
/// It takes messages from the player and enemies and changes the game's state based on that information.
/// </summary>
/// 
/// Convention: the first word in a method declaration declares which instance is performing the action.
/// For example, EnemyEngage means that an enemy engages the player, and PlayerAttack means that the player 
/// attacks an enemy.
public class CombatController : MonoBehaviour {

	public static CombatController instance;

	CombatState currentState = CombatState.DEFAULT;
	List<Enemy> currentlyEngagedEnemies;
	Enemy currentlyTargetedEnemy;

	// Delegate for notifying listeners when the combat state is changed
	public delegate void OnCombatStateChange(CombatState state);
	public event OnCombatStateChange notifyCombatStateChangeObservers;

	public delegate void OnTargetChange(Enemy newTarget);
	public event OnTargetChange notifyOnTargetChangeListeners;

	void Awake() {
		instance = this;
	}

	void Start() {
		currentlyEngagedEnemies = new List<Enemy>();
	}

	public void EnemyEngage(Enemy enemy) {
		// TODO: CHECK IF THIS WORKS PROPERLY
		// Sometimes, the state is set to Engaged even though the player is attacking allready (Combat state)
		currentlyEngagedEnemies.Add(enemy);
		if (currentState == CombatState.DEFAULT) {
			currentState = CombatState.ENGAGED;
			notifyCombatStateChangeObservers(currentState);
		}
	}

	public void EnemyDisengage(Enemy enemy) {
		currentlyEngagedEnemies.Remove(enemy);
		if (currentlyEngagedEnemies.Count == 0 && currentState != CombatState.DEFAULT) {
			// All enemies are dead or gone for some reason
			currentlyTargetedEnemy = null;
			ChangeState(CombatState.DEFAULT);
		}
	}

	public void PlayerAttack(Enemy enemy) {
		currentlyTargetedEnemy = enemy;
		notifyOnTargetChangeListeners(enemy);
		if (currentState != CombatState.COMBAT) {
			ChangeState(CombatState.COMBAT);
		}
	}

	void ChangeState(CombatState state) {
		currentState = state;
		notifyCombatStateChangeObservers(state);
	}

	public CombatState CurrentState { get { return currentState; } }

	public Enemy CurrentlyTargetedEnemy { get { return currentlyTargetedEnemy; } }

}
