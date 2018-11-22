using UnityEngine;

public class Portal : MonoBehaviour {

	public Transform targetPortal;
	public float rotationSpeed;

	[HideInInspector]
	public float passedTime = 0;
	private float cooldown = 1f;
	private GameObject sprite;

	void Start() {
		sprite = transform.GetChild(0).gameObject;
	}

	void Update() {
		sprite.transform.Rotate(0, 0, rotationSpeed * Time.deltaTime, Space.Self);	// крутим спрайт портала
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.layer != LayerMask.NameToLayer("Player")) return;	// порталы принимают только игрока
		if (Time.time - passedTime < cooldown) return;		// делаем перезарядку, чтобы не порталироваться в каждом кадре
		other.transform.position = targetPortal.transform.position;	// перемещаем игрока в другой портал
		targetPortal.GetComponent<Portal>().passedTime = Time.time;	// назначаем другому порталу время для перезарядки
	}
}
