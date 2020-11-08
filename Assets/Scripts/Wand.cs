using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Schema;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Wand : MonoBehaviour
{
    [SerializeField]
    private AnimationScriptController animationController;
    [SerializeField]
    private Transform firePoint;
    [SerializeField]
    private Spell[] spells;

    //=== Values must be equal with AnimationScriptControler ===
    public float castingAnimationSimple = 0.8f;
    public float castingAnimationSimpleReset = 1.4f;
    public float castingAnimationChannel = 1.5f;
    public float castingAnimationChannelReset = 1.5f;
    //=============
    public static bool channeling;
    public static bool castingBasic;
    public bool canCast;

    private int selectedSpell;

    private void Start()
    {
        castingBasic = false;
        channeling = false;
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
            animationController.CastBasic(spells[selectedSpell].GetSource());
            StartCoroutine(castFire1(castingAnimationSimple, castingAnimationSimpleReset));
        }
    }

    public void Fire2(bool holding)
    {
        if (canCast)
        {
            StartCoroutine(castFire2( (holding ? castingAnimationChannel : 0), holding));
        }

    }
    IEnumerator castFire1(float cast, float reset)
    {
        castingBasic = true;
        canCast = false;
        yield return new WaitForSeconds(cast);
        spells[selectedSpell].FireSimple();
        yield return new WaitForSeconds(reset);
        castingBasic = false;
        canCast = true;
    }
    IEnumerator castFire2(float seconds, bool holding)
    {
        yield return new WaitForSeconds(seconds);
        spells[selectedSpell].FireHold(holding);
        channeling = holding;
    }
}
