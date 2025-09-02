/*
// 1 - Before Proxima 16
private string[,] dialoguePairs = {
    { "Haven't seen you in a while.", "shp_normal" },
    { "You're lucky I managed to catch your ship before it ran out of fuel.", "shp_normal" },
    { "You still owe me a lot of scrap, so go out there and fish for some.", "shp_unamused" },
    { "You can auto-navigate to the system I assigned using your terminal.", "shp_normal" },
    { "After you're done, record your progress and I'll assign your next mission.", "shp_normal" },
    { "Also... if you run out of fuel again, I can't promise little old me will be there to save you.", "shp_normal" }
}; 
// 2 - After Proxima-16
private string[,] dialogue2pairs = {
    { "Welcome back. I've seen you made good progress collecting junk.", "shp_normal" },
    { "You can sell your junk by talking to me.", "shp_normal" },
    { "You can also purchase something using your credits to help you.", "shp_normal" }
}; 
// 3 - Rare random encounter
private string[,] hyperdrivePairs = {
    { "Hey there... You have... that hyperdrive.", "shp_normal" }, 
    { "Give it to me.", "shp_normal" },  
};
// No
private string[,] no1pairs = {
    { "What. No, give it to me.", "shp_normal" }
};
private string[,] no2pairs = {
    { "I'll pay you 500.", "shp_normal" }
};
private string[,] no3pairs = {
    { "Fine, 1000. ", "shp_normal" }
};
private string[,] no4pairs = {
    { "...", "shp_normal" }
}; 

private string[,] unamusedCheckInPairs = {
    { "You're back? Did you forget what I said?", "shp_unamused"},
    { "Go and collect junk, dingus.", "shp_unamused"},
};
private string[,] dontBotherPairs = { 
    { "[...I should probably leave him alone before he gets any more angry.]", "player_think"},  
};

// Rare random encounters
private string[,] averyPairs = {
    { "Heya!", "shp_normal" },
    { "Who me? I'm filling in for Shopkeeper today. He's feeling under the weather.", "shp_normal" },
    { "Name's Avery. I pilot this hunk of junk.", "shp_normal" },
    { "I rarely get to meet the junkers since that's Shopkeeper's business, but it's nice talking to other people. Not that Shopkeeper isn't fun to talk to!!", "shp_normal" }
};
// Why's shopkeeper so pissy? 
private string[,] avery1Pairs = {
    { "Hehe... That's just how he is, I suppose. He doesn't mean much by it.", "shp_normal" },
    { "I'd be irritable too if some intergalactic federation has a kill-on-sight order for humans.", "shp_normal" },
    { "When I found him, flying solo, I found him floating in space!", "shp_normal" },
    { "Can you believe that Mamaris can hibernate for long periods of time in space and be totally fine? It's incredible. They can even breathe from their side vents!", "shp_normal" },
    { "Anyways, so yeah, so he had 3 of his limbs chopped off for doing some grand sin against Mamarian society or something, I dunno, he doesn't like to talk much about it.", "shp_normal" },
    { "Since then, I've took him under my wing, taught him how everything works, the Federation, the universal language, et cetera. Turns out, he really has a knack for fiddling and inventing stuff.", "shp_normal" },
    {"So I guess that's why the I.F. decided to keep him, and plus having a non-hostile Mamari to experiment with...", "shp_normal"},
    {"...It's despicable. But it's better than...", "shp_normal"}, 
    {"Let's talk about something else.", "shp_normal"}
};
// Are you his girlfriend?
private string[,] avery2Pairs = {
    { "O-oh umm... Yeah, I suppose you could say that. Though I probably use that exact phrasing. More like partners.", "shp_normal" },
    {"Yeah, partners...", "shp_normal"},
    { "*Peaks his head in* NOT YOUR BUSINESS, JUNKER.", "shp_normal" },
    {"Why are you out of bed? *angry*", "shp_normal"},
    {"Ignore him.", "shp_normal"} 
};
// I'm quitting. 
private string[,] avery3Pairs = {
    { "That's unadvisable. We're assets of the I.F. And they don't really do 'quitting' unfortunately.", "shp_normal" },
    { "That's to say it's either this or forced labor, or execution, or whatever method of torture they invent next.", "shp_normal" },
};

private string[,] statikPairs = {
    { "Hey there.", "stk_smile" },
    { "I'm a friend of your buddy here. Name's Statik. I.T. guy for most of the creatures this quadrant.", "stk_smile" },
    { "Usually, I don't get to meet other Mamari, buuttt.. my totally rad skills merit special consideration. ", "stk_smile" },
    { "And I missed your old buddy here. ", "stk_smile" },
    { "Hurry up and fix my ship already before I throw your ass back to space.", "shp_normal" },
    { "Ah... true bromance! How long have we known each other? And yet in all that time you never called me... I'm starting to think that you don't miss me. ", "stk_smile" },
    { "We could have bonded over our shared trauma or something.", "stk_smile" },
    { "Hmm... Perhaps you were too busy fraternizing with that human pilot of yours?", "stk_smile" },
    { "What's his name again? Or was it a her? Or they? Speaking of which, where are they? You didn't eat them did you? ", "stk_smile" },
    { "I'm gonna kill you. ", "shp_sulking" },
    { "That is unadvisable. I am a very valuable asset to the I.F." , "stk_smile" },
    { "Well, no matter. I'm almost done with the updates to your communications. ", "stk_smile" },
    { "Thank Jods. I don't think I can stand another second of your incessant babbling.", "shp_unamused" },
    { "Strange, usually networking bugs aren't this elaborate. Whatever, it's probably just some troll with too much time. ", "stk_smile" },
    { "Yup, that's pretty much it...", "stk_smile" },
    { "Alright, I'll be leaving now.", "stk_smile" },
    { "Bye cutie.", "stk_smile" },
    { ".....", "shp_unamused" }
};*/

using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogueLine
{
    [TextArea(2, 5)] public string text;
    public string expressionKey;
}

[System.Serializable]
public class ButtonLine
{
    [TextArea(2, 5)] public string buttonText;
    public DialogueData nextDialogue;
}

[CreateAssetMenu(fileName = "NewDialogue", menuName = "Dialogue/Conversation")]
public class DialogueData : ScriptableObject
{
    public string conversationName;
    public List<DialogueLine> lines = new List<DialogueLine>();
    public DialogueData nextDialogue;
    public bool hasButtons = false;
    public List<ButtonLine> buttons; 
}