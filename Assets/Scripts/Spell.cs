using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class Spell : MonoBehaviour
{
    abstract public void FireSimple();
    abstract public void FireHold(bool holding);
    abstract public void SetFirePoints(Transform point1, Transform point2);
    abstract public void WakeUp();
    abstract public ParticleSystem GetSource();
}
