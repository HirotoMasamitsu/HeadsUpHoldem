using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMouseEvent
{
    public void Clicked();
    public void Entered();
    public void Exited();
}