using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;

public class AutonomousAgent : Agent
{
	public Perception flockPerception;
	public AutonomousAgentData data;


	public float wanderAngle { get; set; } = 0;

	// Update is called once per frame
	void Update()
	{
		var gameObjects = perception.GetGameObjects();

		if (gameObjects.Length > 0)
		{
			movement.ApplyForce(Steering.Seek(this, gameObjects[0]) * data.seekWeight);
			movement.ApplyForce(Steering.Flee(this, gameObjects[0]) * data.fleeWeight);
		}

			gameObjects = flockPerception.GetGameObjects();
			if (gameObjects.Length > 0)
			{
				// cohesion
				foreach (var gameObject in gameObjects)
				{
					Debug.DrawLine(transform.position, gameObject.transform.position);
				}
				movement.ApplyForce(Steering.Cohesion(this, gameObjects) * data.cohesionWeight);
				movement.ApplyForce(Steering.Separation(this, gameObjects, data.separationRadius) * data.separationWeight);
				movement.ApplyForce(Steering.Alignment(this, gameObjects) * data.alignmentWeight);
			}


		if (movement.acceleration.sqrMagnitude <= movement.maxForce * 0.1f)
		{
			movement.ApplyForce(Steering.Wander(this));
		}

		transform.position = Utilities.Wrap(transform.position, new Vector3(-20, -20, -20), new Vector3(20, 20, 20));

	}
}
