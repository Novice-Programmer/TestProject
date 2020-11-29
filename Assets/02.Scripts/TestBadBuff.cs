using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EBadBuff
{
    None,
    Radioactivity
}

[Serializable]
public class TestBadBuff
{
    public EBadBuff type;
    public int hp;
    public int mp;
    public int atk;
    public int def;
    public float atkSpd;
    public float movSpd;
    public float checkTime;
    public float dotTime;
    public float remainTime;
}
