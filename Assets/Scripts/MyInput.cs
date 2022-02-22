using UnityEngine;
using TMPro;

public class MyInput : MonoBehaviour
{
    [SerializeField] TMP_InputField output = null;
    public void Claer() => output.text = null;
}
