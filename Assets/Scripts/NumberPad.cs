using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NumberPad : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI numberText;
    private Queue<int> numberList;
    // 0 is not complete, 1 is complete
    private int state = 0;
    [SerializeField]
    private int passCorrect;
    [SerializeField]
    private GameObject keyCard;

    private void Update()
    {
        if (numberList.Count == 4)
        {
            Debug.Log(ConvertStackToString());
            state = 1;
        }

        if(state == 1)
        {
            CheckPassword();
        }
    }

    private void Awake()
    {
        numberList = new Queue<int>();
    }

    public void ButtonPressed(int value)
    {
        if (state == 0)
        {
            numberList.Enqueue(value);
            UpdateView();
        } 
    }

    public void BackButtonPressed()
    {
        if (numberList.Count > 0 && state == 0)
        {
            numberList.Dequeue();
            UpdateView();
        }
    }

    public void ClearButtonPressed()
    {
        if (numberList.Count > 0 && state == 0)
        {
            numberList.Clear();
            UpdateView();
        }
    }

    private void CheckPassword()
    {
        bool isCorrect = passCorrect.ToString().Equals(ConvertStackToString());
        if (isCorrect)
        {
            numberText.SetText("Correct Password!");
            numberText.color = Color.green;
            keyCard.SetActive(true);
        }
        else
        {
            numberText.SetText("Uncorrect Password!");
            numberText.color = Color.red;
            StartCoroutine(Reset());
        }

    }

    private void UpdateView()
    {
        numberText.SetText(ConvertStackToString());
    }

    IEnumerator Reset()
    {
        yield return new WaitForSeconds(3);
        numberList.Clear();
        numberText.color = Color.black;
        state = 0;
        UpdateView();
    }

    private string ConvertStackToString()
    {
        return string.Join("", numberList);
    }

}
