using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIGridButton : MonoBehaviour
{
    [SerializeField] private TMP_Text _value;
    [SerializeField] private int Row;
    [SerializeField] private int Column;

    private Button _button;

    public void SetGridValue() {
        if (GameController.Instance.State != GameState.Run || _value.text != "") return;

        _value.text = GameController.Instance.TurnOrder == 1 ? "X" : "O";
        UIController.Instance.SetGridValue(Row, Column);
    }

    public bool HasValue() => _value.text != "";

    public void ResetButton() {
        _value.text = "";
    }

    private void Update() {
        _button.interactable = !UIController.Instance.ActiveAI() || GameController.Instance.TurnOrder == 1;
    }

    private void Awake() {
        _button = GetComponent<Button>();
    }
}
