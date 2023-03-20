using System;
using System.Collections.Generic;
using UnityEngine;
using Unity.Jobs;
using Unity.Collections;

public struct SolveJob : IJob
{
	private string[] pattern;
	private string[] objectivePattern;
	private List<Order> orderList;
	private int cubeId;

	public SolveJob(string strPattern, string strObjectivePattern, int cubeId)
	{
		pattern = JsonUtility.FromJson<JSONPattern>(strPattern).Pattern;
		objectivePattern = JsonUtility.FromJson<JSONPattern>(strObjectivePattern).Pattern;
		this.cubeId = cubeId;

		orderList = new List<Order>();
	}


	public void Execute()
	{
		for (int counter = 0; counter <= 5; counter++)
		{
			if (Search(counter))
			{
				RubiksCube.FinalizeSolve(orderList, cubeId);
				return;
			}
		}

		RubiksCube.FinalizeSolve(null, cubeId);	
	}



	private bool Search(int counter)
	{
		if (counter == 0)
		{
			return IsPatternFound();
		}

		counter--;

		for (int index = 0; index <= 8; index++)
		{
			for (float direction = -1.0f; direction <= 1.0f; direction += 2.0f)
			{
				Rotate(index, direction);
				
				if (Search(counter))
					return true;

				Rotate(index, -direction);
				orderList.RemoveAt(orderList.Count - 1);
				orderList.RemoveAt(orderList.Count - 1);
			}
		}

		return false;
	}



	private bool IsPatternFound()
	{
		bool isEqual = true;
		
		for (int i = 0; i < objectivePattern.Length; i++)
		{
			if (!objectivePattern[i].Equals(pattern[i]))
			{
				isEqual = false;
				break;
			}
		}

		return isEqual;
	}











	private void Rotate(int index, float direction)
	{
		UpdatePositions(index, direction);
		orderList.Add(new RotateOrder(index, direction));
	}









	private void UpdatePositions(int index, float direction)
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


			if (RubiksCube.IsCorner(cuby))
            		{
				if (index <= 2 || index >= 6)
				{
					bool a = index == 0 || index == 8;
					bool b = direction == 1.0f;
					bool c = RubiksCube.IsCubyOn(RubikData.UP, oldPosition);
					bool d = RubiksCube.IsCubyOn(RubikData.UP, newPosition);

					cuby = RubiksCube.RotateCorner(bufferPositions[i], a != b != c != d ? RubikData.CLOCKWISE: RubikData.COUNTERCLOCKWISE);
				}
			}
			else if (RubiksCube.IsEdge(cuby))
            		{
				if (index == 1 || index == 4 || index >= 6)
                		{
					cuby = RubiksCube.RotateEdge(bufferPositions[i]);
                		}
            		}
			bufferPositions[i] = cuby;
		}
		

		for (int i = 0; i < bufferPositions.Length; i++)
			pattern[RubikData.rotationPositions[index, i]] = bufferPositions[i];
	}






	






}
