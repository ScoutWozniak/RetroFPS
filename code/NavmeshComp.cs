using Sandbox;
using System.Threading.Tasks;

public sealed class NavmeshComp : Component
{
	public NavigationMesh mesh;

	protected override void DrawGizmos()
	{
		base.DrawGizmos();

		if ( mesh is not null )
			Gizmo.Draw.LineNavigationMesh( mesh );
	}
	protected override void OnStart()
	{
		_ = GenMesh();
	}

	async Task GenMesh()
	{
		await Task.Frame();
		await Task.Frame();

		mesh = new NavigationMesh();
		mesh.Generate( Scene.PhysicsWorld );

		Log.Info( mesh.Nodes.Count );
	}
}
