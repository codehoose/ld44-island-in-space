    using System;

    public delegate void CardClickedEventHandler(object sender, CardClickedEventArgs e);

    public class CardClickedEventArgs : EventArgs
    {
        public PlayerCard Details
        {
            get;
        }
    
        public CardBehaviour Card
        {
            get;
        }
    

        public CardClickedEventArgs(CardBehaviour cardBehaviour, PlayerCard card)
        {
            Card = cardBehaviour;
            Details = card;
        }
    }

