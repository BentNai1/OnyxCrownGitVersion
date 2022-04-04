using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowHideMarker_Script : MonoBehaviour
{
    public GameObject Marker;
    public ParticleSystem Particles;
    //private bool onOff = false;
    [SerializeField]
    private float disappearTime;
    private float disappearTimer;
    private bool disappearing;

    private void Start()
    {
        Marker.SetActive(false);
    }

    private void Update()
    {
        //if(Input.GetKeyDown("q") )
        //{
        //    ShowHideMarker(!onOff);
        //    onOff = !onOff;
        //}

        disappearTimer -= Time.deltaTime;
        if(disappearTimer <= 0)
        {
            ShowHideMarker(false);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "tagTrigger")
        {
            ShowHideMarker(true);
            disappearTimer = disappearTime;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "tagTrigger")
        {
            ShowHideMarker(false);
        }
    }

    public void ShowHideMarker(bool state)
    {
        Marker.SetActive(state);

        switch (state)
        {
            case true:
                Particles.Play();
                break;
            case false:
                Particles.Stop();
                break;
        }
    }
}
