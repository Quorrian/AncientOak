using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
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
        ModelDb.Relic<ReuniclusPack>()
        //add more here
    ];

    public override async Task AfterObtained()
    {
        var randomBundles = GeneratePokePacks(Owner);
        var bundles = randomBundles.Select(b => b.cards).ToList();
        var selectedBundle = await CardSelectCmd.FromChooseABundleScreen(Owner, bundles);
        var selectedRelic = randomBundles.First(b => b.cards[0].Id == selectedBundle.First().Id).relic;
        await RelicCmd.Obtain(selectedRelic.ToMutable(), Owner);
    }

    private static List<(MovePack relic, IReadOnlyList<CardModel> cards)> GeneratePokePacks(Player player)
    {
        var rewards = player.PlayerRng.Rewards;
        var movePacks = MovePacks.ToList();
        var randomBundles = new List<(MovePack relic, IReadOnlyList<CardModel> cards)>();
        for (var i = 0; i < NumPacksSelection; ++i)
        {
            var movePack = rewards.NextItem(movePacks);
            if (movePack == null)
                break;
            movePacks.Remove(movePack);
            randomBundles.Add((movePack, movePack.CardList.ToList()));
        }
        return randomBundles;
    }
}