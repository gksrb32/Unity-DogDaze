using System.Collections;
using System.Collections.Generic;
using System;
using Random = UnityEngine.Random;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class GameMaster : MonoBehaviour
{

    public const string game_state_start_loading_level = "StartLoadingLevel";
    public const string game_state_loading_level = "LoadingLevel";
    public const string game_state_playing_game = "PlayingGame";
    public const string game_state_change_nothing = "";
    public string cur_game_state = game_state_loading_level;
    public float levelStartDelay = 2f;
    private Text levelText;
    private GameObject canvasImage;
    Dictionary <string, string> nextStage = new Dictionary<string, string>();

	public bool doingSetup;

    SceneBuilder currentScene;


    public int house_count = 3;



    [Serializable]
    public class Materials {

        public GameObject street_floors;
        public GameObject street_walls;
        public GameObject street_house;
        public GameObject street_doors;
        public GameObject house_floors;
        public GameObject house_walls;
        public GameObject junkyard_floors;
        public GameObject junkyard_walls;
        public GameObject spray_bottle;
        public GameObject roomba;
        public GameObject tennis_ball;
        public GameObject house_door1;
        public GameObject house_door2;
    }





    [Serializable]
    public class HouseComponents{
        public GameObject floor_sprite_holder;
        public GameObject wall_sprite_holder;
        public GameObject entrance_sprite_holder;
        public GameObject special_item_sprite_holder;

    }
    public  HouseComponents houseComponents;

    public Sprite[] spriteListHouse1;
    public Sprite[] spriteListHouse2;
    public Sprite[] spriteListHouse3;
    public Sprite[] streetSpriteList;

    List<Sprite[]> rawHouseSprites;

    IDictionary<string, int[]> sprite_mapper;

    public GameObject akMusicPlayer; 
    
    public AK.Wwise.Switch city;
    public AK.Wwise.Switch greyhound;
    public AK.Wwise.Switch cat;
    public AK.Wwise.Switch dogFather; 


    private void OnLevelWasLoaded(int index) {
    }

    private List<HouseBuilder> houses;
    private StreetBuilder street;
    public Materials materials = new Materials();

    private Transform board_holder_transform;

    private GameObject player;
	public GameObject[] enemyPrefabs;
	private List<GameObject> enemies;


    /**
      Map List:
      - Street
      - Junkyard
      - Houses x3
     */


    private SceneBuilder SceneFactory(
        int house_index, string type, Transform transform) {


        if (type == "house") {
            HouseBuilder builder = new HouseBuilder();
            //builder.floor_tiles = materials.house_floors;
            //builder.wall_tiles = materials.house_walls;
            builder.houseComponents = houseComponents;
            builder.columns = 20;
            builder.rows = 20;
            builder.sprite_list = rawHouseSprites[0]; // hardcoded for now cus we only have 1 list of sprites
            builder.sprite_mapper = sprite_mapper;
            builder.board_holder_transform = transform; 
            builder.door1 = materials.house_door1;
            builder.door2 = materials.house_door2;    
            builder.gm = this;       
            return builder;
        }
        else if (type == "street") {
            StreetBuilder builder = new StreetBuilder();
            builder.floor_tiles = materials.street_floors;
            builder.wall_tiles = materials.street_walls;
            builder.door_tiles = materials.street_doors;
            builder.columns = 30;
            builder.rows = 80;
            builder.board_holder_transform = transform;
            builder.door1 = materials.house_door1;
            builder.door2 = materials.house_door2;
            builder.gm = this;
            return builder;
        }
        else {
            return null;
        }
    }

    public void EnterScene(string location) {

        for (int i = 0; i < enemies.Count; i++) {
            Destroy(enemies[i]);
        }

        currentScene.deMaterialize();

        Debug.Log("EnterScene received: " + location);

        switch (location) {

            case "house0": //Greyhound
                Debug.Log("Entering Scene: House0");
                greyhound.SetValue(akMusicPlayer);
                currentScene = houses[0];
                currentScene.materialize();
                player.transform.position = new Vector3(10,1,-1f);
                SpawnEnemies(0);
                
                break;
            
            case "street":
                Debug.Log("Entering Scene: Street");
                city.SetValue(akMusicPlayer);
                currentScene = street;
                currentScene.materialize();
                player.transform.position = new Vector3(15,0,-1f);
                
                break;

            case "house1": //Cat
                Debug.Log("Entering Scene: House1");
                cat.SetValue(akMusicPlayer);
                currentScene = houses[1];
                currentScene.materialize();
                player.transform.position = new Vector3(10,1,-1f);
                SpawnEnemies(1);
                
                break;

            case "house2": //Bulldog (Final Boss)
                Debug.Log("Entering Scene: House2");
                dogFather.SetValue(akMusicPlayer);
                SceneManager.LoadScene("BOSSHOUSE");
                player.transform.position = new Vector3(-11, 4, -1);
                SpawnEnemies(2);
                //currentScene = houses[2];
                //currentScene.materialize();x
                //player.transform.position = new Vector3(10,1,-1f);
                //SpawnEnemies(2);
               
                break;

            case "junkyard": 
                Debug.Log("Entering Scene: Junkyard");
                city.SetValue(akMusicPlayer);
                break;
            
            default:
                Debug.Log("Your scene dun fked up");
                break;
            
        }
    }


    private void BuildWorld() {


        // Build Houses
        houses = new List<HouseBuilder>();
        board_holder_transform = new GameObject("Board").transform;
        for (int i=0; i < house_count; i++) {
            HouseBuilder hg = SceneFactory(1, "house", board_holder_transform) as HouseBuilder;
            hg.serealize();
            houses.Add(hg);
        }
        // TEMP - JUST GET DOOR
        // materials.street_doors.GetComponent<SpriteRenderer>().sprite = houses[0].sprite_list[sprite_mapper["door"][0]];
        //houses[0].materialize();
        //houses[0].deMaterialize();


        //Build Street
        street = SceneFactory(1, "street", board_holder_transform) as StreetBuilder;
        street.serealize();

        currentScene = street;
        currentScene.materialize();
        //street_builder.materialize();
        // player.transform.position = new Vector3(15, 0, -1f);


        // Build Street
        // StreetBuilder street_builder = new StreetBuilder();
        // street_builder.board_holder_transform = board_holder_transform;
        // street_builder.floor_tiles = materials.street_floors;
        // street_builder.wall_tiles = materials.street_walls;
        // street_builder.seralizeStreet();
        // street_builder.materalizeStreet();

        // player.transform.position = new Vector3(100,0,-1f);

    }

	void SpawnEnemies(int houseNumber)
	{
        GameObject enemyType = enemyPrefabs[houseNumber];
        
		for (int i = 0; i < 3; i++)
		{
			GameObject enemy = Instantiate(enemyType, new Vector3(15,15, -1f), Quaternion.identity);
            //enemy.GetComponent<PlayerMovement>().gameMaster = this;
            enemies.Add(enemy);

		}
        if (houseNumber == 3)
        {
            for (int i = 0; i < 3; i++)
            {
                GameObject enemy = Instantiate(enemyPrefabs[i], new Vector3(15, 15, -1f), Quaternion.identity);
                //enemy.GetComponent<PlayerMovement>().gameMaster = this;
                enemies.Add(enemy);
            }
            GameObject boss = Instantiate(enemyPrefabs[3], new Vector3(15, 15, -1f), Quaternion.identity);
            enemies.Add(boss);
        }
	}


	void Awake() {
		player = GameObject.FindWithTag("Player");
        //player.GetComponent<PlayerMovement>().gameMaster = this;

        enemies = new List<GameObject>();

        sprite_mapper = new Dictionary<string, int[]>();
        sprite_mapper.Add( "floor", new [] {16,18,19} );
        sprite_mapper.Add( "wall", new [] {8, 10, 11});
        sprite_mapper.Add( "door", new [] {54, 62});
		sprite_mapper.Add("specialitem", new[] { 46 });

        rawHouseSprites = new List<Sprite[]>();
        rawHouseSprites.Add(spriteListHouse1);
        rawHouseSprites.Add(spriteListHouse2);
        rawHouseSprites.Add(spriteListHouse3);



        cur_game_state = game_state_start_loading_level;
        canvasImage = GameObject.Find("LevelCanvas");

		BuildWorld();
    }

    public void UpdateCanvas(String destination, bool success, int level)
	{
		Text canvasText = canvasImage.transform.Find("Image").transform.Find("DisplayText").GetComponent<Text>();

		if (destination == "Street" && success)
        {
			canvasText.text = "Success!";
        }
        else if (destination == "Street" && !success)
		{
			canvasText.text = "Failed..";
		}
        else if (destination == "House")
		{
			canvasText.text = "Level " + level.ToString();
		}

	}



    // Start is called before the first frame update
    void Start()
    {            

    }

    public void SetCurrentGameState(String newState) {
        cur_game_state = newState;
    }
    // Update is called once per frame
    void Update()
    {

        /* State Definitions */
        switch (cur_game_state) {
            case game_state_start_loading_level:
                cur_game_state = game_state_loading_level;
                canvasImage.SetActive(true);
                StartCoroutine( changeStateCo(game_state_playing_game, 0.5f) );
                break;

            case game_state_loading_level:
				doingSetup = true;
				canvasImage.SetActive(true);
                break;

            case game_state_playing_game:
				doingSetup = false;
                canvasImage.SetActive(false);
				cur_game_state = game_state_change_nothing;
				break;

            default:
                break;
        }
    }

    IEnumerator changeStateCo(String state_name, float delayTime) {

        yield return new WaitForSeconds(delayTime);
        cur_game_state = state_name;
    }

    // void ChangeGameStates() {
    //     /* Next State Logic */

    //     switch (cur_game_state) {
    //         case game_state_playing_game:
    //             break;
            
    //         case game_state_loading_level:
    //             break;
    //     }

    // }
}