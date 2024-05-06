public class Rule2 : IDeepRepairRule
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
            // In Rule2, submodule is set as removable every time a target is reached
            targetCell.submodule.SetRemovable(true);
        }
    }

    public bool CheckIfRepairFinished(DeepRepairManager manager)
    {
        return manager.GetCurrentLogIndex() >= manager.submoduleCount;
    }
}
