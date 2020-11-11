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

    private SpellIndicatorController indicatorController;

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
            indicatorController.SelectLocation(channelingFirePoint, 3f, 18f);
            tmpBolt.SetActive(true);
        }
        else
        {
            indicatorController.DestroyIndicator();
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

    public override void SetIndicatorController(SpellIndicatorController controller)
    {
        indicatorController = controller;
    }

    public override ParticleSystem GetSource()
    {
        return tmpBolt.transform.Find("Source").GetComponent<ParticleSystem>();
    }

    public override void FireSimple()
    {
    }
}