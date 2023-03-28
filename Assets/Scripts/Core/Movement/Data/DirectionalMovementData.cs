using Core.Enums;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DirectionalMovementData 
{

    [field: SerializeField] public float HorizontalSpeed { get; private set; }
    [field: SerializeField] public Direction Direction { get; private set; }


    [field: SerializeField] public float VerticalSpeed { get; private set; }
    [field: SerializeField] public float MinSize { get; private set; }
    [field: SerializeField] public float MaxSize { get; private set; }
    [field: SerializeField] public float MinVericalPosition { get; private set; }
    [field: SerializeField] public float MaxVericalPosition { get; private set; }
}
