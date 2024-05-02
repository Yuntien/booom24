public class Rule2 : IDeepRepairRule
{
    public void HandlePlayerReachTarget(MazeGenerator.Cell targetCell, DeepRepairManager manager)
    {
        if (targetCell.submodule != null && !targetCell.submodule.isDefined)
        {
            targetCell.submodule.screenText.text = manager.submoduleNames[manager.GetCurrentLogIndex()];
            targetCell.submodule.isDefined = true;
            manager.IncrementLogIndex();

            // In Rule2, submodule is set as removable every time a target is reached
            targetCell.submodule.SetRemovable(true);
        }
    }

    public bool CheckIfRepairFinished(DeepRepairManager manager)
    {
        return manager.GetCurrentLogIndex() >= manager.submoduleCount;
    }
}
