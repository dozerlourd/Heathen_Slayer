using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(TextMeshPro))]
public class DamagedText : MonoBehaviour
{
    #region Variable

    TextMeshPro tmp;
    [SerializeField] float lifeDuration;
    [SerializeField] float floatingSpeed;

    Color originColor;
    Color OAlphaColor;
    Coroutine Co_Floating;

    #endregion

    #region Unity Life Cycle

    void Awake()
    {
        tmp = GetComponent<TextMeshPro>();
        originColor = tmp.color;
        OAlphaColor = new Color(tmp.color.r, tmp.color.g, tmp.color.b, 0);
    }

    private void OnEnable()
    {
        Co_Floating = StartCoroutine(FloatingText());
    }

    private void OnDisable()
    {
        if (Co_Floating != null)
            StopCoroutine(Co_Floating);
    }

    #endregion

    IEnumerator FloatingText()
    {
        float frame = Time.deltaTime;
        WaitForSeconds waitTime = new WaitForSeconds(frame);
        float rate = 0;

        while (rate <= lifeDuration)
        {
            rate += frame;
            yield return waitTime;

            transform.Translate(transform.up * floatingSpeed * Time.deltaTime);
            tmp.color = Color.Lerp(originColor, OAlphaColor, rate / lifeDuration);
        }
        gameObject.SetActive(false);
    }

    public void SetDamagedText(float dmg)
    {
        tmp.text = dmg.ToString("N0");
    }
}
