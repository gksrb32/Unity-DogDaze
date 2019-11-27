using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SceneBuilder : MonoBehaviour
{

    public GameMaster gm;
    public GameObject floor_tiles;
    public GameObject wall_tiles;
    public GameObject door_tiles;

    public GameObject door1;
    public GameObject door2;

    public Sprite[] sprite_list;

    public IDictionary<string, int[]> sprite_mapper;

    

    protected float zOffset = 1;

    public List<List<GameObject>> frame;

    public List<GameObject> topFrameObjects; // furnitures, items, etc.
    public List<List<int>> topFramePositions;

    public int columns;
    public int rows;

    public Transform board_holder_transform;

    private List<GameObject> materialize_objects;

    public abstract void serealize();

    public void materialize() {
        materialize_objects = new List<GameObject>();

        for(int i=0; i < frame.Count; i++) {
            for(int j=0; j < frame[i].Count; j++) {

                if (frame[i][j] == null)
                    continue;

                // print("making tile");

                GameObject instance = Instantiate(
                    frame[i][j],
                    new Vector3(i,j,zOffset),
                    Quaternion.identity
                ) as GameObject;

                // instance.GetComponent<SpriteRenderer>().sprite = "";

                instance.transform.SetParent(board_holder_transform);
                materialize_objects.Add(instance);
            }
        }


        for (int i=0; i < topFrameObjects.Count; i++)
        {
            int x = topFramePositions[i][0];
            int y = topFramePositions[i][1];

            GameObject instance = Instantiate(
                topFrameObjects[i],
                new Vector3(x, y, zOffset-0.5f),
                Quaternion.identity
            ) as GameObject;

            // instance.GetComponent<SpriteRenderer>().sprite = "";

            instance.transform.SetParent(board_holder_transform);
            materialize_objects.Add(instance);
        }

    }

    public void deMaterialize() {
        for(int i=0; i < materialize_objects.Count; i++) {
            Destroy(materialize_objects[i]);
        }
    }

    public void Start() {
        // print("starting Scene Builder");
        Debug.Log("Starting Scene Builder");
        frame = new List<List<GameObject>>();
    }

}


