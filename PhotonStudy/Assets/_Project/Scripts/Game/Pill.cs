using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pill : MonoBehaviourPun
{
	public Renderer render;

	private float healAmont; //힐량 랜덤

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
		{
			other.SendMessage("Heal", healAmont);
            
        }
        Destroy(gameObject);
    }

    private void Awake()
    {
        object[] parm = photonView.InstantiationData;

        if (parm != null)
        {
            Vector3 cv = (Vector3)parm[0];
            float healamount = (float)parm[1];

            render.material.color = new Color(cv.x, cv.y, cv.z);
            this.healAmont = healamount;
        }
    }

    private void Reset() {
		render = GetComponentInChildren<Renderer>();
	}
}

