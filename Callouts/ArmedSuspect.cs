using LSPD_First_Response.Mod.Callouts;
using Rage;

namespace OpenCallouts.Callouts
{
    [CalloutInfo("Armed Suspect On Street", CalloutProbability.Low)]
    public class ArmedSuspect : Callout
    {
        private Ped _suspect;
        private Blip _blip;
        private Vector3 _spawnPoint;
        private bool _callCreated;
        
        public override bool OnBeforeCalloutDisplayed()
        {
            _spawnPoint = World.GetNextPositionOnStreet(Game.LocalPlayer.Character.Position.Around(200f));
            ShowCalloutAreaBlipBeforeAccepting(_spawnPoint, 30f);
            CalloutMessage = "Armed Person";
            CalloutPosition = _spawnPoint;
            //Functions.PlayScannerAudioUsingPosition("WE_HAVE DL_CRIME_STOLEN_VEHICLE_01", _spawnPoint);
            return base.OnBeforeCalloutDisplayed();
        }

        public override bool OnCalloutAccepted()
        {
            Game.LogTrivial("[OpenCallouts] Started callout Armed Suspect");
            
            return base.OnCalloutAccepted();
        }

        public override void Process()
        {
            base.Process();
        }

        public override void End()
        {
            base.End();
        }
    }
}