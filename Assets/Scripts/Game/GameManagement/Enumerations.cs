namespace GameManagement
{
    // PUBLISH THIS NOW, UNITY!!!
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    /// <summary>
    /// A schedule of collectable nuts. The nut types in this enumeration can be cast as integers to get their value.
    /// </summary>
    public enum NutType
    {
        ACORN           = 1,
        GOLDENACORN     = 20
    }

    /// <summary>
    /// Use this handy flag-suitable enumeration to more easily handle movement and other calculations involving the cardinal
    /// directions or combinations of cardinal directions.
    /// </summary>
    [System.Flags]
    public enum Cardinals
    {
        NONE    = 0,
        NORTH   = 1,
        SOUTH   = 2,
        EAST    = 4,
        WEST    = 8
    }

    public enum CharacterClasses
    {
        PLAYER,
        ENEMY,
        NPC
    }

    public enum Characters
    {
        SQUIRREL,
        EVILSQUIRREL,
        CAR,
        DOG,
        TRANSFORMER
    }
}