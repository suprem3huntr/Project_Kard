public class IdleState : State{
    public IdleState(){
        nextState = new GuardState();
        currState = States.IdleState;
    }

    public override State NextState(){
        return nextState;
    }
}