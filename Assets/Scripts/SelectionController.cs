using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionController : MonoBehaviour
{

    public GameObject selectionScreen;
    public GameObject characterScreen;
    public GameObject weaponScreen;
    public GameObject levelScreen;
    public GameObject mikeCharacter;
    public GameObject crewmateCharacter;
    public GameObject knife;
    public GameObject fireball;

    // Start is called before the first frame update
    void Start()
    {
        selectionScreen.SetActive(true);
        characterScreen.SetActive(false);
        weaponScreen.SetActive(false);
        levelScreen.SetActive(false);
        string playerCharacter = PlayerPrefs.GetString("Character");
        string weapon = PlayerPrefs.GetString("Weapon");   

        if (playerCharacter == "Mike" || !PlayerPrefs.HasKey("Character"))
        {
            mikeCharacter.SetActive(true);
            crewmateCharacter.SetActive(false);
        }
        else if (playerCharacter == "Crewmate")
        {
            mikeCharacter.SetActive(false);
            crewmateCharacter.SetActive(true);

        }

        if (weapon == "Knife" || !PlayerPrefs.HasKey("Weapon"))
        {
            knife.SetActive(true);
            fireball.SetActive(false);
        }
        else if (weapon == "Fireball")
        {
            knife.SetActive(false);
            fireball.SetActive(true);
        }
    }
    
    public void ChangeSelection(String input)
    {
        if (input == "Character")
        {
            characterScreen.SetActive(true);
        }
        else if (input == "Weapon")
        {
            weaponScreen.SetActive(true);
        }
        else if (input == "Level")
        {
            levelScreen.SetActive(true);
        }
        selectionScreen.SetActive(false);
    }
    public void ChangeCharacter(String input)
    {
        if (input == "Mike")
        {
            mikeCharacter.SetActive(true);
            crewmateCharacter.SetActive(false);

        }
        else if (input == "Crewmate")
        {
            mikeCharacter.SetActive(false);
            crewmateCharacter.SetActive(true);

        }
        selectionScreen.SetActive(true);
        characterScreen.SetActive(false);
        PlayerPrefs.SetString("Character", input);
    }

    public void ChangeWeapon(String input)
    {
        if (input == "Knife")
        {
            knife.SetActive(true);
            fireball.SetActive(false);

        }
        else if (input == "Fireball")
        {
            knife.SetActive(false);
            fireball.SetActive(true);
        }
        selectionScreen.SetActive(true);
        weaponScreen.SetActive(false);
        PlayerPrefs.SetString("Weapon", input);
    }
}
