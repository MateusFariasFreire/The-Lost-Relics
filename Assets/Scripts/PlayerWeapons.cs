using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapons : MonoBehaviour
{
    [SerializeField] private GameObject wand;
    [SerializeField] private Transform wandSpellPos;
    [SerializeField] private GameObject staff;
    [SerializeField] private Transform staffSpellPos;

    [SerializeField] private GameObject spellbook;

    [SerializeField] private ScenarioManager scenarioManager;

    private void Start()
    {
        wand.SetActive(scenarioManager.hasFoundWand && !scenarioManager.hasFoundStaff);
        staff.SetActive(scenarioManager.hasFoundStaff);
        spellbook.SetActive(scenarioManager.hasFoundBook);
    }

    private void Update()
    {
        wand.SetActive(scenarioManager.hasFoundWand && !scenarioManager.hasFoundStaff);
        staff.SetActive(scenarioManager.hasFoundStaff);
        spellbook.SetActive(scenarioManager.hasFoundBook);
    }

}
