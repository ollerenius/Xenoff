using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

[RequireComponent(typeof(ThirdPersonCharacter))]
[RequireComponent(typeof(AICharacterControl))]
public class Enemy : MonoBehaviour, IDamageable {

	[SerializeField]
	float attackRange = 1f;
	[SerializeField]
	float attackRadius = 4f;
	[SerializeField]
	float maxHealthPoints = 100f;
	float currentHealthPoints = 100f;
	AICharacterControl aiCharacterControl;
	GameObject player;

	void Start() {
		player = GameObject.FindGameObjectWithTag("Player");
		aiCharacterControl = GetComponent<AICharacterControl>();
	}

	void Update() {
		float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
		if (distanceToPlayer < attackRadius) {
			aiCharacterControl.SetTarget(player.transform);
		} else {
			aiCharacterControl.SetTarget(transform);
		}
	}

	public float AttackRange { get { return attackRange; } }

	public float HealthAsPercentage {
		get {
			return currentHealthPoints / maxHealthPoints;
		}
	}

	public void TakeDamage(float damage) {
		currentHealthPoints = Mathf.Clamp(currentHealthPoints - damage, 0f, maxHealthPoints);
	}

	void OnDrawGizmos() {
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position, attackRange);	
	}
}
