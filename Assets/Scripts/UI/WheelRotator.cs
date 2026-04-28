using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WheelRotator : MonoBehaviour
{
    [SerializeField] private GameObject obj;
    [SerializeField] private Button spinButton;
    [SerializeField] private GameObject turnSfxPrefab;

    public event Action OnSpinStart;
    public event Action<int> OnSpinFinished;

    private int targetIndex;
    private float sliceAngle => 360f / GlobalVariables.SLICE_COUNT;

    private void Start()
    {
        spinButton.onClick.RemoveAllListeners();
        spinButton.onClick.AddListener(GameManager.Instance.TurnWheel);
    }

    public void StartSpin(int index)
    {
        targetIndex = index;

        float targetAngle =
            (360f * 4) + (index * sliceAngle);

        StartCoroutine(SpinTo(targetAngle));
        OnSpinStart?.Invoke();
    }


    private void RotateToIndex(int index)
    {
        targetIndex = index;

        float targetAngle = (360f * 4) + (index * sliceAngle); //before landing on the selected reward, turn extra rrounds 

        StartCoroutine(SpinTo(targetAngle));
    }

    private IEnumerator SpinTo(float targetAngle)
    {
        GameObject sfx = Instantiate(turnSfxPrefab, transform);
        Destroy(sfx, 2f);

        spinButton.interactable = false; //to make sure user does not spam turn button and get extra rewards

        float duration = 3f;
        float time = 0f;

        float startAngle = obj.transform.eulerAngles.z;

        while (time < duration)
        {
            time += Time.deltaTime;
            float t = time / duration;

            // ease out
            t = 1 - Mathf.Pow(1 - t, 3);

            float angle = Mathf.Lerp(startAngle, targetAngle, t);
            obj.transform.eulerAngles = new Vector3(0, 0, angle);

            yield return null;
        }

        obj.transform.eulerAngles = new Vector3(0, 0, targetAngle % 360);

        //waiting a bit so player can see their reward then notify and refresh in Game Manager
        yield return new WaitForSeconds(0.5f);
        OnSpinFinished?.Invoke(targetIndex);

        spinButton.interactable = true;
    }



}
