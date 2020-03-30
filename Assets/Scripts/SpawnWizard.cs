using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpawnWizard : MonoBehaviour
{
    public GameObject wizard;
    public GameObject topDownMainCam;

    public Button summonWizard;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        summonWizard.onClick.AddListener(createWizard);
        PlayerStats.onCharDeath += onPlayerDeath;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void createWizard()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        Vector3 spawnPos = new Vector3(0, 1, 0.01f);
        wizard.transform.GetChild(0).gameObject.SetActive(true);
        Instantiate(wizard, spawnPos, Quaternion.identity);
        topDownMainCam.SetActive(false);
        gameObject.SetActive(false);
    }

    void onPlayerDeath()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        topDownMainCam.SetActive(true);
        gameObject.SetActive(true);
    }
}
