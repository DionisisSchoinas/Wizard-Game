using UnityEngine;

public class SpellAOE : MonoBehaviour
{
    public GameObject indicator;
    public Material rangeIndicator;
    public Material aoeIndicator;

    private Vector3 centerOfRadius;
    private float castingRadius;
    private Vector3 centerOfAOE;
    private float aoeRadius;
    private bool picking;
    private GameObject tmpRangeIndicator;
    private GameObject tmpAoeIndicator;

    private PlayerMovementScript controls;

    // Start is called before the first frame update
    void Start()
    {
        picking = false;
        controls = GameObject.FindObjectOfType<PlayerMovementScript>() as PlayerMovementScript;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (picking)
        {
            // Centered on player
            centerOfRadius = controls.gameObject.transform.position;
            tmpRangeIndicator.transform.position = centerOfRadius;
            // Centered on mouse
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                if ((hit.point - centerOfRadius).magnitude <= castingRadius)
                {
                    tmpAoeIndicator.transform.position = hit.point;
                }
                else
                {
                    tmpAoeIndicator.transform.position = (hit.point - centerOfRadius).normalized * castingRadius + centerOfRadius;
                }
                centerOfAOE = tmpAoeIndicator.transform.position;
                tmpAoeIndicator.transform.position += Vector3.up * 0.1f;
            }
        }
    }

    public void SelectLocation(float radious, float aoeSize)
    {
        // Reset previous indicators
        if (tmpAoeIndicator != null) Destroy(tmpAoeIndicator);
        if (tmpRangeIndicator != null) Destroy(tmpRangeIndicator);
        // New indicator sizes
        castingRadius = radious;
        aoeRadius = aoeSize;
        // New indicators
        tmpRangeIndicator = Instantiate(indicator);
        tmpRangeIndicator.GetComponent<MeshRenderer>().material = rangeIndicator;
        tmpRangeIndicator.transform.localScale = Vector3.one * castingRadius * 2f;
        tmpAoeIndicator = Instantiate(indicator);
        tmpAoeIndicator.GetComponent<MeshRenderer>().material = aoeIndicator;
        tmpAoeIndicator.transform.localScale = Vector3.one * aoeRadius * 2f;

        picking = true;
        tmpAoeIndicator.SetActive(true);
    }

    public Vector3 LockLocation()
    {
        picking = false;
        Destroy(tmpRangeIndicator);
        return centerOfAOE;
    }

    public void DestroyIndicator()
    {
        Destroy(tmpAoeIndicator);
    }
}
