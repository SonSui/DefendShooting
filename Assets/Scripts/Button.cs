using UnityEngine;

public class Button : MonoBehaviour
{
    public void OnClick()
    {
        GameManager.instance?.OnStartButtonDown();
    }
}
