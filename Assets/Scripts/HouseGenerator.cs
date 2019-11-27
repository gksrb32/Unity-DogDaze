using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseGenerator : MonoBehaviour
{

    public int columns = 100;
    public int rows = 100;
    private float zOffset = 1f;

    public GameObject[] floor_tiles;
    public GameObject[] wall_tiles;

    private Transform board_holder;
    private List<List<GameObject[]>> frame;

    public void SetupHouseFrame() {
        board_holder = new GameObject("Board").transform;
        for (int i = 0; i < columns; i++) {
            for (int j = 0; j < rows; j++) {
                GameObject to_instantiate = floor_tiles[Random.Range(0, floor_tiles.Length)];
                if (i == 0 || j == 0 || i == columns-1 || j == rows-1) {
                    to_instantiate = wall_tiles[Random.Range(0, wall_tiles.Length)];
                }

                GameObject instance = Instantiate(to_instantiate, new Vector3(i,j,0f), Quaternion.identity) as GameObject;
                instance.transform.SetParent(board_holder);
            }
        }
    }

    public void seralizeHouse(int width, int height, int floors) {
        // Game Objects here are a list of tiles to choose from
        List<List<GameObject[]>> ret = new List<List<GameObject[]>>();

        for( int i=0; i < width; i++ ) {
            ret.Add(new List<GameObject[]>());
            for( int j=0; j < height; j++ ) {
                ret[i].Add(floor_tiles);
            }
        }

        frame = ret;
    }


    public void materializeHouse() {
        
        board_holder = new GameObject("Board").transform;

        for(int i=0; i < frame.Count; i++){
            for(int j=0; j < frame[i].Count; j++) {

                GameObject instance = Instantiate(
                    frame[i][j][Random.Range(0, frame[i][j].Length)],
                    new Vector3(i,j,zOffset),
                    Quaternion.identity
                ) as GameObject;
                instance.transform.SetParent(board_holder);
            }
        }
    }

    public void deMateralizeHouse(){
        Destroy(board_holder);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
