using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class Spell : MonoBehaviour
{
    abstract public void FireSimple();
    abstract public void FireHold(bool holding);
    abstract public void SetFirePoint(Transform point);
    abstract public void WakeUp();
}
