using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class NyanCat : MonoBehaviour {

	public float speed = 1f;

	private Vector3 movement;
	private int dir_x = 1;
	private Rigidbody2D rb;

	private void Start() {
		rb = GetComponent<Rigidbody2D>();
	}

	private void FixedUpdate() {
		movement.x = CrossPlatformInputManager.GetAxis("Horizontal");
		movement.y = CrossPlatformInputManager.GetAxis("Vertical");
		rb.velocity = new Vector2(movement.x * speed, movement.y * speed);
	}

	void Update () {
		DirectionOfSight();
	}

	void DirectionOfSight() {
		if (movement.x > 0 && dir_x < 0) Flip();    // если идем вправо и не смотрим вправо - поворачиваемся
		if (movement.x < 0 && dir_x > 0) Flip();    // если идем влево и смотрим вправо - поворачиваемся
	}

	void Flip() {
		dir_x *= -1;
		Vector3 scale = transform.localScale;
		scale.x *= -1;
		transform.localScale = scale;
	}
}
