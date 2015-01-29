/*
This camera smoothes out rotation around the y-axis and height.
Horizontal Distance to the target is always fixed.

There are many different ways to smooth the rotation but doing it this way gives you a lot of control over how the camera behaves.

For every of those smoothed values we calculate the wanted value and the current value.
Then we smooth it using the Lerp function.
Then we apply the smoothed values to the transform's position.
*/

// The target we are following
var target : Transform;
// The distance in the x-z plane to the target
var distance = 10.0;
// the height we want the camera to be above the target
var height = 5.0;
// How much we 
var heightDamping = 2.0;
var rotationDamping = 3.0;
var leftX=-4f;
var downY=-6.4f;
// Place the script in the Camera-Control group in the component menu
@script AddComponentMenu("Camera-Control/Smooth Follow")


function LateUpdate () {
	// Early out if we don't have a target
	if (!target)
		return;
	
	// Calculate the current rotation angles
//	var wantedRotationAngle = target.eulerAngles.y;
	var wantedHeight = target.position.y + height;
	var wantedx = target.position.x;
		
//	var currentRotationAngle = transform.eulerAngles.y;
	var currentHeight = transform.position.y;
	var currentx = transform.position.x;
	
	// Damp the rotation around the y-axis
//	currentRotationAngle = Mathf.LerpAngle (currentRotationAngle, wantedRotationAngle, rotationDamping * Time.deltaTime);

	// Damp the height
	currentHeight = Mathf.Lerp (currentHeight, wantedHeight, heightDamping * Time.deltaTime);
	currentx = Mathf.Lerp (currentx, wantedx, heightDamping * Time.deltaTime);

	// Convert the angle into a rotation
//	var currentRotation = Quaternion.Euler (0, currentRotationAngle, 0);
	
	// Set the position of the camera on the x-z plane to:
	// distance meters behind the target
//	transform.position = target.position;
//	transform.position -= currentRotation * Vector3.forward * distance;

	// Set the height of the camera
	if((transform.position.y>downY||target.transform.position.y>downY)&&(transform.position.y<-downY||target.transform.position.y<-downY))
	transform.position.y = currentHeight;
	if((transform.position.x>leftX||target.transform.position.x>leftX)&(transform.position.x<-leftX||target.transform.position.x<-leftX))
	transform.position.x = currentx;
//	transform.position.z = currentx;
	
	// Always look at the target
//	transform.LookAt (target);
}