using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IDamageable {

	[SerializeField] int enemyLayer = 9;

	[SerializeField] float maxHealthPoints = 100f;
	[SerializeField] float attackRange = 1f;
	[SerializeField] float damagePerHit = 20f;
	[SerializeField] float minTimeBetweenHits = .5f;

	GameObject currentTarget;
	float currentHealthPoints = 100f;
	CameraRaycaster cameraRaycaster = null;
	float lastHitTime = 0;

	CombatController combatController;

	void Start() {
		cameraRaycaster = FindObjectOfType<CameraRaycaster>();
		cameraRaycaster.notifyMouseClickObservers += OnMouseClick;
		combatController = GameObject.Find("CombatController").GetComponent<CombatController>();
		if (combatController == null)
			Debug.LogWarning("combatController is null - remember to add the prefab to the scene!");
	}

	void Update() {
		if (currentTarget != null) {
			float distanceToEnemy = Vector3.Distance(transform.position, currentTarget.transform.position);
			var damageable = currentTarget.GetComponent<IDamageable>();
			if (distanceToEnemy <= attackRange && damageable != null && Time.time - lastHitTime > minTimeBetweenHits) {
				damageable.TakeDamage(damagePerHit);
				lastHitTime = Time.time;
			}
		}
	}

	void OnMouseClick(RaycastHit raycastHit, int layerHit) {
		if (layerHit == enemyLayer) {
			var enemy = raycastHit.collider.gameObject;
			currentTarget = enemy;
			combatController.PlayerAttack();
		}
	}

	public void TakeDamage(float damage) {
		currentHealthPoints = Mathf.Clamp(currentHealthPoints - damage, 0f, maxHealthPoints);
	}

	public float AttackRange { get { return attackRange; } }
	
	public float HealthAsPercentage { get { return currentHealthPoints / maxHealthPoints; } }

	void OnDrawGizmos() {
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position, attackRange);	
	}
}
