using UnityEngine;

public class EnemyScript : MonoBehaviour {
	
	public int HP = 40;
	public float speed;
	public int damage = 5;
	public float cooldown = 0.5f;
	public Transform frontCheck;
	public Sprite deadEnemy;
	public AudioClip[] deathClips;
	public float volumeOfClip = 0.8f;   // громкость deathClip'а
	public GameObject pickUp;

	private float lastAttackTime = 0;
	private Rigidbody2D rb;
	private Collider2D col;
	private SpriteRenderer rend;
	private Animator anim;
	private AudioSource source;

	void Start() {
		rb = GetComponent<Rigidbody2D>();
		col = GetComponent<Collider2D>();
		rend = GetComponent<SpriteRenderer>();
		anim = GetComponent<Animator>();
		source = GetComponent<AudioSource>();
	}

	void FixedUpdate() {
		MoveEnemy();
		Attack();
	}

	void MoveEnemy() {
		rb.velocity = new Vector2(transform.localScale.x * speed, rb.velocity.y);
		Collider2D[] frontHits = Physics2D.OverlapPointAll(frontCheck.position, 1 << LayerMask.NameToLayer("Ground"));
		foreach (Collider2D c in frontHits) {	// если враг сталкивается с препятствием - он разворачивается
			Vector3 scale = transform.localScale;
			scale.x *= -1;
			transform.localScale = scale;
			break;
		}
	}

	void Attack() {
		RaycastHit2D hit = Physics2D.Linecast(transform.position, frontCheck.position, 1 << LayerMask.NameToLayer("Player"));
		if (hit.collider && (Time.time - cooldown > lastAttackTime)) {
			hit.collider.gameObject.GetComponent<PlayerHP>().Hurt(damage);
			lastAttackTime = Time.time;
		}
	}
	
	public void Hurt(int damage) {
		HP -= damage;
		if (HP <= 0) Death();
	}

	void Death() {
		anim.enabled = false;	// отключаем аниматор, чтобы заменить спрайт
		if(deadEnemy != null) rend.sprite = deadEnemy;
		int index = Random.Range(0, deathClips.Length);
		source.PlayOneShot(deathClips[index]);
		// ставим коллайдер врага в IsTrigger, чтобы он упал и столкнулся с уничтожающим коллайдером внизу
		col.isTrigger = true;
		Instantiate(pickUp, transform.position, Quaternion.identity);	// спавним пикапик
	}
}
