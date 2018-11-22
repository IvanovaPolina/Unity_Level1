using UnityEngine;
using UnityEngine.UI;

public class Bomb_UI : MonoBehaviour {

	public ThrowBomb throwBomb;

	private Text text;

	void Start() {
		text = GetComponent<Text>();
	}

	void Update() {
		text.text = throwBomb.LeftAmount + "/" + throwBomb.maxAmount;
	}
}
