using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class ShakeManager : Singleton<ShakeManager>
{
    private CinemachineImpulseSource source;
    protected override void Awake()
    {
        base.Awake();
        source = GetComponent<CinemachineImpulseSource>();
    }
    public void ShakeScreen(){
        source.GenerateImpulse();
    }
}
