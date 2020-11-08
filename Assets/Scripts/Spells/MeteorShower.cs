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

    private Transform firePoint;
    private Vector3 spellLocation;
    private bool fire;

    private void Start()
    {
        fire = false;
        Physics.IgnoreLayerCollision(8, 9);
        Physics.IgnoreLayerCollision(9, 9);
    }

    public override void WakeUp()
    {
        Start();
    }

    public override void SetFirePoint(Transform point)
    {
        firePoint = point;
    }

    private void FixedUpdate()
    {
    }

    public override void FireSimple()
    {
        if (!fire)
        {
            RaycastHit hit;
            if (Physics.Raycast(firePoint.position, firePoint.TransformDirection(Vector3.forward), out hit))
            {
                spellLocation = hit.point + Vector3.up * spawningHeight;
                fire = true;
                Fire();
                Invoke(nameof(StopFiring), 10f);
            }
        }
    }

    private void Fire()
    {
        if (fire)
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
        fire = false;
    }

    public override void FireHold(bool holding)
    {
    }

    public override ParticleSystem GetSource()
    {
        return ((GameObject)Resources.Load("Spells/Default Fire Source", typeof(GameObject))).GetComponent<ParticleSystem>();
    }
}
