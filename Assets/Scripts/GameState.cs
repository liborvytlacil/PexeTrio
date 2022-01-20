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

    /// <summary>
    /// Initializes the games state unless it has already been initialized in which case
    /// it does nothing.
    /// </summary>
    /// <param name="tripletsInPlay">The triplets to be in play.</param>
    public static void tryInitialize(int tripletsInPlay)
    {
        if (!initialized)
        {
            tries = 0;
            SetTripletsInPlay(tripletsInPlay);
            initialized = true;
        }
    }
    /// <summary>
    /// </summary>
    /// <returns>The number of tries.</returns>
    public static int GetTries()
    {
        return tries;
    }

    /// <summary>
    /// Sets the tries number.
    /// </summary>
    /// <param name="value">number to set as tries</param>
    public static void SetTries(int value)
    {
        tries = value;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns>The current number of triplets in play.</returns>
    public static int GetTripletsInPlay()
    {
        return tripletsInPlay;
    }

    /// <summary>
    /// Sets how many triplets there will be in play
    /// </summary>
    /// <param name="tripletsInPlay">number of triplets to be in play</param>
    public static void SetTripletsInPlay(int tripletsInPlay)
    {
        GameState.tripletsInPlay = tripletsInPlay;
    }
 
    /// <summary>
    /// Marks the game state as uninitialized.
    /// </summary>
    public static void Uninitialize()
    {
        initialized = false;
    }
}
