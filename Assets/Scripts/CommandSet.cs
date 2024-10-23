using System;
using System.Collections.Generic;
using UnityEngine;

public static class CommandSet
{
    static Dictionary<string, Action> commands = new Dictionary<string, Action>();

    public static void AddCommand(string commandCode, Action action)
    {
        if (!commands.ContainsKey(commandCode))
        {
            commands.Add(commandCode, action);
        }
        else
        {
            commands[commandCode] = action;
        }
    }

    public static void ExecuteCommand(string commandCode)
    {
        if (commands.TryGetValue(commandCode, out Action action))
        {
            Debug.Log(string.Format("<color=#{0:X2}{1:X2}{2:X2}>{3}</color>",
                (byte)(Color.green.r * 255f), (byte)(Color.green.g * 255f), (byte)(Color.green.b * 255f),
                $"Executing command: '{commandCode}'"));

            action.Invoke();
        }
        else
        {
            Debug.Log(string.Format("<color=#{0:X2}{1:X2}{2:X2}>{3}</color>",
                (byte)(Color.red.r * 255f), (byte)(Color.red.g * 255f), (byte)(Color.red.b * 255f),
                $"Command: '{commandCode}' not found"));
        }
    }

    public static void RemoveCommand(string commandCode)
    {
        if (commands.ContainsKey(commandCode))
        {
            commands.Remove(commandCode);
        }
    }
}
