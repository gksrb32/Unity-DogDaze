using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StreetBuilder : SceneBuilder
{

    public override void serealize() {
        frame = new List<List<GameObject>>();

        topFrameObjects = new List<GameObject>();
        topFramePositions = new List<List<int>>();

        for (int i=0; i<columns; i++) {
            frame.Add(new List<GameObject>());
            for (int j=0; j<rows; j++ ) {

                if (i > 10 && i < 20) {
                    frame[i].Add(floor_tiles);
                }

                else if (i <= 10 && j > 20 && j < 30) {
                    frame[i].Add(floor_tiles);
                }

                else if (i >= 20 && j > 40 && j < 50) {
                    frame[i].Add(floor_tiles);
                }

                else if ( j > 60 ) {
                    frame[i].Add(floor_tiles);
                }

                else {
                    frame[i].Add(null);
                }
            }
        }


        /* Add Walls */
        for (int i=0; i < columns; i++)
        {
            for (int j=0; j < rows; j++)
            {
                if (j>=0 && j<20)
                {
                    if (i == 10 || i == 20)
                    {
                        frame[i][j] = wall_tiles;
                    }
                } else if(j==20 || j==30)
                {
                    if (i<=10 || i==20)
                    {
                        frame[i][j] = wall_tiles;
                    }
                } else if(j>20 && j<30)
                {
                    if (i == 20)
                    {
                        frame[i][j] = wall_tiles;
                    }
                }
                else if(j>30 && j<40)
                {
                    if (i==10 || i==20)
                    {
                        frame[i][j] = wall_tiles;
                    }
                } else if(j==40 || j==50)
                {
                    if (i==10 || i>=20)
                    {
                        frame[i][j] = wall_tiles;
                    }
                } else if(j>40 && j < 50)
                {
                    if (i == 10)
                    {
                        frame[i][j] = wall_tiles;
                    }
                } else if(j>50 && j<60)
                {
                    if (i==10 || i==20)
                    {
                        frame[i][j] = wall_tiles;
                    }
                } else if(j==60)
                {
                    if (i<=10 || i >= 20)
                    {
                        frame[i][j] = wall_tiles;
                    }
                } else if(j>60 && j < 80)
                {
                    if (i==0 || i == 29)
                    {
                        frame[i][j] = wall_tiles;
                    }
                }

            }
        }

        // Question: Will frame point to same instances?
        // maybe have to use new_var = Instantiate();

        /* Add Door1 */
        frame[0][25] = Instantiate(door1);
        frame[0][26] = Instantiate(door2);
        frame[0][25].GetComponent<Doors>().destination = "house0";
        frame[0][25].GetComponent<Doors>().gm = gm;

        // door1.GetComponent<Doors>().destination = "house0";
        // door1.GetComponent<Doors>().gm = gm;

        /* Add Door2 */        
        frame[29][45] = Instantiate(door1);
        frame[29][46] = Instantiate(door2);
        frame[29][45].GetComponent<Doors>().destination = "house1";
        frame[29][45].GetComponent<Doors>().gm = gm;

        frame[25][60] = Instantiate(door1);
        frame[25][61] = Instantiate(door2);
        frame[25][60].GetComponent<Doors>().destination = "house2";
        frame[25][61].GetComponent<Doors>().gm = gm;
            
    }
   
}
