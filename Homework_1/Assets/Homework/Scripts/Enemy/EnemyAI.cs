using System.Collections;
using UnityEngine;

public class EnemyAI : MonoBehaviour {
	
	public float visibleDistance = 8f;	// дистанция обнаружения игрока (она же - дистанция дальней атаки)
	public float attackDistance = 1f;   // дистанция ближней атаки
	public int attackDamage = 10;	// сила атаки
	public float cooldown = 1f;    // время перезарядки
	private float lastAttackTime = 0;   // время последней атаки
	public GameObject bazooka;		// ссылка на базуку для дальней атаки
	public GameObject bullet;       // ссылка на снаряд для дальней атаки
	public Transform firepoint;     // начальная точка для стрельбы
	public AudioClip shotClip;		// звук выстрела

	public LayerMask layerMask;     // слои объектов, на которых противник реагирует
	private bool isAngry = false;        // проверка: сагрил ли игрок противника
	public float angryTime = 2f;    // сколько по времени противник будет агрессивным?
	private float lastAngryTime = 0;	// когда противник последний раз был агрессивным?
	private GameObject target;       // цель (помещаем туда игрока, когда тот подходит)
	private Vector2 targetPosition;	// позиция игрока, в которой его последний раз обнаружил противник

	private bool grounded = false;  // проверка, стоит ли противник на земле
	public Transform groundCheck;	// индикатор столкновения с землей
	private Vector2 startPosition;	// стартовая позиция противника (чтобы он мог на неё вернуться)

	public int health = 100;        // количество жизней
	public AudioClip[] deathClips;
	public float volume = 0.8f;     // громкость звука для deathClips
	public Sprite deadEnemy;
	public GameObject pickUp;

	public float speed = 2f;		// скорость
	private float currentDirection_x = 1;   // направление взгляда противника
	private Rigidbody2D rb;
	private Collider2D col;
	private Animator anim;
	private SpriteRenderer rend;
	private AudioSource source;

	IEnumerator Start() {
		yield return new WaitUntil(() => IsGrounded());	// ждём до тех пор, пока не приземлимся
		startPosition = transform.position;     // инициализируем начальную позицию
		rb = GetComponent<Rigidbody2D>();
		col = GetComponent<Collider2D>();
		anim = GetComponent<Animator>();
		rend = GetComponent<SpriteRenderer>();
		source = GetComponent<AudioSource>();
		InvokeRepeating("InvertFlip", 2f, 2f);	// разворачиваем противника каждые две секунды по умолчанию
	}

	bool IsGrounded() {
		if (!grounded)	// если противник пока не приземлился - снова проверяем приземление
			grounded = Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Ground"));
		return grounded;	// возвращаем результат - приземлился или нет
	}

	void Update() {
		if (rb != null) anim.SetFloat("Speed", Mathf.Abs(rb.velocity.x));	// ставим анимацию бега
		if(Time.time - lastAngryTime > angryTime) {		// если прошло время режима агрессии
			isAngry = false;	// противник становится не агрессивным
			target = null;		// игрок теряется из виду
		}
	}

	void FixedUpdate() {
		CheckRaycast();     // проверяем обнаружение игрока
		if (isAngry && grounded) {      // если противник сагрился и находится на земле
			MoveToPosition(targetPosition);     // он двигается к игроку
			ChooseAttack();     // Если дистанция позволяет - выбирает атаку и атакует
		} else if (!isAngry && grounded && transform.position.x != startPosition.x) {  // если не сагрился, находится на земле, и не на исходной позиции
			MoveToPosition(startPosition);  // возвращается на исходную позицию
			bazooka.SetActive(false);	// убираем базуку, если доставали её
		}
	}

	void CheckRaycast() {
		RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.right * currentDirection_x, visibleDistance, layerMask);
		if(hit.collider) {	// если попали лучом в игрока
			target = hit.collider.gameObject;   // Делаем ссылку на игрока
			targetPosition = target.transform.position;		// запоминаем позицию игрока
			isAngry = true;     // Становимся агрессивными
			lastAngryTime = Time.time;	// записываем время, когда противник стал агрессивным
		}
//		Debug.DrawRay(transform.position, transform.right * currentDirection_x * visibleDistance, Color.red);
	}

	void MoveToPosition(Vector2 position) {
		float delta_x = position.x - transform.position.x;	// находим расстояние от противника до цели 
		if (Mathf.Abs(delta_x) <= attackDistance) return;    // если оно меньше или равно, чем дистанция атаки, - не двигаемся никуда
		if (Mathf.Sign(delta_x) != Mathf.Sign(currentDirection_x))
			Flip(); // разворачиваемся в зависимости от того, где игрок и куда мы смотрим
		rb.velocity = transform.right * currentDirection_x * speed + new Vector3(0, rb.velocity.y);	// двигаем противника в сторону игрока
	}

	public void Hurt(int damage) {  // Метод, нанесения урона противнику
		health -= damage; // Отнимаем жизни
		if (health <= 0) Die();     // Если жизней вдруг <= 0, умираем
	}

	void Die() {
		grounded = false;		// дабы мы не стремились в полёте выполнить MoveToPosition()
		anim.enabled = false;
		rend.sprite = deadEnemy;
		AudioClip clip = deathClips[Random.Range(0, deathClips.Length)];
		source.PlayOneShot(clip);
		col.isTrigger = true;		// падаем со сцены
		Instantiate(pickUp, transform.position, Quaternion.identity);	// спавним пикапик
	}

	void ChooseAttack() {
		if (Time.time - lastAttackTime <= cooldown) return; // если не прошло время перезарядки - не атакуем
		if (target == null) return;		// если игрок умер - не атакуем
		float distance = Vector3.Distance(transform.position, target.transform.position);
		if (distance <= attackDistance)		// если дистанция небольшая
			NearAttack();	// используем ближнюю атаку
		else if (distance > attackDistance && distance <= visibleDistance)	// если дистанция не слишком маленькая и не слишком большая
			FarAttack();	// используем дальнюю атаку
		lastAttackTime = Time.time;		// записываем время последней атаки
	}

	void NearAttack() { // Метод ближней атаки
		bazooka.SetActive(false);	// при ближней атаке нам базука не нужна
		target.GetComponent<PlayerHP>().Hurt(attackDamage);	// просто раним игрока
	}

	void FarAttack() {  // Метод дальней атаки
		bazooka.SetActive(true);    // достаём базуку
		AudioSource.PlayClipAtPoint(shotClip, firepoint.position, volume);
		GameObject spawnBullet = Instantiate(bullet, firepoint.position, bullet.transform.rotation);    // спавним снаряд
		spawnBullet.transform.right *= Mathf.Sign(currentDirection_x);
		spawnBullet.GetComponent<Bullet>().damage = attackDamage * 2;    // устанавливаем дамаг от снаряда = двойному урону от ближней атаки
		float bulletSpeed = spawnBullet.GetComponent<Bullet>().speed;
		spawnBullet.GetComponent<Rigidbody2D>().velocity = new Vector2(currentDirection_x * bulletSpeed, 0);    // придаём снаряду скорость
	}

	void InvertFlip() {
		if (isAngry || Mathf.Abs(rb.velocity.x) > 0.1f) return;	// если в режиме агрессии или есть скорость - не выполняем метод
		Flip();		// в противном случае - разворачиваемся
	}

	void Flip() {   // разворот
		currentDirection_x *= -1;   // инвертируем направление взгляда и разворачиваемся
		transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
	}
}
