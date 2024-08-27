using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    private bool dialogueActive = false;
    private bool inventoryActive = false;
    private bool isPaused = false;

    private static GameManager instance;
    public static GameManager Instance { get { return instance; } }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("More than one instance of GameManager found!");
            return;
        }
        instance = this;
    }

    public void SetDialogueActive(bool active)
    {
        PlayerController playerController = FindObjectOfType<PlayerController>();
        PlayerInput playerInput = playerController.GetComponent<PlayerInput>();
        playerInput.enabled = !active;
        playerController.enabled = !active;

        dialogueActive = active;
        Time.timeScale = active ? 0 : 1;

    }

    public void SetInventoryActive(bool active)
    {
        inventoryActive = active;
    }

    public void SetPause(bool active)
    {
        isPaused = active;
    }

    public bool IsDialogueActive()
    {
        return dialogueActive;
    }

}
