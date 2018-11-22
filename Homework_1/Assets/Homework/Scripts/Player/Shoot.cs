using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;

public class Shoot : MonoBehaviour {

	public PlayerController playerController;
	public Rigidbody2D bullet;      // ссылка на префаб пули
	public float cooldown = 0.5f;   // перезарядка

	private float lastShot = 0;		// время прошлого выстрела
	public Transform firepoint;     // начальная точка для стрельбы
	private AudioSource sound;      // звук выстрела
	private bool canShoot = false;	// флаг, определяющий, можно ли стрелять

	public GraphicRaycaster graphicRaycaster;	// компонент на Canvas, отвечающий за raycast'ы
	public EventSystem eventSystem;		// без него UI элементы не будут интерактивными
	private PointerEventData eventData;		// регистратор события нажатия кнопки мыши

	void Start() {
		sound = GetComponent<AudioSource>();
	}

	void Update() {
		if (Time.timeScale == 0) return;	// если игра на паузе - не стреляем
		if (!CrossPlatformInputManager.GetButtonDown("Fire1")) return;      // проверяем нажатие кнопки
		//if (IsClickOnUI()) return;		// если кнопка нажата на UI элементе - не стреляем
		if (Time.time - lastShot < cooldown) return;	// проверяем перезарядку
		canShoot = true;		// разрешаем выстрел
		lastShot = Time.time;	// ставим время для следующей перезарядки
	}

	bool IsClickOnUI() {
		eventData = new PointerEventData(eventSystem);	// нажатие ЛКМ будем проверять на UI элементах
		eventData.position = Input.mousePosition;	// регистрируем в событии текущую позицию мыши
		List<RaycastResult> results = new List<RaycastResult>();	// для вывода результатов об объектах, в которые мы попали
		graphicRaycaster.Raycast(eventData, results);	// пускаем луч в соотв. с настройками eventData и заносим результаты в список
		if (results.Count > 0) return true;	// если список содержит хоть один результат - мы попали в UI
		return false;
	}

	void FixedUpdate() {
		if (!canShoot) return;
		Fire();
	}

	void Fire() {
		sound.Play();
		canShoot = false;
		Rigidbody2D bulletRB = Instantiate(bullet, firepoint.position, bullet.transform.rotation) as Rigidbody2D;
		bulletRB.transform.right *= Mathf.Sign(playerController.dir_x);
		Vector2 speed = new Vector2(bulletRB.GetComponent<Bullet>().speed, 0);
		bulletRB.velocity = speed * Mathf.Sign(playerController.dir_x);
	}
}
