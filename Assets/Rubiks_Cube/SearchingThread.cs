using System.Collections.Generic;
using System.Threading;


public class SearchingThread : Thread
{
	private Queue<Task> taskQueue;
	public bool Done { get; private set; }

	public SearchingThread(Queue<Task> taskQueue)
	{
		
	}
}
