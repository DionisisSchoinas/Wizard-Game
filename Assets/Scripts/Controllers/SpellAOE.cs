using Cinemachine;
using System.Collections;
using UnityEngine;
using UnityEngine.SocialPlatforms.GameCenter;

public class SpellAOE : MonoBehaviour
{
    public GameObject indicator;
    public Material rangeCircleMaterial;
    public Material aoeCircleMaterial;
    public Material aoeSquareMaterial;

    private int mode;

    private Vector3 centerOfRadius;
    private float castingRadius;
    private Vector3 centerOfAOE;
    private Vector3 spellRotation;
    private float aoeRadius;
    private float aoeLength;
    private float aoeWidth;

    private bool picking;
    private GameObject tmpRangeIndicator;
    private GameObject tmpAoeIndicator;
    private int layerMasks;
    private Plane plane;

    private PlayerMovementScript controls;

    // Start is called before the first frame update
    void Start()
    {
        picking = false;
        controls = GameObject.FindObjectOfType<PlayerMovementScript>() as PlayerMovementScript;
        layerMasks = LayerMask.GetMask("Ground");
        plane = new Plane(Vector3.up, controls.transform.position);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (picking)
        {
            // Centered on player
            if (mode != 2)
            {
                centerOfRadius = controls.transform.position;
                tmpRangeIndicator.transform.position = centerOfRadius;
            }
            // Centered on mouse
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMasks))
            {
                SetIndicators(hit.point, centerOfRadius, castingRadius);
            }
            else if (mode == 2)
            {
                float hitPlane;
                plane.Raycast(ray, out hitPlane);
                SetIndicators(ray.GetPoint(hitPlane), centerOfRadius, aoeLength);
            }
        }
    }

    private void SetIndicators(Vector3 hit, Vector3 center, float range)
    {
        switch (mode)
        {
            // 1 circle and 1 polygon
            case 1:
                tmpAoeIndicator.transform.position = OutOfRange(hit, center, range);
                //Look at player
                tmpAoeIndicator.transform.LookAt(controls.transform, Vector3.up);
                //Rotate to look up
                tmpAoeIndicator.transform.eulerAngles = new Vector3(
                    90f,
                    tmpAoeIndicator.transform.eulerAngles.y,
                    tmpAoeIndicator.transform.eulerAngles.z
                );
                break;
            
            // Locked center and 1 polygon
            case 2:
                //Look at player
                tmpAoeIndicator.transform.LookAt(controls.transform, Vector3.up);
                //Rotate to look up
                tmpAoeIndicator.transform.eulerAngles = new Vector3(
                    90f,
                    tmpAoeIndicator.transform.eulerAngles.y,
                    tmpAoeIndicator.transform.eulerAngles.z
                );
                break;

            // 2 circles
            default:
                tmpAoeIndicator.transform.position = OutOfRange(hit, center, range);
                break;
        }
        if (mode != 2)
        {
            centerOfAOE = tmpAoeIndicator.transform.position;
            tmpAoeIndicator.transform.position += Vector3.up * 0.1f;
        }
    }

    private Vector3 OutOfRange(Vector3 hit, Vector3 center, float range)
    {
        if ((hit - center).magnitude <= range)
        {
            return hit;
        }
        else
        {
            return (hit - center).normalized * range + center;
        }
    }

    public void SelectLocation(float rangeRadious, float aoeSize)
    {
        // Use 2 circles
        mode = 0;

        // Reset previous indicators
        if (tmpAoeIndicator != null) Destroy(tmpAoeIndicator);
        if (tmpRangeIndicator != null) Destroy(tmpRangeIndicator);
        // New indicator sizes
        castingRadius = rangeRadious;
        aoeRadius = aoeSize;
        // New indicators
        tmpRangeIndicator = Instantiate(indicator, centerOfRadius, indicator.transform.rotation);
        tmpRangeIndicator.GetComponent<MeshRenderer>().material = rangeCircleMaterial;
        tmpRangeIndicator.transform.localScale = Vector3.one * castingRadius * 2f;
        tmpAoeIndicator = Instantiate(indicator);
        tmpAoeIndicator.SetActive(false);
        tmpAoeIndicator.GetComponent<MeshRenderer>().material = aoeCircleMaterial;
        tmpAoeIndicator.transform.localScale = Vector3.one * aoeRadius * 2f;
        tmpAoeIndicator.SetActive(true);

        picking = true;
        tmpAoeIndicator.SetActive(true);
    }
    public void SelectLocation(float rangeRadious, float leftToRight, float backToForward)
    {
        // Use 1 cirlce and 1 polygon
        mode = 1;

        // Reset previous indicators
        if (tmpRangeIndicator != null) Destroy(tmpRangeIndicator);
        if (tmpAoeIndicator != null) Destroy(tmpAoeIndicator);
        // New indicator sizes
        castingRadius = rangeRadious;
        aoeLength = leftToRight;
        aoeWidth = backToForward;
        // New indicators
        tmpRangeIndicator = Instantiate(indicator, centerOfRadius, indicator.transform.rotation);
        tmpRangeIndicator.GetComponent<MeshRenderer>().material = rangeCircleMaterial;
        tmpRangeIndicator.transform.localScale = Vector3.one * castingRadius * 2f;
        tmpAoeIndicator = Instantiate(indicator);
        tmpAoeIndicator.SetActive(false);
        tmpAoeIndicator.GetComponent<MeshRenderer>().material = aoeSquareMaterial;
        tmpAoeIndicator.transform.localScale = Vector3.right * leftToRight + Vector3.up * backToForward;
        tmpAoeIndicator.SetActive(true);

        picking = true;
        tmpAoeIndicator.SetActive(true);
    }

    public void SelectLocation(Transform center, float leftToRight, float backToForward)
    {
        // Use locked center and 1 polygon
        mode = 2;

        // Reset previous indicators
        if (tmpRangeIndicator != null) Destroy(tmpRangeIndicator);
        if (tmpAoeIndicator != null) Destroy(tmpAoeIndicator);
        // New indicator sizes
        aoeLength = backToForward;
        aoeWidth = leftToRight;
        // New indicators
        tmpAoeIndicator = Instantiate(indicator, center);
        tmpAoeIndicator.SetActive(false);
        tmpAoeIndicator.GetComponent<MeshRenderer>().material = aoeSquareMaterial;
        tmpAoeIndicator.transform.localScale = Vector3.right * leftToRight + Vector3.up * backToForward;
        tmpAoeIndicator.transform.position += center.forward * backToForward / 2f + Vector3.down * 2f;
        tmpAoeIndicator.SetActive(true);

        picking = true;
        tmpAoeIndicator.SetActive(true);
    }

    public Vector3[] LockLocation()
    {
        picking = false;
        if (tmpAoeIndicator != null)
        {
            spellRotation = tmpAoeIndicator.transform.eulerAngles;
            spellRotation.x = 0f;
            Destroy(tmpRangeIndicator);
        }
        switch (mode)
        {
            case 1:
                return new Vector3[] { centerOfAOE, spellRotation };
            default:
                return new Vector3[] { centerOfAOE };
        }
    }

    public void DestroyIndicator()
    {
        Destroy(tmpAoeIndicator);
        mode = -1;
        picking = false;
    }
}
