public class GuardState : State{
    public GuardState(){
        nextState = new DrawState();
        currState = States.DrawState;
    }

    public override State NextState(){
        return nextState;
    }
}