using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningStorm : Spell
{
    [SerializeField]
    private float spawningHeight;

    private Transform firePoint;
    private GameObject tmpStorm;

    // Start is called before the first frame update
    void Start()
    {
        tmpStorm = Instantiate(gameObject) as GameObject;
        tmpStorm.SetActive(false);
    }

    public override void SetFirePoint(Transform point)
    {
        firePoint = point;
    }

    public override void WakeUp()
    {
        Start();
    }

    public override void FireSimple()
    {
        if (!tmpStorm.activeInHierarchy)
        {
            RaycastHit hit;
            if (Physics.Raycast(firePoint.position, firePoint.TransformDirection(Vector3.forward), out hit))
            {
                tmpStorm.transform.position = hit.point + Vector3.up * spawningHeight;
                tmpStorm.SetActive(true);
                Invoke(nameof(StopStorm), 10f);
            }
        }
    }

    private void StopStorm()
    {
        tmpStorm.SetActive(false);
    }

    public override void FireHold(bool holding)
    {
    }
}
