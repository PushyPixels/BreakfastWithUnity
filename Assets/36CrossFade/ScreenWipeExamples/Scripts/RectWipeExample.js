var camera1 : Camera;
var camera2 : Camera;
var wipeTime = 2.0;
private var inProgress = false;
private var swap = false;

function Update () {
	if (Input.GetKeyDown("up")) {
		DoWipe(ZoomType.Grow);
	}
	else if (Input.GetKeyDown("down")) {
		DoWipe(ZoomType.Shrink);
	}
}

function DoWipe (zoom : ZoomType) {
	if (inProgress) return;
	inProgress = true;
	
	swap = !swap;
	yield ScreenWipe.use.RectWipe (swap? camera1 : camera2, swap? camera2 : camera1, wipeTime, zoom);
	
	inProgress = false;
}