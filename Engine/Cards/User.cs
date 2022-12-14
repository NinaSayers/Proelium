using ProeliumEngine;

public class User : IStrategy
{
    private Rules rules;
        private int playerID;
        public User(Rules rules, int playerID)
        {
            this.rules = rules;
            this.playerID = playerID;
        }
    public Move Play(State state)
    {
        try
        {
        if(state.ActualPhase == PhasesEnum.mainPhase)
        {
            GameModes.Clear();
            int i = PrintMainPhase();
            if(i == 1)
            {
                try{
                    var card = GetCardFromHand(state);
                    var move = new Move(ActionsEnum.invoke, new List<Card>{card});

                    if(rules.IsValidMove(move, state.ActualPhase, playerID, state))
                    return move;
                    else
                    {
                        Print("Invalid move");

                        return Play(state);
                    }

                }
                catch
                {
                    return Play(state);
                }

            }
            if(i == 2)
            {
                try{
                var card = GetCardFromTable(state, typeof(MonsterCard));
                Move move = new Move(ActionsEnum.activateEffect, new List<Card>{card});

                if(rules.IsValidMove(move, state.ActualPhase, playerID, state))
                return new Move(ActionsEnum.activateEffect, new List<Card>{card});
                else
                {
                    Print("Invalid move");

                    return Play(state);
                }
                }
                catch
                {
                    return Play(state);
                }
            }
            if(i == 3)
            {
                return new Move(ActionsEnum.endPhase, new List<Card>());
            }
            if(i == 4)
            {
                return new Move(ActionsEnum.endTurn, new List<Card>());
            }
        }
        if(state.ActualPhase == PhasesEnum.battlePhase)
        {
            GameModes.Clear();

            int i = PrintBattlePhase();
            if(i == 1)
            {
                try{
                    var card = GetCardFromTable(state, typeof(MonsterCard));
                    bool directAttack = false;

                    var cards = state.Table.MonsterCardsInvokeds;
                    for(int j = 0; j < cards.Count; j++)
                    {
                        if(j != playerID && state.Table.GetMonsterCardsInvokeds(j).Count == 0)
                        {
                            directAttack = true;
                            break;
                        }
                    }

                    if(directAttack)
                    {
                        Move move = new Move(ActionsEnum.attackLifePoints, new List<Card>{card});

                        if(rules.IsValidMove(move, state.ActualPhase, playerID, state))
                        return move;
                        else
                        {
                            Print("Invalid move");

                            return Play(state);
                        }
                    }
                    else
                    {
                        for(int j = 0; j < cards.Count; j++)
                        {
                            if(j != playerID)
                            {
                                var targets = cards[j];

                                var objetive = SetObjetive(targets);

                                Move move = new Move(ActionsEnum.attackCard, new List<Card>{card, objetive});

                                if(rules.IsValidMove(move, state.ActualPhase, playerID, state))
                                return move;
                                else
                                {
                                    Print("Invalid move");

                                    return Play(state);
                                }
                            }
                        }
                    }
                }
                catch
                {
                    return Play(state);
                }

            }
            if(i == 2)
            {
                return new Move(ActionsEnum.endPhase, new List<Card>());

            }
            if(i == 3)
            {
                return new Move(ActionsEnum.endTurn, new List<Card>());
            }
        }
        if(state.ActualPhase == PhasesEnum.endPhase)
        {
            GameModes.Clear();
            int i = PrintEndPhase();
            if(i == 1)
            {
                try{
                    var card = GetCardFromHand(state);
                    var move = new Move(ActionsEnum.invoke, new List<Card>{card});

                    if(rules.IsValidMove(move, state.ActualPhase, playerID, state))
                    return move;
                    else
                    {
                        Print("Jugada Invalida");

                        return Play(state);
                    }

                }
                catch
                {
                    return Play(state);
                }

            }
            if(i == 2)
            {
                try{
                var card = GetCardFromTable(state, typeof(MonsterCard));
                Move move = new Move(ActionsEnum.activateEffect, new List<Card>{card});

                if(rules.IsValidMove(move, state.ActualPhase, playerID, state))
                return new Move(ActionsEnum.activateEffect, new List<Card>{card});
                else
                {
                    Print("Invalid move");

                    return Play(state);
                }
                }
                catch
                {
                    return Play(state);
                }
            }
            if(i == 3)
            {
                return new Move(ActionsEnum.endTurn, new List<Card>());
            }
        }
        }
        catch
        {
            return Play(state);
            
        }
        return Play(state);
    }

    private Card SetObjetive(List<Card> targets)
    {
        GameModes.Clear();
        int i = 0;
        foreach(var Card in targets)
        {
            Console.SetCursorPosition(65,4+i);
            System.Console.WriteLine(i+1+" Name: "+(Card as MonsterCard)!.Name+" Attack: " +(Card as MonsterCard)!.Attack +" Life: "+(Card as MonsterCard)!.Life +" Defense: "+(Card as MonsterCard)!.Defense);
            i++;
        }
        Console.SetCursorPosition(65,i+5);
        Console.Write("> ");
        int j = int.Parse(Console.ReadLine());
        return targets[j-1];
    }
    private void Print(string s)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.SetCursorPosition(65,3);
        System.Console.WriteLine(s);
    }
    private int PrintMainPhase()
    {
        Console.ForegroundColor = ConsoleColor.White;
        Console.SetCursorPosition(65,4);
        Console.WriteLine("1 Summon");
        Console.SetCursorPosition(65,5);
        Console.WriteLine("2 Activate an effect");
        Console.SetCursorPosition(65,6);
        Console.WriteLine("3 End phase");
        Console.SetCursorPosition(65,7);
        Console.WriteLine("4 End turn");
        Console.SetCursorPosition(65,8);
        Console.Write("> ");
        int i = int.Parse(Console.ReadLine());
        return i;
    }
    private int PrintBattlePhase()
    {
        Console.ForegroundColor = ConsoleColor.White;
        Console.SetCursorPosition(65,4);
        Console.WriteLine("1 Attack");
        Console.SetCursorPosition(65,5);
        Console.WriteLine("2 End phase");
        Console.SetCursorPosition(65,6);
        Console.WriteLine("3 End turn");
        Console.SetCursorPosition(65,7);
        Console.Write("> ");
        int i = int.Parse(Console.ReadLine());
        return i;

    }
    private int PrintEndPhase()
    {
        Console.ForegroundColor = ConsoleColor.White;
        Console.SetCursorPosition(65,4);
        Console.WriteLine("1 Summon");
        Console.SetCursorPosition(65,5);
        Console.WriteLine("2 Activate an effect");
        Console.SetCursorPosition(65,6);
        Console.WriteLine("3 End turn");
        Console.SetCursorPosition(65,7);
        Console.Write("> ");
        int i = int.Parse(Console.ReadLine());
        return i;
        
    }

    private Card GetCardFromHand(State state)
    {
        GameModes.Clear();
        Console.ForegroundColor = ConsoleColor.White;
        Console.SetCursorPosition(65,4);
        Console.WriteLine("Choose a card from your hand");
        Console.SetCursorPosition(65,5);
        int k = 0;
        int j = 0;
        var hand = state.GetHand(playerID);
        GameModes.Clear();
        foreach(var card in hand)
        {
            if(card is MonsterCard)
            {
                Console.SetCursorPosition(65,k+4);
                Console.WriteLine(j+1 + " Monster : " + (card as MonsterCard)!.Name+ " --> "+" Attack: " +(card as MonsterCard)!.Attack +" Life: "+(card as MonsterCard)!.Life +" Defense: "+(card as MonsterCard)!.Defense);
                Console.SetCursorPosition(65,k+5);
                Console.ForegroundColor = ConsoleColor.DarkMagenta;
                Console.Write("Effect : ");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine((card as MonsterCard)!.Description);
                k+=3;
                j++;
            }
            if(card is MagicCard)
            {
                Console.SetCursorPosition(65,4+k);
                Console.WriteLine(j+1 + " Magic : " + (card as MagicCard)!.Name);
                Console.SetCursorPosition(65,k+5);
                Console.ForegroundColor = ConsoleColor.DarkBlue;
                Console.Write("Effect : ");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine((card as MagicCard)!.Description);
                j++;
                k+=3;
            }
        }
        Console.SetCursorPosition(65,k +5);
        Console.Write("> ");
        int i = int.Parse(Console.ReadLine());
        return hand[i-1];
    }

    private Card GetCardFromTable(State state, Type type)
    {
        GameModes.Clear();
        Console.ForegroundColor = ConsoleColor.White;
        Console.SetCursorPosition(65,4);
        Console.WriteLine("Choose a card from your Table");
        Console.SetCursorPosition(65,5);
        int j = 0;
        int k = 0;
        List<Card> cards = new List<Card>();
        if(type.Name == "MonsterCard")
        {
          cards = state.Table.GetMonsterCardsInvokeds(playerID);   
        }
        else if(type.Name == "MagicCard")
        {
            cards = state.Table.GetMagicCardsInvokeds(playerID);
        }

        GameModes.Clear();
        foreach(var card in cards)
        {
            if(card.GetType() == type)
            {
                Console.SetCursorPosition(65,4+k);
                string s = (card is MonsterCard) ? (card as MonsterCard)!.Name+ " --> "+" Attack: " +(card as MonsterCard)!.Attack +" Life: "+(card as MonsterCard)!.Life +" Defense: "+(card as MonsterCard)!.Defense : (card as MagicCard)!.Name;
                Console.WriteLine(j+1 + " " + s);
                Console.SetCursorPosition(65,k+5);
                Console.ForegroundColor = ConsoleColor.DarkMagenta;
                Console.Write("Effect : ");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine((card as MonsterCard)!.Description);
                k+=3;
                j++;
            }
        }
        Console.SetCursorPosition(65,k+5);
        Console.Write("> ");
        int i = int.Parse(Console.ReadLine());
        return cards[i-1];
    }
}