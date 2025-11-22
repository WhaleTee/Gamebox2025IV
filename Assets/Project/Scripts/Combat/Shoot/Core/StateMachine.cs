using System.Collections.Generic;
using UnityEngine;

namespace Combat.Weapon.State
{
    public class StateMachine
    {
        private Dictionary<StateType, StateBase> states;
        private StateBase currentState;
        private StateType currentStateType;
        private WeaponStats stats;
        private Blackboard blackboard;

        private StateType Transitions()
        {
            #region StatesList
            var ready = StateType.Ready;
            var start = StateType.FireStart;
            var loop = StateType.FireLoop;
            var finish = StateType.FireFinish;
            var exhausted = StateType.Exhausted;
            #endregion
            StateType state = currentStateType;

            bool releasedFireWhileShooting = (!blackboard.Fire && (state == start || state == loop));
            bool runOutOfManaWhileShooting = (state == loop && blackboard.Mana <= 0);

            if (releasedFireWhileShooting || runOutOfManaWhileShooting)
                return finish;
            if (blackboard.Mana <= 0)
                return exhausted;
            if (state == ready && blackboard.Fire && Time.time > blackboard.NextShotTime)
                return start;
            if (state == start && blackboard.Fire && stats.Mode != FireMode.Semi && Time.time > blackboard.NextShotTime)
                return loop;
            if (state != exhausted && state != start && state != loop)
                return ready;
            return state;
        }

        public void Install(WeaponStats stats, Blackboard blackboard, Events events)
        {
            this.stats = stats;
            this.blackboard = blackboard;

            states = new()
            {
                { StateType.Ready, new Ready() },
                { StateType.FireStart, new FireStart() },
                { StateType.FireLoop, new FireLoop() },
                { StateType.FireFinish, new FireFinish() },
                { StateType.Cancel, new Cancel() },
                { StateType.Exhausted, new Exhausted() }
            };

            foreach (var state in states)
                state.Value.Install(stats, events, blackboard);
        }
        public void Init()
        {
            currentState = states[StateType.Ready];
        }

        public void Tick()
        {
            SetState(Transitions());
            currentState.Tick();
        }

        public void OnDisable()
        {
            FireCancel();
            Tick();
        }

        public void Fire()
        {
            blackboard.Fire = true;
        }
        public void FireCancel()
        {
            blackboard.Fire = false;
            blackboard.Cancel = true;
        }

        private void SetState(StateType state)
        {
            if (currentStateType == state)
                return;

            Debug.Log($"[{currentStateType}] >>> [{state}]");
            currentState.Exit();
            currentStateType = state;
            currentState = states[state];
            currentState.Enter();
        }

    }
}
