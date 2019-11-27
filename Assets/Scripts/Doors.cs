using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Doors : MonoBehaviour
{
    public string destination;
    public GameMaster gm;

    private GameObject player;

    void Start() {
        player = GameObject.FindGameObjectWithTag("Player");

        Debug.Log("Created Door to:" + destination);
    }
    
    void OnTriggerEnter2D(Collider2D c) 
    {
        // if (gm.cur_game_state != GameMaster.game_state_playing_game)
        //     return;

        // PolygonCollider2D collider = c.GetComponent<PolygonCollider2D>();
        if (c.tag == "Player") {
            gm.SetCurrentGameState(GameMaster.game_state_start_loading_level);
            gm.EnterScene(destination);
        }
    }
}
