using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

public class CharactersAligner : MonoBehaviour
{
    [SerializeField] private GameObject runnerPrefab;
    [SerializeField] private List<Runner> characters;
    [SerializeField] private float maxX = 3f;
    [SerializeField] private float maxZ = 2f;
    [SerializeField] private GameFSM gameFsm;

    private Camera _mainCamera;
    private Vector3 _axis = new Vector3(1, 0, 1);
    private bool _firstAlign = true;

    [SerializeField] private List<Animator> characterAnimators;

    private const string VictoryAnim = "Victory";
    private const string RunningAnim = "StartRunning";
    private static readonly int StartRunning = Animator.StringToHash(RunningAnim);
    private static readonly int Victory = Animator.StringToHash(VictoryAnim);

    private void Start()
    {
        _mainCamera = Camera.main;
        foreach (var character in characters)
        {
            characterAnimators.Add(character.animator);
            character.charactersAligner = this;
        }
    }

    public void AddCharacter(Vector3 position)
    {
        var runner = Instantiate(runnerPrefab, position, Quaternion.identity, transform).GetComponent<Runner>();
        characters.Add(runner);
        characterAnimators.Add(runner.animator);
        runner.charactersAligner = this;
        runner.animator.SetTrigger(StartRunning);
    }

    public void RemoveCharacter(Runner runner)
    {
        characterAnimators.Remove(runner.animator);
        characters.Remove(runner);
    }

    public void RealignCharacters(List<Vector3> positions)
    {
        int plusValue;
        
        if (positions.Count > characters.Count)
        {
            plusValue = positions.Count / characters.Count;
        }
        else
        {
            plusValue = 1;
        }

        int i = 0;
        int maxI;
        int currentI = 0;
        bool ch;
        if (characters.Count() >= positions.Count)
        {
            maxI = characters.Count() / positions.Count;
            ch = true;
        }
        else
        {
            maxI = Mathf.CeilToInt(positions.Count / characters.Count);
            ch = false;
        }
        foreach (var character in characters)
        {
            Vector3 pos = new();
            if (ch)
            {
                if (currentI % maxI != 0)
                {
                    pos = _mainCamera.ScreenToWorldPoint(positions[i]);
                    for (int iter = 1; iter < maxI; iter++)
                    {
                        pos += _mainCamera.ScreenToWorldPoint(positions[i + iter]);
                    }

                    pos /= maxI;
                }
                else
                {
                    pos = _mainCamera.ScreenToWorldPoint(positions[i]);
                }
                
                var positionTemp = pos;
                positionTemp = transform.InverseTransformPoint(positionTemp);
                var newPlace = new Vector3(positionTemp.x, characters[i].transform.localPosition.y, positionTemp.y - 7);
                newPlace.z = Mathf.Clamp(newPlace.z, -maxZ, maxZ);
                newPlace.x = Mathf.Clamp(newPlace.x, -maxX, maxX);
                // character.transform.localPosition = newPlace;
                character.transform.DOLocalMove(newPlace, 0.5f);
                if (currentI != maxI)
                {
                    currentI++;
                }
                else
                {
                    i++;
                    currentI = 0;
                }
                i = Mathf.Clamp(i, 0, positions.Count - 1);
            }
            else
            {
                pos = _mainCamera.ScreenToWorldPoint(positions[i]);
                var positionTemp = pos;
                positionTemp = transform.InverseTransformPoint(positionTemp);
                var newPlace = new Vector3(positionTemp.x, character.transform.localPosition.y, positionTemp.y - 5.5f);
                newPlace.z = Mathf.Clamp(newPlace.z, -maxZ, maxZ);
                newPlace.x = Mathf.Clamp(newPlace.x, -maxX, maxX);
                // character.transform.localPosition = newPlace;
                character.transform.DOLocalMove(newPlace, 0.5f);
                i += maxI;
                i = Mathf.Clamp(i, 0, positions.Count - 1);
            }
        }

        if (_firstAlign)
        {
            gameFsm.ChangeState(gameFsm.playState);
            foreach (var animator in characterAnimators)
            {
                animator.SetTrigger(StartRunning);
            }
        }


    }
    
    public void Finish()
    {
        gameFsm.ChangeState(gameFsm.finishState);
        foreach (var character in characters)
        {
            var localScale = character.transform.localScale;
            character.transform.localScale = new Vector3(localScale.x, localScale.y,
                localScale.z * -1);
        }
        foreach (var animator in characterAnimators)
        {
            animator.SetTrigger(Victory);
        }
    }
}
