using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;

namespace AncientOak.AncientOakCode.Relics;

public abstract class MovePack() : CustomRelicModel
{
    public bool CardsAlreadyAdded { get; set; }
    
    public override RelicRarity Rarity =>
        RelicRarity.Ancient;
    
    public override bool HasUponPickupEffect => true;
    public override bool AddsPet => true;
    public override bool SpawnsPets => true;

    public abstract List<CardModel> CardList { get; }

    public override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new IntVar("CardCount", CardList.Count)
    ];
    
    public override IEnumerable<IHoverTip> ExtraHoverTips
        => CardList.Select(c => HoverTipFactory.FromCard(c));
    
    public override async Task AfterObtained()
    {
        if (CardsAlreadyAdded) return;
        var results = new List<CardPileAddResult>();
        foreach (var card in CardList)
            results.Add(await CardPileCmd.Add(Owner.RunState.CreateCard(card, Owner), PileType.Deck));
        CardCmd.PreviewCardPileAdd(results, 2f);
    }
    
    public override async Task BeforeCombatStart() => await SummonPet();

    protected abstract Task SummonPet();
}