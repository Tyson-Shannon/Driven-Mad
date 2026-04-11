//Tyson Shannon 2026-04-11

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPowerUpVisitor
{
    void Visit(CarHealthManager car);
}
