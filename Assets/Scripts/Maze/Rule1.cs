public class Rule1 : IDeepRepairRule
{
    public void HandlePlayerReachTarget(MazeGenerator.Cell targetCell, DeepRepairManager manager)
    {
        if (targetCell.submodule != null && !targetCell.submodule.isDefined)
        {
            string text = manager.submoduleNames[manager.GetCurrentLogIndex()];
            targetCell.submodule.screenText.text = text;
            targetCell.submodule.isDefined = true;
            manager.IncrementLogIndex();
            ConversationController.Instance.ContinueChoice(text);
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

