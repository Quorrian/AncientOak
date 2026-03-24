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
    public static Dictionary<Type, string> PetVisualsByType { get; } = new ()
    {
        {typeof(ReuniclusPack), "res://PokemonAncient/images/pets/Reuniclus/reuniclus.tscn"}
    };
    
    public override RelicRarity Rarity =>
        RelicRarity.Ancient;
    
    public override bool HasUponPickupEffect => true;
    public override bool AddsPet => true;
    public override bool SpawnsPets => true;

    public abstract IEnumerable<CardModel> CardList { get; }

    public override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new IntVar("CardCount", CardList.Count())
    ];
    
    public override IEnumerable<IHoverTip> ExtraHoverTips
        => CardList.Select(c => HoverTipFactory.FromCard(c));
    
    public override async Task AfterObtained()
    {
        foreach (var card in CardList)
        {
            await CardPileCmd.Add(Owner.RunState.CreateCard(card, Owner), PileType.Deck);
        }
    }
    
    public override async Task BeforeCombatStart() => await SummonPet();

    protected abstract Task SummonPet();
}