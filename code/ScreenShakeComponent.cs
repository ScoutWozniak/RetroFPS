using Sandbox;

public sealed class ScreenShakeComponent : Component
{
	[Property] GameObject eye { get; set; }
	[Property] float Shake { get; set; } = 0.0f;
	[Property] float ShakeAmount { get; set; } = 0.7f;

	[Property] float DecreaseFactor { get; set; } = 1.0f;

	Vector3 OriginalPos;
	Rotation OriginalRot;
	protected override void OnEnabled()
	{
		base.OnEnabled();
		OriginalPos = eye.Transform.LocalPosition;
		OriginalRot = eye.Transform.LocalRotation;
	}

	protected override void OnUpdate()
	{
		if (Shake > 0)
		{
			eye.Transform.LocalPosition = OriginalPos + Game.Random.VectorInSphere() * ShakeAmount;
			eye.Transform.LocalRotation = OriginalRot * new Angles(Game.Random.Float(-ShakeAmount,ShakeAmount), Game.Random.Float(-ShakeAmount,ShakeAmount) ,0).ToRotation();

			Shake -= Time.Delta * DecreaseFactor;
		}
		else
		{
			Shake = 0.0f;
			eye.Transform.LocalPosition = OriginalPos;
			eye.Transform.LocalRotation = OriginalRot;
		}
	}

	public void AddShake(float amount, float length)
	{
		Shake = length;
		ShakeAmount = amount;
	}
}
