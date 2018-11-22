using UnityEngine;

public class FollowingCamera : MonoBehaviour {

	public Rect borders;
	public Transform target;
	
	void LateUpdate () {
		float clamp_x = Mathf.Clamp(target.position.x, borders.xMin, borders.width);
		float clamp_y = Mathf.Clamp(target.position.y, borders.yMin, borders.height);
		transform.position = new Vector3(clamp_x, clamp_y, transform.position.z);
	}
}
