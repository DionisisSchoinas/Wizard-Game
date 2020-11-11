using UnityEngine;

public class ForceBall : Spell
{
    [SerializeField]
    private float force = 5f;
    [SerializeField]
    private GameObject pullParticles;
    [SerializeField]
    private GameObject pushParticles;

    private Transform simpleFirePoint;
    private Transform channelingFirePoint;
    private GameObject tmpBall;
    private bool holding;

    private SpellIndicatorController indicatorController;


    private void Start()
    {
        holding = false;
    }

    public override void SetFirePoints(Transform point1, Transform point2)
    {
        simpleFirePoint = point1;
        channelingFirePoint = point2;
    }

    public override void WakeUp()
    {
        Start();
    }

    public override void FireSimple()
    {
        RaycastHit hit;
        if (Physics.Raycast(simpleFirePoint.position, simpleFirePoint.TransformDirection(Vector3.forward), out hit))
        {
            tmpBall = Instantiate(pushParticles, hit.point, simpleFirePoint.rotation) as GameObject;
            Push(Physics.OverlapSphere(hit.point, 10), hit.point);
            Destroy(tmpBall, 1f);
        }
    }

    public override void FireHold(bool hold)
    {
        if (hold)
        {
            RaycastHit hit;
            if (Physics.Raycast(channelingFirePoint.position, channelingFirePoint.TransformDirection(Vector3.forward), out hit))
            {
                if (!holding)
                {
                    tmpBall = Instantiate(pullParticles, hit.point, channelingFirePoint.rotation) as GameObject;
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

    public override void SetIndicatorController(SpellIndicatorController controller)
    {
        indicatorController = controller;
    }

    public override ParticleSystem GetSource()
    {
        throw new System.NotImplementedException();
    }
}
