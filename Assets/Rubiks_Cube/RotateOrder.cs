using System;

public class RotateOrder : Order
{
	public int Index { get; }
	public float Direction { get; }

	public RotateOrder(int index, float direction)
	{
		if (Math.Abs(direction) != 1.0f)
			throw new ArgumentException($"Direction \"{direction}\" must be -1.0f or 1.0f.");

		if (index < 0 || index > 8)
			throw new ArgumentException($"index \"{index}\" must be between 0 and 8.");

		Index = index;
		Direction = direction;		
	}

}
