using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

	[SerializeField]
	float attackRange = 1f;
	[SerializeField]
	float maxHealthPoints = 100f;
	float currentHealthPoints = 100f;

	public float AttackRange { get { return attackRange; } }

	public float HealthAsPercentage {
		get {
			return currentHealthPoints / maxHealthPoints;
		}
	}

	void OnDrawGizmos() {
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position, attackRange);	
	}
}
