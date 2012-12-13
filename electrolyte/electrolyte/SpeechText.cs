using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace electrolyte
{
    public enum SpeechTextState
    {
        NewGameNarration,
        OneVsOneNarration,
        Level1PlusSpeech1,
        Level1MinusSpeech1,
        Level1Info,
        Boss1PlusSpeech1,
        Boss1PlusSpeech2,
        Boss1MinusSpeech1,
        Boss1BossSpeech1,
        Boss1BossSpeech2,
        Boss1PlusSpeechVictory1,
        Boss1PlusSpeechVictory2,
        Boss1MinusSpeechVictory1,
        Boss1MinusSpeechVictory2,
        Boss1MinusSpeechVictory3,
        Boss1BossSpeechVictory1,
        Boss1BossSpeechDefeat1,
        Boss1BossSpeechDefeat2,
        PlusSpeechVictory,
        MinusSpeechVictory
    }

    class SpeechText
    {
        public static Dictionary<SpeechTextState, String> gameText = new Dictionary<SpeechTextState, String>();

        public static void initializeTextEng()
        {
            gameText = new Dictionary<SpeechTextState, String>();

            // New Game Text
            gameText.Add(SpeechTextState.NewGameNarration, "A gamer's console is broken! The gamer decides to run the system repair feature. Little to the gamer's knowledge, they send Plus and Minus on a mission to find and fix the problem!");
            gameText.Add(SpeechTextState.OneVsOneNarration, "Beat the other player to the exit to win!");
            
            // Level 1 Beginning Text
            gameText.Add(SpeechTextState.Level1PlusSpeech1, "Plus: Woo! New mission! FINALLY! Let's do this!");
            gameText.Add(SpeechTextState.Level1MinusSpeech1, "Minus: Calm down Plus... It seems like a simple electric disturbance. Looks like it's in this area. This shouldn't take long.");
            gameText.Add(SpeechTextState.Level1Info, "Red Resistors = Safe for Plus, Blue Resistors = Safe for Minus, Green = NOT safe for either of them.");

            // Boss Level 1 Beginning Speech
            gameText.Add(SpeechTextState.Boss1PlusSpeech1, "Plus: Hey what do you thing you're doing?!");
            gameText.Add(SpeechTextState.Boss1PlusSpeech2, "Plus: Depriving a gamer of their console.. on purpose! Unforgivable! Minus, let's get him!");
            gameText.Add(SpeechTextState.Boss1MinusSpeech1, "Minus: We gotta stop him! We won't be able to restore the current at this rate!");
            gameText.Add(SpeechTextState.Boss1BossSpeech1, "Volt: What does it look like? Breakin' stuff! What are you gonna do about it?!");
            gameText.Add(SpeechTextState.Boss1BossSpeech2, "Volt: Ha! Try and stop me!");

            // Boss Level 1 End Speech
            gameText.Add(SpeechTextState.Boss1BossSpeechVictory1, "Volt: Weaklings! You never stood a chance!");
            gameText.Add(SpeechTextState.Boss1PlusSpeechVictory1, "Plus: Aww yea! We win! In your face!");
            gameText.Add(SpeechTextState.Boss1PlusSpeechVictory2, "Plus: Hehe, no worries Minus. As long as we work together, we can beat anyone!");
            gameText.Add(SpeechTextState.Boss1MinusSpeechVictory1, "Minus: Us? Explain yourself.");
            gameText.Add(SpeechTextState.Boss1MinusSpeechVictory2, "Minus: Great, more enemies... so much for a simple task...");
            gameText.Add(SpeechTextState.Boss1MinusSpeechVictory3, "Minus: True... *sigh*. Well, let's get going. We got a lot of work ahead of us.");
            gameText.Add(SpeechTextState.Boss1BossSpeechDefeat1, "Volt: Ha, you think your little victory will stop us?");
            gameText.Add(SpeechTextState.Boss1BossSpeechDefeat2, "Volt: There's plently more like me all over this console! You'll never be able to stop us all!");

            // generic victory speech
            gameText.Add(SpeechTextState.PlusSpeechVictory, "Plus: On to the next one!");
            gameText.Add(SpeechTextState.MinusSpeechVictory, "Minus: Let's do it!");
        }

        // Returns the text of a speech text key
        public static String getText(SpeechTextState key)
        {
            if(gameText.ContainsKey(key)){
                return gameText[key];
            }
            return "";
        }

        // Returns the text of a speech text key in list format
        public static List<String> getTextArray(SpeechTextState key, int lineLimit)
        {
            List<String> textList = new List<String>();
            int counter = 0;

            if (gameText.ContainsKey(key))
            {
                String text = gameText[key];
                while(counter < text.Length){
                    int length = lineLimit;
                    
                    // Checks if the text is near the end
                    if (counter + lineLimit > text.Length)
                    {
                        length = text.Length - counter;
                    }
                    else{
                        // Adds the text up to the last space accrding to the limit of the text per line
                        int lastSpaceIndex = text.LastIndexOf(" ", counter + length, length);
                        length = lastSpaceIndex - counter;
                    }
                    textList.Add(text.Substring(counter, length));
                    
                    counter += length;
                }
                return textList;
            }
            return null;
        }

        // Returns a list of keys for the game text
        public static List<SpeechTextState> getGameTextKeys()
        {
            return gameText.Keys.ToList();
        }
    }
}
