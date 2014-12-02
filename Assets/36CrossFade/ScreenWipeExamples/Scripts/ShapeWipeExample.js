var camera1 : Camera;
var camera2 : Camera;
var wipeTime = 2.0;
var rotateAmount = 360.0;
var shapeMesh : Mesh[];
private var inProgress = false;
private var swap = false;
private var useShape = 0;

function Update () {
	if (Input.GetKeyDown("up")) {
		DoWipe(ZoomType.Grow);
	}
	else if (Input.GetKeyDown("down")) {
		DoWipe(ZoomType.Shrink);
	}

	if (Input.GetKeyDown("1")) {useShape = 0;}
	if (Input.GetKeyDown("2")) {useShape = 1;}
	if (Input.GetKeyDown("3")) {useShape = 2;}
}

function DoWipe (zoom : ZoomType) {
	if (inProgress) return;
	inProgress = true;
	
	swap = !swap;
	yield ScreenWipe.use.ShapeWipe (swap? camera1 : camera2, swap? camera2 : camera1, wipeTime, zoom, shapeMesh[useShape], rotateAmount);
	
	inProgress = false;
}