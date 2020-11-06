using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Schema;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Wand : MonoBehaviour
{
    [SerializeField]
    private Transform firePoint;
    [SerializeField]
    private Spell[] spells;
    [SerializeField]
    private Camera view;

    public float cooldownTimer = 1f;
    public bool channeling = false;
    public bool canCast;

    private int selectedSpell;

    private void Start()
    {
        canCast = true;
        foreach (Spell s in spells)
        {
            s.SetFirePoint(firePoint);
            s.WakeUp();
        }
    }

    public List<Spell> GetSpells()
    {
        return spells.ToList();
    }

    public void SetSelectedSpell(int value)
    {
        selectedSpell = value;
    }

    public void Fire1()
    {
        if (canCast)
        {
            StartCoroutine(castFire1(cooldownTimer));
        }
    }

    public void Fire2(bool holding)
    {
        if (canCast)
        {
            StartCoroutine(castFire2(cooldownTimer, holding));
        }

    }
    IEnumerator castFire1(float second)
    {
        canCast = false;
        yield return new WaitForSeconds(0.7f);
        spells[selectedSpell].FireSimple();
        yield return new WaitForSeconds(second);
        canCast = true;
    }
    IEnumerator castFire2(float second, bool holding)
    {
        yield return new WaitForSeconds(1.2f);
        spells[selectedSpell].FireHold(holding);
        channeling = holding;
    }
}
