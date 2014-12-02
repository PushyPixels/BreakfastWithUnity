var camera1 : Camera;
var camera2 : Camera;
var wipeTime = 2.0;
private var inProgress = false;
private var swap = false;

function Update () {
	if (Input.GetKeyDown("up")) {
		DoWipe(TransitionType.Up);
	}
	else if (Input.GetKeyDown("down")) {
		DoWipe(TransitionType.Down);
	}
	else if (Input.GetKeyDown("left")) {
		DoWipe(TransitionType.Left);
	}
	else if (Input.GetKeyDown("right")) {
		DoWipe(TransitionType.Right);
	}
}

function DoWipe (transitionType : TransitionType) {
	if (inProgress) return;
	inProgress = true;
	
	swap = !swap;
	yield ScreenWipe.use.SquishWipe (swap? camera1 : camera2, swap? camera2 : camera1, wipeTime, transitionType);
	
	inProgress = false;
}