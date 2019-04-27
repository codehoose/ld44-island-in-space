using System;
using System.Collections.Generic;

public class PlayerDeckFactory
{
    private readonly List<PlayerCard> _cards = new List<PlayerCard>();
    private readonly Random _rnd = new Random();

    public PlayerDeckFactory()
    {
        _cards.Add(new PlayerCard("Build Houses", 
                                  "Build new houses for the worshipers", 
                                  1, 
                                  -1, 
                                  -1));

        _cards.Add(new PlayerCard("Industrial Production", 
                                  "Industrial production increases!", 
                                  -1, 
                                  1, 
                                  -1));

        _cards.Add(new PlayerCard("Make Money", 
                                  "New money is required for population to buy goods", 
                                  -1, 
                                  -1, 
                                  +1, 
                                  -1));

        _cards.Add(new PlayerCard("Build Cathedral", 
                                  "A new place of worship!", 
                                  -1, 
                                  -1, 
                                  -1, 
                                  +2));
    }

    public PlayerCard Deal()
    {
        var index = _rnd.Next(0, _cards.Count);
        return new PlayerCard(_cards[index]);
    }
}
