using AGS.Core.Classes.ActionProperties;
using AGS.Core.Enums;
using AGS.Core.Systems.BaseSystem;
using AGS.Core.Systems.CharacterSystem;
using UnityEngine;

namespace AGS.Core.Systems.CharacterControlSystem
{
    /// <summary>
    /// This is the base class for any controlled character. It can be implemented as an InputController for the player, or as an AI for enemies or NPCs.
    /// Extend this with any needed input values
    /// </summary>
    public abstract class CharacterControllerBase : ActionModel
    {
		#region Properties

        public ActionProperty<CombatEntityBase> OwnerCombatEntity { get; protected set; } // Reference to the owner CombatEntity
        public ActionProperty<Vector2> MoveVector { get; protected set; } // MoveVector corresponds to the X and Y axis.
        public ActionProperty<Direction> Direction { get; protected set; } // Direction is a simplified direction based on MoveVector. I.E a D-pad thats goes in 8 directions.

        // Input buttons
        public ActionProperty<bool> Jump { get; private set; }
        public ActionProperty<bool> Sprint { get; private set; }
        public ActionProperty<bool> Sneak { get; private set; }
        public ActionProperty<bool> Crouch { get; private set; }
        public ActionProperty<bool> Interact { get; private set; }
        public ActionProperty<bool> Action { get; private set; }
        public ActionProperty<bool> Aim { get; private set; }
        public ActionProperty<bool> Fire1 { get; private set; }
        public ActionProperty<bool> Fire2 { get; private set; }
        public ActionProperty<bool> Fire3 { get; private set; }
        public ActionProperty<bool> Attack1 { get; private set; }
        public ActionProperty<bool> Attack2 { get; private set; }
        public ActionProperty<bool> Attack3 { get; private set; }
        public ActionProperty<bool> NextWeapon { get; private set; }
        public ActionProperty<bool> PreviousWeapon { get; private set; }
        public ActionProperty<bool> NextThrowable { get; private set; }
		#endregion Properties


        /// <summary>
        /// Initializes a new instance of the <see cref="CharacterControllerBase"/> class.
        /// </summary>
        protected CharacterControllerBase()
        {
            OwnerCombatEntity = new ActionProperty<CombatEntityBase>();
            MoveVector = new ActionProperty<Vector2>();
            Direction = new ActionProperty<Direction>();
            Jump = new ActionProperty<bool>();
            Sprint = new ActionProperty<bool>();
            Sneak = new ActionProperty<bool>();
            Crouch = new ActionProperty<bool>();
            Interact = new ActionProperty<bool>();
            Action = new ActionProperty<bool>();
            Aim = new ActionProperty<bool>();
            Fire1 = new ActionProperty<bool>();
            Fire2 = new ActionProperty<bool>();
            Fire3 = new ActionProperty<bool>();
            Attack1 = new ActionProperty<bool>();
            Attack2 = new ActionProperty<bool>();
            Attack3 = new ActionProperty<bool>();
            NextWeapon = new ActionProperty<bool>();
            PreviousWeapon = new ActionProperty<bool>();
            NextThrowable = new ActionProperty<bool>();
		}
	}
}
