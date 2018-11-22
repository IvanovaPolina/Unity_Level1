using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PickUp_UI : MonoBehaviour {

	public PickUpKind kind;
	public PickUpController pickUpCtrl;
	public ParticleSystem fenceParticles;   // частицы, которые движутся в сторону ограждения, когда оно ломается

	private Text pickUpText;
	private PickupData data;
	private bool canMoving = false;		// могут ли частицы начинать двигаться до ограждения
	private float moveSpeed = 4.5f;		// скорость движения частиц

	void Start() {
		pickUpText = GetComponent<Text>();
		foreach (var _data in pickUpCtrl.PickupsData)   // делаем ссылку на нужный пикап
			if (_data.type == kind)
				data = _data;
		StartCoroutine(MoveParticlesToFence());
	}

	void Update() {
		pickUpText.text = data.CollectedAmount + "/" + data.targetAmount;
		if (data.LeftAmount == 0) canMoving = true;
	}

	IEnumerator MoveParticlesToFence() {
		yield return new WaitUntil(() => canMoving);    // ждём, пока соберутся все пикапы данного вида
		if (data.fences.Length > 0) {
			// спавним частицы возле того пикапа, который собрали
			var main = fenceParticles.main;
			main.startColor = pickUpText.color;
			ParticleSystem particles = Instantiate(fenceParticles, pickUpCtrl.LastPickUpPosition, Quaternion.identity);
			yield return new WaitForSeconds(0.5f);  // даём игроку заметить, откуда они появились
			yield return StartCoroutine(Move(particles));    // постепенно двигаем их к сломанному ограждению
		}
	}

	IEnumerator Move(ParticleSystem particles) {
		Vector3 targetPos = data.fences[0].transform.position;
		while (Vector3.Distance(particles.transform.position, targetPos) > 0) {
			Vector3 newPos = Vector3.MoveTowards(particles.transform.position, targetPos, Time.deltaTime * moveSpeed);
			particles.transform.position = newPos;
			yield return null;
		}
		Destroy(particles.gameObject, 0.5f);	// уничтожаем частицы
	}
}
