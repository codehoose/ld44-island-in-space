using System;

public class PlanetCard
{
    public string Description { get; }

    public string Body { get; }

    public Action<PlayerCard, CardBehaviour> Action { get; }

    public PlanetCard(string description, string body, Action<PlayerCard, CardBehaviour> action)
    {
        Description = description;
        Body = body;
        Action = action;
    }

    public void ApplyAction(PlayerCard card, CardBehaviour behaviour)
    {
        Action?.Invoke(card, behaviour);
    }
}
