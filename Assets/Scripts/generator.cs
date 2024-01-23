using System;
using System.Collections.Generic;
using UnityEngine;

public class generator : MonoBehaviour
{
    public int[] generatepath()
    {
        int gridWidth = 10;
        int gridHeight = 10;
        int pathLength = 28;

        var (randomPathGrid, pathCoordinates) = GenerateRandomPath(gridWidth, gridHeight, pathLength);

        // Print the resulting grid
        for (int i = 0; i < gridHeight; i++)
        {
            for (int j = 0; j < gridWidth; j++)
            {
                //Debug.Log($"{randomPathGrid[i][j]} ");
            }
            //Debug.Log("");
        }

        // Print the path coordinates
        string pathCoordString = "Path Coordinates: ";
        foreach (var coord in pathCoordinates)
        {
            pathCoordString += $"({coord.Item1}, {coord.Item2}) ";
        }
        //Debug.Log(pathCoordString);

        // Convert coordinates to the new rule
        var newRuleCoordinates = ConvertToNewRule(pathCoordinates);
        string newRuleCoordString = "New Rule Coordinates: ";
        int[] returnedlist = new int[100];
        int a = 0;
        foreach (var newCoord in newRuleCoordinates)
        {
            newRuleCoordString += $"{newCoord} ";
            returnedlist[a] = newCoord;
            a++;

        }
        //Debug.Log(newRuleCoordString);
        return returnedlist;

    }

    (List<List<int>>, List<Tuple<int, int>>) GenerateRandomPath(int gridWidth, int gridHeight, int pathLength)
    {
        // Initialize the grid with zeros
        var grid = new List<List<int>>();
        for (int i = 0; i < gridHeight; i++)
        {
            grid.Add(new List<int>());
            for (int j = 0; j < gridWidth; j++)
            {
                grid[i].Add(0);
            }
        }

        // Start the path at the top-left corner (0, 0)
        var currentPos = Tuple.Create(0, 0);
        grid[currentPos.Item1][currentPos.Item2] = 1; // Mark the starting point

        var pathCoordinates = new List<Tuple<int, int>> { currentPos }; // List to store the coordinates of the path

        for (int step = 0; step < pathLength - 1; step++)
        {
            // Define possible moves (right, down)
            var possibleMoves = new List<Tuple<int, int>>();

            // Check if moving right is possible and won't violate constraints
            if (currentPos.Item2 < gridWidth - 1 && grid[currentPos.Item1][currentPos.Item2 + 1] == 0)
            {
                possibleMoves.Add(Tuple.Create(0, 1));
            }

            // Check if moving down is possible and won't violate constraints
            if (currentPos.Item1 < gridHeight - 1 && grid[currentPos.Item1 + 1][currentPos.Item2] == 0)
            {
                possibleMoves.Add(Tuple.Create(1, 0));
            }

            if (possibleMoves.Count == 0)
            {
                break; // No valid moves, exit the loop
            }

            // Randomly choose one of the possible moves
            var move = possibleMoves[UnityEngine.Random.Range(0, possibleMoves.Count)];

            // Update the current position
            currentPos = Tuple.Create(currentPos.Item1 + move.Item1, currentPos.Item2 + move.Item2);

            // Mark the cell as part of the path
            grid[currentPos.Item1][currentPos.Item2] = 1;

            // Add the current coordinates to the list
            pathCoordinates.Add(currentPos);
        }

        return (grid, pathCoordinates);
    }

    List<int> ConvertToNewRule(List<Tuple<int, int>> coordinates)
    {
        var newRuleCoordinates = new List<int>();
        foreach (var coord in coordinates)
        {
            int newNumber = (90 - (coord.Item1 * 10)) + coord.Item2;
            newRuleCoordinates.Add(newNumber);
        }
        return newRuleCoordinates;
    }
}
