using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {


	public float projectileSpeed;

	float damageCaused;

	public void SetDamageCaused(float damageCaused) {
		this.damageCaused = damageCaused;
	}

	void OnTriggerEnter(Collider other) {
		IDamageable damageable = other.transform.GetComponent<IDamageable>();
		if (damageable != null) {
			damageable.TakeDamage(damageCaused);
		}
	}
}
