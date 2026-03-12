using System;
using System.Collections.Generic;

namespace Grate.Extensions;

public static class MathExtensions
{
	private static readonly Random rng = new Random();

	public static int Wrap(int x, int min, int max)
	{
		int num = max - min;
		int num2 = (x - min) % num;
		if (num2 < 0)
		{
			num2 += num;
		}
		return num2 + min;
	}

	public static float Map(float x, float a1, float a2, float b1, float b2)
	{
		float num = a2 - a1;
		float num2 = b2 - b1;
		float num3 = (x - a1) / num;
		return b1 + num3 * num2;
	}

	public static void Shuffle<T>(this IList<T> list)
	{
		int num = list.Count;
		while (num > 1)
		{
			num--;
			int index = rng.Next(num + 1);
			T value = list[index];
			list[index] = list[num];
			list[num] = value;
		}
	}
}
