using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField] private TMP_InputField _value = null;
    [SerializeField] [Range(4, 98)] private byte _defaultValue = 4;
    [SerializeField] private VerticalLayoutGroup _slotsPrefab = null;
    [SerializeField] private Cell _buttonPrefab = null;
    [SerializeField] private Transform[] _content = new Transform[2];

    private byte value = 0;

    private (List<Cell> leftTeam, List<Cell> rightTeam) teamsCells = (null, null);

    public void Create()
    {
        if (string.IsNullOrEmpty(_value.text)) _value.text = _defaultValue.ToString();
        value = System.Convert.ToByte(_value.text);
        if (value <= 3) value = _defaultValue;
        if (value % 2 != 0) value++;
        teamsCells = (FillingTeam(true, _content[0]), FillingTeam(true, _content[1]));
    }

    private List<Cell> FillingTeam(bool random, Transform diraction)
    {
        VerticalLayoutGroup directionTeamPanel = Instantiate(_slotsPrefab, diraction);
        List<Cell> tmp = new List<Cell>(value / 2);

        for (int i = 0; i < value / 2; i++)
        {
            Cell button = Instantiate(_buttonPrefab, directionTeamPanel.gameObject.transform);
            button.selfButton.onClick.AddListener(() => ClickCell(button));
            button.Set(random ? (State)Random.Range(0, 3) : State.Waiting);
            tmp.Add(button);
        }

        return tmp;
    }

    private List<Cell> ReFillingTeam(ref List<Cell> teams, Transform direction, Cell cell)
    {
        if (teams == null) return null;
        VerticalLayoutGroup directionTeamPanel = Instantiate(_slotsPrefab, direction);
        byte nextTeamCell = 0;
        bool slotsWinner = false;
        for (int i = 0; i < teams.Count; i++)
        {
            if (cell.Equals(teams[i])) slotsWinner = true;

            if (teams[i].selfState == State.Victory || teams[i].selfState == State.Waiting)
                nextTeamCell++;
        }

        if (teams.Count == 1) return null;

        teams.Clear();
        List<Cell> tmp = new List<Cell>(nextTeamCell);

        for (int i = 0; i < nextTeamCell; i++)
        {
            Cell button = Instantiate(_buttonPrefab, directionTeamPanel.gameObject.transform);
            button.selfButton.onClick.AddListener(() => ClickCell(button));
            if ((nextTeamCell - 1) % 2 != 0 && i == 0) button.Set(State.Losing);
            else if (slotsWinner && nextTeamCell <=3 && i > 0) button.Set(State.Waiting);
            else if (nextTeamCell == 1) button.Set(State.Victory);
            else button.Set((State)Random.Range(0, 3));

            tmp.Add(button);
        }

        if (!slotsWinner)
            if (nextTeamCell > 1 && nextTeamCell <= 2)
                tmp[0].Set(State.Losing);

        return tmp;
    }

    //private List<Cell> ReFillingTeam(ref List<Cell> teams,Transform direction)
    //{
    //    VerticalLayoutGroup directionTeamPanel = Instantiate(_slotsPrefab, direction);
    //    byte nextTeamCell = 0;
    //    for (int i = 0; i < teams.Count / 2; i++)
    //        if (teams[i].selfState == State.Victory || teams[i].selfState == State.Waiting)
    //            nextTeamCell++;

    //    teams.Clear();
    //    List<Cell> tmp = new List<Cell>(nextTeamCell);
    //    for (int i = 0; i < nextTeamCell; i++)
    //    {
    //        Cell button = Instantiate(_buttonPrefab, directionTeamPanel.gameObject.transform);
    //        button.selfButton.onClick.AddListener(() => ClickCell(button));
    //        if ((nextTeamCell - 1) % 2 != 0 && i == 0) button.Set(State.Losing);
    //        else if (nextTeamCell == 2) button.Set(State.Waiting);
    //        else button.Set((State)Random.Range(0, 3));

    //        tmp.Add(button);
    //    }

    //    return tmp;
    //}

    public void Restart() => UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);

    public void ClickCell(Cell button)
    {
        if (teamsCells.leftTeam != null)
        teamsCells.leftTeam.ForEach(cell =>
        {
            if (button.Equals(cell)) cell.Set(State.Victory);
            cell.selfButton.interactable = false;
        });

        if (teamsCells.rightTeam != null)
        teamsCells.rightTeam.ForEach(cell =>
        {
            if (button.Equals(cell)) cell.Set(State.Victory);
            cell.selfButton.interactable = false;
        });

        teamsCells = (ReFillingTeam(ref teamsCells.leftTeam, _content[0], button), ReFillingTeam(ref teamsCells.rightTeam, _content[1], button));
    }
}
public enum State
{
    NoPlayer,
    Waiting,
    Losing,
    Victory
}
