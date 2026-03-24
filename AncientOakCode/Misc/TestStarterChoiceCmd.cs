using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.DevConsole;
using MegaCrit.Sts2.Core.DevConsole.ConsoleCommands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Nodes;
using MegaCrit.Sts2.Core.Nodes.Cards;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Screens.CardSelection;
using MegaCrit.Sts2.Core.Runs;
using AncientOak.AncientOakCode.Relics;

namespace AncientOak.AncientOakCode.Misc;

public class TestStarterChoiceCmd : AbstractConsoleCmd
{
    public override string CmdName => "test_starter_choice";
    public override string Args => "";
    public override string Description => "End-to-end test of StarterChoice with auto-clicking via ForceClick.";
    public override bool IsNetworked => true;

    public override CmdResult Process(Player? issuingPlayer, string[] args)
    {
        if (issuingPlayer == null || !RunManager.Instance.IsInProgress)
            return new CmdResult(false, "No run in progress");

        Task task = RunFullTest(issuingPlayer);
        return new CmdResult(task, true, "Running StarterChoice end-to-end test...");
    }

    private static async Task RunFullTest(Player player)
    {
        // Step 1: Verify CreateCard fix - same logic as StarterChoice.AfterObtained
        var movePacks = StarterChoice.MovePacks.ToList();
        var rewards = player.PlayerRng.Rewards;
        var randomPackRelics = new List<MovePack>();
        for (var i = 0; i < 3; ++i)
        {
            if (movePacks.Count == 0) break;
            var movePack = rewards.NextItem(movePacks);
            movePacks.Remove(movePack);
            randomPackRelics.Add(movePack);
        }
        Log($"Step 1: Generated {randomPackRelics.Count} pack relics");

        var bundles = randomPackRelics
            .Select(b => b.CardList.Select(c => player.RunState.CreateCard(c, player)).ToList())
            .ToList();

        foreach (var bundle in bundles)
            foreach (var card in bundle)
                _ = card.Owner.Character.TrailPath; // NullRefs without CreateCard fix
        Log("Step 2: All cards have valid Owner.Character.TrailPath (CreateCard fix verified)");

        // Step 3: Show bundle screen + auto-click
        Log("Step 3: Opening bundle selection screen...");
        ScheduleAutoClick(NGame.Instance!.GetTree());
        var selectedBundle = (await CardSelectCmd.FromChooseABundleScreen(player, bundles)).ToList();
        Log($"Step 4: Bundle screen CLOSED! Selected {selectedBundle.Count} cards");

        // Step 5: Obtain relic
        var selectedRelic = randomPackRelics.First(b => b.CardList[0].Id == selectedBundle[0].Id);
        selectedRelic = (MovePack)selectedRelic.ToMutable();
        selectedRelic.CardsAlreadyAdded = true;
        await RelicCmd.Obtain(selectedRelic, player);
        foreach (var card in selectedBundle)
            await CardPileCmd.Add(card, PileType.Deck);
        Log($"Step 5: Obtained relic '{selectedRelic.GetType().Name}' and added cards to deck");
        Log("=== ALL TESTS PASSED ===");
    }

    private static void ScheduleAutoClick(SceneTree tree)
    {
        tree.CreateTimer(1.5).Connect("timeout", Callable.From(() =>
        {
            var screen = tree.Root.FindChild("NChooseABundleSelectionScreen", true, false) as Control;
            if (screen == null) { Log("ERROR: Bundle screen not found"); return; }

            var bundleRow = screen.FindChild("BundleRow", true, false) as Control;
            if (bundleRow == null || bundleRow.GetChildCount() == 0) { Log("ERROR: No bundles"); return; }

            var hitbox = bundleRow.GetChild(0).FindChild("Hitbox", false, false) as NClickableControl;
            if (hitbox == null) { Log("ERROR: Hitbox not found"); return; }

            Log("ForceClick on bundle hitbox...");
            hitbox.ForceClick();

            tree.CreateTimer(1.5).Connect("timeout", Callable.From(() =>
            {
                var confirmBtn = screen.FindChild("Confirm", true, false) as NClickableControl;
                if (confirmBtn == null) { Log("ERROR: Confirm button not found"); return; }

                Log("ForceClick on confirm button...");
                confirmBtn.ForceClick();
            }));
        }));
    }

    private static void Log(string msg)
    {
        MegaCrit.Sts2.Core.Logging.Log.Info($"[TestStarterChoice] {msg}");
    }
}
