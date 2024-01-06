using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonSE : BaseMono
{
    [SerializeField]private AudioClip buttonSE;
    
    void Awake()
    {
       var buttonSelf =  gameObject.GetComponent<Button>();
       if(buttonSelf != null)
       {
           buttonSelf.onClick.AddListener(OnButtonClick);
       }

       var triggerSelf = gameObject.GetComponent<EventTrigger>();
       if(triggerSelf != null)
       {
            EventTrigger.Entry eventEntry = new EventTrigger.Entry();
            eventEntry.eventID = EventTriggerType.PointerClick;
            eventEntry.callback.AddListener( (eventData) => { OnButtonClick(); } );
            triggerSelf.triggers.Add(eventEntry);
       }
    }

    public void OnButtonClick()
    {
        if(buttonSE != null)
        {
            AudioManager.Instance.PlaySE(buttonSE);
        }
        else
        {
            AudioManager.Instance.PlayClick();
        }
    }
}
