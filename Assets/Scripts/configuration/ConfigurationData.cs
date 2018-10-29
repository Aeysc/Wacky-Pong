﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

/// <summary>
/// A container for the configuration data
/// </summary>
public class ConfigurationData
{
    #region Fields

    const string ConfigurationDataFileName = "ConfigurationData.csv";

    // configuration data
    static float paddleMoveUnitsPerSecond;
    static float ballImpulseForce;
    static float ballHits;
    static float ballLifetime;
    static float ballSpawnTime;
    static float ballMinSpawnSecs;
    static float ballMaxSpawnSecs;

    #endregion

    #region Properties

    /// <summary>
    ///     Gets the paddle move units per second
    /// </summary>
    public float PaddleMoveUnitsPerSecond
    {
        get { return paddleMoveUnitsPerSecond; }
    }
    /// <summary>
    ///     Gets the impulse force to apply to move the ball
    /// </summary>
    public float BallImpulseForce
    {
        get { return ballImpulseForce; }
    }
    /// <summary>
    ///     How much a ball adds to your score when you hit it.
    /// </summary>
    public float BallHits
    {
        get { return( ballHits ); }
    }
    /// <summary>
    ///     How long before a ball despawns.
    /// </summary>
    public float BallLifetime
    {
        get { return( ballLifetime ); }
    }
    /// <summary>
    ///     How long it takes for the ball to respawn after being destroyed.
    /// </summary>
    public float BallSpawnTime
    {
        get { return( ballSpawnTime ); }
    }
    /// <summary>
    ///     Minimum amount of time it takes to spawn another ball while one is already active.
    /// </summary>
    public float BallMinSpawnSecs
    {
        get { return( ballMinSpawnSecs ); }
    }
    /// <summary>
    ///     Maximum amount of time it takes to spawn another ball when others are still on the field.
    /// </summary>
    public float BallMaxSpawnSecs
    {
        get { return( ballMaxSpawnSecs ); }
    }

    #endregion

    #region Constructor

    /// <summary>
    /// Constructor
    /// Reads configuration data from a file. If the file
    /// read fails, the object contains default values for
    /// the configuration data
    /// </summary>
    public ConfigurationData()
    {
        string name = Application.streamingAssetsPath + '/' +
            ConfigurationDataFileName;

        StreamReader input = null;
        try
        {
            input = new StreamReader( name );
            input.ReadLine();

            string str = input.ReadLine();
            string[] vals = str.Split( ',' );
            paddleMoveUnitsPerSecond = float.Parse( vals[0] );
            ballImpulseForce = float.Parse( vals[1] );
            ballHits = float.Parse( vals[2] );
            ballLifetime = float.Parse( vals[3] );
            ballSpawnTime = float.Parse( vals[4] );
            ballMinSpawnSecs = float.Parse( vals[5] );
            ballMaxSpawnSecs = float.Parse( vals[6] );
        }
        catch( Exception e )
        {
            Debug.Log( "You failed!" );
        }
        finally
        {
            if( input != null ) input.Close();
        }
    }

    #endregion
}