using System;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class InputTypeValue
{
    public InputAction Action { get; set; }
    public abstract Type Type { get; }    
}

public sealed class InputTypeValue<T> : InputTypeValue
{
    public T Value { get; set; }
    public override Type Type => typeof(T);
}