using System;

[Serializable]
public struct TestIntVector2
{

	public int x;
	public int y;

	public static readonly TestIntVector2 zero = new TestIntVector2(0, 0);

	public TestIntVector2(int x, int y)
	{
		this.x = x;
		this.y = y;
	}

	public static TestIntVector2 operator +(TestIntVector2 left, TestIntVector2 right)
	{
		return new TestIntVector2(left.x + right.x, left.y + right.y);
	}
}
