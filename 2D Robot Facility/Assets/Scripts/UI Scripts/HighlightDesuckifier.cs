//Work-around for Unity's failure to deselect the last-clicked UI
//element when it highlights something else. See:
//https://forum.unity3d.com/threads/285167/

//Special thanks for JoeStrout from the Unity forums for supplying this

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HighlightDesuckifier : EventTrigger
{

    private Selectable selectable;

    public void Awake()
    {
        selectable = GetComponent<Selectable>();
    }

    public void Desuckify()
    {
        EventSystem e = EventSystem.current;
        if (selectable.interactable && e.currentSelectedGameObject != gameObject)
        {
            //Somebody else is still selected?!? Screw that. Select us now.
            e.SetSelectedGameObject(gameObject);
        }
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        Desuckify();
    }
}
