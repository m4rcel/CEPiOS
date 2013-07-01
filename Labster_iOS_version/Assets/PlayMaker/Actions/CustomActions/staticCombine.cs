using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.UnityObject)]
	[Tooltip("Sets all child objects of a parent to batching if they can CAUTION! object can not move once in batching only deleted.")]
	public class staticCombine : FsmStateAction
	{
		public FsmGameObject root;
		
		public override void Reset()
		{
			root = null;
		}
		
		public override void OnEnter()
		{		
			 	StaticBatchingUtility.Combine(root.Value);
				Finish ();
		}
	}
}