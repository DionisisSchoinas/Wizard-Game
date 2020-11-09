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
    private Transform simpleFirePoint;
    [SerializeField]
    private Transform channelingFirePoint;
    [SerializeField]
    private Spell[] spells;

    //=== Values must be equal with AnimationScriptControler ===
    public float castingAnimationSimple = 0.8f;
    public float castingAnimationSimpleReset = 1.4f;
    public float castingAnimationChannel = 1.3f;
    public float castingAnimationChannelReset = 1f;
    //=============
    public static bool channeling;
    public static bool castingBasic;

    private bool canCast;
    private int selectedSpell;
    private Coroutine runningCoroutine;

    private void Start()
    {
        castingBasic = false;
        channeling = false;
        canCast = true;
        foreach (Spell s in spells)
        {
            s.SetFirePoints(simpleFirePoint, channelingFirePoint);
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
            //start playing animation
            animationController.CastBasic(spells[selectedSpell].GetSource(), castingAnimationSimple, castingAnimationSimpleReset);
            //start spell attack
            StartCoroutine(castFire1(castingAnimationSimple, castingAnimationSimpleReset));
        }
    }

    public void Fire2(bool holding)
    {
        if (canCast || channeling)
        {
            //start playing animation
            animationController.CastChannel(holding, spells[selectedSpell].GetSource(), castingAnimationChannel, castingAnimationChannelReset);
            //start spell attack
            if (runningCoroutine != null) StopCoroutine(runningCoroutine);
            runningCoroutine = StartCoroutine(castFire2( (holding ? castingAnimationChannel : 0), holding));
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
        canCast = !holding;
        channeling = holding;
        yield return new WaitForSeconds(seconds);
        spells[selectedSpell].FireHold(holding);
    }
}
