using UnityEngine;
using UnityEngine.UI;

public class LifeUI : MonoBehaviour
{
    [SerializeField] Image[] hearts;
    [SerializeField] private Sprite emptyHeart;
    [SerializeField] private Sprite filledHeart;

    void OnEnable()
    {
        GameEvents.OnLivesChanged += UpdateHearts;
    }

    void OnDisable()
    {
        GameEvents.OnLivesChanged -= UpdateHearts;
    }

    private void UpdateHearts(int lives)
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            hearts[i].sprite = i < lives ? filledHeart : emptyHeart;
        }
    }


}
