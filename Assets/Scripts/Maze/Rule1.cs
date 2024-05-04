public class Rule1 : IDeepRepairRule
{
    public void HandlePlayerReachTarget(MazeGenerator.Cell targetCell, DeepRepairManager manager)
    {
        if (targetCell.submodule != null && !targetCell.submodule.isDefined)
        {
            targetCell.submodule.screenText.text = manager.submoduleNames[manager.GetCurrentLogIndex()];
            targetCell.submodule.isDefined = true;
            manager.IncrementLogIndex();
            ConversationController.Instance.ContinueChoice(targetCell.submodule.screenText.text);
            if (manager.GetCurrentLogIndex() == manager.submoduleCount)
            {
                targetCell.submodule.SetRemovable(true);
            }
        }
    }

    public bool CheckIfRepairFinished(DeepRepairManager manager)
    {
        return manager.GetCurrentLogIndex() >= manager.submoduleCount;
    }
}

