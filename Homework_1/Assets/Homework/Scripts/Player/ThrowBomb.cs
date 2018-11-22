using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class ThrowBomb : MonoBehaviour {

	public Transform firepoint;
	public GameObject bomb;
	public float throwForce = 250f;

	private PlayerController playerController;
	private bool canThrow = false;	// флаг, разрешающий поставить мину
	
	public int maxAmount = 3;	// максимальный запас мин
	public int LeftAmount { get { return maxAmount - bombs.Count; } }	// сколько мин осталось в запасе
	private List<GameObject> bombs;		// запас мин

	void Start() {
		playerController = GetComponent<PlayerController>();
		bombs = new List<GameObject>(maxAmount);
	}

	void Update () {
		if (Time.timeScale == 0) return;    // если игра на паузе - не ставим мину
		CheckBombs();		// проверяем, взорвались ли какие-то из установленных мин
		if (!CrossPlatformInputManager.GetButtonDown("Fire2")) return;    // проверяем нажатие ПКМ
		if (LeftAmount <= 0) return;	// и есть ли в запасе еще мины
		canThrow = true;
	}

	void FixedUpdate() {
		if (canThrow) {
			Throw();
			canThrow = false;
		}
	}

	void CheckBombs() {
		for (int i = 0; i < bombs.Count; i++)
			if (bombs[i] == null) {
				bombs.Remove(bombs[i]);		// убираем взорванную мину из запаса
				i--;
			}
	}

	void Throw() {
		Vector2 direction = new Vector2(throwForce * Mathf.Sign(playerController.dir_x), throwForce); // берем направление под углом 45 градусов
		GameObject _bomb = Instantiate(bomb, firepoint.position, Quaternion.identity);  // спавним мину
		_bomb.GetComponent<Rigidbody2D>().AddForce(direction * Time.fixedDeltaTime, ForceMode2D.Impulse); // кидаем её в нужном направлении
		bombs.Add(_bomb);   // учитываем её в запасе
	}
}
