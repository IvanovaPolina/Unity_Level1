using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

	public AudioClip clickButton;

	public void StartGame(int sceneIndex) {
		AudioSource.PlayClipAtPoint(clickButton, Vector3.zero);
		SceneManager.LoadScene(sceneIndex);
	}

	public void ExitGame() {
		AudioSource.PlayClipAtPoint(clickButton, Vector3.zero, 1f);
		Application.Quit();
	}
}
