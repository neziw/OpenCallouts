using LSPD_First_Response.Mod.API;
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
            _spawnPoint = World.GetNextPositionOnStreet(Game.LocalPlayer.Character.Position.Around(250f));
            
            ShowCalloutAreaBlipBeforeAccepting(_spawnPoint, 40f);
            CalloutMessage = "Armed Person";
            CalloutPosition = _spawnPoint;
            //Functions.PlayScannerAudioUsingPosition("WE_HAVE DL_CRIME_STOLEN_VEHICLE_01", _spawnPoint);
            
            return base.OnBeforeCalloutDisplayed();
        }

        public override bool OnCalloutAccepted()
        {
            Game.LogTrivial("[OpenCallouts] Started callout Armed Suspect");
            
            _suspect = new Ped("g_m_m_chemwork_01", _spawnPoint, 0f);
            _suspect.Inventory.GiveNewWeapon(WeaponHash.AssaultRifle, 400, true);
            _suspect.MaxHealth = 200;
            _suspect.Health = 200;
            _suspect.Armor = 50;
            _blip = _suspect.AttachBlip();
            _blip.IsFriendly = false;
            
            return base.OnCalloutAccepted();
        }

        public override void Process()
        {
            if (_callCreated == false)
            {
                _callCreated = true;
                _suspect.Tasks.FightAgainst(Game.LocalPlayer.Character);
            }
            base.Process();
        }

        public override void End()
        {
            Game.LogTrivial("[OpenCallouts] Ended callout Armed Suspect");
            
            if (_suspect) _suspect.Dismiss();
            if (_blip) _blip.Delete();
            Functions.PlayScannerAudio("ATTENTION_THIS_IS_DISPATCH_HIGH ALL_UNITS_CODE4 DL_NO_FURTHER_UNITS_REQUIRED");
            
            base.End();
        }
    }
}