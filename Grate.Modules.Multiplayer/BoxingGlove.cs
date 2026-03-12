using UnityEngine;

namespace Grate.Modules.Multiplayer;

public class BoxingGlove : MonoBehaviour
{
	public static int uuid;

	public AudioSource punchSound;

	public VRRig rig;

	public GorillaVelocityEstimator velocity;

	private void Start()
	{
		punchSound = ((Component)this).GetComponent<AudioSource>();
		velocity = ((Component)this).gameObject.AddComponent<GorillaVelocityEstimator>();
	}
}
