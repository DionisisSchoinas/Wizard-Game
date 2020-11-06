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

    private int selectedSpell;

    private Ray ray;
    private GameObject mark;


    private void Start()
    {
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
        spells[selectedSpell].FireSimple();
    }

    public void Fire2(bool holding)
    {
        spells[selectedSpell].FireHold(holding);
    }
}
