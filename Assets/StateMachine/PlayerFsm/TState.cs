using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface TState
{
    void OnEnter();

    void OnUpdate();

    void OnFixedUpdate();

    void OnExit();
}
