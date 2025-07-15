using System.Collections.Generic;

public enum ChatStep
{
    None,               // Initial state, before anything starts
    Introduction,       // Welcome message
    AskBrickCount,      // "How many bricks do you want?"
    AskColors,          // "Which colors do you want?"
    AskShape,           // "Any specific shape?"
    AskDifficulty,      // "What difficulty level?"
    AskPowerups,        // "Do you want any power-ups or penalties?"
    Confirmation,       // Recap before generating the level
    Generating,         // Message while generating
    Done                // End of dialogue
}

public static class ChatStepPrompts
{
    public static Dictionary<ChatStep, string> Prompts = new()
    {
        { ChatStep.Introduction, "Welcome, traveler. Ready to craft your custom level?" },
        { ChatStep.AskBrickCount, "How many bricks do you want? (max 25)" },
        { ChatStep.AskColors, "Which colors should I use? (e.g. 5 red, 5 blue)" },
        { ChatStep.AskShape, "Do you want a specific shape? (cross, wave, heart...)"},
        { ChatStep.AskDifficulty, "What difficulty level? (easy, medium, hard)" },
        { ChatStep.AskPowerups, "Would you like any power-ups or penalties?" },
        { ChatStep.Confirmation, "Here is your challenge. Shall I begin the creation?" },
        { ChatStep.Generating, "Summoning the bricks... One moment." },
        { ChatStep.Done, "Your custom level is ready. Good luck!" }
    };
}
