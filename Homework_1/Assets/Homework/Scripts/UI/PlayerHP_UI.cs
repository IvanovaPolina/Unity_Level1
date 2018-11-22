using UnityEngine;
using UnityEngine.UI;

public class PlayerHP_UI : MonoBehaviour {

	public PlayerHP playerHP;   // ссылка на класс, содержащий информацию о HP игрока
	private int startHP;		// HP игрока при старте
	public Sprite fullHeartIcon;		// иконка с полным здоровьем
	public Sprite halfHeartIcon;     // иконка с неполным здоровьем
	public Sprite emptyHeartIcon;        // иконка с нулевым здоровьем

	public Image iconHeart;		// иконка с сердцем
	public Slider slider;       // полоска HP
	public Text textHP;         // числовое значение

	void Start() {
		startHP = playerHP.HP;
		slider.minValue = 0;
		// для корректного отображения (помнится, могут быть ошибки в вычислениях, если пытаться явно выдать int за float)
		slider.maxValue = float.Parse(startHP.ToString());
	}

	void Update() {
		slider.value = float.Parse(playerHP.HP.ToString());
	}

	public void HeartLabel() {
		if (playerHP.HP > startHP / 2) iconHeart.sprite = fullHeartIcon;
		else if (playerHP.HP > 0 && playerHP.HP <= startHP / 2) iconHeart.sprite = halfHeartIcon;
		else if (playerHP.HP <= 0) iconHeart.sprite = emptyHeartIcon;
	}

	public void TextValue() {
		textHP.text = playerHP.HP.ToString();
	}
}
