using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour {

	public AudioClip clickButton;
	public GameObject gamePanel, pausePanel, winPanel, losePanel;

	void Start() {
		Time.timeScale = 1;
		Activate(gamePanel);
	}

	void Update() {
		if (Input.GetKeyDown(KeyCode.P)) {
			AudioSource.PlayClipAtPoint(clickButton, Vector3.zero, 1f);
			pausePanel.SetActive(true);
			Time.timeScale = 0;
		}
	}

	void Activate(GameObject panel) {			// для Canvas'a
		gamePanel.SetActive(gamePanel == panel);
		pausePanel.SetActive(pausePanel == panel);
		winPanel.SetActive(winPanel == panel);
		losePanel.SetActive(losePanel == panel);
	}

	// для панелей
	public void PanelActive(GameObject panel) {
		Time.timeScale = 0;		// ставим саму игру на паузу, когда активируем какую-либо панель
		Activate(panel);
	}

	// для кнопок
	public void Pause() {
		AudioSource.PlayClipAtPoint(clickButton, Vector3.zero, 1f);
		PanelActive(pausePanel);
	}
	
	public void Resume() {
		Time.timeScale = 1;     // убираем игру с паузы, когда возвращаемся в неё
		AudioSource.PlayClipAtPoint(clickButton, Vector3.zero, 1f);
		Activate(gamePanel);
	}
	
	public void Restart() {
		var activeScene = SceneManager.GetActiveScene().buildIndex;
		SceneManager.LoadScene(activeScene);
	}
	
	public void MainMenu() {
		Time.timeScale = 1;
		AudioSource.PlayClipAtPoint(clickButton, Vector3.zero, 1f);
		SceneManager.LoadScene(0);	// отдельная сцена для MainMenu
	}
}
