using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;


public class MazeGenerator : MonoBehaviour {

    #region Variables:

    [Header("Maze generation values:")]
    [Tooltip("How many cells tall is the maze. MUST be an even number. " +
        "If number is odd, it will be reduced by 1.\n\n" +
        "Minimum value of 4.")]
    public int mazeRows;
    [Tooltip("How many cells wide is the maze. Must be an even number. " +
        "If number is odd, it will be reduced by 1.\n\n" +
        "Minimum value of 4.")]
    public int mazeColumns;

    [Header("Maze object variables:")]
    [Tooltip("Cell prefab object.")]
    [SerializeField]
    private GameObject cellPrefab;
    [Tooltip("Player prefab object.")]
    [SerializeField]
    private GameObject playerPrefab;
    private GameObject player;

    [Tooltip("If you want to disable the main sprite so the cell has no background, set to TRUE. This will create a maze with only walls.")]
    public bool disableCellSprite;
    public Color cellColor;

    [Header("Player object variables:")]
    [Tooltip("Player prefab object.")]
    [SerializeField]
    private int centreSize = 2;

    public Dictionary<Vector2, Cell> allCells = new Dictionary<Vector2, Cell>();

    private List<Cell> unvisited = new List<Cell>();

    private List<Cell> stack = new List<Cell>();

    private Cell[] centreCells = new Cell[4];

    private Cell currentCell;
    private Cell checkCell;

    public Vector2[] neighbourPositions = new Vector2[] { new Vector2(-1, 0), new Vector2(1, 0), new Vector2(0, 1), new Vector2(0, -1) };
    private float cellSize;

    private GameObject mazeParent;
    #endregion

    private void Start()
    {
        GenerateMaze(mazeRows, mazeColumns);
    }
public MazeGenerator.Cell GetStartingCell() {
    return currentCell;
}
private void SpawnPlayer()
{
    // Get the starting cell
    Cell startCell = GetStartingCell();

    // Instantiate the player at the starting cell's position
    player = Instantiate(playerPrefab, startCell.cellObject.transform.position, Quaternion.identity);
}
public Cell GetCell(Vector2 position) {
        // Get the cell at the given position
        return allCells[position];
    }
    private void GenerateMaze(int rows, int columns)
    {
        if (mazeParent != null) DeleteMaze();

        mazeRows = rows;
        mazeColumns = columns;
        CreateLayout();
    }

    // Creates the grid of cells.
    public void CreateLayout()
    {
        InitValues();

        // Set starting point, set spawn point to start.
        Vector2 startPos = new Vector2(-(cellSize * (mazeColumns / 2)) + (cellSize / 2), -(cellSize * (mazeRows / 2)) + (cellSize / 2));
        Vector2 spawnPos = startPos;

        for (int x = 1; x <= mazeColumns; x++)
        {
            for (int y = 1; y <= mazeRows; y++)
            {
                GenerateCell(spawnPos, new Vector2(x, y));

                // Increase spawnPos y.
                spawnPos.y += cellSize;
            }
            

            // Reset spawnPos y and increase spawnPos x.
            spawnPos.y = startPos.y;
            spawnPos.x += cellSize;
        }
        CreateCentre();
        SpawnPlayer();
        StartCoroutine(RunAlgorithm());
    }

public IEnumerator RunAlgorithm()
{
    int totalSteps = mazeColumns * mazeRows;
    int targetSteps = totalSteps / 3;
    int currentSteps = 0;
    int targetCount = 0;

    // Get start cell, make it visited (i.e. remove from unvisited list).
    unvisited.Remove(currentCell);

    if (!currentCell.cScript.isTarget)
    {
        currentCell.cScript.ChangeColor(cellColor);
    }

    // While we have unvisited cells.
    while (unvisited.Count > 0)
    {
        List<Cell> unvisitedNeighbours = GetUnvisitedNeighbours(currentCell);
        if (unvisitedNeighbours.Count > 0)
        {
            // Get a random unvisited neighbour.
            checkCell = unvisitedNeighbours[UnityEngine.Random.Range(0, unvisitedNeighbours.Count)];
            // Add current cell to stack.
            stack.Add(currentCell);
            // Compare and remove walls.
            CompareWalls(currentCell, checkCell);
            // Make currentCell the neighbour cell.
            currentCell = checkCell;
            // Mark new current cell as visited.
            if (!currentCell.cScript.isTarget)
            {
                currentCell.cScript.ChangeColor(cellColor);
            }
            unvisited.Remove(currentCell);

            // Increase the counter
            currentSteps++;

            // If the counter reaches the target steps, mark the current cell as a target
            if (currentSteps >= targetSteps && targetCount < 3)
            {
                currentCell.cScript.isTarget = true;
                currentCell.cScript.ChangeColor(Color.red); // Set your target color here
                targetCount++;
                currentSteps = 0;
            }
        }
        else if (stack.Count > 0)
        {
            // Make current cell the most recently added Cell from the stack.
            currentCell = stack[stack.Count - 1];
            // Remove it from stack.
            stack.Remove(currentCell);

            // Change color to backtrack color and then back to normal color after some delay
            if (!currentCell.cScript.isTarget)
            {
                currentCell.cScript.ChangeColor(Color.black); // Set your backtrack color here
                yield return new WaitForSeconds(0.02f); // Wait for 0.02 second
                if (!currentCell.cScript.isTarget)
                {
                    currentCell.cScript.ChangeColor(cellColor);
                }
            }
        }
        yield return new WaitForSeconds(0.002f); // Wait for 0.002 second before the next step
    }
    
}

    public List<Cell> GetUnvisitedNeighbours(Cell curCell)
    {
        // Create a list to return.
        List<Cell> neighbours = new List<Cell>();
        // Create a Cell object.
        Cell nCell = curCell;
        // Store current cell grid pos.
        Vector2 cPos = curCell.gridPos;

        foreach (Vector2 p in neighbourPositions)
        {
            // Find position of neighbour on grid, relative to current.
            Vector2 nPos = cPos + p;
            // If cell exists.
            if (allCells.ContainsKey(nPos)) nCell = allCells[nPos];
            // If cell is unvisited.
            if (unvisited.Contains(nCell)) neighbours.Add(nCell);
        }

        return neighbours;
    }

    public void CompareWalls(Cell cCell, Cell nCell)
    {
        // If neighbour is left of current.
        if (nCell.gridPos.x < cCell.gridPos.x)
        {
            RemoveWall(nCell.cScript, 2);
            RemoveWall(cCell.cScript, 1);
        }
        // Else if neighbour is right of current.
        else if (nCell.gridPos.x > cCell.gridPos.x)
        {
            RemoveWall(nCell.cScript, 1);
            RemoveWall(cCell.cScript, 2);
        }
        // Else if neighbour is above current.
        else if (nCell.gridPos.y > cCell.gridPos.y)
        {
            RemoveWall(nCell.cScript, 4);
            RemoveWall(cCell.cScript, 3);
        }
        // Else if neighbour is below current.
        else if (nCell.gridPos.y < cCell.gridPos.y)
        {
            RemoveWall(nCell.cScript, 3);
            RemoveWall(cCell.cScript, 4);
        }
    }


    public void RemoveWall(CellScript cScript, int wallID)
    {
        if (wallID == 1) cScript.wallL.SetActive(false);
        else if (wallID == 2) cScript.wallR.SetActive(false);
        else if (wallID == 3) cScript.wallU.SetActive(false);
        else if (wallID == 4) cScript.wallD.SetActive(false);
    }

    public void CreateCentre()
    {
        // Get the centre cell using the rows and columns variables.
        Cell centreCell = allCells[new Vector2((mazeColumns / 2) + 1, (mazeRows / 2) + 1)];

        // Set current cell to centre cell.
        currentCell = centreCell;

        // Remove centre cell from the 'unvisited' list.
    // This ensures that the centre cell will connect to the maze.
        unvisited.Remove(centreCell);
    }

    public void GenerateCell(Vector2 pos, Vector2 keyPos)
    {
        // Create new Cell object.
        Cell newCell = new Cell();

        // Store reference to position in grid.
        newCell.gridPos = keyPos;
        // Set and instantiate cell GameObject.
        newCell.cellObject = Instantiate(cellPrefab, pos, cellPrefab.transform.rotation);
        // Child new cell to parent.
        if (mazeParent != null) newCell.cellObject.transform.parent = mazeParent.transform;
        // Set name of cellObject.
        newCell.cellObject.name = "Cell - X:" + keyPos.x + " Y:" + keyPos.y;
        // Get reference to attached CellScript.
        newCell.cScript = newCell.cellObject.GetComponent<CellScript>();
        // Disable Cell sprite, if applicable.
        if (disableCellSprite) newCell.cellObject.GetComponent<SpriteRenderer>().enabled = false;

        // Add to Lists.
        allCells[keyPos] = newCell;
        unvisited.Add(newCell);
    }

    public void DeleteMaze()
    {
        if (mazeParent != null) Destroy(mazeParent);
    }

    public void InitValues()
    {
        // Check generation values to prevent generation failing.
        if (IsOdd(mazeRows)) mazeRows--;
        if (IsOdd(mazeColumns)) mazeColumns--;

        if (mazeRows <= 3) mazeRows = 4;
        if (mazeColumns <= 3) mazeColumns = 4;

        // Determine size of cell using localScale.
        cellSize = cellPrefab.transform.localScale.x;

        // Create an empty parent object to hold the maze in the scene.
        mazeParent = new GameObject();
        mazeParent.transform.position = Vector2.zero;
        mazeParent.name = "Maze";
    }

    public bool IsOdd(int value)
    {
        return value % 2 != 0;
    }

    public class Cell
    {
        public Vector2 gridPos;
        public GameObject cellObject;
        public CellScript cScript;
    }
}