using UnityEngine;

public class PlayerHP : MonoBehaviour {

	public int HP = 100;
	public AudioClip[] ouchClips;
	public AudioClip deathClip;
	public Animator anim;

	private AudioSource source;
	private Collider2D col;

	void Start() {
		source = GetComponent<AudioSource>();
		col = GetComponent<Collider2D>();
	}

	public void Hurt(int damage) {
		HP -= damage;
		if (HP <= 0) {
			HP = 0;		// для корректного вывода в GUI
			Die();
			return;
		}
		AudioClip clip = ouchClips[Random.Range(0, ouchClips.Length)];
		source.PlayOneShot(clip);
	}

	void Die() {
		anim.SetTrigger("Die");
		source.PlayOneShot(deathClip, 1f);
		col.isTrigger = true;
	}
}
