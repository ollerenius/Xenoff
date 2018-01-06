using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

[RequireComponent(typeof(ThirdPersonCharacter))]
[RequireComponent(typeof(AICharacterControl))]
public class Enemy : MonoBehaviour, IDamageable {

	[SerializeField] bool isAggressive;
	[SerializeField] float aggroRadius = 10f;
	[SerializeField] float escpapeRadius = 20f;
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

	bool isEngaged;
	bool isAttacking;

	void Start() {
		currentHealthPoints = maxHealthPoints;
		player = GameObject.FindGameObjectWithTag("Player");
		aiCharacterControl = GetComponent<AICharacterControl>();
		// Being non-aggressive means that the enemy will never attack the player by itself. 
		// Therefore, this is sufficient.
		// TODO: Put if statement in combat management method instead
		if (!isAggressive) {
			aggroRadius = 0;
		}
	}

	void Update() {
		ManageCombat();
	}

	void ManageCombat() {
		float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);

		// Attack player if close enough
		if (distanceToPlayer <= attackRadius && !isAttacking && isEngaged) {
			isAttacking = true;
			InvokeRepeating("SpawnProjectile", 0, secondsBetweenShots); // TODO: Switch to coroutines (*spit*)
		}
		if (distanceToPlayer > attackRadius) {
			isAttacking = false;
			CancelInvoke();
		}

		// Engage enemy if close enough
		if (!isEngaged) {
			if (distanceToPlayer <= aggroRadius) {
				EngagePlayer();
			} else {
				aiCharacterControl.SetTarget(transform);
			}
		} else {
			// If player gets far enough, disengage
			if (distanceToPlayer > escpapeRadius) {
				DisengagePlayer();
			}
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

	/// <summary>
	/// This method is called when the enemy notices or is attacked by the player.
	/// </summary>
	void EngagePlayer() {
		if (!isEngaged) {
			isEngaged = true;
			aiCharacterControl.SetTarget(player.transform);
			CombatController.instance.EnemyEngage(this);
		}
	}

	/// <summary>
	/// This method is called when the enemy loses interest in the player (if it runs away, for instance)
	/// </summary>
	void DisengagePlayer() {
		if (isEngaged) {
			isEngaged = false;
			aiCharacterControl.SetTarget(transform);
			CombatController.instance.EnemyDisengage(this);
		}
	}

	public float HealthAsPercentage { get { return currentHealthPoints / maxHealthPoints; } }

	public void TakeDamage(float damage) {
		currentHealthPoints = Mathf.Clamp(currentHealthPoints - damage, 0, maxHealthPoints);
		EngagePlayer();
		if (currentHealthPoints <= 0) { 
			DisengagePlayer();
			Destroy(gameObject);
		}
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
