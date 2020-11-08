using System.Collections.Generic;
using UnityEngine;

public class LightningBolt : Spell
{
    [SerializeField]
    private float damagePerFrame = 5f;

    private Transform simpleFirePoint;
    private Transform channelingFirePoint;
    private GameObject tmpBolt;
    private List<GameObject> collisions;

    private void Start()
    {
        tmpBolt = Instantiate(gameObject, channelingFirePoint) as GameObject;
        tmpBolt.SetActive(false);
        collisions = new List<GameObject>();
    }

    private void FixedUpdate()
    {
        foreach (GameObject gm in collisions)
        {
            gm.SendMessage("Damage", damagePerFrame);
        }
        collisions.Clear();
    }

    public override void FireHold(bool holding)
    {
        if (holding)
        {
            tmpBolt.SetActive(true);
        }
        else
        {
            tmpBolt.SetActive(false);
        }
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

    private void OnParticleCollision(GameObject other)
    {
        if (other.CompareTag("Damageable"))
        {
            if (!collisions.Contains(other))
                collisions.Add(other);
        }
    }
    public override void FireSimple()
    {
    }

    public override ParticleSystem GetSource()
    {
        return tmpBolt.transform.Find("Source").GetComponent<ParticleSystem>();
    }
}