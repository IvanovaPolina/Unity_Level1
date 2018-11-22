using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {

	public float startTime = 2f;		// задержка перед началом спавна
	public float repeatTime = 3f;		// периодичность спавна
	public GameObject[] enemies;        // виды врагов
	public ParticleSystem aura;			// визуальный эффект спавна

	void Start() {
		StartCoroutine(Spawn());
	}

	IEnumerator Spawn() {
		yield return new WaitForSeconds(startTime);
		while (true) {
			int index = Random.Range(0, enemies.Length);    // берем рандомного врага
			GameObject enemy = enemies[index];
			Instantiate(enemy, transform.position, Quaternion.identity);    // спавним его
			aura.Play();
			yield return new WaitForSeconds(repeatTime);
		}
	}
}
