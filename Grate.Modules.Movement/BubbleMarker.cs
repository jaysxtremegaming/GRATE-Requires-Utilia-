using Grate.Gestures;
using UnityEngine;

namespace Grate.Modules.Movement;

public class BubbleMarker : MonoBehaviour
{
	private GameObject bubble;

	private void Start()
	{
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		bubble = Object.Instantiate<GameObject>(Bubble.bubblePrefab);
		bubble.transform.SetParent(((Component)this).transform, false);
		bubble.transform.localPosition = new Vector3(0f, -0.1f, 0f);
		bubble.gameObject.layer = GrateInteractor.InteractionLayer;
	}

	private void OnDestroy()
	{
		Object.Destroy((Object)(object)bubble);
	}
}
