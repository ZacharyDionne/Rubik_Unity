using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class RubiksCube : MonoBehaviour
{


	//-----------------------------------------------------PUBLIC STATIC----------------------------------------------------------------//
	public static RubiksCube GenerateCube(string[] pattern)
	{
		RubiksCube cube = Instantiate(Resources.Load<GameObject>("Rubik's Cube")).GetComponent<RubiksCube>();
		cube.currentPattern = pattern;


		for (int i = 0; i < RubikData.DEFAULT_PATTERN.Length; i++)
        	{
			Transform cuby = cube.transform.Find(RubikData.DEFAULT_PATTERN[FindCuby(RubikData.DEFAULT_PATTERN, pattern[i])]);
			cuby.position = RubikData.worldPositions[i];

			cube.cubyPositions[i] = cuby;

			
			int defaultPosition = FindCuby(RubikData.DEFAULT_PATTERN, pattern[i]);

			switch (pattern[i].Length)
            		{
				case 1:
					cuby.localRotation = Quaternion.Euler(RubikData.middlesFrame[defaultPosition][i]);
						
					break;

				case 2:
					
					cuby.Rotate(RubikData.edgesFrame[defaultPosition] - RubikData.edgesFrame[i]);
					cube.PhysicRotateEdge(pattern[i], cuby);
			
					if (
						defaultPosition == 9 && i == 15 ||
						defaultPosition == 15 && i == 9 ||
						defaultPosition == 11 && i == 17 ||
						defaultPosition == 17 && i == 11)
					cuby.localRotation *= Quaternion.Euler(180.0f, 180.0f, 0.0f);

					
					break;

				case 3:
					cube.CorrectCornerOrientation(cuby);
					cube.PhysicRotateCorner(pattern[i], cuby);

					break;
            		}
			


        	}

		return cube;
    	}











	//-----------------------------------------PROTECTED STATIC-----------------------------------//
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






	protected static string[] GetCubiesOn(string[] pattern, int[] face)
    	{
		string[] cubies = new string[9];

		for (int i = 0; i < face.Length; i++)
        	{
			cubies[i] = pattern[i];
        	}

		return cubies;
    	}



	/*
		Effectue une rotation dans le nom d'un cuby.
	*/
	protected static void RotateCorner(Transform cuby, byte orientation)
	{
		if (orientation == RubikData.CLOCKWISE)
			cuby.name = cuby.name[2].ToString() + cuby.name[0].ToString() + cuby.name[1].ToString();
		else
			cuby.name = cuby.name[1].ToString() + cuby.name[2].ToString() + cuby.name[0].ToString();
	}

	protected static string RotateCorner(string cuby, byte orientation)
	{
		if (orientation == RubikData.CLOCKWISE)
			return cuby[2].ToString() + cuby[0].ToString() + cuby[1].ToString();

		return cuby[1].ToString() + cuby[2].ToString() + cuby[0].ToString();
	}


	protected static void RotateEdge(Transform cuby)
	{
		cuby.name = cuby.name[1].ToString() + cuby.name[0].ToString();
	}

	protected static string RotateEdge(string cuby)
	{
		return cuby[1].ToString() + cuby[0].ToString();
	}




	protected static bool IsCubyOn(int[] face, int position)
    	{
		for (int i = 0; i < face.Length; i++)
		{
			if (position == face[i])
				return true;
		}

		return false;
	}

	protected static bool IsCorner(string name)
	{
		return name.Length == 3;
	}
	
	protected static bool IsEdge(string name)
	{
		return name.Length == 2;
	}

	protected static bool IsMiddle(string name)
	{
		return name.Length == 1;
	}

	protected int[] GetCubyFaceFromColor(string cuby, int position, char color)
    {
		return RubikData.colorFaceMap[position][cuby.IndexOf(color)];
    }







	
	//Permet de garder l'etat interne du cube sans effectuer de reelles rotations
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

					cuby = RotateCorner(bufferPositions[i], a != b != c != d ? RubikData.CLOCKWISE: RubikData.COUNTERCLOCKWISE);
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





















	//-------------------PUBLIC ATTRIBUTS---------------------------//





	//----------------PROTECTED ATTRIBUTS---------------------------------//
	
	//variable de controle
	protected bool isRotating;

	//variables pour l'animation de rotation
	protected int angleCounter;
	protected const float ROTATION_SLOWNESS = 30.0f;
	protected const float ROTATION = 180.0f / ROTATION_SLOWNESS;
	protected const float ROTATION_STEP = ROTATION_SLOWNESS / 2.0f;


	// -1.0f ou 1.0f, definisse dans quelle direction tourner
	protected float direction;


	//Variable utilisee pour melanger le cube
	protected int randomCounter;
	protected System.Random random;


	//Stocke l'information avant de melanger
	protected int lastIndex;
	protected float lastDirection;

	//Queue des actions que le cube doit faire
	protected Queue<Order> orderQueue;
	protected Order currentOrder;
	


	protected Vector3 currentAxis;


	//variables pour garder a l'interne l'etat du cube
	protected Transform[] cubyPositions;
	protected Transform[] bufferPositions;



	protected string[] currentPattern;
	


	//Entre 0 et 8, indique quel partie du cube est selectionnee pour tourner
	protected int index;











	//----------------------PUBLIC METHODES------------------------------------------//

	public override string ToString()
	{
		return GetJSONPattern();
	}




	public void Rotate(int index, float direction)
	{
		orderQueue.Enqueue(new RotateOrder(index, direction));
	}


    	public void Solve(string[] objectivePattern)
    	{
		orderQueue.Enqueue(new SolveOrder(objectivePattern));
	}

	
	public string GetJSONPattern()
    	{
		string json = "[";

		for (int i = 0; i < currentPattern.Length; i++)
        	{
			json += "\"" + currentPattern[i] + "\"";
			if (i + 1 != currentPattern.Length)
            		{
				json += ",";
            		}
        	}

		json += "]";

		return json;
    	}
	


	public void Randomize()
    	{
		orderQueue.Enqueue(new RandomizeOrder());
	}














	void Update()
	{
		if (!isRotating)
		{
			if (Input.GetKeyDown(KeyCode.A))
			{
				orderQueue.Enqueue(new RotateOrder(index, 1.0f));
			}
			if (Input.GetKeyDown(KeyCode.D))
			{
				orderQueue.Enqueue(new RotateOrder(index, -1.0f));
			}
			else if (Input.GetKeyDown(KeyCode.LeftArrow))
			{
				if (index != 0)
				{
					index -= 1;
				}
			}
			else if (Input.GetKeyDown(KeyCode.RightArrow))
			{
				if (index != 8)
				{
					index += 1;
				}
			}
			
		}
	}




	void FixedUpdate()
    	{
		if (isRotating)
			RotateAnimation();
		else if (currentOrder is RotateOrder)
		{
			currentOrder = null;
		}	
		else if (randomCounter > 0)
			NextRandom();
		else if (randomCounter == 0)
        	{
			direction = lastDirection;
			index = lastIndex;
			randomCounter = -1;

			
			if (currentOrder is RandomizeOrder)
			{
				currentOrder = null;
			}
			
			
		}

		NextOrder();
	}


    	void Awake()
    	{
		isRotating = false;
		index = 0;
		randomCounter = -1;

		bufferPositions = new Transform[9];
		random = new System.Random();
		cubyPositions = new Transform[27];
		orderQueue = new Queue<Order>();
	}










	protected void NextOrder()
	{
		if (currentOrder != null || orderQueue.Count == 0)
			return;

		currentOrder = orderQueue.Dequeue();

		if (currentOrder is RandomizeOrder)
		{
			StartRandomize();
		}
		else if (currentOrder is RotateOrder)
		{
			RotateOrder rotateOrder = (RotateOrder) currentOrder;

			index = rotateOrder.Index;
			direction = rotateOrder.Direction;

			RotateBegin();
		}
		else if (currentOrder is SolveOrder)
		{
			SolveOrder solveOrder = (SolveOrder) currentOrder;

			StartSolve(solveOrder.Pattern);
		}
	}






	protected void StartRandomize()
	{
		randomCounter = 20;
		lastDirection = direction;
		lastIndex = index;

		NextRandom();
	}

	protected void StartSolve(string[] objectivePattern)
	{
		string[] pattern = (string[]) currentPattern.Clone();
		List<Order> orderList = new List<Order>();
		

		//Positionnement des milieux
		if (
			IsCubyOn(RubikData.FRONT, FindCuby(pattern, objectivePattern[RubikData.UP[4]])) ||
			IsCubyOn(RubikData.BACK, FindCuby(pattern, objectivePattern[RubikData.UP[4]])) ||
			IsCubyOn(RubikData.DOWN, FindCuby(pattern, objectivePattern[RubikData.UP[4]])))
		{
			RotateUntilEqual(7, RubikData.UP, 4, orderList, pattern, objectivePattern);
		}
		else if (IsCubyOn(RubikData.LEFT, FindCuby(pattern, objectivePattern[RubikData.UP[4]])))
		{
			UpdatePositions(pattern, 1, -1.0f);
			orderList.Add(new RotateOrder(1, -1.0f));
		}
		else if (IsCubyOn(RubikData.RIGHT, FindCuby(pattern, objectivePattern[RubikData.UP[4]])))
		{
			UpdatePositions(pattern, 1, 1.0f);
			orderList.Add(new RotateOrder(1, 1.0f));
		}
		RotateUntilEqual(4, RubikData.FRONT, 4, orderList, pattern, objectivePattern);



		


		//Faire la croix du dessus
		int[] topCross = { 1, 9, 11, 19 };
		for (int i = 0; i < topCross.Length; i++)
		{
			int position = topCross[i];
			string cubyRef = pattern[topCross[i]];

			if (pattern[position].Equals(objectivePattern[position]))
				continue;
			
			//Si sur UP, descendre
			if (IsCubyOn(RubikData.UP, FindCuby(pattern, objectivePattern[position])))
			{
				int index = Array.IndexOf(RubikData.indexFaceMap, RubikData.TOUCHING_FACES[FindCuby(pattern, objectivePattern[position])][1]);
				int refPosition = FindCuby(pattern, cubyRef);
				UpdatePositions(pattern, index, -1.0f);
				orderList.Add(new RotateOrder(index, -1.0f));
				cubyRef = pattern[refPosition];
			}//Si sur la face du dessus, il faut aligner les references, monter et puis mettre a jour la reference
			else if (IsCubyOn(RubikData.DOWN, FindCuby(pattern, objectivePattern[position])))
			{
				int counter3 = 0;
				for (;
					RubikData.TOUCHING_FACES[FindCuby(pattern, objectivePattern[position])][1] != RubikData.TOUCHING_FACES[FindCuby(pattern, cubyRef)][1];
					counter3++)
				{
					if (counter3 == 3)
						throw new Exception("Maximum of 3 rotations reached");

					UpdatePositions(pattern, 3, 1.0f);
					orderList.Add(new RotateOrder(3, 1.0f));
				}
				if (counter3 == 3)
				{
					orderList.RemoveAt(orderList.Count - 1);
					orderList.RemoveAt(orderList.Count - 1);
					orderList.RemoveAt(orderList.Count - 1);
					orderList.Add(new RotateOrder(3, -1.0f));
				}
				
				int index = Array.IndexOf(RubikData.indexFaceMap, RubikData.TOUCHING_FACES[FindCuby(pattern, objectivePattern[position])][1]);
				UpdatePositions(pattern, index, 1.0f);
				orderList.Add(new RotateOrder(index, 1.0f));

				cubyRef = pattern[RubikData.rotationPositions[index, 1]];
			}
			


			
			//rotate Top Until Reference On Cuby SideFace
			int counter = 0;
			while (
				!(
					pattern[FindCuby(pattern, objectivePattern[position])][0].ToString().Equals(objectivePattern[RubikData.UP[4]]) ?
					RubikData.TOUCHING_FACES[FindCuby(pattern, objectivePattern[position])][1]:
					RubikData.TOUCHING_FACES[FindCuby(pattern, objectivePattern[position])][0]
				).Equals(RubikData.TOUCHING_FACES[FindCuby(pattern, cubyRef)][1])
			)
			{
				UpdatePositions(pattern, 3, 1.0f);
				orderList.Add(new RotateOrder(3, 1.0f));
				
				if (++counter > 3)
					throw new Exception("counter greater than 3, Side");
			}
			if (counter == 3)
			{
				orderList.RemoveAt(orderList.Count - 1);
				orderList.RemoveAt(orderList.Count - 1);
				orderList.RemoveAt(orderList.Count - 1);
				orderList.Add(new RotateOrder(3, -1.0f));
			}

			
			//go up until in on UP
			counter = 0;
			int index2 = Array.IndexOf(RubikData.indexFaceMap, RubikData.TOUCHING_FACES[FindCuby(pattern, cubyRef)][1]);
			while (!IsCubyOn(RubikData.UP, FindCuby(pattern, objectivePattern[position])))
			{
				UpdatePositions(pattern, index2, 1.0f);
				orderList.Add(new RotateOrder(index2, 1.0f));
			
				if (++counter > 3)
					throw new Exception("counter greater than 3, UP");
			}
			if (counter == 3)
			{
				orderList.RemoveAt(orderList.Count - 1);
				orderList.RemoveAt(orderList.Count - 1);
				orderList.RemoveAt(orderList.Count - 1);
				orderList.Add(new RotateOrder(index2, -1.0f));
			}
			


			//replacer le UP pour le prochain tour
			counter = 0;
			while (
				!pattern[FindCuby(pattern, objectivePattern[position])][1].ToString()
				.Equals(
				pattern[RubikData.TOUCHING_FACES[FindCuby(pattern, objectivePattern[position])][1][4]]))
			{
				UpdatePositions(pattern, 3, 1.0f);
				orderList.Add(new RotateOrder(3, 1.0f));

				if (++counter > 3)
					throw new Exception("counter greater than 3 while going UP for top cross");
			}
			if (counter == 3)
			{
				orderList.RemoveAt(orderList.Count - 1);
				orderList.RemoveAt(orderList.Count - 1);
				orderList.RemoveAt(orderList.Count - 1);
				orderList.Add(new RotateOrder(3, -1.0f));
			}
		}





		//Faire la face du dessus
		foreach (int position in new int[]{ 0, 2, 18, 20 })
		{
			if (pattern[position].Equals(objectivePattern[position]))
				continue;

			//Si sur le dessus, descendre
			if (IsCubyOn(RubikData.UP, FindCuby(pattern, objectivePattern[position])))
			{
				int index = Array.IndexOf(RubikData.indexFaceMap, RubikData.TOUCHING_FACES[FindCuby(pattern, objectivePattern[position])][1]);
				float firstDirection = 1.0f;
				UpdatePositions(pattern, index, 1.0f);
				orderList.Add(new RotateOrder(index, 1.0f));

				if (!IsCubyOn(RubikData.DOWN, FindCuby(pattern, objectivePattern[position])))
				{
					UpdatePositions(pattern, index, -1.0f);
					UpdatePositions(pattern, index, -1.0f);
					orderList.RemoveAt(orderList.Count - 1);
					orderList.Add(new RotateOrder(index, -1.0f));
					firstDirection = -1.0f;
				}

				
				//sauvegarder la face utilisé et la direction
				int faceIndex = index;


				//on deplace le bas
				UpdatePositions(pattern, 5, 1.0f);
				orderList.Add(new RotateOrder(5, 1.0f));

				//correction si il se trompe de bord
				if (
					Array.IndexOf(RubikData.indexFaceMap, RubikData.TOUCHING_FACES[FindCuby(pattern, objectivePattern[position])][0]) == faceIndex ||
					Array.IndexOf(RubikData.indexFaceMap, RubikData.TOUCHING_FACES[FindCuby(pattern, objectivePattern[position])][1]) == faceIndex
				)
				{
					UpdatePositions(pattern, 5, -1.0f);
					UpdatePositions(pattern, 5, -1.0f);
					orderList.RemoveAt(orderList.Count - 1);
					orderList.Add(new RotateOrder(5, -1.0f));
					direction = -1.0f;
				}

				//remonter la face
				UpdatePositions(pattern, index, -firstDirection);
				orderList.Add(new RotateOrder(index, -firstDirection));
			}
			

			//Si la couleur up du cuby est sur le dessous
			if (pattern[FindCuby(pattern, objectivePattern[position])][0].ToString().Equals(objectivePattern[RubikData.UP[4]]))
			{
				int index = Array.IndexOf(RubikData.indexFaceMap, RubikData.TOUCHING_FACES[FindCuby(pattern, objectivePattern[position])][1]);
				float oldDirection = 1.0f;
				int[][] oldFaces = RubikData.TOUCHING_FACES[FindCuby(pattern, objectivePattern[position])];

				//on descend
				UpdatePositions(pattern, index, oldDirection);
				orderList.Add(new RotateOrder(index, oldDirection));

				//Si il se trompe de bord, on retourne de l'autre bord
				if (IsCubyOn(RubikData.UP, FindCuby(pattern, objectivePattern[position])))
                {
					oldDirection = -1.0f;
					UpdatePositions(pattern, index, oldDirection);
					UpdatePositions(pattern, index, oldDirection);
					orderList.RemoveAt(orderList.Count - 1);
					orderList.Add(new RotateOrder(index, oldDirection));
				}


				//On deplace le bas
				UpdatePositions(pattern, 5, 1.0f);
				orderList.Add(new RotateOrder(5, 1.0f));

				//Si il se trompe de bord, on retourne de l'autre bord
				if (IsCubyOn(oldFaces[1], FindCuby(pattern, objectivePattern[position])))
				{
					UpdatePositions(pattern, 5, -1.0f);
					UpdatePositions(pattern, 5, -1.0f);
					orderList.RemoveAt(orderList.Count - 1);
					orderList.Add(new RotateOrder(5, -1.0f));
				}

				//on remonte la face
				UpdatePositions(pattern, index, -oldDirection);
				orderList.Add(new RotateOrder(index, -oldDirection));
			}


			//on realigne le cuby en vu de le monter
			int counter = 0;
			while (
				!(
					RubikData.TOUCHING_FACES[FindCuby(pattern, objectivePattern[position])][1] == RubikData.TOUCHING_FACES[position][1] &&
					RubikData.TOUCHING_FACES[FindCuby(pattern, objectivePattern[position])][2] == RubikData.TOUCHING_FACES[position][2]
				)
			)
            {
				UpdatePositions(pattern, 5, 1.0f);
				orderList.Add(new RotateOrder(5, 1.0f));

				if (++counter > 3)
					throw new Exception("maximum 3 reached while moving down");
			}
			if (counter == 3)
            {
				orderList.RemoveAt(orderList.Count - 1);
				orderList.RemoveAt(orderList.Count - 1);
				orderList.RemoveAt(orderList.Count - 1);
				orderList.Add(new RotateOrder(5, -1.0f));
			}




			//on monte ************************ IL Y A UNE ERREUR ICI, EN ATTENTE DE LA CREATION DE LA FONCTION GetCubyColorOnface()
			index = Array.IndexOf(RubikData.indexFaceMap, GetCubyFaceFromColor(pattern[FindCuby(pattern, objectivePattern[position])], FindCuby(pattern, objectivePattern[position]), objectivePattern[RubikData.UP[4]][0]));
			int oldPosition = FindCuby(pattern, objectivePattern[position]);
			direction = 1.0f;
			//on descend
			UpdatePositions(pattern, index, direction);
			orderList.Add(new RotateOrder(index, direction));

			//correction s'il se trompe de direction
			if (IsCubyOn(RubikData.UP, FindCuby(pattern, objectivePattern[position])))
            {
				direction = -direction;
				UpdatePositions(pattern, index, direction);
				UpdatePositions(pattern, index, direction);
				orderList.RemoveAt(orderList.Count - 1);
				orderList.Add(new RotateOrder(index, direction));
			}

			//on tourne le bas jusqu'a sa bonne position
			counter = 0;
			while (FindCuby(pattern, objectivePattern[position]) != oldPosition)
			{
				UpdatePositions(pattern, 5, 1.0f);
				orderList.Add(new RotateOrder(5, 1.0f));

				if (++counter > 3)
					throw new Exception("maximum 3 reached while moving down");
			}
			if (counter == 3)
			{
				orderList.RemoveAt(orderList.Count - 1);
				orderList.RemoveAt(orderList.Count - 1);
				orderList.RemoveAt(orderList.Count - 1);
				orderList.Add(new RotateOrder(5, -1.0f));
			}

			//on remonte la face
			UpdatePositions(pattern, index, -direction);
			orderList.Add(new RotateOrder(index, -direction));
		}




		//deuxieme etage








		foreach (Order order in orderList)
		{
			orderQueue.Enqueue(order);
		}
			
			

		currentOrder = null;

	}



	protected void RotateUntilEqual(int index, int[] face, int position, List<Order> orderList, string[] pattern1, string[] pattern2)
	{
		int counter = 0;
		while (pattern1[face[position]] != pattern2[face[position]])
		{
			UpdatePositions(pattern1, index, 1.0f);
			orderList.Add(new RotateOrder(index, 1.0f));

			if (++counter > 3)
				throw new Exception("counter bigger than 3");
		}

		if (counter == 3)
		{
			orderList.RemoveAt(orderList.Count - 1);
			orderList.RemoveAt(orderList.Count - 1);
			orderList.RemoveAt(orderList.Count - 1);
			orderList.Add(new RotateOrder(index, -1.0f));
		}
	}








	protected int FindCuby(Transform cuby)
    	{
		string[] cubies = new string[27];


		for (int i = 0; i < 27; i++)
        	{
			cubies[i] = transform.GetChild(i).name;
		}


		return Array.IndexOf(cubies, Array.Find<string>(cubies, (tmpCuby) => {
			
			if (cuby.name.Length != tmpCuby.Length)
				return false;

			for (int i = 0; i < cuby.name.Length; i++)
			{
				if (!tmpCuby.Contains(cuby.name[i]))
					return false;
			}

			for (int i = 0; i < tmpCuby.Length; i++)
			{
				if (!cuby.name.Contains(tmpCuby[i]))
					return false;
			}


			return true;
		}));

    	}



	protected int FindCuby(string name)
    	{
		string cuby = Array.Find<string>(currentPattern, (string currentName) => {


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

		return Array.IndexOf(currentPattern, cuby);
    	}

	protected int FindCuby(Transform[] positions, Transform cuby)
    	{
		return Array.IndexOf(positions, cuby);
    	}



	protected bool IsCubyOn(int[] face, Transform cuby)
    	{
		int position = FindCuby(cuby);

		for (int i = 0; i < face.Length; i++)
        	{
			if (position == face[i])
				return true;
        	}

		return false;
    	}


	public bool IsSharingFace(int[] face, int position1, int position2)
	{
		return IsCubyOn(face, position1) && IsCubyOn(face, position2);
	}



	protected bool IsCorner(Transform cuby)
	{
		return Array.IndexOf(RubikData.CORNERS, FindCuby(cuby)) != -1;
	}

	protected bool IsMiddle(int position)
	{
		return Array.IndexOf(RubikData.MIDDLES, position) != -1;
	}

	protected bool IsEdge(Transform cuby)
	{
		return Array.IndexOf(RubikData.EDGES, FindCuby(cuby)) != -1;
	}
	







	protected void CorrectCornerOrientation(Transform cuby)
    	{
		int i = FindCuby(cuby.name);
		int defaultPosition = FindCuby(RubikData.DEFAULT_PATTERN, cuby.name);

		cuby.Rotate(RubikData.cornerCorrections[i][defaultPosition]);
	}



	protected void PhysicRotateCorner(string desiredOrientation, Transform cuby)
    	{
		if (cuby.name.Equals(desiredOrientation))
			return;

		int position = FindCuby(cuby.name);

		for (int i = 0; i < 2; i++)
        	{
			cuby.Rotate(RubikData.cornerRotations[position], Space.World);
			RotateCorner(cuby, RubikData.CLOCKWISE);

			if (cuby.name.Equals(desiredOrientation))
				return;
        	}

		throw new Exception($"Impossible de trouver l'orientation {desiredOrientation} pour {cuby.name}");
    	}



	protected void PhysicRotateEdge(string desiredOrientation, Transform cuby)
	{
		if (cuby.name.Equals(desiredOrientation))
			return;


		int position = FindCuby(cuby.name);
		Vector3 rotation = Vector3.zero;


		cuby.Rotate(RubikData.edgesFrame[position], Space.World);
		cuby.Rotate(rotation, Space.World);
		cuby.Rotate(-RubikData.edgesFrame[position], Space.World);


		cuby.Rotate(RubikData.edgeRotations[FindCuby(cuby.name)], Space.World);
		RotateEdge(cuby);


		if (cuby.name.Equals(desiredOrientation))
			return;


		throw new Exception($"Impossible de trouver l'orientation {desiredOrientation} pour {cuby.name}");
	}















	/*
		Cette methode n'est pas securitaire a utiliser durant la rotation du cube.
		Utilisez cubyPositions durant la rotation du cube.
	 */
	protected Transform[] GetPattern()
    	{
		Transform[] pattern = new Transform[27];

		for (int i = 0; i < 27; i++)
        	{
			

			for (int j = 0; j < 27; j++)
            		{
				Transform cuby = transform.GetChild(j);

				Vector3 cubyPosition = new Vector3(Mathf.RoundToInt(cuby.localPosition.x), Mathf.RoundToInt(cuby.localPosition.y), Mathf.RoundToInt(cuby.localPosition.z));

				if (cubyPosition == RubikData.worldPositions[i])
                		{
					pattern[i] = cuby;
					break;
                		}
            		}
        	}

		return pattern;
    	}












	protected void NextRandom()
    	{

		index = random.Next(0, 9);
		direction = random.Next(0, 2) == 0 ? 1.0f : -1.0f;


		randomCounter--;

		RotateBegin();
	}




	protected void RotateBegin()
    	{
		isRotating = true;
		angleCounter = 0;
		

		if (index <= 2)
		{
			currentAxis = Vector3.back;
		}
		else if (index <= 5)
        	{
			currentAxis = Vector3.down;
		}
		else
        	{
			currentAxis = Vector3.right;
		}


	}




	protected void RotateAnimation()
    	{
		for (int i = 0; i < 9; i++)
			cubyPositions[RubikData.rotationPositions[index, i]].RotateAround(transform.position, currentAxis, ROTATION * direction);
	
			

		angleCounter++;


		if (angleCounter == ROTATION_STEP)
			RotateEnd();
	}


	protected void RotateEnd()
    	{

		UpdatePositions();

		isRotating = false;
	}




	//Permet de garder l'etat interne du cube
	protected void UpdatePositions()
    	{
		/*
			Ici, on reutilise un meme principe pour tous les rotations.
			Voici un exemple: 
			0|1|2
			3|4|5
			6|7|8
			
			Avant la rotation, la position des cuby est : 
			[0, 1, 2, 3, 4, 5, 6, 7, 8]
			
			Et apres la rotation : 
			[2, 5, 8, 1, 4, 7, 0, 3, 6]
			
			La prochaine pile de 'for' ne fait que refleter ces
			changements.
		 */


		
		int currentPosition = 0;
		for (int i = 2; i > -1; i--)
		{
			for (int j = 0; j < 7; j += 3)
			{
				bufferPositions[currentPosition] = cubyPositions[RubikData.rotationPositions[index, i + j]];

				currentPosition++;
			}
		}
		

		//Il suffit de retourner le tableau dans l'autre sens pour
		//tourner dans l'autre sens.
		if (direction == -1.0f)
			Array.Reverse(bufferPositions);





		//Correction des noms des cubies une fois tourner, car
		//les cubies changent de noms selon leur position.
		for (int i = 0; i < 9; i++)
        	{
			Transform cuby = cubyPositions[RubikData.rotationPositions[index, i]];
			int oldPosition = i;
			int newPosition = Array.IndexOf(bufferPositions, cuby);


			if (IsCorner(cuby))
            		{
				if (index <= 2 || index >= 6)
				{
					bool a = index == 0 || index == 8;
					bool b = direction == 1.0f;
					bool c = IsCubyOn(RubikData.UP, oldPosition);
					bool d = IsCubyOn(RubikData.UP, newPosition);

					RotateCorner(bufferPositions[i], !(a != b != c != d) ? RubikData.CLOCKWISE: RubikData.COUNTERCLOCKWISE);
				}
			}
			else if (IsEdge(cuby))
            		{
				if (index == 1 || index == 4 || index >= 6)
                		{
					RotateEdge(bufferPositions[i]);
                		}
            		}
			

			cubyPositions[RubikData.rotationPositions[index, i]] = bufferPositions[i];
		}
		

		//Met a jour currentPattern pour refleter les changements
		Transform[] pattern = GetPattern();
		for (int i = 0; i < 27; i++)
		{
			currentPattern[i] = pattern[i].name;
		}
		
	}


	protected void Print(string[] pattern)
	{
		string message = "[";

		for (int i = 0; i < pattern.Length; i++)
		{
			message += pattern[i];
			if (i + 1 != pattern.Length)
			message += ", ";
		}

		Debug.Log(message);
	}


}
