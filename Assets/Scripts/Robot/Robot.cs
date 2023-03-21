using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Robot : MonoBehaviour
{
    RubiksCube cube;
	public static readonly byte CLOCKWISE = 1;
	public static readonly byte COUNTERCLOCKWISE = 2;
	

	private string[] pattern = (string[]) RubikData.DEFAULT_PATTERN.Clone();

	float degree = 0.0f;

	void Start()
    {

    }
    void Update()
    {
		RotateDown(CLOCKWISE);
    }




	void Coude()
    {

    }

	void RotateDown(byte direction)
    {
		if (direction == CLOCKWISE)
        {

        }
		else
        {

        }
    }





	public static bool IsCorner(string name)
	{
		return name.Length == 3;
	}

	public static bool IsEdge(string name)
	{
		return name.Length == 2;
	}

	public static bool IsCubyOn(int[] face, int position)
	{
		for (int i = 0; i < face.Length; i++)
		{
			if (position == face[i])
				return true;
		}

		return false;
	}

	protected static void UpdatePositions(string[] pattern, int index, float direction)
	{
		string[] bufferPositions = new string[9];

		int currentPosition = 0;
		for (int i = 2; i > -1; i--)
		{
			for (int j = 0; j < 7; j += 3)
			{
				bufferPositions[currentPosition] = pattern[RubikData.rotationPositions[index, i + j]];

				currentPosition++;
			}
		}

		if (direction == -1.0f)
			Array.Reverse(bufferPositions);

		for (int i = 0; i < 9; i++)
		{
			string cuby = bufferPositions[i];
			int oldPosition = Array.IndexOf(pattern, cuby);
			int newPosition = Array.IndexOf(bufferPositions, cuby);


			if (IsCorner(cuby))
			{
				if (index <= 2 || index >= 6)
				{
					bool a = index == 0 || index == 8;
					bool b = direction == 1.0f;
					bool c = IsCubyOn(RubikData.UP, oldPosition);
					bool d = IsCubyOn(RubikData.UP, newPosition);

					cuby = RotateCorner(bufferPositions[i], a != b != c != d ? RubikData.CLOCKWISE : RubikData.COUNTERCLOCKWISE);
				}
			}
			else if (IsEdge(cuby))
			{
				if (index == 1 || index == 4 || index >= 6)
				{
					cuby = RotateEdge(bufferPositions[i]);
				}
			}
			bufferPositions[i] = cuby;
		}


		for (int i = 0; i < bufferPositions.Length; i++)
			pattern[RubikData.rotationPositions[index, i]] = bufferPositions[i];
	}

	public static string RotateEdge(string cuby)
	{
		return cuby[1].ToString() + cuby[0].ToString();
	}




	public static string RotateCorner(string cuby, byte orientation)
	{
		if (orientation == CLOCKWISE)
			return cuby[2].ToString() + cuby[0].ToString() + cuby[1].ToString();

		return cuby[1].ToString() + cuby[2].ToString() + cuby[0].ToString();
	}



	protected static int FindCuby(string[] pattern, string name)
	{
		string cuby = Array.Find<string>(pattern, (string currentName) =>
		{


			if (name.Length != currentName.Length)
				return false;

			for (int i = 0; i < name.Length; i++)
			{
				if (!currentName.Contains(name[i]))
					return false;
			}

			for (int i = 0; i < currentName.Length; i++)
			{
				if (!name.Contains(currentName[i]))
					return false;
			}


			return true;
		});

		if (cuby == null)
			return -1;

		return Array.IndexOf(pattern, cuby);
	}



}
