using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class PlayerController : MonoBehaviourPun, IPunObservable
{
	private Animator anim;
	private Rigidbody rb;
	private Transform pointer; //캐릭터가 쳐다볼 곳
	private Transform shotpoint; //투사체 생성될 곳


	private float hp = 100;
	private int shotCount = 0;

	public float moveSpeed; //이동속도
	public float shotPower; //투사체 발사 파워

	public Text hpText; //채력을 표시할 text
	public Text shotText; //발사 횟수를 표시할 text
	public Bomb bombprefab;

    private void Awake()
    {
		anim = GetComponent<Animator>();	
        rb = GetComponent<Rigidbody>();
		pointer = transform.Find("PlayerPointer");
		shotpoint = transform.Find("ShotPoint");
		tag = photonView.IsMine ? "Player" : "Enemy";
	}

	private void Update()
    {
		hpText.text = hp.ToString();
        shotText.text = shotCount.ToString();
        if (false == photonView.IsMine) return;
		Move();
		if (Input.GetButtonDown("Fire1"))
		{
			photonView.RPC("Fire", RpcTarget.All, shotpoint.position, shotpoint.forward);
			shotCount++;
			anim.SetTrigger("Attack");
		}
    }

    private void FixedUpdate()
    {
		if (false == photonView.IsMine) return;
        Rotate();
    }

    private void Move()
    {
		float x = Input.GetAxis("Horizontal");
		float z = Input.GetAxis("Vertical");
		rb.velocity = new Vector3(x, 0, z) * moveSpeed;

		anim.SetBool("IsMoving", rb.velocity.magnitude > 0.01f);
    }

	private void Rotate()
	{
		Vector3 pos = rb.position;
		pos.y = 0;
		Vector3 forword = pointer.position - pos;
		rb.angularVelocity = Vector3.zero;	
		rb.rotation = Quaternion.LookRotation(forword, Vector3.up);
	}

	private void Hit(float damage)
	{
		hp -= damage;
		
	}

	private void Heal(float amount)
	{
		hp += amount;
		
	}


	//Fire를 통해서 생성하는 bomb 객체는 "데드레커닝"(추측항법 알고리즘)을 통해서 각 클라이언트들이 직접 생성하고,
	//Fire 함수를 호출 받는 시점을 온라인에서 원겨으로 호출받음 (Remote procedure Call)
	[PunRPC]
	private void Fire(Vector3 shotpoint, Vector3 shotDir, PhotonMessageInfo info)
	{
		print($"Fire Procedure called by {info.Sender.NickName}");
		print($"my local Time:{PhotonNetwork.Time}");
		print($"server time when procedure called : {info.SentServerTime}");

		//"지연보상" : (추측항법을 위해) rpc를 받은 시점은 서버에서 호출된 시간보다 항상 늦기 때문에
		//헤당 지연시간 만큼 위치, 또는 연산량을 보정해주어야 최대한 원격에서의 플레이가 동기화가 될 수 있음.

		//보정해야 할 지연시간 
		float lag = (float)(PhotonNetwork.Time - info.SentServerTime);

		Bomb bomb = Instantiate(bombprefab, shotpoint, Quaternion.identity);
		bomb.rb.AddForce(shotDir * shotPower, ForceMode.Impulse);
		bomb.owner = photonView.Owner;

		//지연보상들어간다
		bomb.rb.position += bomb.rb.velocity * lag;

	}

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    { //stream을 통해 주고받는 데이터는 server에서 받는 시간 기준으로 queue형태로 전달
	  //데이터 자체도 큐
        if(stream.IsWriting) //내 데이터를 서버로 보냄
		{
			stream.SendNext(hp);
			stream.SendNext(shotCount);
		}
		else
		{
			hp = (float)stream.ReceiveNext();
			shotCount = (int)stream.ReceiveNext();
		}
    }
}
