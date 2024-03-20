using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkerObject
{

    public Vector2 Position;

    public Vector2 Direction;

    public float ChanceToChange;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    public WalkerObject(Vector2 pos, Vector2 dir, float chanceToChange)
    { 
        Position = pos;
        Direction = dir;
        ChanceToChange = chanceToChange;
    }
}
