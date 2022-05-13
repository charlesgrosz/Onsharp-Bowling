using System;

namespace BowlingCalculation
{
    public class Bowling {         
        static void Main(string[] args)
        {
            System.Console.WriteLine("Welcome to the yearly Onsharp Bowling Tournament!");
            PlayerState state = new PlayerState();
            state = InitState(state);

            // main game loop
            while (state.round <= 10) {
                Console.WriteLine("Current Frame: " + state.round);
                PlayFrame(state);
                ResetFrame(state);
            }
            Console.WriteLine("Congratulations! Score: " + CalculateTotalScore(state));
        }

        // start of game values
        public static PlayerState InitState(PlayerState state) {
            state.round = 1;
            state.pinsThisRound = 0;
            state.totalScore = 0;
            state.turnsThisFrame = 0;
            state.pinsLeftThisFrame = 10;
            state.scoreCard = new List<FrameScore>(10);
            for (var i = 0; i < 10; i++) {
                state.scoreCard.Add(new FrameScore());
            }
            return state;
        }

        // simulate moving to next frame
        public static PlayerState ResetFrame(PlayerState state) {
            state.turnsThisFrame = 0;
            state.pinsLeftThisFrame = 10;
            state.pinsThisRound = 0;
            return state;
        }

        // simulate playing a single frame
        public static PlayerState PlayFrame(PlayerState state) {
            int turnsAvailable = 2;
            while (state.turnsThisFrame < turnsAvailable) {
                state.turnsThisFrame++;
                int knockedDownPins = Bowl(state);
                state.pinsThisRound += knockedDownPins;

                if (turnsAvailable == 3) {
                    if (state.turnsThisFrame == 2 ) {
                        state.scoreCard[state.round-1].bowl2 = knockedDownPins;
                        if (knockedDownPins == 10) {
                            Console.WriteLine("Strike!");
                        }
                        
                    }
                    else if (state.turnsThisFrame == 3 ) {
                        state.scoreCard[state.round-1].bowl3 = knockedDownPins;
                        if (knockedDownPins == 10) {
                            Console.WriteLine("Strike!");
                        } else if (state.scoreCard[state.round-1].bowl2 + state.scoreCard[state.round-1].bowl3 == 10) {
                            Console.WriteLine("Spare!");
                        }
                    } 

                }
                else if (state.pinsThisRound == 10 && state.turnsThisFrame == 1 && turnsAvailable == 2) {
                    state.scoreCard[state.round-1].isStrike = true;
                    state.scoreCard[state.round-1].bowl1 = 10;
                    Console.WriteLine("Strike!");
                    if (state.round == 10) {
                        turnsAvailable = 3;
                        state.pinsLeftThisFrame = 20;
                        state.scoreCard[state.round-1].isStrike = false;
                    } else {
                        break;
                    }
                    
                }
                else if (state.pinsThisRound == 10 && state.turnsThisFrame == 2 && turnsAvailable == 2) {
                    state.scoreCard[state.round-1].isSpare = true;
                    state.scoreCard[state.round-1].bowl2 = knockedDownPins;
                    Console.WriteLine("Spare!");
                    if (state.round == 10) {
                        turnsAvailable = 3;
                        state.pinsLeftThisFrame = 10;
                        state.scoreCard[state.round-1].isSpare = false;
                    }
                } 
                else {
                    if (state.turnsThisFrame == 1) {
                        state.scoreCard[state.round-1].bowl1 = knockedDownPins;
                    } else if (state.turnsThisFrame == 2) {
                        state.scoreCard[state.round-1].bowl2 = knockedDownPins;
                    }
                    
                }
            }
            state.round++;
            return state;
        }

        // player bowls one bowling ball
        public static int Bowl(PlayerState state) {
            Console.Write("Ball " + state.turnsThisFrame + " - Input how many pins were knocked down. ");
            int pins = 0;
            do {
                Int32.TryParse(Console.ReadLine(), out pins);
                if (state.round == 10) {
                    if (state.pinsLeftThisFrame > 10) {
                        if (pins > 10 || pins < 0) {
                            Console.WriteLine("Please input a number between 0 and " + 10 + ".");
                        }
                    } else {
                        if (pins > state.pinsLeftThisFrame || pins < 0) {
                            Console.WriteLine("Please input a number between 0 and " + state.pinsLeftThisFrame + ".");
                        }
                    }
                    
                } else {
                    if (pins > state.pinsLeftThisFrame || pins < 0) {
                        Console.WriteLine("Please input a number between 0 and " + state.pinsLeftThisFrame + ".");
                    }
                }
                
            } while (pins > state.pinsLeftThisFrame || pins < 0);

            state.pinsLeftThisFrame = state.pinsLeftThisFrame - pins;
            return pins;
        }

        public static int CalculateTotalScore(PlayerState state) {
            state.totalScore = 0;
            for (var i = 0; i < state.scoreCard.Count; i++) {
                if (i == state.scoreCard.Count - 1 && state.scoreCard[i].bowl3 > 0) {
                    state.totalScore += state.scoreCard[i].bowl1 + state.scoreCard[i].bowl2 + state.scoreCard[i].bowl3;
                }
                else if (!state.scoreCard[i].isSpare && !state.scoreCard[i].isStrike) {
                    state.totalScore += state.scoreCard[i].bowl1 + state.scoreCard[i].bowl2;
                } 
                else if (state.scoreCard[i].isSpare) {
                    state.totalScore += state.scoreCard[i].bowl1 + state.scoreCard[i].bowl2 + state.scoreCard[i+1].bowl1;
                } 
                else if (state.scoreCard[i].isStrike) {
                    if (state.scoreCard[i+1].isStrike) {
                        state.totalScore += state.scoreCard[i].bowl1 + state.scoreCard[i].bowl2 + state.scoreCard[i+1].bowl1 + state.scoreCard[i+2].bowl1;
                    } else {
                        state.totalScore += state.scoreCard[i].bowl1 + state.scoreCard[i].bowl2 + state.scoreCard[i+1].bowl1 + state.scoreCard[i+1].bowl2;
                    }
                    
                }
            }

            return state.totalScore;
        }
    }

    // model to hold the state of the player bowling
    public class PlayerState {
        public int round {get;set;}
        public int pinsThisRound {get;set;}
        public int totalScore {get;set;}
        public List<FrameScore> scoreCard {get;set;}
        public int turnsThisFrame {get;set;}
        public int pinsLeftThisFrame {get;set;}
    }

    public class FrameScore {
        public int bowl1 {get;set;}
        public int bowl2 {get;set;}
        public int bowl3 {get;set;}
        public bool isStrike {get;set;}
        public bool isSpare {get;set;}
    }
}
