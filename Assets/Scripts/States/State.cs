using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State 
{
    State nextState;
    public States currState;

    public virtual bool isCondition(){
        return true;
    }
    public virtual void doAction(){
        return null;
    }
    public virtual State NextState(){
        return nextState;
    }
}
