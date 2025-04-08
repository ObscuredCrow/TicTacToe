using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public static UIController Instance;

    [SerializeField] private GameObject _start;
    [SerializeField] private GameObject _play;
    [SerializeField] private GameObject _gameOver;
    [SerializeField] private TMP_Text _winner;
    [SerializeField] private Toggle _ai;

    private void SetVisibility() {
        _start.SetActive(GameController.Instance.State == GameState.Start);
        _play.gameObject.SetActive(GameController.Instance.State != GameState.Start);
        _gameOver.SetActive(GameController.Instance.State == GameState.Stop);
    }

    public void SetGridValue(int row, int column) {
        if (GameController.Instance.State != GameState.Run) return;

        GameController.Instance.Grid[row, column] = GameController.Instance.TurnOrder;
        GameController.Instance.NextTurn();
    }

    public void ResetButtons() {
        foreach (var button in FindObjectsByType<UIGridButton>(FindObjectsInactive.Include, FindObjectsSortMode.None))
            button.ResetButton();
    }

    public bool ActiveAI() => _ai.isOn;

    public void ResetAI() => _ai.isOn = false;

    public void AISelect() {
        if (!ActiveAI()) return;

        var buttons = FindObjectsByType<UIGridButton>(FindObjectsInactive.Include, FindObjectsSortMode.None);

        var randomGrid = Random.Range(0, buttons.Length);
        do {
            buttons[randomGrid].SetGridValue();
            randomGrid = Random.Range(0, buttons.Length);
        } while (buttons[randomGrid].HasValue());
    }

    public void SetWinner(int player) {
        _winner.text = player == 2 ? "O WINS" : player == 1 ? "X WINS" : "DRAW";
    }

    private void Awake() {
        Instance ??= this;

    }

    private void Update() => SetVisibility();
}