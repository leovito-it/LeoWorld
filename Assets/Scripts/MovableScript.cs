using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovableScript : MonoBehaviour
{
    public MoveBy moveBy; 
    public MoveType moveType;
    public MoveDirections direction = MoveDirections.Right;

    public float speed = 0f;
    public float acceleration = 0f;

    private Vector3 startPos;
    private float a = 0f, v = 0f, t = 0f;
    private float x, y;
    private bool running = true;

    void Start()
    {
        startPos = transform.position;
        FlipByDirection();
    }

    void FixedUpdate()
    {
        if (running)
        {
            GetStats();
            SwitchMoveType();
            SwitchMoveDirections();
            SwitchMoveBy();
        }
    }

    void OnCollisionEnter2D()
    {
        ChangeDirection();
        FlipByDirection();
    }

    void SwitchMoveBy()
    {
        switch (moveBy)
        {
            case MoveBy.Rigidbody:
                {
                    gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(x,y);
                        //(new Vector3(x,y,0),ForceMode2D.Impulse);
                    break;
                }
            case MoveBy.Tranform:
                {
                    transform.position += new Vector3(x * Time.fixedDeltaTime, y * Time.fixedDeltaTime, 0);
                    break;
                }
        }
    }

    void SwitchMoveType()
    {
        switch (moveType)
        {
            case MoveType.DefineVeloc:
                {
                    DefineVeloc();
                    break;
                }
            case MoveType.SlowThenFast:
                {
                    SlowThenFast();
                    break;
                }
            case MoveType.SlowThenStop:
                {
                    SlowThenStop();
                    break;
                }
        }
    }

    void SwitchMoveDirections()
    {
        switch (direction)
        {
            case MoveDirections.Down:
                {
                    x = 0; y = -v;
                    break;
                }
            case MoveDirections.Left:
                {
                    x = -v; y = 0;
                    break;
                }
            case MoveDirections.Right:
                {
                    x = v; y = 0;
                    break;
                }
            case MoveDirections.Up:
                {
                    x = 0; y = v;
                    break;
                }
        }
    }

    void DefineVeloc()
    {
        v = speed;
    }

    void SlowThenFast()
    {
        //s = v0 * t + (1.0f / 2) * a * Mathf.Pow(t, 2) - s0;
        v = a * t;
    }

    void SlowThenStop()
    {
        v = speed - a * t;
        if (v <= 0)
            running = false;
    }

    void GetStats()
    {
        t = Time.time;
        a = acceleration;
    }

    void ChangeDirection()
    {
        // switch direction
        direction =
            direction == MoveDirections.Right ? MoveDirections.Left :
        (direction == MoveDirections.Up ? MoveDirections.Down :
        (direction == MoveDirections.Left ? MoveDirections.Right :
        (direction == MoveDirections.Down ? MoveDirections.Up : direction)));
    }

    public void ChangeDirectionByPosition(Vector2 position)
    {
        Vector2 thisPos = gameObject.TryGetComponent(out Transform trans) ?
            (Vector2)trans.localPosition : (Vector2)gameObject.GetComponent<RectTransform>().localPosition;
        float angle = Vector2.Angle(thisPos, position);

        direction = (angle < 45) && (angle > -45) ? MoveDirections.Right :
            (angle < 45 + 90) && (angle > -45 + 90) ? MoveDirections.Up :
            (angle < 45 + 90*2) && (angle > -45+ 90*2) ? MoveDirections.Left :
            (angle < 45 + 90*3) && (angle > -45 + 90*3) ? MoveDirections.Down : direction;
    }

    void FlipByDirection()
    {
        if (gameObject.TryGetComponent(out SpriteRenderer sr))
        {
            sr.flipX = direction == MoveDirections.Left ? false :
                (direction == MoveDirections.Right ? true : sr.flipX );
            sr.flipY = direction == MoveDirections.Up ? false :
                (direction == MoveDirections.Down ? true : sr.flipY);
        }
        else
        {
            if (gameObject.TryGetComponent(out RectTransform rt))
            {
                rt.localScale = (direction == MoveDirections.Left ? new Vector2(1, 1) :
                    (direction == MoveDirections.Right ? new Vector2(-1, 1) : new Vector2(rt.localScale.x, rt.localScale.y)));
                rt.localScale = (direction == MoveDirections.Up ? new Vector2(1, 1) :
                    (direction == MoveDirections.Down ? new Vector2(1, -1) : new Vector2(rt.localScale.x, rt.localScale.y)));
            }
        }
    }

    public enum MoveBy
    {
        Rigidbody, Tranform,
    }

    public enum MoveType
    {
        DefineVeloc, SlowThenFast, SlowThenStop,
    }

    public enum MoveDirections
    {
        Up, Right, Left, Down,
    }

    public enum HowMoving
    {
        AddForce, MovePosition,
    }
}
