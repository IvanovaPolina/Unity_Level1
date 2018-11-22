using UnityEngine;

public class SelfDestroyInTime : MonoBehaviour {

	public float lifetime = 10f;

	void Start () {
		Destroy(gameObject, lifetime);
	}
}
