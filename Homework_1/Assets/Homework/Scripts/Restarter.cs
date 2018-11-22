using UnityEngine;

public class Restarter : MonoBehaviour {

	public MenuController menuController;
	public GameObject losePanel;

	private AudioSource source;

	void Start() {
		source = GetComponent<AudioSource>();
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.layer == LayerMask.NameToLayer("Player")) {
			source.Play();
			GameObject.FindGameObjectWithTag("MainCamera").GetComponent<FollowingCamera>().enabled = false;
			Destroy(other.gameObject);
			Invoke("ActivateLosePanel", 1.0f);
			return;
		}
		Destroy(other.gameObject);	// если упал не игрок - уничтожаем всё остальное
	}

	void ActivateLosePanel() {
		menuController.PanelActive(losePanel);
	}
}
