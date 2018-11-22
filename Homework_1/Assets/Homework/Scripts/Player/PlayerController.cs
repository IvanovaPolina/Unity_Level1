using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerController : MonoBehaviour {
	
	[HideInInspector]
	public float dir_x = 1;	// направление взгляда персонажа

	public float speed = 4f;             // максимальная скорость движения персонажа
	public float moveForce = 32000f;		// скорость разгона персонажа
	public float jumpForce = 650f;         // высота прыжка
	public AudioClip[] jumpClips;	// звуки прыжка
	public Transform groundCheck;	// индикатор наличия земли под ногами
	public Animator anim;      // ссылка на компонент "Animator" у персонажа (реализует анимацию движения)

	private Vector2 movement;   // вектор направления движения
	private Rigidbody2D rb;
	private bool jump = false;	// индикатор прыжка

	void Start () {
		rb = GetComponent<Rigidbody2D>();
	}
	
	void Update () {
		if (Time.timeScale == 0) return;	// если игра на паузе - не двигаемся
		DirectionOfSight();
		CheckJump();
	}

	void FixedUpdate() {
		MovePlayer();
		Jump();
	}

	void CheckJump() {
		int layerIndex = LayerMask.NameToLayer("Ground");	// индекс слоя под названием Ground
		bool grounded = Physics2D.Linecast(transform.position, groundCheck.position, 1 << layerIndex);	// проверяем лучом касание слоя Ground
		if(CrossPlatformInputManager.GetButtonDown("Jump") && grounded)	// если нажали "прыжок" и персонаж на земле
			jump = true;	// разрешаем прыжок
	}

	void Jump() {
		if (!jump) return;
		AudioClip clip = jumpClips[Random.Range(0, jumpClips.Length)];	// выбираем рандомный звук прыжка
		AudioSource.PlayClipAtPoint(clip, transform.position, 1f);  // проигрываем его
		Vector2 jumpVector = new Vector2(0, jumpForce);
		rb.AddForce(jumpVector * Time.fixedDeltaTime, ForceMode2D.Impulse);
		anim.SetTrigger("Jump");	// проигрываем анимацию прыжка
		jump = false;		// не забываем опустить персонажа на землю
	}
	
	void MovePlayer() {
		movement.x = CrossPlatformInputManager.GetAxis("Horizontal");     // реализуем движение игрока по оси X
		if (movement.x * rb.velocity.x < speed)
			rb.AddForce(Vector2.right * movement.x * moveForce * Time.fixedDeltaTime);
		if (Mathf.Abs(rb.velocity.x) > speed)
			rb.velocity = new Vector2(Mathf.Sign(rb.velocity.x) * speed, rb.velocity.y);
		// делаем зависимость параметра Speed в аниматоре от ускорения
		anim.SetFloat("Speed", Mathf.Abs(rb.velocity.x));  // берем только значение числа по модулю
	}

	void DirectionOfSight() {
		if (movement.x > 0 && dir_x < 0) Flip();	// если идем вправо и не смотрим вправо - поворачиваемся
		if (movement.x < 0 && dir_x > 0) Flip();	// если идем влево и смотрим вправо - поворачиваемся
	}

	void Flip() {
		dir_x *= -1;
		Vector3 scale = transform.localScale;
		scale.x *= -1;
		transform.localScale = scale;
	}
}
