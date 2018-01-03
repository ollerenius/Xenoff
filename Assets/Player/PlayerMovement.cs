using UnityEngine;

[RequireComponent(typeof(CustomThirdPersonCharacter))]
public class PlayerMovement : MonoBehaviour {

	CustomThirdPersonCharacter character; // A reference to the ThirdPersonCharacter on the object
	Transform mainCamera;                 // A reference to the main camera in the scenes transform
	Vector3 camForward;             	  // The current forward direction of the camera
	Vector3 move;					  	  // the world-relative desired move direction, calculated from the camForward and user input.
	bool jump;                      

	bool isSprinting;
	CameraRaycaster cameraRaycaster;

	void Start () {
		// get the transform of the main camera
		if (Camera.main != null) {
			mainCamera = Camera.main.transform;
			cameraRaycaster = mainCamera.GetComponent<CameraRaycaster>();
		} else {
			Debug.LogWarning(
				"Warning: no main camera found. Third person character needs a Camera tagged \"MainCamera\", for camera-relative controls.", gameObject);
			// we use self-relative controls in this case, which probably isn't what the user wants, but hey, we warned them!
		}

		// get the third person character ( this should never be null due to require component )
		character = GetComponent<CustomThirdPersonCharacter>();

		cameraRaycaster.notifyMouseClickObservers += ProcessMouseClick;
	}

	// TODO: Consider moving this function (and functionality) to a "PlayerController" script or such.
	void ProcessMouseClick(RaycastHit raycastHit, int layerHit) {
		Debug.Log("Click!");
	}

	void Update () {
		if (!jump) {
			jump = Input.GetButtonDown("Jump");
		}
	}

	// Fixed update is called in sync with physics
	void FixedUpdate() {
		// read inputs
		float h = Input.GetAxis("Horizontal");
		float v = Input.GetAxis("Vertical");
		bool crouch = Input.GetKey(KeyCode.C);

		if (Input.GetKeyUp(KeyCode.LeftShift)) {
			isSprinting = !isSprinting;
		}

		// calculate move direction to pass to character
		if (mainCamera != null) {
			// calculate camera relative direction to move:
			camForward = Vector3.Scale(mainCamera.forward, new Vector3(1, 0, 1)).normalized;
			move = v * camForward + h * mainCamera.right;
		} else {
			// we use world-relative directions in the case of no main camera
			move = v * Vector3.forward + h * Vector3.right;
		}

		// pass all parameters to the character control script
		character.Move(move, crouch, jump, isSprinting);
		jump = false;
	}
}
