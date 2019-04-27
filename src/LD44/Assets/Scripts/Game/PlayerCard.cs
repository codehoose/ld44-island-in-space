public class PlayerCard
{
    public string Description { get; }

    public string Body { get; }

    public int WaterDelta { get; set; }

    public int MineralsDelta { get; set; }

    public int WoodDelta { get; set; }

    public int WorshipDelta { get; set; }

    public PlayerCard(PlayerCard copy)
    {
        Description = copy.Description;
        Body = copy.Body;
        WaterDelta = copy.WaterDelta;
        MineralsDelta = copy.MineralsDelta;
        WoodDelta = copy.WoodDelta;
        WorshipDelta = copy.WorshipDelta;
    }

    public PlayerCard(string description, 
                      string body, 
                      int water = 0, 
                      int mineral = 0, 
                      int wood = 0, 
                      int worship = 0)
    {
        Description = description;
        Body = body;
        WaterDelta = water;
        MineralsDelta = mineral;
        WoodDelta = wood;
        WorshipDelta = worship;
    }

    public void Apply(IslandResources resources)
    {
        resources.Wood += WoodDelta;
        resources.Minerals += MineralsDelta;
        resources.Water += WaterDelta;
        resources.Worship += WorshipDelta;

        if (resources.Wood < 0)
            resources.Wood = 0;

        if (resources.Minerals < 0)
            resources.Minerals = 0;

        if (resources.Water < 0)
            resources.Water = 0;

        if (resources.Worship < 0)
            resources.Worship = 0;
    }
}
