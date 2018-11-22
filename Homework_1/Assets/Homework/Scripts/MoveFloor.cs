using UnityEngine;
using UnityEngine.EventSystems;

public class MoveFloor : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

	public Animator floorAnim;

	public void OnPointerEnter(PointerEventData eventData) {
		if (floorAnim == null) return;
		floorAnim.SetTrigger("MoveUp");
	}

	public void OnPointerExit(PointerEventData eventData) {
		if (floorAnim == null) return;
		floorAnim.SetTrigger("MoveDown");
	}
}
