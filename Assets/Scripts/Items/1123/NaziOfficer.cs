using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NaziOfficer : MonoBehaviour
{
    private AudioSource naziSounds;
    private Animator naziAnims;
    [SerializeField] Rigidbody roomDoor;
    [SerializeField] AudioClip[] roomSpeech;
    [SerializeField] AudioClip killSpeech, gunshot;
    private float doorForce = 10;
    private bool facePlayer = false;
    private GameObject player;

    private void Awake()
    {
        naziSounds = GetComponent<AudioSource>();
        naziAnims = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
    }



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void openRoomDoor()
    {
        StartCoroutine(doOpenRoomDoor());
    }

    private IEnumerator doOpenRoomDoor()
    {
        yield return new WaitForSeconds(10f);
        roomDoor.AddForce(-roomDoor.transform.right * doorForce, ForceMode.VelocityChange);
        yield return new WaitForSeconds(0.75f);
        naziSounds.PlayOneShot(roomSpeech[0]);
        yield return new WaitForSeconds(roomSpeech[0].length + 3f);
        naziSounds.PlayOneShot(roomSpeech[1]);
    }

    public void equipGun()
    {
        naziAnims.SetTrigger("equip_gun");
        naziAnims.SetBool("hasGunEquipt", true);
    }

    public void shootGun(Transform oldPos)
    {
        naziAnims.SetTrigger("shoot_gun");
        StartCoroutine(shootPlayer(oldPos));
    }

    IEnumerator shootPlayer(Transform oldPos)
    {
        facePlayer = true;
        StartCoroutine(lookAtPlayer());
        float time = 0f;
        RuntimeAnimatorController ac = naziAnims.runtimeAnimatorController;    //Get Animator controller
        for (int i = 0; i < ac.animationClips.Length; i++)                 //For all animations
        {
            if (ac.animationClips[i].name == "load_gun_slowmo")        //If it has the same name as your clip
            {
                time = ac.animationClips[i].length;
            }
        }
        yield return new WaitForSeconds(time);
        facePlayer = false;
        player.GetComponent<PlayerEvents>().playSound(gunshot);
        player.transform.position = oldPos.position;
        player.transform.rotation = oldPos.rotation;
    }

    IEnumerator lookAtPlayer()
    {
        while(facePlayer)
        {
            transform.LookAt(new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z));
            yield return null;
        }
    }
}
