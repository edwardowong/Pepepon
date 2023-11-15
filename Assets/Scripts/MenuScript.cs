using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuScript : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject settings;

    private void Start()
    {
        mainMenu.SetActive(true);
        settings.SetActive(false);
    }
    public void ToMenu()
    {
        mainMenu.SetActive(true);
        settings.SetActive(false);
    }

    public void ToSettings()
    {
        mainMenu.SetActive(false);
        settings.SetActive(true);
    }
}
