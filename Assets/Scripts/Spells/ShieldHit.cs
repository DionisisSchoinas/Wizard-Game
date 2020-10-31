using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldHit : MonoBehaviour
{
    [SerializeField]
    private Material idleMaterial;
    [SerializeField]
    private Vector3 hitShapeChangeOnHit;
    [SerializeField]
    private float hitDisturbanceRateOnHit;
    [SerializeField]
    private float hitDisturbanceDurationeOnHit;
    [SerializeField]
    private bool randomShapeChangeOnHit;

    private Vector3 defaultShapeChange = new Vector3(0.01f, 0.01f, 0.01f);
    private float defaultDisturbanceRate = 5f;

    private Vector3 randomizedHitShape;

    private void Start()
    {
        ResetMaterial();
        Physics.IgnoreLayerCollision(0, 8);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Projectile"))
        {
            Damaged();
        }
    }

    public void Damaged()
    {
        if (randomShapeChangeOnHit)
            randomizedHitShape = new Vector3(Random.Range(0.01f, hitShapeChangeOnHit.x), Random.Range(0.01f, hitShapeChangeOnHit.y), Random.Range(0.01f, hitShapeChangeOnHit.z));
        else
            randomizedHitShape = hitShapeChangeOnHit;

        idleMaterial.SetFloat("_VertexOffsetFrequency", hitDisturbanceRateOnHit);
        idleMaterial.SetVector("_VertexOffsetDirection", randomizedHitShape);
        Invoke(nameof(ResetMaterial), hitDisturbanceDurationeOnHit);
    }

    void ResetMaterial()
    {
        idleMaterial.SetFloat("_VertexOffsetFrequency", defaultDisturbanceRate);
        idleMaterial.SetVector("_VertexOffsetDirection", defaultShapeChange);
    }

}
