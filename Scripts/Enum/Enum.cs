public enum AttackType
{
    Melee,
    Range
}

public enum ObjectType
{
    InteractiveObject = 1,
    AutoDialogue,
    Stone,
    Lever,
    Arrow,
    Trap
}

public enum GameState
{
    Gameplay,
    Paused
}

public enum DialogueState
{
    Inactive,
    IsInteracting,
    DialogueStarted
}