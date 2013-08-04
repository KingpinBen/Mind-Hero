using UnityEngine;
using System.Collections;

public class MenuObjectsHandler : MonoBehaviour
{

    public Transform[] offPositions;
    public Transform[] onPositions;
    public MenuObjectSelectable[] menuObjects;

    private MenuDifficultyGui _handlerGui;
    private MenuObjectsHandler _activatedBy;
    private bool _finishedTransitioning;

    public MenuObjectsHandler activatedBy
    {
        get { return _activatedBy; }
    }

    public MenuDifficultyGui gui { get { return _handlerGui; } }

    private void Awake()
    {
        _handlerGui = GetComponent<MenuDifficultyGui>();

        if (menuObjects[0].GetType() == typeof (MenuObjectChangeLevel))
            for (var i = 0; i < menuObjects.Length; i++)
                menuObjects[i].renderer.material.color = new Color(.5f, .5f, .5f);
    }

    private void Start()
    {
        for (var i = 0; i < menuObjects.Length; i++)
        {
            menuObjects[i].handler = this;
            menuObjects[i].transform.position = offPositions[i].position;
        }
    }

    private void Update()
    {
        if (_finishedTransitioning == false) return;

        if (Input.GetKeyDown(KeyCode.Escape) && activatedBy)
            GoBack();
    }

    public void GoBack()
    {
        activatedBy.SetState(true, false, this);
        SetState(false, false, null);   //  Should be safe
    }

    public void SetState(bool state, bool forward, MenuObjectsHandler sender)
    {
        _finishedTransitioning = false;

        //  We need to stop all current coroutines to stop issues where
        //  HideHandler cr hadn't finished making all the menus deactivate.
        StopAllCoroutines();

        if (state)
        {
            ActivateThis();
            if (gui) StartCoroutine(ToggleGui());
            //  We only want to save the one who activated the object
            //  so if it's turned off, we shouldn't change it.
            if (forward)
                _activatedBy = sender;
        }
        else
        {
            StartCoroutine(HideHandler());
            if (gui) gui.beingShown = false;
        }
            

        //  Have to do it here as the gameobject may not yet be active.
        StartCoroutine(FinishTransition());

        for (var i = 0; i < menuObjects.Length; i++)
        {
            var trans = state ? onPositions[i] : offPositions[i];
            menuObjects[i].targetPosition = trans;

            //  Set this to stop them being clickable
            menuObjects[i].transitioningOut = !state;   
        }
    }

    void ActivateThis()
    {
        gameObject.SetActive(true);

        if (!gui)
            return;

        var levelSelectObject = menuObjects[0] as MenuObjectChangeLevel;

        if (levelSelectObject)
            _handlerGui.NewMenuObjectSelected(levelSelectObject);
    }

    IEnumerator HideHandler()
    {
        yield return new WaitForSeconds(1.5f);

        gameObject.SetActive(false);
        if (gui) 
            gui.beingShown = false;
    }

    /// <summary>
    /// Finish the transitioning state to allow menu input from keypresses.
    /// </summary>
    IEnumerator FinishTransition()
    {
        yield return new WaitForSeconds(1f);

        _finishedTransitioning = true;
    }

    IEnumerator ToggleGui()
    {
        yield return new WaitForSeconds(.5f);

        gui.beingShown = gameObject.activeSelf;
    }
}
