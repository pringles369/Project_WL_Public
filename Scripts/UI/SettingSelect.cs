using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SettingSelect : MonoBehaviour
{
    [SerializeField] RectTransform[] settingBtn;
    [SerializeField] RectTransform indicator;
    [SerializeField] float moveDelay;

    int indicatorPos;
    float moveTimer;

    private void Start()
    {
        for(int i = 0; i < settingBtn.Length; i++)
        {
            int index = i;
            EventTrigger trigger = settingBtn[i].gameObject.AddComponent<EventTrigger>();
            EventTrigger.Entry entry = new EventTrigger.Entry
            {
                eventID = EventTriggerType.PointerEnter
            };
            entry.callback.AddListener((eventData) => { OnMouseOverButton(index); });
            trigger.triggers.Add(entry);
        }
    }

    void Update()
    {
        if (moveTimer < moveDelay)
        {
            moveTimer += Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.DownArrow))
        {
            if (moveTimer >= moveDelay)
            {
                if (indicatorPos < settingBtn.Length - 1)
                {
                    indicatorPos++;
                }
                else
                {
                    indicatorPos = 0;
                }
                moveTimer = 0;
            }
        }
        else if (Input.GetKey(KeyCode.UpArrow))
        {
            if (moveTimer >= moveDelay)
            {
                if (indicatorPos > 0)
                {
                    indicatorPos--;
                }
                else
                {
                    indicatorPos = settingBtn.Length - 1;
                }
                moveTimer = 0;
            }
        }

        Vector2 currentPos = indicator.localPosition;
        float newY = settingBtn[indicatorPos].localPosition.y - 17;

        indicator.localPosition = new Vector2(currentPos.x, newY);

        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Space))
        {
            Button currentButton = settingBtn[indicatorPos].GetComponent<Button>();
            if (currentButton != null)
            {
                currentButton.onClick.Invoke();
            }
        }
    }
    private void OnMouseOverButton(int index)
    {
        indicatorPos = index;

        Vector2 currentPos = indicator.localPosition;
        float newY = settingBtn[indicatorPos].localPosition.y - 17;
        indicator.localPosition = new Vector2(currentPos.x, newY);
    }
}