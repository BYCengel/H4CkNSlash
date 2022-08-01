using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelGeneration : MonoBehaviour
{

    public bool copCanSpawn = false;

    public Text levelText;
    public Text chapterText;
    public GameObject canvasHud;
    public GameObject canvasPause;
    public int level;
    private int chapter;
    public int roomKill = 0;
    public bool firstRoom = true;
    private bool levelComplete = false;
    

    public static bool GameIsPaused = false;
    private int levelRoomNumber = 0;
    public int levelRoomCount = 0;
    Vector2 worldSize = new Vector2(4, 4);
    ArrayRoom[,] rooms;
    private List<ArrayRoom> AllRooms;
    List<Vector2> takenPositions = new List<Vector2>();
    int gridSizeX, gridSizeY, numberOfRooms = 3;

    public GameObject 	spU, spD, spR, spL, spUD, spRL, spUR, spUL, spDR, spDL, spULD, spRUL, spDRU, spLDR, spUDRL;// 0: normal, 1: enter

    void Awake(){
        LevelData savedLevel = SaveSystem.LoadLevel();
        if (savedLevel != null)
        {
            level = savedLevel.level;
            Debug.Log("Save varsa");
        }
        else
        {
            level = 1;
            Debug.Log("save Yoksa");
        }
    }
    
    void Start()
    {
        if (level % 5 != 0)
        {
            numberOfRooms += level;
        }
        else
        {
            numberOfRooms = 2;
        }
        
        if (numberOfRooms >= (worldSize.x * 2) * (worldSize.y * 2))
        {
            // make sure we dont try to make more rooms than can fit in our grid
            numberOfRooms = Mathf.RoundToInt((worldSize.x * 2) * (worldSize.y * 2));
        }

        levelRoomNumber = numberOfRooms;
        gridSizeX = Mathf.RoundToInt(worldSize.x); //note: these are half-extents
        gridSizeY = Mathf.RoundToInt(worldSize.y);
        CreateRooms(); //lays out the actual map
        SetRoomDoors(); //assigns the doors where rooms would connect
        DrawMap(); //instantiates objects to make up a map
    }
    
    void CreateRooms(){
        //setup
        rooms = new ArrayRoom[gridSizeX * 2,gridSizeY * 2];
        rooms[gridSizeX,gridSizeY] = new ArrayRoom(Vector2.zero, 1);
        takenPositions.Insert(0,Vector2.zero);
        Vector2 checkPos = Vector2.zero;
        //magic numbers
        float randomCompare = 0.2f, randomCompareStart = 0.2f, randomCompareEnd = 0.01f;
        //add rooms
        for (int i =0; i < numberOfRooms -1; i++){
            float randomPerc = ((float) i) / (((float)numberOfRooms - 1));
            randomCompare = Mathf.Lerp(randomCompareStart, randomCompareEnd, randomPerc);
            //grab new position
            checkPos = NewPosition();
            //test new position
            if (NumberOfNeighbors(checkPos, takenPositions) > 1 && Random.value > randomCompare){
                int iterations = 0;
                do{
                    checkPos = SelectiveNewPosition();
                    iterations++;
                }while(NumberOfNeighbors(checkPos, takenPositions) > 1 && iterations < 100);
                if (iterations >= 50)
                    print("error: could not create with fewer neighbors than : " + NumberOfNeighbors(checkPos, takenPositions));
            }
            //finalize position
            rooms[(int) checkPos.x + gridSizeX, (int) checkPos.y + gridSizeY] = new ArrayRoom(checkPos, 0);
            takenPositions.Insert(0,checkPos);
        }	
    }
    
    Vector2 NewPosition(){
        int x = 0, y = 0;
        Vector2 checkingPos = Vector2.zero;
        do{
            int index = Mathf.RoundToInt(Random.value * (takenPositions.Count - 1)); // pick a random room
            x = (int) takenPositions[index].x;//capture its x, y position
            y = (int) takenPositions[index].y;
            bool UpDown = (Random.value < 0.5f);//randomly pick wether to look on hor or vert axis
            bool positive = (Random.value < 0.5f);//pick whether to be positive or negative on that axis
            if (UpDown){ //find the position bnased on the above bools
                if (positive){
                    y += 1;
                }else{
                    y -= 1;
                }
            }else{
                if (positive){
                    x += 1;
                }else{
                    x -= 1;
                }
            }
            checkingPos = new Vector2(x,y);
        }while (takenPositions.Contains(checkingPos) || x >= gridSizeX || x < -gridSizeX || y >= gridSizeY || y < -gridSizeY); //make sure the position is valid
        return checkingPos;
    }
    
    Vector2 SelectiveNewPosition(){ // method differs from the above in the two commented ways
        int index = 0, inc = 0;
        int x =0, y =0;
        Vector2 checkingPos = Vector2.zero;
        do{
            inc = 0;
            do{ 
                //instead of getting a room to find an adject empty space, we start with one that only 
                //as one neighbor. This will make it more likely that it returns a room that branches out
                index = Mathf.RoundToInt(Random.value * (takenPositions.Count - 1));
                inc ++;
            }while (NumberOfNeighbors(takenPositions[index], takenPositions) > 1 && inc < 100);
            x = (int) takenPositions[index].x;
            y = (int) takenPositions[index].y;
            bool UpDown = (Random.value < 0.5f);
            bool positive = (Random.value < 0.5f);
            if (UpDown){
                if (positive){
                    y += 1;
                }else{
                    y -= 1;
                }
            }else{
                if (positive){
                    x += 1;
                }else{
                    x -= 1;
                }
            }
            checkingPos = new Vector2(x,y);
        }while (takenPositions.Contains(checkingPos) || x >= gridSizeX || x < -gridSizeX || y >= gridSizeY || y < -gridSizeY);
        if (inc >= 100){ // break loop if it takes too long: this loop isnt garuanteed to find solution, which is fine for this
            print("Error: could not find position with only one neighbor");
        }
        return checkingPos;
    }
    
    int NumberOfNeighbors(Vector2 checkingPos, List<Vector2> usedPositions){
        int ret = 0; // start at zero, add 1 for each side there is already a room
        if (usedPositions.Contains(checkingPos + Vector2.right)){ //using Vector.[direction] as short hands, for simplicity
            ret++;
        }
        if (usedPositions.Contains(checkingPos + Vector2.left)){
            ret++;
        }
        if (usedPositions.Contains(checkingPos + Vector2.up)){
            ret++;
        }
        if (usedPositions.Contains(checkingPos + Vector2.down)){
            ret++;
        }
        return ret;
    }
    
    void SetRoomDoors(){
        for (int x = 0; x < ((gridSizeX * 2)); x++){
            for (int y = 0; y < ((gridSizeY * 2)); y++){
                if (rooms[x,y] == null){
                    continue;
                }
                Vector2 gridPosition = new Vector2(x,y);
                if (y - 1 < 0){ //check above
                    rooms[x,y].doorBot = false;
                }else{
                    rooms[x,y].doorBot = (rooms[x,y-1] != null);
                }
                if (y + 1 >= gridSizeY * 2){ //check bellow
                    rooms[x,y].doorTop = false;
                }else{
                    rooms[x,y].doorTop = (rooms[x,y+1] != null);
                }
                if (x - 1 < 0){ //check left
                    rooms[x,y].doorLeft = false;
                }else{
                    rooms[x,y].doorLeft = (rooms[x - 1,y] != null);
                }
                if (x + 1 >= gridSizeX * 2){ //check right
                    rooms[x,y].doorRight = false;
                }else{
                    rooms[x,y].doorRight = (rooms[x+1,y] != null);
                }
            }
        }
    }
    
    void DrawMap(){
        foreach (ArrayRoom room in rooms){
            if (room == null){
                continue; //skip where there is no room
            }
            Vector2 drawPos = room.gridPos;
            drawPos.x *= 32;//aspect ratio of map sprite
            drawPos.y *= 26;
            //create map obj and assign its variables
            DrawTile(room.doorTop, room.doorBot, room.doorLeft, room.doorRight, drawPos);
        }
    }
    
    void DrawTile (bool up, bool down, bool left, bool right, Vector2 TilePos)
    { //picks correct sprite based on the four door bools
            if (up){
                if (down){
                    if (right){
                        if (left)
                        {
                            Instantiate(spUDRL, TilePos, Quaternion.identity);
                        }else{
                            Instantiate(spDRU, TilePos, Quaternion.identity);
                        }
                    }else if (left){
                        Instantiate(spULD, TilePos, Quaternion.identity);
                    }else{
                        Instantiate(spUD, TilePos, Quaternion.identity);
                    }
                }else{
                    if (right){
                        if (left){
                            Instantiate(spRUL, TilePos, Quaternion.identity);
                        }else{
                            Instantiate(spUR, TilePos, Quaternion.identity);
                        }
                    }else if (left){
                        Instantiate(spUL, TilePos, Quaternion.identity);
                    }else{
                        Instantiate(spU, TilePos, Quaternion.identity);
                    }
                }
                return;
            }
            if (down){
                if (right){
                    if(left){
                        Instantiate(spLDR, TilePos, Quaternion.identity);
                    }else{
                        Instantiate(spDR, TilePos, Quaternion.identity);
                    }
                }else if (left){
                    Instantiate(spDL, TilePos, Quaternion.identity);
                }else{
                    Instantiate(spD, TilePos, Quaternion.identity);
                }
                return;
            }
            if (right){
                if (left){
                    Instantiate(spRL, TilePos, Quaternion.identity);
                }else{
                    Instantiate(spR, TilePos, Quaternion.identity);
                }
            }else{
                Instantiate(spL, TilePos, Quaternion.identity);
            }
    }

    private void FixedUpdate()
    {
        
        levelText.text = level.ToString();
        chapter = (int) Mathf.Ceil(level / 5);
        chapter += 1;
        chapterText.text = chapter.ToString();
        
        if (levelRoomCount >= levelRoomNumber - 1 && levelComplete == false)
        {
            level += 1;
            SaveSystem.SaveLevel(this);
            canvasHud.SetActive(true);
            levelComplete = true;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        canvasPause.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    void Pause()
    {
        canvasPause.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    public void NextDungeon()
    {
        Time.timeScale = 1f;
        GameIsPaused = false;
        SceneManager.LoadScene("Aybars Level Generator");
    }

    public void BackToMenu()
    {
        Time.timeScale = 1f;
        GameIsPaused = false;
        SceneManager.LoadScene("Main Menu");
    }
    
}
