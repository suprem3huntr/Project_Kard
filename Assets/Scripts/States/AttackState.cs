public class AttackState : State{
    public AttackState(){
        nextState = new IdleState();
        currState = States.AttackState;
    }

    public override State NextState(){
        return nextState;
    }
}