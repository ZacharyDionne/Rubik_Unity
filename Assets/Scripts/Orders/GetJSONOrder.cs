using System;

public class GetJSONOrder : Order
{
    public Action<string> Callback { get; }

    public GetJSONOrder(Action<string> callback)
    {
        Callback = callback;
    }
}
