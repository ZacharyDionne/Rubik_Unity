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

	protected static string[] FindCubiesWithColor(string[] pattern, string color)
    	{
		return Array.FindAll(pattern, (string str) => { return str.Contains(color[0]); });
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
				direction = 1.0f;
				RotateBegin();
			}
			if (Input.GetKeyDown(KeyCode.D))
			{
				direction = -1.0f;
				RotateBegin();
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
		

		int counter;

		//Positionnement des milieux
		if (
			IsCubyOn(RubikData.FRONT, FindCuby(objectivePattern[RubikData.UP[4]])) ||
			IsCubyOn(RubikData.BACK, FindCuby(objectivePattern[RubikData.UP[4]])) ||
			IsCubyOn(RubikData.DOWN, FindCuby(objectivePattern[RubikData.UP[4]])))
		{
			counter = 0;
			do
			{
				UpdatePositions(pattern, 7, -1.0f);


				orderList.Add(new RotateOrder(7, -1.0f));
				counter++;

				if (counter > 3)
					throw new Exception("counter bigger than 3");

			} while (pattern[RubikData.UP[4]] != objectivePattern[RubikData.UP[4]]);

			if (counter == 3)
			{
				orderList.RemoveAt(orderList.Count - 1);
				orderList.RemoveAt(orderList.Count - 1);
				orderList.RemoveAt(orderList.Count - 1);
				orderList.Add(new RotateOrder(7, 1.0f));
			}
		}
		else if (IsCubyOn(RubikData.LEFT, FindCuby(objectivePattern[RubikData.UP[4]])))
		{
			UpdatePositions(pattern, 1, -1.0f);
			orderList.Add(new RotateOrder(1, -1.0f));
		}
		else if (IsCubyOn(RubikData.RIGHT, FindCuby(objectivePattern[RubikData.UP[4]])))
		{
			UpdatePositions(pattern, 1, 1.0f);
			orderList.Add(new RotateOrder(1, 1.0f));
		}



		/*
		counter = 0;
		while (pattern[RubikData.FRONT[4]] != objectivePattern[RubikData.FRONT[4]])
		{
			UpdatePositions(pattern, 4, 1.0f);
			orderList.Add(new RotateOrder(1, 1.0f));

			if (++counter > 3)
				throw new Exception("counter bigger than 3");
		}

		if (counter == 3)
		{
			orderList.RemoveAt(orderList.Count - 1);
			orderList.RemoveAt(orderList.Count - 1);
			orderList.RemoveAt(orderList.Count - 1);
			orderList.Add(new RotateOrder(4, -1.0f));
		}
		*/














		foreach (Order order in orderList)
		{
			Debug.Log(order);
			orderQueue.Enqueue(order);
		}
			
			

		currentOrder = null;

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



}
