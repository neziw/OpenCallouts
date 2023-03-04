using LSPD_First_Response;
using LSPD_First_Response.Mod.API;
using LSPD_First_Response.Mod.Callouts;
using Rage;

namespace OpenCallouts.Callouts
{
    
    [CalloutInfo("Armed Person Pursuit", CalloutProbability.Medium)]
    public class ArmedPursuit : Callout
    {

        //private string[] _cars = { "cogcabrio", "dominator", "bison", "chernobog" };
        private Ped _suspect;
        private Vehicle _stolenCar;
        private Blip _blip;
        private LHandle _pursuit;
        private Vector3 _spawnPoint;
        private bool _pursuitCreated;

        public override bool OnBeforeCalloutDisplayed()
        {
            _spawnPoint = World.GetNextPositionOnStreet(Game.LocalPlayer.Character.Position.Around(400f));
            ShowCalloutAreaBlipBeforeAccepting(_spawnPoint, 30f);
            CalloutMessage = "Armed Person Pursuit";
            CalloutPosition = _spawnPoint;
            Functions.PlayScannerAudioUsingPosition("WE_HAVE DL_CRIME_STOLEN_VEHICLE_01", _spawnPoint);
            return base.OnBeforeCalloutDisplayed();
        }

        public override bool OnCalloutAccepted()
        {
            Game.LogTrivial("[OpenCallouts] Started callout Armed Person Pursuit");
            
            _stolenCar = new("chernobog", _spawnPoint)
            {
                IsPersistent = true,
                IsStolen = true
            };

            _suspect = _stolenCar.CreateRandomDriver();
            _suspect.BlockPermanentEvents = true;
            //_suspect.Inventory.GiveNewWeapon(WeaponHash.APPistol, 80, false);
            _suspect.Inventory.EquippedWeapon = _suspect.Inventory.GiveNewWeapon(WeaponHash.APPistol, 80, true);
            _suspect.Tasks.CruiseWithVehicle(40f, VehicleDrivingFlags.Emergency);
            _suspect.MaxHealth = 200;
            _suspect.Health = 200;
            _suspect.Armor = 50;
            _suspect.Tasks.FightAgainst(Game.LocalPlayer.Character);
            _blip = _suspect.AttachBlip();
            _blip.IsFriendly = false;
            
            return base.OnCalloutAccepted();
        }

        public override void Process()
        {
            if (_pursuitCreated == false)
            {
                _pursuit = Functions.CreatePursuit();
                Functions.AddPedToPursuit(_pursuit, _suspect);
                Functions.SetPursuitIsActiveForPlayer(_pursuit, true);
                Functions.RequestBackup(_spawnPoint, EBackupResponseType.Pursuit, EBackupUnitType.AirUnit);
                _pursuitCreated = true;
                if (Game.LocalPlayer.IsDead || _suspect.IsDead) End();
            }
            base.Process();
        }

        public override void End()
        {
            Game.LogTrivial("[OpenCallouts] Ended callout Armed Person Pursuit");
            
            if (_suspect) _suspect.Dismiss();
            if (_stolenCar) _stolenCar.Dismiss();
            if (_blip) _blip.Delete();
            Functions.PlayScannerAudio("ATTENTION_THIS_IS_DISPATCH_HIGH ALL_UNITS_CODE4 DL_NO_FURTHER_UNITS_REQUIRED");
            
            base.End();
        }
    }
}