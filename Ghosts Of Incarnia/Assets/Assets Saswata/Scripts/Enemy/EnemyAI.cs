using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Assertions.Must;
//gg
public class EnemyAI : MonoBehaviour
{
    [SerializeField]float roamChangeDir = 2f;
    private enum State{
        Roaming
    }
    private State state;
    private EnemyPathfinding enemyPathfinding;
    private void Awake(){
        enemyPathfinding = GetComponent<EnemyPathfinding>();
        state = State.Roaming;
    }
    private void Start(){
        StartCoroutine(RoamingRoutine());
    }
    private IEnumerator RoamingRoutine(){
        while(state==State.Roaming){
            Vector2 roamPosition = GetRoamingPosition();
            enemyPathfinding.MoveTo(roamPosition);
            yield return new WaitForSeconds(roamChangeDir);
        }
    }
    private Vector2 GetRoamingPosition(){
        return new Vector2(Random.Range(-1f,1f),Random.Range(-1f,1f)).normalized;   
    }
}
