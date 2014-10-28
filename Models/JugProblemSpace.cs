using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Http.ModelBinding;

namespace WaterJugsMVC.Models
{

    public class JugsProblemSpace
    {
        [Required(ErrorMessage = "Capacity of Jug X is Required")]
        // Arbitrarily restricted problem space to size of int
        [Range(1, int.MaxValue, ErrorMessage = "Jug X Capacity must be a positive non-zero number")]
        public int JugXCapacity { get; set; }

        [Required(ErrorMessage = "Capacity of Jug X is Required")]
        [Range(1, int.MaxValue, ErrorMessage = "Jug Y Capacity must be a positive non-zero number")]
        public int JugYCapacity { get; set; }

        [Required(ErrorMessage = "Goal Capacity Required")]
        [Range(1, int.MaxValue, ErrorMessage = "Goal Capacity must be a positive non-zero number")]
        public int GoalCapacity { get; set; }

        public List<JugState> ShortestSolution { get; private set; }

        // If the problem space is unsolvable -- populated with reason why it can't be solved
        public string UnsolvableReason { get; private set; }

        public JugsProblemSpace(int jugXCapacity, int jugYCapacity, int goalCapacity)
        {
            JugXCapacity = jugXCapacity;
            JugYCapacity = jugYCapacity;
            GoalCapacity = goalCapacity;
        }


        public List<JugState> SolveForShortestSolution()
        {
            ShortestSolution = null;
            // Test if problem space is solvable
            if (IsSolvable())
            {
                // Two solutions exist for solvable problems, solve for filling JugX first then solve for filling JugY first
                var solutionXY = GetSolution(JugXCapacity, JugYCapacity, GoalCapacity);
                var solutionYX = GetSolution(JugYCapacity, JugXCapacity, GoalCapacity);
                //Compare the number of jug states and return the solution with lowest state count
                ShortestSolution = solutionXY.Count < solutionYX.Count ? solutionXY : solutionYX;
            }
            return ShortestSolution;
        }

        private List<JugState> GetSolution(int first, int second, int goal)
        {
            var solution = new List<JugState>();
            var currentState = new JugState(0, first, 0, second, "Start with both jugs empty");
            while (currentState.Jug1State != GoalCapacity && currentState.Jug2State != GoalCapacity)
            {
                solution.Add(currentState);
                Console.WriteLine(String.Format("{0},{1}", currentState.Jug1State, currentState.Jug2State));
                var nextState = currentState.GetNextState();
                currentState = nextState;
            }
            solution.Add(currentState);
            return solution;
        }

        private bool IsSolvable()
        {
            var issolvable = true;
            if (IsGoalCapacityGreaterThanBothJugCapacities())
            {
                issolvable = false;
                UnsolvableReason = "Goal Capacity is greater than the capacity of Jug X and Jug Y. Problem cannot be solved.";
            }
            else if (AreJugsEqualButNotEqualToGoal())
            {
                issolvable = false;
                UnsolvableReason = "Jug Capacities are equal to eachother but not equal to Goal.  Problem cannot be solved.";
            }
            else if (IsGoalNotAMultipleofGCD_XY())
            {
                issolvable = false;
                UnsolvableReason = "Goal is not a multiple of GCD(X,Y). Problem cannot be solved.";
            }
            else
            {
                //do nothing and proceed to solving problem
            }
            return issolvable;
        }

        //Problem is unsolvable if Goal is not a multiple of the GCD(X,Y)
        private bool IsGoalNotAMultipleofGCD_XY()
        {
            var gcd = Util.GCD(JugXCapacity, JugYCapacity);
            return (GoalCapacity % gcd) != 0;
        }

        //Problem is unsolvable if jug capacities are equal to eachother and not equal to goal capacity
        private bool AreJugsEqualButNotEqualToGoal()
        {
            return JugXCapacity == JugYCapacity && JugXCapacity != GoalCapacity;
        }

        // Problem is unsolvable is Goal capacity is larger than both the Jug Capacities
        private bool IsGoalCapacityGreaterThanBothJugCapacities()
        {
            return GoalCapacity > JugXCapacity && GoalCapacity > JugYCapacity;
        }

    }

}