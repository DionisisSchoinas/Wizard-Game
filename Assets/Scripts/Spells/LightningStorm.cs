using UnityEngine;

public class LightningStorm : Spell
{
    [SerializeField]
    private float spawningHeight;

    private GameObject tmpStorm;
    private Vector3 spawningLocation;
    private bool pickedSpot;
    private SpellIndicatorController indicatorController;

    private Transform simpleFirePoint;
    private Transform channelingFirePoint;

    void Start()
    {
        tmpStorm = Instantiate(gameObject) as GameObject;
        tmpStorm.SetActive(false);
        pickedSpot = false;
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
        if (pickedSpot)
        {
            indicatorController.DestroyIndicator();
            pickedSpot = false;
            tmpStorm.transform.position = spawningLocation + Vector3.up * spawningHeight;
            tmpStorm.SetActive(true);
            Invoke(nameof(StopStorm), 10f);
        }
    }

    public override void FireHold(bool holding)
    {
        if (!tmpStorm.activeInHierarchy)
        {
            if (holding)
            {
                indicatorController.SelectLocation(20f, 15f);
                pickedSpot = false;
            }
            else
            {
                if (indicatorController != null)
                {
                    spawningLocation = indicatorController.LockLocation()[0];
                    pickedSpot = true;
                    Invoke(nameof(CancelSpell), 5f);
                }
            }
        }
    }

    private void StopStorm()
    {
        tmpStorm.SetActive(false);
    }

    private void CancelSpell()
    {
        indicatorController.DestroyIndicator();
        pickedSpot = false;
    }
    public override void SetIndicatorController(SpellIndicatorController controller)
    {
        indicatorController = controller;
    }

    public override ParticleSystem GetSource()
    {
        return ((GameObject)Resources.Load("Spells/Default Lightning Source", typeof(GameObject))).GetComponent<ParticleSystem>();
    }
}
