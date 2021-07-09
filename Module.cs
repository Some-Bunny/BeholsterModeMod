using ItemAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Reflection;
using Gungeon;
using MonoMod.RuntimeDetour;

namespace BeholsterMode
{
    public class BeholsterModule : ETGModule
    {
        public static readonly string MOD_NAME = "Beholster Mode";
        public static readonly string VERSION = "1.0";
        public static readonly string TEXT_COLOR = "#850000";

        public override void Start()
        {
            ETGModConsole.Commands.AddUnit("beholstermode", delegate (string[] e)
            {
                bool flag = BeholsterModule.BeholsterModeHook != null;
                if (flag)
                {
                    BeholsterModule.BeholsterModeHook.Dispose();
                    BeholsterModule.BeholsterModeHook = null;
                    ETGModConsole.Log(BraveUtility.RandomElement<string>(YoureUnFucked), false);
                }
                else
                {
                    BeholsterModule.BeholsterModeHook = new Hook(typeof(AIActor).GetMethod("Awake", BindingFlags.Instance | BindingFlags.Public), typeof(BeholsterModule).GetMethod("HandleCustomEnemyChanges"));
                    ETGModConsole.Log(BraveUtility.RandomElement<string>(YoureFucked), false);
                }
            });
            Log($"{MOD_NAME} v{VERSION} started successfully.", TEXT_COLOR);
        }

        public static void HandleCustomEnemyChanges(Action<AIActor> orig, AIActor self)
        {

            orig(self);
            try
            {
                if (self.IsHarmlessEnemy != true)
                {
                    self.gameObject.AddComponent<AddBeholsterBeamComponent>();
                }
            }
            catch
            {
                ETGModConsole.Log("Something went wrong.");
            }
        }
        public static List<string> YoureFucked = new List<string>
        {
            "Why.",
            "I hope you know what you're doing.",
            "This wont end well",
            "I'll grab the popcorn",
            "Masochist.",
            "Please don't do this to yourself."
        };

        public static List<string> YoureUnFucked = new List<string>
        {
            "I can't blame you honestly.",
            "Good choice.",
            "And don't turn it on again.",
            "Hope you had fun!"
        };
        private static Hook BeholsterModeHook;

        public static void Log(string text, string color="#FFFFFF")
        {
            ETGModConsole.Log($"<color={color}>{text}</color>");
        }

        public override void Exit() { }
        public override void Init() { }
    }
}
