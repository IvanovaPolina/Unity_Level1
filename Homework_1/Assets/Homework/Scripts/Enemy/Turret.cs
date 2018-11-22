using System.Collections;
using UnityEngine;

public class Turret : MonoBehaviour {

	public GameObject gun;		// ссылка на префаб башни турели
	public Transform firepoint;		// начальная точка для стрельбы
	public Rigidbody2D bullet;      // снаряд
	public int damage = 5;		// урон от снаряда турели
	public float rotationSpeed = 5f;	// скорость поворота башни
	public float cooldown = 0.5f;     // время перезарядки турели

	private float reloadTime = 0;		// время следующего выстрела
	private float angle;        // угол, на который должна повернуться башня
	private bool IsTriggered = false;        // флаг, указывающий, вошел ли игрок в радиус поражения турели
	private Rigidbody2D gunRigidbody;	// башня, которую будем поворачивать и из которой стрелять
	private Vector3 direction;  // вектор направления поворота башни
	private AudioSource audioSource;    // ссылка на компонент для воспроизведения звука выстрела

	//void Start() {
	//	gunRigidbody = gun.GetComponent<Rigidbody2D>();
	//	audioSource = GetComponent<AudioSource>();
	//}

	//void OnTriggerEnter2D(Collider2D other) {
	//	if (other.gameObject.layer != LayerMask.NameToLayer("Player")) return;
	//	IsTriggered = true;
	//	StartCoroutine(FindAngle(other));
	//}

	//void OnTriggerExit2D(Collider2D other) {
	//	if (other.gameObject.layer != LayerMask.NameToLayer("Player")) return;
	//	IsTriggered = false;
	//}

	//IEnumerator FindAngle(Collider2D other) {
	//	// если башня двигается вправо - значение поворота уменьшается, если влево - увеличивается
	//	while (IsTriggered) {
	//		// находим вектор направления для поворота башни
	//		Vector3 gunPosition = gun.transform.position;
	//		Vector3 playerPosition = other.transform.position;
	//		direction = playerPosition - gunPosition;
	//		// находим угол, на который будем поворачивать башню
	//		angle = Vector3.Angle(direction, transform.right);
	//		yield return StartCoroutine(Rotate());  // поворачиваем башню
	//	}
	//}
	//private void Update() {
	//	//Debug.DrawRay(gun.transform.position, direction, Color.green);
	//	//Debug.DrawRay(gun.transform.position, transform.right, Color.red);
	//}

	//#region FindAngle_2
	////IEnumerator FindAngle(Collider2D other) {
	////	while (IsTriggered) {
	////		// угол между двумя векторами (cos) = скалярное произведение / произведение модулей
	////		Vector3 gunPosition = gun.transform.position;
	////		Vector3 playerPosition = other.transform.position;
	////		Vector3 a = playerPosition - gunPosition;   // вектор с направлением на игрока
	////		Vector3 b = Vector3.down;                   // вектор, направленный вниз
	////		float angle_1 = Vector3.Dot(a, b) / (a.magnitude * b.magnitude);    // находим косинус угла по формуле
	////		// чтобы найти нужный угол в градусах - находим arccos этого угла и переводим в градусы
	////		// прибавила 180f, потому что крутится в противоположном направлении почему-то...
	////		float angle_2 = Mathf.Acos(angle_1) * Mathf.Rad2Deg + 180f;
	////		angle = Mathf.LerpAngle(gunRigidbody.rotation, angle_2, 1);  // это реальная точка, до которой должен повернуться объект
	////		Debug.Log(angle);
	////		yield return StartCoroutine(Rotate());	// поворачиваем пушку
	////	}
	////}
	//#endregion

	//float rotAngle = 0;
	//Quaternion rotation = Quaternion.identity;
	//IEnumerator Rotate() {
	//	while (Mathf.RoundToInt(gunRigidbody.rotation) != Mathf.RoundToInt(angle)) {
	//		//Vector3 newDir = Vector3.RotateTowards(gun.transform.up, direction, rotationSpeed * Time.deltaTime, 0);
	//		//Debug.DrawRay(transform.position, newDir, Color.red);
	//		//rotation = Quaternion.LookRotation(newDir);
	//		//gun.transform.rotation = rotation;
	//		gun.transform.LookAt()
	//		yield return null;
	//	}
	//	// при проверке округляем значения углов до int, потому что имеет место плавный поворот башни
	//	// При float ждать совпадения значений углов придется весьма долгое время
	//	//while (Mathf.RoundToInt(rotAngle) != Mathf.RoundToInt(angle)) {
	//		// Mathf.Clamp(rotAngle, minAngle, maxAngle);
	//		//rotAngle = Mathf.LerpAngle(gunRigidbody.rotation, angle, rotationSpeed * Time.fixedDeltaTime); // замедляем поворот
	//		//gunRigidbody.MoveRotation(rotAngle);    // поворачиваем на нужный угол

	//		//yield return new WaitForFixedUpdate();
	//	//}
	//	yield return StartCoroutine(Shoot());
	//}

	//IEnumerator Shoot() {
	//	if (Time.time > reloadTime) {       // если прошло время перезарядки
	//		audioSource.Play();     // проигрываем звук выстрела
	//		Quaternion rotation = Quaternion.Euler(0, 0, angle + 90f);  // задаем поворот снаряда при спавне
	//		Rigidbody2D turretBullet = Instantiate(bullet, firepoint.position, rotation) as Rigidbody2D;    // спавним снаряд
	//		turretBullet.GetComponent<Bullet>().damage = damage;
	//		float speed = turretBullet.GetComponent<Bullet>().speed;
	//		turretBullet.GetComponent<Rigidbody2D>().velocity = direction.normalized * speed;
	//		reloadTime = Time.time + cooldown;  // обновляем время для следующего выстрела
	//		yield return new WaitForFixedUpdate();      // выходим из корутины
	//	}
	//	yield return null;      // если время перезарядки еще не прошло - просто выходим из корутины
	//}

	void Start() {
		gunRigidbody = gun.GetComponent<Rigidbody2D>();
		audioSource = GetComponent<AudioSource>();
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.tag != "Player") return;
		IsTriggered = true;
		StartCoroutine(FindAngle(other));
	}

	void OnTriggerExit2D(Collider2D other) {
		if (other.tag != "Player") return;
		IsTriggered = false;
	}

	IEnumerator FindAngle(Collider2D other) {
		while (IsTriggered) {
			// находим вектор направления для поворота башни
			Vector3 gunPosition = gun.transform.position;
			Vector3 playerPosition = other.transform.position;
			direction = playerPosition - gunPosition;
			// находим угол, на который будем поворачивать башню
			angle = Vector3.Angle(direction, Vector3.down) - 180f;
			yield return StartCoroutine(Rotate());  // поворачиваем башню
		}
	}

	#region FindAngle_2
	//IEnumerator FindAngle(Collider2D other) {
	//	while (IsTriggered) {
	//		// угол между двумя векторами (cos) = скалярное произведение / произведение модулей
	//		Vector3 gunPosition = gun.transform.position;
	//		Vector3 playerPosition = other.transform.position;
	//		Vector3 a = playerPosition - gunPosition;   // вектор с направлением на игрока
	//		Vector3 b = Vector3.down;                   // вектор, направленный вниз
	//		float angle_1 = Vector3.Dot(a, b) / (a.magnitude * b.magnitude);    // находим косинус угла по формуле
	//		// чтобы найти нужный угол в градусах - находим arccos этого угла и переводим в градусы
	//		// прибавила 180f, потому что крутится в противоположном направлении почему-то...
	//		float angle_2 = Mathf.Acos(angle_1) * Mathf.Rad2Deg + 180f;
	//		angle = Mathf.LerpAngle(gunRigidbody.rotation, angle_2, 1);  // это реальная точка, до которой должен повернуться объект
	//		Debug.Log(angle);
	//		yield return StartCoroutine(Rotate());	// поворачиваем пушку
	//	}
	//}
	#endregion

	IEnumerator Rotate() {
		float rotAngle = 0;
		// при проверке округляем значения углов до int, потому что имеет место плавный поворот башни
		// При float ждать совпадения значений углов придется весьма долгое время
		while (Mathf.RoundToInt(rotAngle) != Mathf.RoundToInt(angle)) {
			rotAngle = Mathf.LerpAngle(gunRigidbody.rotation, angle, rotationSpeed * Time.fixedDeltaTime); // замедляем поворот
			gunRigidbody.MoveRotation(rotAngle);    // поворачиваем на нужный угол
			yield return null;
		}
		yield return StartCoroutine(Shoot());
	}

	IEnumerator Shoot() {
		if (Time.time > reloadTime) {       // если прошло время перезарядки
			audioSource.Play();     // проигрываем звук выстрела
			Quaternion rotation = Quaternion.Euler(0, 0, angle + 90f);  // задаем поворот снаряда при спавне
			Rigidbody2D turretBullet = Instantiate(bullet, firepoint.position, rotation) as Rigidbody2D;    // спавним снаряд
			turretBullet.GetComponent<Bullet>().damage = damage;
			float speed = turretBullet.GetComponent<Bullet>().speed;
			turretBullet.GetComponent<Rigidbody2D>().velocity = direction.normalized * speed;
			reloadTime = Time.time + cooldown;  // обновляем время для следующего выстрела
			yield return null;      // выходим из корутины
		}
		yield return null;      // если время перезарядки еще не прошло - просто выходим из корутины
	}
}
