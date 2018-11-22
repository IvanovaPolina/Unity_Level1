using UnityEngine;

public class PickUpController : MonoBehaviour
{
	public GameOver gameOver;
	public Animator anim;
	public PickupData[] PickupsData;

	public Vector3 LastPickUpPosition { get { return lastPickUpPosition; } }
	private Vector3 lastPickUpPosition;

	private AudioSource source;

	void Start() {
		source = GetComponent<AudioSource>();
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.layer != LayerMask.NameToLayer("Pickups")) return;
		foreach (var data in PickupsData) {
			if (other.GetComponent<PickUpType>().type == data.type) {
				lastPickUpPosition = other.transform.position;
				data.CollectPickup();
				anim.SetTrigger("PickUp");
				source.PlayOneShot(data.collectClip);
				Destroy(other.gameObject);
				if(data.type == PickUpKind.Key) gameOver.OpenDoor();
			}
		}
	}
}
