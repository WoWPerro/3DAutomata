using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Automata
{
    private bool _isAlive;
    private bool _isDiying;
    private int _State;
    private int _MaxState;

    public Automata(bool isAlive, int MaxState)
    {
        _isAlive = isAlive;
        _MaxState = MaxState;
    }

    public bool GetIsAlive()
    {
        return _isAlive;
    }

    public bool GetIsDying()
    {
        return _isDiying;
    }

    public void SetIsDying(bool isDiying)
    {
        _isDiying = isDiying;
    }

    public void AdvanceState()
    {
        if(_isDiying)
        {
            _State--;
        }
        if(_State <= 0)
        {
            _isAlive = false;
        }
    }
}
