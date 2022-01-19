using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A static class used to hold information about current triplets in play and
/// the current number of tries.
/// </summary>
public sealed class GameState
{
    private static int tries;
    private static int tripletsInPlay;
    private static bool initialized = false;

    private GameState() { }

    public static void tryInitialize(int tripletsInPlay)
    {
        if (!initialized)
        {
            tries = 0;
            GameState.tripletsInPlay = tripletsInPlay;
            initialized = true;
        }
    }
    public static int GetTries()
    {
        return tries;
    }

    public static void SetTries(int value)
    {
        tries = value;
    }

    public static int GetTripletsInPlay()
    {
        return tripletsInPlay;
    }

    public static void SetTripletsInPlay(int tripletsInPlay)
    {
        GameState.tripletsInPlay = tripletsInPlay;
    }
}
