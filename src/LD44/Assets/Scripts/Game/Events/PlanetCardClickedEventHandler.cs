using System;

public delegate void PlanetCardClickedEventHandler(object sender, PlanetCardClickedEventArgs e);

public class PlanetCardClickedEventArgs : EventArgs
{
    public PlanetCard Details
    {
        get;
    }

    public PlanetCardBehaviour Card
    {
        get;
    }


    public PlanetCardClickedEventArgs(PlanetCardBehaviour cardBehaviour, PlanetCard card)
    {
        Card = cardBehaviour;
        Details = card;
    }
}

