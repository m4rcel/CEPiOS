using UnityEngine;
using System.Collections;

public class MouseOrbit : MonoBehaviour {

    private Transform target;
    private float distance;

    public float xSpeed = 0.5f;
    public float ySpeed = 0f;

    public float yMinLimit = -20f;
    public float yMaxLimit = 80f;

    private float x = 0f;
    private float y = 0f;

	// Use this for initialization
	void Start () {
        // look for target and calculate distance to it
        recalculateDistance(transform);

        var angles = transform.eulerAngles;
        x = angles.y;
        y = angles.x;

        // Make the rigid body not change rotation
        if (rigidbody)
            rigidbody.freezeRotation = true;
	}
    private void recalculateDistance(Transform targetTransform)
    {
        target = targetTransform.parent.FindChild("Target");
        distance = Vector3.Distance(transform.position, target.position);
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    void LateUpdate()
    {
        if (target)
        {
            x += Input.GetAxis("Mouse X") * xSpeed * 0.02f;
            y -= Input.GetAxis("Mouse Y") * ySpeed * 0.02f;

            y = ClampAngle(y, yMinLimit, yMaxLimit);

            var rotation = Quaternion.Euler(y, x, 0);
            var position = rotation * new Vector3(0, 0, -distance) + target.position;

            transform.rotation = rotation;
            transform.position = position;
        }
    }

    public static float ClampAngle (float angle, float min, float max) {
	if (angle < -360f)
		angle += 360f;
	if (angle > 360f)
		angle -= 360f;
	return Mathf.Clamp (angle, min, max);
}
}
