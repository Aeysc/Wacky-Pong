﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class TwoVec2
{
    public Vector2 first;
    public Vector2 sec;
    public TwoVec2( Vector2 first,Vector2 sec )
    {
        this.first = first;
        this.sec = sec;
    }
}

/// <summary>
/// A ball that bounces off of paddles and walls.
/// </summary>
public class Ball
    :
    MonoBehaviour
{
    Rigidbody2D body;
    float hits;
    Timer lifetimer;
    Timer moveTimer;
    bool startedMoving = false;
    bool destroying = false;
    static bool setBounds = false;
    static Vector2 topLeft = new Vector2( 0.0f,0.0f );
    static Vector2 botRight = new Vector2( 0.0f,0.0f );
    // 
    public float Hits
    {
        get { return hits; }
    }
    /// <summary>
    /// Use this for initialization
    /// </summary>
    void Start()
    {
        // Set up initial vars pls ty.
        body = GetComponent<Rigidbody2D>();

        hits = ConfigurationUtils.BallHits;

        lifetimer = gameObject.AddComponent<Timer>();
        lifetimer.Duration = ConfigurationUtils.BallLifetime;
        lifetimer.Run();

        moveTimer = gameObject.AddComponent<Timer>();
        print( ConfigurationUtils.BallSpawnTime );
        moveTimer.Duration = ConfigurationUtils.BallSpawnTime;
        moveTimer.Run();

        // Set up top left and lower right only once.
        if( !setBounds )
        {
            setBounds = true;
            var halfDims = GetComponent<BoxCollider2D>().size / 2.0f;
            var pos = transform.position;

            topLeft.Set( pos.x - halfDims.x,
                pos.y + halfDims.y );
            botRight.Set( pos.x - halfDims.x,
                pos.y - halfDims.y );
        }

        Freeze(); // So we don't start prematurely.
	}
    /// <summary>
    ///     Updates timers and starts/destroys when they're done.
    /// </summary>
    void Update()
    {
        if( lifetimer.Finished )
        {
            DestroyAndMakeNewBall();
        }

        if( moveTimer.Finished && !startedMoving )
        {
            // print( "We started moving!" );
            Unfreeze(); // Make sure we can move again.
            StartMoving();
            startedMoving = true;
        }
    }
    /// <summary>
    ///     Change the direction you're going in.
    /// </summary>
    /// <param name="dir">New direction to go.</param>
    public void SetDirection( Vector2 dir )
    {
        body.velocity = dir * body.velocity.magnitude *
            // The line below makes it more like real pong.
            ( Mathf.Max( Mathf.Abs( dir.y ),0.5f ) * 2.1f );
    }
    void OnBecameInvisible()
    {
        // This does nothing, as it needs to be on the same
        //  object as a mesh/sprite renderer component.

        // I attached a `DestroyBallWhenInvisible` script
        //  to the model, so it will destroy the ball once
        //  that becomes invisible.

        // Just ctrl + click
        //  \/  \/  \/  \/  \/ here to check it out
        DestroyBallWhenInvisible example;
    }
    /// <summary>
    ///     Applies initial ball velocity.
    /// </summary>
    void StartMoving()
    {
        float pi = Mathf.PI;
        float qPi = pi / 4.0f;

        // Set up angle deviations, -45 -> 45 & 135 -> 225.
        Vector2 min = new Vector2( -qPi,qPi );
        Vector2 max = new Vector2( pi - qPi,pi + qPi );

        // Find a random angle 50% chance from first set,
        //  50% chance of second set.
        float randAngle = Random.Range( min.x,min.y );
        if( Random.Range( 0.1f,0.9f ) >= 0.5f )
        {
            randAngle = Random.Range( max.x,max.y );
        }

        var spd = ConfigurationUtils.BallImpulseForce;
        // Calculate force vector and apply.
        body.AddForce( new Vector2( Mathf.Cos( randAngle ),
            Mathf.Sin( randAngle ) ) * spd,
            ForceMode2D.Impulse );
    }
    /// <summary>
    ///     Respawn a ball, it's in a function because we
    ///      have to call it from multiple places.
    /// </summary>
    public void DestroyAndMakeNewBall()
    {
        if( !destroying )
        {
            destroying = true;

            var cam = GameObject.FindGameObjectsWithTag( "MainCamera" )[0];
            var ballSpawner = cam.GetComponent<BallSpawner>();
            if( ballSpawner != null ) ballSpawner.SpawnNewBall();

            Destroy( gameObject );
        }
    }
    /// <summary>
    ///     Used by DestroyBallWhenInvisible to not spawn
    ///      or destroy too many balls at once.
    /// </summary>
    /// <returns>Whether or not the ball is marked to be destroyed.</returns>
    public bool IsDead()
    {
        return( destroying );
    }
    /// <summary>
    ///     Gives you the top left and lower right of my hitbox, for collision test reasons.
    /// </summary>
    /// <returns>The top left and bot right of my hitbox.</returns>
    public static TwoVec2 GetDiagBounds()
    {
        return( new TwoVec2( topLeft,botRight ) );
    }
    /// <summary>
    ///     Use this guy to stop movement.
    /// </summary>
    void Freeze()
    {
        Assert.IsNotNull( body );
        body.constraints = RigidbodyConstraints2D.FreezePosition |
            RigidbodyConstraints2D.FreezeRotation;
    }
    /// <summary>
    ///     Use this one to continue movement but keep the
    ///      other constraints (like x) that we want.
    /// </summary>
    void Unfreeze()
    {
        Assert.IsNotNull( body );
        body.constraints = RigidbodyConstraints2D.None |
            RigidbodyConstraints2D.FreezeRotation;
    }
}