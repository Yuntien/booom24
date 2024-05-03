public interface IDeepRepairRule
{
    void HandlePlayerReachTarget(MazeGenerator.Cell targetCell, DeepRepairManager manager);
    bool CheckIfRepairFinished(DeepRepairManager manager);
}
