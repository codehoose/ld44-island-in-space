public class IslandResources
{
    public int Water { get; set; } = 10;
    public int Minerals { get; set; } = 10;
    public int Wood { get; set; } = 10;
    public int Worship { get; set; } = 5;
    public int Generation { get; set; } = 0;    

    public bool IsDead
    {
        get
        {
            var count = 0;
            var values = new int[] { Water, Minerals, Wood, Worship };
            foreach (var value in values)
            {
                if (value == 0)
                    count++;
            }

            return count > 1;
        }
    }
}
