using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

[RequireComponent(typeof(ThirdPersonCharacter))]
[RequireComponent(typeof(AICharacterControl))]
public class Enemy : MonoBehaviour, IDamageable {

	[SerializeField] float aggroRadius = 10f;
	[SerializeField] float attackRadius = 5f;
	[SerializeField] float damagePerShot = 9f;
	[SerializeField] float secondsBetweenShots = .5f;
	[SerializeField] Vector3 aimOffset = new Vector3(0, 1f, 0);

	[SerializeField] GameObject projectileToUse;
	[SerializeField] GameObject projectileSocket;

	[SerializeField] float maxHealthPoints = 100f;
	float currentHealthPoints;

	AICharacterControl aiCharacterControl;
	GameObject player;

	bool isAttacking = false;

	void Start() {
		currentHealthPoints = maxHealthPoints;
		player = GameObject.FindGameObjectWithTag("Player");
		aiCharacterControl = GetComponent<AICharacterControl>();
	}

	void Update() {
		float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
		if (distanceToPlayer <= attackRadius && !isAttacking) {
			isAttacking = true;
			InvokeRepeating("SpawnProjectile", 0, secondsBetweenShots); // TODO: Switch to coroutines (*spit*)
		}
		if (distanceToPlayer > attackRadius) {
			isAttacking = false;
			CancelInvoke();
		}

		if (distanceToPlayer <= aggroRadius) {
			aiCharacterControl.SetTarget(player.transform);
		} else {
			aiCharacterControl.SetTarget(transform);
		}
	}

	void SpawnProjectile() {
		GameObject newProjectile = Instantiate(projectileToUse, projectileSocket.transform.position, Quaternion.identity);
		Projectile projectileComponent = newProjectile.GetComponent<Projectile>();
		projectileComponent.SetDamageCaused(damagePerShot);

		Vector3 unitVectorToPlayer = (player.transform.position + aimOffset - projectileSocket.transform.position).normalized;
		float projectileSpeed = projectileComponent.projectileSpeed;
		newProjectile.GetComponent<Rigidbody>().velocity = unitVectorToPlayer * projectileSpeed;

	}

	public float HealthAsPercentage {
		get {
			return currentHealthPoints / maxHealthPoints;
		}
	}

	public void TakeDamage(float damage) {
		currentHealthPoints = Mathf.Clamp(currentHealthPoints - damage, 0f, maxHealthPoints);
		if (currentHealthPoints <= 0) { Destroy(gameObject); }
	}

	void OnDrawGizmos() {
		// Draw attack radius
		Gizmos.color = new Color(255f, 0, 0, .5f);
		Gizmos.DrawWireSphere(transform.position, attackRadius);
		// Draw aggro radius
		Gizmos.color = new Color(0, 0, 255f, .5f);
		Gizmos.DrawWireSphere(transform.position, aggroRadius);
	}
}
