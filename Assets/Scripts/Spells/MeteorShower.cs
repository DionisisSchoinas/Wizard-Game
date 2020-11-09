using UnityEngine;

public class MeteorShower : Spell
{
    [SerializeField]
    private GameObject meteorPrefab;
    [SerializeField]
    private float spawningRadius;
    [SerializeField]
    private float spawningHeight;
    [SerializeField]
    private float projectilesPerSecond;

    private Transform simplefiringPoint;
    private Transform channelingfiringPoint;
    private Vector3 spellLocation;
    private bool pickedSpot;
    private Vector3 spawningLocation;
    private SpellAOE aoe;
    private bool firing;

    private void Start()
    {
        Physics.IgnoreLayerCollision(8, 9);
        Physics.IgnoreLayerCollision(9, 9);

        pickedSpot = false;
        firing = false;
    }

    public override void WakeUp()
    {
        Start();
    }

    public override void SetFirePoints(Transform point1, Transform point2)
    {
        simplefiringPoint = point1;
        channelingfiringPoint = point2;
    }

    public override void FireSimple()
    {
        if (pickedSpot)
        {
            aoe.DestroyIndicator();
            pickedSpot = false;
            spellLocation = spawningLocation + Vector3.up * spawningHeight;
            firing = true;
            Fire();
            Invoke(nameof(StopFiring), 10f);
        }
    }

    public override void FireHold(bool holding)
    {
        if (!firing)
        {
            if (holding)
            {
                aoe = FindObjectOfType<SpellAOE>();
                aoe.SelectLocation(20f, 20f);
                pickedSpot = false;
            }
            else
            {
                if (aoe != null)
                {
                    spawningLocation = aoe.LockLocation();
                    pickedSpot = true;
                    Invoke(nameof(CancelSpell), 5f);
                }
            }
        }
    }

    private void Fire()
    {
        if (firing)
        {
            for (int i=1; i <= projectilesPerSecond; i++)
            {
                Vector2 rad = (Random.insideUnitCircle - Vector2.one * 0.5f) * spawningRadius * 2;
                Vector3 spawn = spellLocation;
                spawn[0] += rad[0];
                spawn[2] += rad[1];
                
                Destroy(Instantiate(meteorPrefab, spawn, Quaternion.identity), 5f);
            }
            Invoke(nameof(Fire), 1f);
        }
    }

    private void StopFiring()
    {
        firing = false;
    }

    private void CancelSpell()
    {
        aoe.DestroyIndicator();
        pickedSpot = false;
    }

    public override ParticleSystem GetSource()
    {
        return ((GameObject)Resources.Load("Spells/Default Fire Source", typeof(GameObject))).GetComponent<ParticleSystem>();
    }
}
