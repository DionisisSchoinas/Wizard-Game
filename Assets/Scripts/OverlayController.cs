using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OverlayController : MonoBehaviour
{
    [SerializeField]
    private Dropdown dropdown;
    [SerializeField]
    private Wand wand;

    // Start is called before the first frame update
    void Start()
    {
        List<string> spellNames = new List<string>();
        foreach (Spell s in wand.GetSpells())
        {
            spellNames.Add(s.name);
        }
        dropdown.ClearOptions();
        dropdown.AddOptions(spellNames);
        dropdown.interactable = false;
        dropdown.onValueChanged.AddListener(delegate {
            DropdownValueChanged(dropdown);
        });
    }

    void DropdownValueChanged(Dropdown change)
    {
        wand.SetSelectedSpell(change.value);
    }

    public void Enable(bool state)
    {
        dropdown.interactable = state;
    }
}
