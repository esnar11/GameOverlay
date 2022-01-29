﻿// <copyright file="DynamicConditionState.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace SimpleFlaskManager.ProfileManager.Conditions.DynamicCondition
{
    using System.Collections.Generic;
    using System.Linq;
    using GameHelper.RemoteEnums;
    using GameHelper.RemoteObjects.Components;
    using GameHelper.RemoteObjects.States;
    using Interface;

    /// <summary>
    ///     The structure that can be queried using DynamicCondition
    /// </summary>
    public class DynamicConditionState : IDynamicConditionState
    {
        /// <summary>
        ///     Creates a new instance
        /// </summary>
        /// <param name="state">State to build the structure from</param>
        public DynamicConditionState(InGameState state)
        {
            if (state != null)
            {
                var player = state.CurrentAreaInstance.Player;
                if (player.TryGetComponent<Buffs>(out var playerBuffs))
                {
                    this.Ailments = JsonDataHelper.StatusEffectGroups
                                                  .Where(x => x.Value.Any(playerBuffs.StatusEffects.ContainsKey))
                                                  .Select(x => x.Key).ToHashSet();
                    this.Buffs = new BuffDictionary(playerBuffs.StatusEffects);
                }

                if (player.TryGetComponent<Actor>(out var actorComponent))
                {
                    this.Animation = actorComponent.Animation;
                }

                if (player.TryGetComponent<Life>(out var lifeComponent))
                {
                    this.Vitals = new VitalsInfo(lifeComponent);
                }

                this.Flasks = new FlasksInfo(state);
            }
        }

        /// <summary>
        ///     The buff list
        /// </summary>
        public IBuffDictionary Buffs { get; }

        /// <summary>
        ///     The current animation
        /// </summary>
        public Animation Animation { get; }

        /// <summary>
        ///     The ailment list
        /// </summary>
        public IReadOnlyCollection<string> Ailments { get; } = new List<string>();

        /// <summary>
        ///     The vitals information
        /// </summary>
        public IVitalsInfo Vitals { get; }

        /// <summary>
        ///     The flask information
        /// </summary>
        public IFlasksInfo Flasks { get; }
    }
}
