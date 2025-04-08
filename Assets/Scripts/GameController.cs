using System.Reflection;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController Instance;

    [HideInInspector] public GameState State = GameState.Start;
    [HideInInspector] public int[,] Grid = { { 0, 0, 0 }, { 0, 0, 0 }, { 0, 0, 0 } };
    [HideInInspector] public int TurnOrder = 1;

    private int _maxTurns = 3;

    private void Update() {
        StartGame();
        ResetGame();
    }

    public void StartGame() {
        if (Input.GetKeyDown(KeyCode.Space) && State == GameState.Start) {
            State = GameState.Run;
        }
    }

    public void StopGame() { 
        State = GameState.Stop;
    }

    public void ResetGame() {
        if (Input.GetKeyDown(KeyCode.R) && State == GameState.Stop) {
            State = GameState.Start;
            UIController.Instance.ResetButtons();
            UIController.Instance.ResetAI();
            TurnOrder = 1;

            for (int x = 0; x < Grid.GetLength(0); x++)
                for (int y = 0; y < Grid.GetLength(1); y++)
                    Grid[x, y] = 0;
        }
    }

    private void ProcessAI() {
        if (State != GameState.Run || TurnOrder == 1) return;

        UIController.Instance.AISelect();
    }

    public void NextTurn() {
        if (State != GameState.Run) return;

        TurnOrder++;
        if (TurnOrder == _maxTurns)
            TurnOrder = 1;

        bool filled = true;
        for(int x = 0; x < Grid.GetLength(0); x++)
            for (int y = 0; y < Grid.GetLength(1); y++)
                if (Grid[x, y] == 0)
                    filled = false;

        if (CheckWinner())
            StopGame();
        else if (filled)
            StopGame();
    }

    private bool CheckWinner() {
        int x = CheckPlayer(1);
        int o = CheckPlayer(2);

        if (x != 0) {
            UIController.Instance.SetWinner(x);
            return true;
        }
        else if (o != 0) {
            UIController.Instance.SetWinner(o);
            return true;
        }
        else UIController.Instance.SetWinner(0);

        return false;
    }

    private int CheckPlayer(int player) {
        bool winner = false;

        // ROWS
        if (Grid[0, 0] == player && Grid[0, 1] == player && Grid[0, 2] == player)
            winner = true;

        if (Grid[1, 0] == player && Grid[1, 1] == player && Grid[1, 2] == player)
            winner = true;

        if (Grid[2, 0] == player && Grid[2, 1] == player && Grid[2, 2] == player)
            winner = true;

        // COLUMNS
        if (Grid[0, 0] == player && Grid[1, 0] == player && Grid[2, 0] == player)
            winner = true;

        if (Grid[0, 1] == player && Grid[1, 1] == player && Grid[2, 1] == player)
            winner = true;

        if (Grid[0, 2] == player && Grid[1, 2] == player && Grid[2, 2] == player)
            winner = true;

        // DIAGONAL
        if (Grid[0, 0] == player && Grid[1, 1] == player && Grid[2, 2] == player)
            winner = true;

        if (Grid[0, 2] == player && Grid[1, 1] == player && Grid[2, 0] == player)
            winner = true;

        return winner ? player : 0;
    }

    private void Awake() { 
        Instance ??= this;
        InvokeRepeating("ProcessAI", 0, 1);
    }
}