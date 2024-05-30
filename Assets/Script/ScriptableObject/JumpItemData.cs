using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum JumpItemType
{
    JumpPad
}

[CreateAssetMenu(fileName = "JumpItem", menuName = "New JumpItem")]
public class JumpItemData : ScriptableObject
{
    [Header("Info")]
    public string displayName;
    public string description;
    public JumpItemType type;

    [Header("JumpPad")]
    public GameObject JumpPrefab;
    public float JumpForce;
}
