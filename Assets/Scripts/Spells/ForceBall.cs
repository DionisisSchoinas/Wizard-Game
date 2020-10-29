using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class ForceBall : Spell
{
    [SerializeField]
    private float force = 5f;
    [SerializeField]
    private GameObject pullParticles;
    [SerializeField]
    private GameObject pushParticles;

    private Transform firePoint;
    private GameObject tmpBall;


    private float sizeIncreasePerFrame;
    private float sizeChangePerSecond = 100f;
    private bool push;
    private bool allow;
    private bool holding;


    private void Start()
    {
        allow = true;
        holding = false;
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
        RaycastHit hit;
        if (Physics.Raycast(firePoint.position, firePoint.TransformDirection(Vector3.forward), out hit))
        {
            tmpBall = Instantiate(pushParticles, hit.point, firePoint.rotation) as GameObject;
            Push(Physics.OverlapSphere(hit.point, 10), hit.point);
            Destroy(tmpBall, 1f);
        }
    }

    public override void FireHold(bool hold)
    {
        if (hold)
        {
            RaycastHit hit;
            if (Physics.Raycast(firePoint.position, firePoint.TransformDirection(Vector3.forward), out hit))
            {
                if (!holding)
                {
                    tmpBall = Instantiate(pullParticles, hit.point, firePoint.rotation) as GameObject;
                }
                tmpBall.transform.position = hit.point;
                Pull(Physics.OverlapSphere(hit.point, 10), hit.point);
            }
            holding = true;
        }
        else
        {
            holding = false;
            Destroy(tmpBall);
        }
    }

    void Push(Collider[] colliders, Vector3 pos)
    {
        foreach (Collider other in colliders)
        {
            if (other.CompareTag("Damageable"))
                other.GetComponent<Rigidbody>().AddExplosionForce(force, pos, 10, 1f, ForceMode.Impulse);
        }
    }

    void Pull(Collider[] colliders, Vector3 pos)
    {
        foreach (Collider other in colliders)
        {
            if (other.CompareTag("Damageable"))
                other.GetComponent<Rigidbody>().AddForce((pos - other.transform.position).normalized * force * Time.deltaTime * 100);
        }
    }
}
