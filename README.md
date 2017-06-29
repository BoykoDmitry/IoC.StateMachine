# IoC.StateMachine

Implementation of State Machine pattern, pretty abstract with support of IoC containers and serialization.

The development was inspired by working Windows Workflow Fundation, in particular with state machines. WWF without doubts is very cool and nice framework to build long running processes with high level of flexibility, version control, persistance and etc. but in the same moment the framework is very heavy, complicated and sometimes is not very stable. 

Current project targeting the following goals
- Light framework based on IoC concept
- Support of IoC containers
- Persistence friendly 

By design versioning of the state machine instances is not supported. 

### The Gollosary

- State machine - instance of IStateMachine, holds current state and history of the transitions
- State machine context - Derived from abstract class ContextBase, holds parameters for state machine logic, should be complaint with selected persistance strategy 
- State machine definition - definition of the flow, list of states and list of transitions 
- Action - class that implements ISMAction, and can be resolved via container
- Trigger - class that implements ISMTrigger, and can be resolved via container 

### Push strategy

Default push strategy considers the following algorithm (call of ISMService.Push(
1) Executes list of exit actions of current state
2) select all transistions from current state 
3) executes all triggers from transistions
4) if no trigger gives positive result, exception will be thrown
5) if more than one trigger give positive result, exception will be thrown
6) moves statemachine to transition.TargetStateId 
7) executes enter actions of target state

### IoC

In order to support different version of IoC containers, the framework exposes static class IoC with method SetContainer(IAmContainer container) which should be called in your application root. The container should be able to resolve the following interfaces 
- ISMService
- IPersistenceService
- IStateProcessor 
Actions that will be used in statemachine logic should be registered in the container with key words(to be used in state machine definition) 

IAmContainer is high level abstraction with minimum set of methods needed for framework. During loading of the statemachine (IPersistenceService.Load) framework tries to create child container and register there given statemachine as singelton, then it goes for all actions\triggers from all states\transitions and resolve the nested actions from container with given key(from definition) 

### Example 

Sample StateMachine
```c#
    [DataContract]
    public class GuessStateMachine : StateMachineBase<GuessContext>, IStateMachine
    {
    }
```
Sample context 
```c#
    [DataContract]
    public class GuessContext : ContextBase
    {
        public GuessContext()
        {
            Number = -1;
            Result = GuessResult.NA;
        }
        [DataMember]
        public int Number { get; set; }
        [DataMember]
        public GuessResult Result { get; set; }
    }
```

