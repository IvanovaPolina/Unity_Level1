using UnityEngine;
using UnityEngine.EventSystems;

public class FingerTrail : MonoBehaviour, IPointerDownHandler, IDragHandler {
	
	public GameObject fingerTrail;

	private GameObject instTrail;

	public void OnDrag(PointerEventData eventData) {
		Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		pos.z = 0;
		instTrail.transform.position = pos;
	}

	public void OnPointerDown(PointerEventData eventData) {
		Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		pos.z = 0;
		instTrail = Instantiate(fingerTrail, pos, Quaternion.identity) as GameObject;
	}
}
