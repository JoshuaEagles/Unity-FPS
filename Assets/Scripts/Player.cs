using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
	float moveSpeed = 5f;
	float moveAccel = 5f;
	float lookSpeed = 5f;
	float gravity = 0.1f;
	float accelerationFactor = 0.01f;
	float frictionFactor = 0.05f;
	float jumpForce = 0.04f;

	float cameraRotationY = 0f;
	[SerializeField] float velocityX = 0;
	[SerializeField] float velocityY = 0;
	[SerializeField] float velocityZ = 0;

	[SerializeField] Camera camera;

	void Start()
	{
		Cursor.lockState = CursorLockMode.Locked;
	}

	void Update()
	{
		CameraControl();

		CheckActionInputs();

		CalculateVelocity();
		ApplyVelocity();
	}

	void CameraControl()
	{
		float lookX = Input.GetAxisRaw("Mouse X") * lookSpeed;
		float lookY = Input.GetAxisRaw("Mouse Y") * lookSpeed;

		cameraRotationY -= lookY;
		cameraRotationY = Mathf.Clamp(cameraRotationY, -90, 90);

		transform.Rotate(Vector3.up, lookX);
		camera.transform.localEulerAngles = new Vector3(cameraRotationY, 0, 0);
	}

	void CheckActionInputs()
	{
		if (GetComponent<CharacterController>().isGrounded && Input.GetButtonDown("Jump"))
		{
			velocityY = jumpForce;
		}
	}

	void CalculateVelocity()
	{
		// Calculate and Apply Acceleration
		Vector3 accelX = camera.transform.right * Input.GetAxisRaw("Horizontal") * moveAccel * Time.deltaTime;
		Vector3 accelZ = camera.transform.forward * Input.GetAxisRaw("Vertical") * moveAccel * Time.deltaTime;
		Vector3 accelVector = accelX + accelZ;

		velocityX = Mathf.Lerp(velocityX, accelVector.x, accelerationFactor);
		velocityZ = Mathf.Lerp(velocityZ, accelVector.z, accelerationFactor);

		// Apply Gravity
		if (!GetComponent<CharacterController>().isGrounded)
		{
			velocityY -= gravity * Time.deltaTime;
		}
	}

	void ApplyVelocity()
	{
		Vector3 velocity = new Vector3(velocityX, velocityY, velocityZ);
		GetComponent<CharacterController>().Move(velocity);
	}
}
