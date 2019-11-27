﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


public class HouseBuilder : SceneBuilder
{

    private string specialItem;

    GameObject wall_tile;
    GameObject floor_tile;
    GameObject special_item_tile;

    public GameMaster.HouseComponents houseComponents;

    // void Start() {
    //     wall_tile = new GameObject();                   
    //     floor_tile = new GameObject();
    //     special_item_tile = new GameObject();

    //     wall_tile.AddComponent<SpriteRenderer>();
    //     floor_tile.AddComponent<SpriteRenderer>();
    //     special_item_tile.AddComponent<SpriteRenderer>();

    //     wall_tile.AddComponent<BoxCollider2D>();
    // }

    public override void serealize() {
        // Game Objects here are a list of tiles to choose from
        List<List<GameObject>> ret = new List<List<GameObject>>();
        frame = new List<List<GameObject>>();
        topFrameObjects = new List<GameObject>();
        topFramePositions = new List<List<int>>();

        wall_tile = new GameObject();                   
        floor_tile = new GameObject();
        special_item_tile = new GameObject();

        wall_tile.AddComponent<SpriteRenderer>();
        floor_tile.AddComponent<SpriteRenderer>();
        special_item_tile.AddComponent<SpriteRenderer>();

        wall_tile.AddComponent<BoxCollider2D>();
        wall_tile.GetComponent<BoxCollider2D>().size = new Vector2(1,1);


        for( int i=0; i < columns; i++ ) {
            frame.Add(new List<GameObject>());
            for( int j=0; j < rows; j++ ) {
                if (i == 0 || j == 0 || i == columns-1 || j == rows-1) {
                    wall_tile.GetComponent<SpriteRenderer>().sprite = sprite_list[sprite_mapper["wall"][0]];
                    frame[i].Add(wall_tile);
                }
                else {
                    floor_tile.GetComponent<SpriteRenderer>().sprite = sprite_list[sprite_mapper["floor"][0]];
                    frame[i].Add(floor_tile);    
                }
            }
        }



		special_item_tile.GetComponent<SpriteRenderer>().sprite = sprite_list[sprite_mapper["specialitem"][0]];
        if (special_item_tile.GetComponent<BoxCollider2D>() == null)
		{
			special_item_tile.AddComponent<BoxCollider2D>();
		}
		special_item_tile.GetComponent<BoxCollider2D>().isTrigger = true;
		special_item_tile.GetComponent<BoxCollider2D>().tag = "SpecialItem";

        Debug.Log(topFrameObjects);

		topFrameObjects.Add(special_item_tile);
		topFramePositions.Add(new List<int>(new int[] { 6, 6 }));




        // Add Walls
        // Create middle divider
        int row_divider = Random.Range(4, rows-4);
        CreateWalls("horizontal", new int[]{0, columns}, new int[]{row_divider, row_divider+1});

        // Create Divider in either top or bottom 1
        int column_divider1 = Random.Range(4, columns - 4);
        CreateRandomVerticalWall(row_divider, column_divider1);

        // Create Divider in either top or bottom 2 
        int column_divider2;
        if (column_divider1 > columns/2) {
            column_divider2 = Random.Range(4, column_divider1 - 4);
            CreateRandomVerticalWall(row_divider, column_divider2);
        }
        
        else {
            column_divider2 = Random.Range(column_divider1 + 4, columns);
            CreateRandomVerticalWall(row_divider, column_divider2);
        }



        // Add Doors
        frame[8][0] = Instantiate(door1);
        frame[8][1] = Instantiate(door2);

        frame[8][1].GetComponent<Doors>().destination = "street";
        frame[8][1].GetComponent<Doors>().gm = gm;
    }

    void CreateRandomVerticalWall(int horizontal_wall_location, int new_column_location) {
        int column_divider1 = Random.Range(4, columns - 4);
        switch (Random.Range(0,2)) {
            case 0:
                CreateWalls("vertical", new int[]{new_column_location, new_column_location+1}, new int[]{0, horizontal_wall_location});
                break;

            case 1:
                CreateWalls("vertical", new[]{new_column_location, new_column_location+1}, new int[]{horizontal_wall_location, rows});
                break;

            default:
                Debug.Log("Random fked up");
                break;
        }
    }

    void CreateWalls(string orientation, int[] xRange, int[] yRange) {

        int total_index_counter = 0;


        switch (orientation) {
            case "horizontal":
                for (int i = xRange[0]; i < xRange[1]; i++) {
                    for (int j = yRange[0]; j < yRange[1]; j++) {
                        total_index_counter += 1;
                        if (total_index_counter < 5 && total_index_counter > 2)
                            continue;
                        frame[i][j] = wall_tile;
                    }
                }
                break;

            case "vertical":
                for (int i = xRange[0]; i < xRange[1]; i++) {
                    for (int j = yRange[0]; j < yRange[1]; j++) {
                        total_index_counter += 1;
                        if (total_index_counter < 5 && total_index_counter > 2)
                            continue;
                            
                        frame[i][j] = wall_tile;
                    }
                } 

                break;

            default:
                Debug.Log("You dun fuked up.");
                break;
        }
    }
}

