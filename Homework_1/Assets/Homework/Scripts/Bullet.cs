using UnityEngine;

public class Bullet : MonoBehaviour {
	
	[Range(1, 50)]
	public int damage = 20;     // урон пули
	[Header("Слои, которые пуля проигнорирует")]
	public LayerMask layerMask;
	public float speed = 20f;     // скорость движения
	public float lifetime;  // время самоуничтожения
	public GameObject explosion;
	
	void Start () {
		Destroy(gameObject, lifetime);
	}

	void OnTriggerEnter2D(Collider2D other) {
		bool isIgnoredLayer = (1 << other.gameObject.layer & layerMask.value) != 0;
		if (isIgnoredLayer) return;
		if (other.tag == "Enemy") other.GetComponent<EnemyScript>().Hurt(damage);
		else if (other.tag == "EnemyAI") other.GetComponent<EnemyAI>().Hurt(damage);
		else if (other.tag == "Player") other.gameObject.GetComponent<PlayerHP>().Hurt(damage);
		Instantiate(explosion, transform.position, Quaternion.identity);	// спавним эффект взрыва
		Destroy(gameObject);		// уничтожаем пулю
	}
}
