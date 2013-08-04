using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FollowerCrowdScript : MonoBehaviour
{
    public GUISkin skin;
    public Character followTarget;
    public float separationDistance = 1.0f;
    public GameObject gainedFollowerPrefab;

    private readonly List<AiCharacter> _listOfFollowers = new List<AiCharacter>();
    private Camera _camera;
    private PlayerCharacter _player;
    private string _newCharacterMessage = string.Empty;
    private Vector2 _messageSize;
    private HeadScript _headScript;
    private SceneCharacterTracker _scores;

    private Matrix4x4 _guiMatrix;
    private Rect _guiRect;
    private Rect _messageRect;

    private void Awake()
    {
        _camera = camera;

        _player = GameObject.FindWithTag("WorldPlayer").GetComponent<PlayerCharacter>();
        _headScript = GameObject.FindWithTag("BriansHead").GetComponent<HeadScript>();

        if (!followTarget) 
            followTarget = _player;
    }

    void Update()
    {
        UpdateGUISettings();
    }

    void UpdateGUISettings()
    {
        var scale = Screen.height * .001f;
        _guiRect = new Rect(0, 0, 210, 95);
        var offset = new Vector3(_camera.pixelRect.x * 1.005f,
                                 (_camera.pixelRect.y - _camera.pixelHeight) * .5f, 0f);

        _guiMatrix = Matrix4x4.TRS(offset, Quaternion.identity,
            new Vector3(scale, scale, 1.0f));
        
    }

    private void OnGUI()
    {
        GUI.matrix = _guiMatrix;

        GUI.skin = skin;
        GUI.Box(_guiRect, "Followers");
        GUI.Label(_guiRect, _listOfFollowers.Count.ToString());

        if (_newCharacterMessage != string.Empty)
            GUI.Box(_messageRect, _newCharacterMessage, skin.customStyles[0]);
    }

    public void AddFollower(bool success, AiCharacter character)
    {
        if (success)
        {
            if (gainedFollowerPrefab) 
                Instantiate(gainedFollowerPrefab, character.transform.position + new Vector3(0,1, -1), Quaternion.identity);

            _listOfFollowers.Add(character);
            _headScript.jaw.CreateChatter(Random.Range(3,5));
            _scores.successfulCharacters++;

            if (character.passReaction.message.Length > 0)
                CreateMessage(character.passReaction.message);
        } 
        else
        {
            audio.Play();
            _headScript.eye.LookAt(EyeBehaviourScript.EyeLookAtPosition.Ground);
            _headScript.jaw.CreateShock();
            _scores.failedCharacters++;

            if (character.failReaction.message.Length > 0)
                CreateMessage(character.failReaction.message);
        }
    }

    public void LoseFollower()
    {
        if (_listOfFollowers.Count == 0) return;

        _scores.lostCharacters++;
        var followerIndex = _listOfFollowers.Count - 1;
        var follower = _listOfFollowers[followerIndex];

        follower.ForceFailure();

        _listOfFollowers.RemoveAt(followerIndex);
    }

    public void CreateMessage(string message)
    {
        if (message != string.Empty)
        {
            _newCharacterMessage = message;
            _messageSize = skin.customStyles[0].CalcSize(new GUIContent(message)) + skin.customStyles[0].contentOffset;
            _messageRect = new Rect(785 - _messageSize.x, 405 - _messageSize.y, _messageSize.x, _messageSize.y);
			StartCoroutine(TimeoutMessage());
        }
    }

    IEnumerator TimeoutMessage()
    {
        yield return new WaitForSeconds(4.0f);

        _newCharacterMessage = string.Empty;
    }

    public SceneCharacterTracker GetFollowerScores()
    {
        return _scores;
    }

    public HeadScript GetHead()
    {
        return _headScript;
    }
}
