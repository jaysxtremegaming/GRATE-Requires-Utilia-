using System;
using GorillaLocomotion;
using GorillaLocomotion.Climbing;
using Grate.Modules.Physics;
using Grate.Tools;
using UnityEngine;

namespace Grate.Modules.Movement;

public class Platform : MonoBehaviour
{
	public GorillaClimbable Climbable;

	private Material cloudMaterial;

	private Collider collider;

	private Transform hand;

	public bool isSticky;

	public bool isActive;

	public bool isLeft;

	private GameObject model;

	private string modelName;

	private ParticleSystem rain;

	private Vector3 scale;

	private float spawnTime;

	private Transform wings;

	public bool Sticky
	{
		set
		{
			isSticky = value;
		}
	}

	public float Scale
	{
		set
		{
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			scale = new Vector3((float)((!isLeft) ? 1 : (-1)), 1f, 1f) * value;
		}
	}

	public string Model
	{
		get
		{
			return modelName;
		}
		set
		{
			modelName = value;
			string text = modelName;
			if (modelName.Contains("cloud"))
			{
				text = "cloud";
			}
			model = ((Component)((Component)this).transform.Find(text)).gameObject;
			((Component)((Component)this).transform.Find("cloud")).gameObject.SetActive(text == "cloud");
			((Component)((Component)this).transform.Find("doug")).gameObject.SetActive(text == "doug");
			((Component)((Component)this).transform.Find("invisible")).gameObject.SetActive(text == "invisible");
			((Component)((Component)this).transform.Find("ice")).gameObject.SetActive(text == "ice");
			collider = (Collider)(object)model.GetComponent<BoxCollider>();
		}
	}

	private void FixedUpdate()
	{
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		if (isActive)
		{
			spawnTime = Time.time;
			float num = Mathf.Clamp((Time.time - spawnTime) / 1f, 0.2f, 1f);
			float num2 = ((modelName == "storm cloud") ? 0.2f : 1f);
			cloudMaterial.color = new Color(num2, num2, num2, Mathf.Lerp(1f, 0f, num));
			if (((Object)model).name == "doug")
			{
				((Component)wings).transform.localRotation = Quaternion.Euler((float)((Time.frameCount % 2 == 0) ? (-30) : 0), 0f, 0f);
			}
		}
	}

	public void Initialize(bool isLeft)
	{
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0141: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			this.isLeft = isLeft;
			((Object)this).name = "Grate Platform " + (isLeft ? "Left" : "Right");
			Scale = 1f;
			foreach (Transform item in ((Component)this).transform)
			{
				((Component)item).gameObject.AddComponent<GorillaSurfaceOverride>().overrideIndex = 110;
				Transform val = ((Component)this).transform.Find("cloud");
				cloudMaterial = ((Component)val).GetComponent<Renderer>().material;
				cloudMaterial.color = new Color(1f, 1f, 1f, 0f);
				rain = ((Component)val).GetComponent<ParticleSystem>();
				wings = ((Component)this).transform.Find("doug/wings");
			}
			Transform val2 = (isLeft ? GTPlayer.Instance.leftHand.controllerTransform : GTPlayer.Instance.rightHand.controllerTransform);
			hand = ((Component)val2).transform;
			Climbable = CreateClimbable();
			((Component)Climbable).transform.SetParent(((Component)this).transform);
			((Component)Climbable).transform.localPosition = Vector3.zero;
			rain.loop = true;
		}
		catch (Exception e)
		{
			Logging.Exception(e);
		}
	}

	public void Activate()
	{
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		isActive = true;
		spawnTime = Time.time;
		((Component)this).transform.position = ((Component)hand).transform.position;
		((Component)this).transform.rotation = ((Component)hand).transform.rotation;
		((Component)this).transform.localScale = scale * GTPlayer.Instance.scale;
		((Component)collider).gameObject.layer = (NoClip.active ? NoClip.layer : 0);
		((Component)collider).gameObject.layer = (NoClip.active ? NoClip.layer : 0);
		collider.enabled = !isSticky;
		((Component)Climbable).gameObject.SetActive(isSticky);
		model.SetActive(true);
		if (modelName == "storm cloud")
		{
			rain.Play();
		}
	}

	public GorillaClimbable CreateClimbable()
	{
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		GameObject obj = GameObject.CreatePrimitive((PrimitiveType)0);
		((Object)obj).name = "Grate Climb Obj";
		obj.AddComponent<GorillaClimbable>();
		obj.layer = LayerMask.NameToLayer("GorillaInteractable");
		obj.GetComponent<Renderer>().enabled = false;
		obj.transform.localScale = Vector3.one * 0.15f;
		return obj.GetComponent<GorillaClimbable>();
	}

	public void Deactivate()
	{
		isActive = false;
		collider.enabled = false;
		((Component)Climbable).gameObject.SetActive(false);
		model.SetActive(false);
	}
}
