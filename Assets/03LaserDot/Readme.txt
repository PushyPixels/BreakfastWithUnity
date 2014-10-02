The following are notes on what I've improved on this part of the project after the original episode aired.  Included is a brief description of what I actually changed and how I changed it.

Lowered scene brightness for better contrast with laser
	Simply click Directional Light, I changed intensity from 0.5f to 0.1f

Changed settings on Laser Dot particle system so it leaves a trail
	Set the Simulation Space to World
	Lowered Start Lifetime to minimum value
	
Changed ProjectLaser.cs so that the laser doesn't get "Stuck" if your raycast misses.  Additional notes in ProjectLaser.cs comments.