using System;
using System.Collections.Generic;

public class PlanetDeckFactory
{
    private readonly List<PlanetCard> _cards = new List<PlanetCard>();
    private readonly Random _rnd = new Random();

    public PlanetDeckFactory()
    {
        /*
            -- Multiply card value
            -- Decrease value
            -- Invert
            Discard one card
            Worship at 10 can request new cards - requires cooldown of X turns
         */

        _cards.Add(new PlanetCard("Invert", "Multiply values by -1", (c, cb) => {
            c.WaterDelta *= -1;
            c.WoodDelta *= -1;
            c.MineralsDelta *= -1;
            c.WorshipDelta *= -1;
            cb.ApplyCard(c);
        }));

        _cards.Add(new PlanetCard("Increase", "Increase everything by 1", (c, cb) => {
            c.WaterDelta += 1;
            c.WoodDelta += 1;
            c.MineralsDelta += 1;
            c.WorshipDelta += 1;
            cb.ApplyCard(c);
        }));

        _cards.Add(new PlanetCard("Decrease", "Decrease everything by 1", (c, cb) => {
            c.WaterDelta -= 1;
            c.WoodDelta -= 1;
            c.MineralsDelta -= 1;
            c.WorshipDelta -= 1;
            cb.ApplyCard(c);
        }));

        _cards.Add(new PlanetCard("Discard", "Discard a card from the player's deck", (c, cb) =>
        {
            cb.ShowFront(false);
        }));
    }

    public PlanetCard Deal()
    {
        var index = _rnd.Next(0, _cards.Count);
        return _cards[index];
    }
}
