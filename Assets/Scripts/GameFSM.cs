using System;
using Dreamteck.Splines;
using States;
using UnityEngine;
using UnityEngine.UI;

public class GameFSM : StateMachine
{
    public GameObject characters;
    public Image drawToStartImage;

    [HideInInspector] public SplineFollower charactersSplineFollower;
    [HideInInspector] public StartState startState;
    [HideInInspector] public PlayState playState;
    [HideInInspector] public FinishState finishState;

    private void Awake()
    {
        startState = new StartState(this);
        playState = new PlayState(this);
        finishState = new FinishState(this);

        charactersSplineFollower = characters.GetComponent<SplineFollower>();
    }

    protected override BaseState GetInitialState()
    {
        return startState;
    }
}
