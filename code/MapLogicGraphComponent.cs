using Sandbox;
using Sandbox.ActionGraphs;
using Editor;
using System;

public sealed class MapLogicGraphComponent : Component
{
	[Property] public Action<GameObject, GameObject> OnButtonPressed { get; set; }

	[Property] public GameObject ToUse { get; set; }

	protected override void OnUpdate()
	{

	}
}
