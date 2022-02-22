using UnityEngine.UI;
using UnityEngine;

public class Cell : MonoBehaviour
{
    [SerializeField] private Color[] colorState = new Color[] { Color.gray, Color.blue, Color.black, Color.yellow };
    [HideInInspector] public Button selfButton = null;
    [HideInInspector] public State selfState = State.Victory;

    private Image selfImage = null;

    private void Awake()
    {
        selfImage ??= GetComponent<Image>();
        selfButton ??= GetComponent<Button>();
    }
    public void Set(State state)
    {
        selfImage.color = colorState[(int)state];
        selfButton.interactable = state == State.Waiting;
        selfState = state;
    }
}
