using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

	public float damageCaused;
	public float projectileSpeed;

	void OnTriggerEnter(Collider other) {
		IDamageable damageable = other.transform.GetComponent<IDamageable>();
		if (damageable != null) {
			damageable.TakeDamage(damageCaused);
		}
	}
}
