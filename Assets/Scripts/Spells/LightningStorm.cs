using UnityEngine;

public class LightningStorm : Spell
{
    [SerializeField]
    private float spawningHeight;

    public GameObject marker;

    private Transform firePoint;
    private GameObject tmpStorm;
    private Vector3 spawningLocation;
    private bool pickedSpot;

    private GameObject tmpMarker;


    void Start()
    {
        tmpStorm = Instantiate(gameObject) as GameObject;
        tmpStorm.SetActive(false);
        pickedSpot = false;

        tmpMarker = Instantiate(marker) as GameObject;
        tmpMarker.SetActive(false);
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
        if (pickedSpot)
        {
            if (!tmpStorm.activeInHierarchy)
            {
                /*
                RaycastHit hit;
                if (Physics.Raycast(firePoint.position, firePoint.TransformDirection(Vector3.forward), out hit))
                {
                    tmpStorm.transform.position = hit.point + Vector3.up * spawningHeight;
                    tmpStorm.SetActive(true);
                    Invoke(nameof(StopStorm), 10f);
                }
                */
                tmpMarker.SetActive(false);
                pickedSpot = false;
                tmpStorm.transform.position = spawningLocation + Vector3.up * spawningHeight;
                tmpStorm.SetActive(true);
                Invoke(nameof(StopStorm), 10f);
            }
        }
    }

    public override void FireHold(bool holding)
    {
        if (!tmpStorm.activeInHierarchy)
        {
            if (holding)
            {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit))
                {
                    tmpMarker.SetActive(true);
                    Vector3 loc = hit.point;
                    loc.y = 4f;
                    tmpMarker.transform.position = loc;
                }
                pickedSpot = false;
            }
            else
            {
                Debug.Log("picked");
                spawningLocation = tmpMarker.transform.position - Vector3.down * 4;
                pickedSpot = true;
            }
        }
    }

    private void StopStorm()
    {
        tmpStorm.SetActive(false);
    }

    public override ParticleSystem GetSource()
    {
        return ((GameObject)Resources.Load("Spells/Default Lightning Source", typeof(GameObject))).GetComponent<ParticleSystem>();
    }
}
