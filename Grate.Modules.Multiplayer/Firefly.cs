using System;
using System.Linq;
using GorillaLocomotion;
using Grate.Extensions;
using Grate.Tools;
using UnityEngine;

namespace Grate.Modules.Multiplayer;

public class Firefly : MonoBehaviour
{
	public static float duration = 1.5f;

	public GameObject fly;

	public Transform hand;

	public Transform leftWing;

	public Transform rightWing;

	private Renderer modelRenderer;

	public ParticleSystemRenderer particleRenderer;

	public ParticleSystemRenderer trailRenderer;

	public ParticleSystem particles;

	public ParticleSystem trail;

	public VRRig rig;

	public bool seek;

	private Vector3 startPos;

	public float startTime;

	private void Awake()
	{
		try
		{
			rig = ((Component)this).gameObject.GetComponent<VRRig>();
			fly = Object.Instantiate<GameObject>(Plugin.AssetBundle.LoadAsset<GameObject>("Firefly")).gameObject;
			modelRenderer = ((Component)fly.transform.Find("Model")).GetComponent<Renderer>();
			leftWing = fly.transform.Find("Model/Wing L");
			rightWing = fly.transform.Find("Model/Wing R");
			particles = ((Component)fly.transform.Find("Particles")).GetComponent<ParticleSystem>();
			trail = ((Component)fly.transform.Find("Trail")).GetComponent<ParticleSystem>();
			particleRenderer = ((Component)particles).GetComponent<ParticleSystemRenderer>();
			((Renderer)particleRenderer).material = Object.Instantiate<Material>(((Renderer)particleRenderer).material);
			trailRenderer = ((Component)trail).GetComponent<ParticleSystemRenderer>();
			trailRenderer.trailMaterial = Object.Instantiate<Material>(trailRenderer.trailMaterial);
			particles.Play();
			trail.Play();
			startTime = Time.time;
		}
		catch (Exception e)
		{
			Logging.Exception(e);
		}
	}

	public void Reset(VRRig rig, Transform hand)
	{
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		particles.Stop();
		trail.Stop();
		particles.Clear();
		trail.Clear();
		this.rig = rig;
		this.hand = hand;
		seek = false;
		fly.transform.localScale = Vector3.one * GTPlayer.Instance.scale;
		fly.transform.position = hand.position;
	}

	private void FixedUpdate()
	{
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0102: Unknown result type (might be due to invalid IL or missing references)
		//IL_0109: Unknown result type (might be due to invalid IL or missing references)
		//IL_010e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0118: Unknown result type (might be due to invalid IL or missing references)
		//IL_0128: Unknown result type (might be due to invalid IL or missing references)
		//IL_012d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0132: Unknown result type (might be due to invalid IL or missing references)
		//IL_013e: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0202: Unknown result type (might be due to invalid IL or missing references)
		//IL_0212: Unknown result type (might be due to invalid IL or missing references)
		//IL_0217: Unknown result type (might be due to invalid IL or missing references)
		//IL_0224: Unknown result type (might be due to invalid IL or missing references)
		//IL_0225: Unknown result type (might be due to invalid IL or missing references)
		//IL_0227: Unknown result type (might be due to invalid IL or missing references)
		//IL_023c: Unknown result type (might be due to invalid IL or missing references)
		//IL_024c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0178: Unknown result type (might be due to invalid IL or missing references)
		//IL_017d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0180: Unknown result type (might be due to invalid IL or missing references)
		//IL_0195: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c0: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (!NetworkSystem.Instance.PlayerListOthers.Contains(rig.OwningNetPlayer) || !((Behaviour)Fireflies.instance).enabled || !NetworkSystem.Instance.InRoom)
			{
				fly.Obliterate();
				Fireflies.fireflies.Remove(this);
				((Component)(object)this).Obliterate();
				return;
			}
			int num = ((Time.frameCount % 2 == 0) ? 30 : 0);
			((Component)leftWing).transform.localRotation = Quaternion.Euler(0f, (float)(-num), 0f);
			((Component)rightWing).transform.localRotation = Quaternion.Euler(0f, (float)num, 0f);
			VRRig obj = rig;
			Transform val = ((obj != null) ? ((Component)obj).transform : null);
			if (!((Object)(object)val != (Object)null))
			{
				return;
			}
			Color playerColor = rig.playerColor;
			modelRenderer.materials[1].color = playerColor;
			((Renderer)particleRenderer).material.color = playerColor;
			trailRenderer.trailMaterial.color = playerColor;
			Vector3 val2 = val.position + Vector3.up * 0.4f * rig.scaleFactor;
			fly.transform.LookAt(val2);
			if (seek)
			{
				float num2 = (Time.time - startTime) / duration;
				if (num2 < 1f)
				{
					fly.transform.position = Vector3.Slerp(startPos, val2, num2);
					fly.transform.localScale = Vector3.Lerp(Vector3.one * GTPlayer.Instance.scale, Vector3.one * rig.scaleFactor, num2);
					return;
				}
				float num3 = Time.time * 5f % (MathF.PI * 2f);
				float num4 = Mathf.Cos(num3);
				float num5 = Mathf.Sin(num3);
				Vector3 val3 = new Vector3(num4, 0f, num5) * 0.2f * rig.scaleFactor;
				fly.transform.position = val2 + val3;
				fly.transform.localScale = Vector3.one * rig.scaleFactor;
			}
		}
		catch (Exception e)
		{
			Logging.Exception(e);
		}
	}

	private void OnDestroy()
	{
		fly?.Obliterate();
	}

	public void Launch()
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		startTime = Time.time;
		startPos = fly.transform.position;
		seek = true;
		particles.Play();
		trail.Play();
	}
}
