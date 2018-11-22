using UnityEngine;

public class WindChimes : MonoBehaviour {

	public AudioClip[] clips;	// музыкальные эффекты
	[Range(0, 1)]
	public float volume = 1f;	// громкость звука для эффектов
	public float addingForce = 50f;		// сила раскачивания объекта при столкновении с кем-либо

	private AreaEffector2D effector;
	private float magnitude;

	void Start() {
		effector = transform.parent.GetComponentInChildren<AreaEffector2D>(false);
		magnitude = effector.forceMagnitude;
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.tag != "Player") return;
		AudioClip clip = clips[Random.Range(0, clips.Length)];
		AudioSource.PlayClipAtPoint(clip, transform.position, volume);
		effector.forceMagnitude += addingForce;
	}

	void OnTriggerExit2D(Collider2D other) {
		if (other.tag != "Player") return;
		effector.forceMagnitude = magnitude;
	}
}
