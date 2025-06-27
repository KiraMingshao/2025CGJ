using AI.FSM;

public class NotSupportedFSMTypeException : System.Exception {
    public NotSupportedFSMTypeException(FSMStateID stateID, FSMBase fsm)
        : base($"FSM state '{stateID}' is not supported for FSM type '{fsm.GetType().Name}'.") { }
    public NotSupportedFSMTypeException(FSMTriggerID triggerID, FSMBase fsm)
        : base($"FSM trigger '{triggerID}' is not supported for FSM type '{fsm.GetType().Name}'.") { }
}
