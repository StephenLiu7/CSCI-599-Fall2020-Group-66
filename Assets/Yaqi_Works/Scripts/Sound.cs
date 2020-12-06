using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sound : MonoBehaviour
{
    public AudioSource bloodSound;
    public AudioSource changeGunSound1;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void playSoundEffect()
    {
        bloodSound.Play();
    }

    public void playSoundEffect1()
    {
        changeGunSound1.Play();
    }

}
