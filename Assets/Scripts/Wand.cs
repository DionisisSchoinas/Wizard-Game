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
    private GameObject marker;
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
        mark = Instantiate(marker);
        mark.GetComponent<MeshRenderer>().enabled = false;
    }

    private void FixedUpdate()
    {
        RaycastHit hit;
        if (Physics.Raycast(firePoint.position, firePoint.TransformDirection(Vector3.forward), out hit))
        {
            mark.GetComponent<MeshRenderer>().enabled = true;
            ray.origin = view.transform.position;
            ray.direction = (hit.point - view.transform.position).normalized;
            mark.transform.position = ray.GetPoint(1);
        }
        else
        {
            mark.GetComponent<MeshRenderer>().enabled = false;
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
