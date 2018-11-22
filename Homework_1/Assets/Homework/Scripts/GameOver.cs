using UnityEngine;

public class GameOver : MonoBehaviour {

	public Sprite openedDoor;
	public GameObject particlesOpenedDoor;
	public GameObject particlesWin;
	public MenuController menuController;
	public GameObject winPanel;

	private SpriteRenderer rend;
	private Collider2D col;
	private AudioSource source;

	void Start() {
		rend = GetComponent<SpriteRenderer>();
		col = GetComponent<Collider2D>();
		source = GetComponent<AudioSource>();
	}

	public void OpenDoor() {
		rend.sprite = openedDoor;	// меняем спрайт закрытой двери на спрайт открытой
		col.enabled = true;			// активируем триггер "конца игры"
		particlesOpenedDoor.SetActive(true);	// запускаем заманивающие спецэффекты со звездами
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.layer != LayerMask.NameToLayer("Player")) return;
		source.Play();
		particlesOpenedDoor.SetActive(false);	// выключаем спецэффекты со звездами
		particlesWin.SetActive(true);		// включаем спецэффект вхождения в дверь
		GameObject.FindGameObjectWithTag("MainCamera").GetComponent<FollowingCamera>().enabled = false;
		Destroy(other.gameObject);
		Invoke("ActivateWinPanel", 3f);
	}

	void ActivateWinPanel() {
		menuController.PanelActive(winPanel);
	}
}
