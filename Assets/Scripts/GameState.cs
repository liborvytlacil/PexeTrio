using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class GameState
{
    private static int tries;

    public static int GetTries()
    {
        return tries;
    }

    public static void SetTries(int value)
    {
        tries = value;
    }
}
