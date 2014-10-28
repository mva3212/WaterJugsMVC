using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WaterJugsMVC.Models
{

    public struct JugState
    {
        public int Jug1State;
        public int Jug2State;
        public int Jug1Capacity;
        public int Jug2Capacity;
        public string Message;

        public JugState(int state1, int capacity1, int state2, int capacity2, string message)
        {
            Jug1State = state1;
            Jug2State = state2;
            Jug1Capacity = capacity1;
            Jug2Capacity = capacity2;
            Message = message;
        }

        internal JugState GetNextState()
        {
            //Deep Copy
            var tmpState = new JugState(Jug1State, Jug1Capacity, Jug2State, Jug2Capacity, Message);
            if (tmpState.IsJug2Full())
            {
                tmpState.EmptyJug2();
            }
            else if (tmpState.IsJug1Empty())
            {
                tmpState.FillJug1();
            }
            else
            {
                tmpState.DumpJug1IntoJug2();
            }
            return tmpState;
        }

        private void FillJug1()
        {
            Jug1State = Jug1Capacity;
            Message = String.Format("Fill {0} gallon Jug", Jug1Capacity);
        }

        private void DumpJug1IntoJug2()
        {
            var amount = Jug2Capacity - Jug2State;
            if (amount > Jug1State)
            {
                Jug2State = Jug2State + Jug1State;
                Jug1State = 0;
            }
            else
            {
                var leftover = Jug1State - amount;
                Jug2State = Jug2Capacity;
                Jug1State = leftover;
            }
            Message = String.Format("Dump {0} gallon Jug into {1} gallon Jug", Jug1Capacity, Jug2Capacity);
        }

        private void EmptyJug2()
        {
            Jug2State = 0;
            Message = String.Format("Empty {0} gallon Jug", Jug2Capacity);
        }

        private bool IsJug1Empty()
        {
            return Jug1State == 0;
        }

        private bool IsJug2Full()
        {
            return Jug2State == Jug2Capacity;
        }
    }
}