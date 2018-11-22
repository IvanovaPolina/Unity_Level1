using UnityEngine;

public class Bomb : MonoBehaviour {

	public int damage = 20;
	[Header("Слои, на которые реагирует мина")]
	public LayerMask layerMask;

	public GameObject effector;

	void OnCollisionEnter2D(Collision2D other) {
		// определяем, содержит ли маска слой, на который реагирует мина
		bool isNeededLayer = (1 << other.gameObject.layer & layerMask.value) != 0;
		if (!isNeededLayer) return;		// если нет - игнорируем слой
		effector.SetActive(true);	// активируем отталкивающий эффектор
		transform.GetChild(0).gameObject.SetActive(false);  // деактивируем спрайт бомбы
		SetDamage(other);
		Destroy(gameObject, 0.5f);	// уничтожаем бомбу
	}

	void SetDamage(Collision2D other) {
		if (other.gameObject.tag == "Enemy")
			other.gameObject.GetComponent<EnemyScript>().Hurt(damage);
		else if (other.gameObject.tag == "EnemyAI")
			other.gameObject.GetComponent<EnemyAI>().Hurt(damage);
		else if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
			other.gameObject.GetComponent<PlayerHP>().Hurt(damage);
	}
}
