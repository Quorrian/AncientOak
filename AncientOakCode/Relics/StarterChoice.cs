using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.RelicPools;

namespace AncientOak.AncientOakCode.Relics;


[Pool(typeof(EventRelicPool))]
public class StarterChoice() : CustomRelicModel
{
    public override RelicRarity Rarity =>
        RelicRarity.Ancient;

    private const int NumPacksSelection = 3;

    // Add all move pack relics to this list
    public static IReadOnlyCollection<MovePack> MovePacks =>
    [
        ModelDb.Relic<ReuniclusPack>(),
        ModelDb.Relic<RotomPack>()
        //add more here
    ];

    public override async Task AfterObtained()
    {
        var randomPackRelics = GetRandomPackRelics(Owner);
        var bundles = randomPackRelics
            .Select(b => b.CardList.Select(c => Owner.RunState.CreateCard(c, Owner)).ToList())
            .ToList();
        var selectedBundle = (await CardSelectCmd.FromChooseABundleScreen(Owner, bundles)).ToList();
        var selectedRelic = randomPackRelics.First(b => b.CardList[0].Id == selectedBundle[0].Id);
        selectedRelic = (MovePack)selectedRelic.ToMutable();
        selectedRelic.CardsAlreadyAdded = true;
        await RelicCmd.Obtain(selectedRelic, Owner);
        foreach (var card in selectedBundle)
            await CardPileCmd.Add(card, PileType.Deck);
    }

    private static List<MovePack> GetRandomPackRelics(Player player)
    {
        var rewards = player.PlayerRng.Rewards;
        var movePacks = MovePacks.ToList();
        var randomBundles = new List<MovePack>();
        for (var i = 0; i < NumPacksSelection; ++i)
        {
            var movePack = rewards.NextItem(movePacks);
            if (movePack == null)
                break;
            movePacks.Remove(movePack);
            randomBundles.Add(movePack);
        }
        return randomBundles;
    }
}