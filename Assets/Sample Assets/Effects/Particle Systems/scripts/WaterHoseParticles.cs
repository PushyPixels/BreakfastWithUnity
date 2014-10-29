using UnityEngine;

public class WaterHoseParticles : MonoBehaviour {
	
    ParticleSystem.CollisionEvent[] collisionEvents = new ParticleSystem.CollisionEvent[16];
	
	public static float lastSoundTime;
	public float force = 1;
	
    void OnParticleCollision(GameObject other) {
		
        int safeLength = particleSystem.safeCollisionEventSize;

        if (collisionEvents.Length < safeLength) 
		{
            collisionEvents = new ParticleSystem.CollisionEvent[safeLength];
		}
        
        int numCollisionEvents = particleSystem.GetCollisionEvents(other, collisionEvents);
        int i = 0;

        while (i < numCollisionEvents)
		{
		
			if (Time.time > lastSoundTime + 0.2f)
			{
				lastSoundTime = Time.time;
			}
			
			var col = collisionEvents[i].collider;

			if (col.attachedRigidbody != null)
			{
                Vector3 vel = collisionEvents[i].velocity;
                col.attachedRigidbody.AddForce(vel*force,ForceMode.Impulse);
            }

			other.BroadcastMessage("Extinguish",SendMessageOptions.DontRequireReceiver);

            i++;
        }
    }
}