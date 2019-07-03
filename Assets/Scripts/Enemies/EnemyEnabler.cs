using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyEnabler : MonoBehaviour
{
    public bool larryEnabled, doctorEnabled, peanutEnabled, shyGuyEnabled = true;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if(scene.buildIndex == 1)
        {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("EnemyNPC");
            foreach(GameObject enemy in enemies)
            {
                switch(enemy.name)
                {
                    case "scp096":
                        enemy.SetActive(shyGuyEnabled);
                        break;
                    case "scp106":
                        enemy.SetActive(larryEnabled);
                        break;
                    case "scp049":
                        enemy.SetActive(doctorEnabled);
                        break;
                    case "scp173":
                        enemy.SetActive(peanutEnabled);
                        break;
                }
            }
        }
    }

    public void setLarryEnabled(bool enable)
    {
        larryEnabled = enable;
    }
    public void setDoctorEnabled(bool enable)
    {
        doctorEnabled = enable;
    }
    public void setPeanutEnabled(bool enable)
    {
        peanutEnabled = enable;
    }
    public void setShyGuyEnabled(bool enable)
    {
        shyGuyEnabled = enable;
    }
}
