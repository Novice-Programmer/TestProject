public struct SelectObstacleData
{
    public EObstacleType obstacleType;
    public TestResearchData[] researchDatas;

    public SelectObstacleData(EObstacleType obstacleType, TestResearchData[] researchDatas)
    {
        this.obstacleType = obstacleType;
        this.researchDatas = researchDatas;
    }
}
