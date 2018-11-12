using System.Collections;
using UnityEngine;

public class KeycardAuth : MonoBehaviour {
    public MeshRenderer[] cardMat;
    public Material[] keycardMats;
    [SerializeField] int lvl = 1;
    public int Lvl
    {
        get
        {
            return lvl;
        }
        set
        {
            lvl = value;
            foreach(MeshRenderer mat in cardMat)
            {
                mat.material = keycardMats[lvl - 1];
            }
        }
    }
	// Use this for initialization
	void Start () {
        Lvl = lvl;
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.U))
        {
            Lvl++;
        }
	}
}
