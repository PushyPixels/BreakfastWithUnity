private var motor : CharacterMotor;
public var horizontalAxisName : String = "Horizontal";
public var verticalAxisName : String = "Vertical";
public var mobileHack : boolean = false;
public var directionVector : Vector3;

// Use this for initialization
function Awake () {
	motor = GetComponent(CharacterMotor);
}

// Update is called once per frame
function Update () {
	// Get the input vector from keyboard or analog stick
	
	if(!mobileHack)
	{
		directionVector = new Vector3(Input.GetAxis(horizontalAxisName), 0, Input.GetAxis(verticalAxisName));
	}
	
	if (directionVector != Vector3.zero) {
		// Get the length of the directon vector and then normalize it
		// Dividing by the length is cheaper than normalizing when we already have the length anyway
		var directionLength = directionVector.magnitude;
		directionVector = directionVector / directionLength;
		
		// Make sure the length is no bigger than 1
		directionLength = Mathf.Min(1, directionLength);
		
		// Make the input vector more sensitive towards the extremes and less sensitive in the middle
		// This makes it easier to control slow speeds when using analog sticks
		directionLength = directionLength * directionLength;
		
		// Multiply the normalized direction vector by the modified length
		directionVector = directionVector * directionLength;
	}
	
	// Apply the direction to the CharacterMotor
	motor.inputMoveDirection = transform.rotation * directionVector;
	motor.inputJump = Input.GetButton("Jump");
}

public function MoveForward()
{
	directionVector = Vector3.forward;
}

public function MoveBackward()
{
	directionVector = Vector3.back;
}

public function Stop()
{
	directionVector = Vector3.zero;
}


// Require a character controller to be attached to the same game object
@script RequireComponent (CharacterMotor)
@script AddComponentMenu ("Character/FPS Input Controller")
