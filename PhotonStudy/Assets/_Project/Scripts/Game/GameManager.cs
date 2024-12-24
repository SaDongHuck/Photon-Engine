using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public Transform PlayrPositions;
    public static bool isGameReady;
    private void Awake()
    {
        Instance = this;
    }

    //Photon에서 컨트롤 동기화 하는 방법
    //1. 프리팹에서 phtonview 컴포넌트를 붙이고, phtotnNetwork.Instatiate를 통해 원격 클라이언트들에게도
    // 동기화된 오브젝트르를 생성하도록 함
    //2. phtonview가 Observing 할 수 있도록 view 컴포넌트를 부착
    //3.내 view가 부착되지 않은 오브젝트는 내가 제어하지 않도록

    private IEnumerator Start()
    {
        yield return new WaitUntil(() => isGameReady);
        yield return new WaitForSeconds(1f);
        //GetPlayerNumber 확장함수 : 포톤 네트워트에 연결된 다른 플레이어들 사이에서 동기화 된 플레이어 번호.
        //Actor Number와 다름(scene마다 선착순으로 0~플레이어 수 만큼 부여됨)
        //GetPlayerNumber 확장함수가 동작하기 위해서는 씬에 PlayerNumbering 컴포넌트가 필요하다.
        int playerNum = PhotonNetwork.LocalPlayer.GetPlayerNumber();
        Vector3 PlayerPos = PlayrPositions.GetChild(playerNum).position;
        GameObject playrobj = PhotonNetwork.Instantiate("Player", PlayerPos, Quaternion.identity);
        playrobj.name = $"Player {playerNum}";

        //이 밑에서는 내가 MasterClient가 아니면 동작하지 않음
        if(false == PhotonNetwork.IsMasterClient)
        {
            yield break;
        }
        //Master Client만 5초마다 Pill을 PhotonNetwork를 통해 Instatitate
        while (true)
        {

            //photonNetwork.Instantiate를 통해 생성할 경우, positon과 rotation이 반드시 필요
            Vector3 spawnPos = Random.insideUnitSphere * 15;
            spawnPos.y = 0;
            Quaternion SpawnRot = Quaternion.Euler(0, Random.Range(0, 180f), 0);

            //각 pill마다 radom color(Color)와 random healamount(float)를 주입하고 싶으면?

            Vector3 color = new Vector3(Random.value, Random.value, Random.value);
            float healamount = Random.Range(10f, 30f);

            PhotonNetwork.Instantiate("Pill", spawnPos, SpawnRot, data: 
                new object[] {color, healamount });

            yield return new WaitForSeconds(5f);
        }
    }
}
