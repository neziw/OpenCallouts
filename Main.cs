using System;
using LSPD_First_Response.Mod.API;
using OpenCallouts.Callouts;
using Rage;

namespace OpenCallouts
{
    public class Main : Plugin
    {
        private static readonly Random random = new((int)DateTime.Now.Ticks);
        
        public override void Initialize()
        {
            Functions.OnOnDutyStateChanged += OnDutyStateChanged;
            Game.LogTrivial("OpenCallouts by Techno Neziw successfully loaded.");
        }

        public override void Finally() { }

        private static void OnDutyStateChanged(bool onDuty)
        {
            if (!onDuty) return;
            StartCallouts();
        }

        private static void StartCallouts()
        {
            Functions.RegisterCallout(typeof(ArmedPursuit));
            Functions.RegisterCallout(typeof(ArmedSuspect));
            //Functions.RegisterCallout(typeof(GangActivity));
            //Functions.RegisterCallout(typeof(StolenCarPursuit));
            //Functions.RegisterCallout(typeof(TerroristAttack));
            //Functions.RegisterCallout(typeof(ZancudoBurglary));
        }

        public static Random Random()
        {
            return random;
        }
    }
}