using System;
using AGS.Core.Systems.CharacterSystem;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AGS.Core.Classes.ViewScripts
{
    /// <summary>
    /// ViewScript that handles targeting. Fills the player enemies within range array with appropriate targets
    /// </summary>
    public class TargetingSystem : ViewScriptBase
    {
        public float TargetingRange;
        protected PlayerBaseView PlayerBaseView;
        protected Player Player;

        protected override void SetupModelBindings()
        {
            if (ViewReference != null)
            {
                PlayerBaseView = ViewReference as PlayerBaseView;
                if (PlayerBaseView == null) return;

                Player = PlayerBaseView.Player;

            }
            if (Player == null) return;

        }

        public override void Update()
        {
            base.Update();
            FindTargets();
        }

        /// <summary>
        /// Finds all enemies within range and stores them in Player.EnemiesWithinRange array.
        /// </summary>
        /// <returns></returns>
        protected virtual void FindTargets()
        {
            Player.EnemiesWithinRange = null;
            if (FindObjectOfType<PlayerBaseView>() == null || Player == null || Player.Transform == null) return;

            var enemyViewsRaw = FindObjectsOfType<KillableBaseView>().Where(livingEntityBaseView => !(livingEntityBaseView as PlayerBaseView));
            var enemyViewsInRange = enemyViewsRaw.Where(livingEntityBaseView => Vector3.Distance(livingEntityBaseView.transform.position, Player.Transform.position) <= TargetingRange)
                                .OrderBy(livingEntityBaseView => Vector3.Distance(livingEntityBaseView.transform.position, Player.Transform.position));
            Player.EnemiesWithinRange = enemyViewsInRange.Select(x => x.Killable).ToArray();
        }
    }
}
