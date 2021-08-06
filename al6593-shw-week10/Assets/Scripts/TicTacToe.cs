using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TicTacToe : MonoBehaviour
{
    public int width = 3; //width of grid
    public int height = 3; //height of grid 

    private int[,] grid; //2darray that will assign the sprites and w/e empty space there is

    public Text display; //displays text

    private bool xTurn = false; //each player will take turns... 

    private List<GameObject> spawnedPieces = new List<GameObject>(); //the list for the spawned x's and o's 

    public GameObject xPrefab, oPrefab; //prefabs for x and o 

    // Start is called before the first frame update
    private void Start()
    {
        grid = new int[width, height]; //instantiate grid. BRAND NEW, shiny, oooooh

        for (var x = 0; x < width; x++)
        {
            for (var y = 0; y < height; y++)
            {
                grid[x, y] = 0;
            }
        }

    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) //press space and it will reload the scene. so if u wanna play it again
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    public bool IsEmpty(int x, int y) //if the space is 0 then it's "empty" 
    {
        return grid[x, y] == 0;
    }

    public bool ContainsX(int x, int y) //if the space is 1 then it's a "X"
    {
        return grid[x, y] == 1;
    }

    public bool ContainsO(int x, int y) //if the space is 2 then it's an "O" 
    {
        return grid[x, y] == 2;
    }

    public bool ColumnFilled(int column)
    {
        return !IsEmpty(column, height - 1);
    }
    

    public void AddToColumn(int column) //function that adds one of the x's or o's to a column 
    {
        if (ColumnFilled(column)) return; //if the column is filled then don't do anything
        
        if (XWin() || OWin()) return; //if one of the "X" or "O" players won, don't do anything 

        for (var y = 0; y < height; y++) //add a piece
        {
            if (IsEmpty(column, y))
            {
                if (xTurn)
                    grid[column, y] = 1;
                else
                    grid[column, y] = 2;
                
                xTurn = !xTurn;

                UpdateDisplay(); //when a piece is added update the display
                return;
            }
        }
    }

    private void UpdateDisplay() //function for UpdateDisplay
    {
        foreach (var piece in spawnedPieces) //if you update display you have to....
        {
            Destroy(piece); //DESTROYYYYYYYYYYY
        }

        spawnedPieces.Clear();
        
        for (var x = 0; x < width; x++) //if it's not 0, spawn the piece 
        {
            for (var y = 0; y < height; y++)
            {
                if (ContainsX(x, y))
                {
                    var xPiece = Instantiate(xPrefab);
                    xPiece.transform.position = new Vector3(x, y);
                    spawnedPieces.Add(xPiece);
                }

                if (ContainsO(x, y))
                {
                    var oPiece = Instantiate(oPrefab);
                    oPiece.transform.position = new Vector3(x, y);
                    spawnedPieces.Add(oPiece);
                }
            }
        }

        //now you check to see if the x player or o player won....
        if (XWin())
        {
            display.text = "Congrats to the X player";
            display.color = Color.black;
        }
        else if (OWin())
        {
            display.text = "Congrats to the O player";
            display.color = Color.black;
        }
        else
        {
            display.text = "";
        }
    }

    public bool XWin()
    {
        return ThreeInARow() == 1;
    }

    public bool OWin()
    {
        return ThreeInARow() == 2;
    }

    private int ThreeInARow() //function that checks if you have either x or o three in a row
    {
        for (var x = 0; x < width; x++)
        {
            for (var y = 0; y < height; y++) //checks for 3 pieces being the same(?)
            {
                if (y <= height - 3) //checking if each individual piece is the same as the one next to it
                    if (grid[x,y] != 0 && //0 is also a slot bc it's an ARRAY
                        grid[x,y] == grid[x, y + 1] && 
                        grid[x, y] == grid[x, y + 2])
                        return grid[x, y];
                
                if (x <= width - 3) 
                    if (grid[x,y] != 0 &&
                        grid[x,y] == grid[x + 1, y] && 
                        grid[x, y] == grid[x + 2, y])
                        return grid[x, y];
                
                if(x <= width - 3 && y <= height -3)
                    if (grid[x, y] != 0 && 
                        grid[x, y] == grid[x + 1, y + 1] && 
                        grid[x, y] == grid[x + 2, y + 2])
                        return grid[x, y];
                
                if (x >= width - 1 && y <= height - 3) //for the diagonal
                    if (grid[x,y] != 0 && 
                        grid[x, y] == grid[x - 1, y + 1] && 
                        grid[x, y] == grid[x - 2, y + 2])
                        return grid[x, y];
            }
        }

        return 0; //if none of them work return 0 
    }
}
