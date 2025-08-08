using System.Collections; 
using UnityEngine;
using UnityEngine.UI; 
using TMPro;
using System.Collections.Generic;

public class TerminalAstronomicon : MonoBehaviour
{
    public GameObject menu; 
    public GameObject entries; 
    public ScrollRect scrollRect;
    public TextMeshProUGUI speciesTitle; 
    public TextMeshProUGUI speciesBody; 
    public TextMeshProUGUI menuTitle; 
    public TextMeshProUGUI menuBody;  
    public TextMeshProUGUI speciesExBody; 
    public Image astroImage; 
    public Sprite spr_human; 
    public Sprite spr_zwailek;
    public Sprite spr_mamari;
    public Sprite spr_wewari;
    public Sprite spr_shadowmen; 
    private int currentCount = 0;  
    public int screenID = 0;  
    private const int MAIN_MENU_ID = 0;
    private const int SOLAR_EX_ID = 1;
    private const int SPECIES_EX_ID = 2;
    private const int JOURNAL_EX_ID = 3; 
    private const int SOLAR_ID = 4;
    private const int SPECIES_ID = 5;
    private const int JOURNAL_ID = 6;
    public int menuCount = 0; 
    private int speciesCount = 0;
    private int journalCount = 0; 
    private int planetCount = 0;   
    public bool canInteract = false;   
    private string[,] allJournalTexts = {
        {"Entry 1","Jared told me to keep a journal of what’s happening, just for the sake of my own mental health. He told me to write stuff, whatever it is. So that’s what I’m doing now. I don’t really know what to write, so I’ll just write about whatever systems we’re currently on. If I run out of systems to write about, I’ll think of something else.", "Inherent"},
        {"Entry 2","Today we’re at Proxima-16, a nice little system. The planet that’s there reminds me of Prime Earth - mild weather, cool oceans. Jared and I caught some of the local demonic crab-like cretins while we were goofing off at the beach (my suggestion to land). Still hate him for that. It’s not funny, Jared. Stop smiling.\n\nWe did a scan on the ‘crab’ we caught to see if it was something familiar, and turns out, it was! A 97.7% match with Metacarcinus magister. Guess these freaks really liked crabs in the past. Personally, I see no such appeal. They’re evil.", "Locked"},
        {"Entry 3","Every day is the same to us. We hang out in a system for a while, gather up all that we can, then scram when I.F. cops come for us. Dumb fools don’t know they’re paying us to get their junk, only to say it’s illegal when it suits them. Whatever though, as long as me and Jared get coin, I’d say it’s a win. When it’s all over, we’re gonna buy a nice condo, or a suite somewhere in the Lavenders. Wouldn’t that be nice?", "Locked"},
        {"Entry 4","I know we weren’t supposed to, but it was calling out to us. Plus, all the other systems this side of the Uni has been scalped already. There’s nothing left for us. If they really do say it has an edge, why haven't there been any real accounts of it? Jared thought I was crazy, but after a while, he relented. It’s been tough meeting Shopkeeper’s quota lately, and we’re afraid if next time we don’t get enough to please him, something… something real bad might happen. We can’t trust him. He’s all smiles but at the end of the day he’s an I.F. bootlicker. If it goes well, there will be another entry detailing what’s at, or past, the Far Shore.", "Locked"},
        {"Entry 5","[ERROR: DATA CORRUPTION, UNABLE TO DISPLAY]", "Locked"}
    };
    private List<string[]> validJournalTextList = new List<string[]>();
    private string[,] allSystemsTexts = {
        {"Lone-88", "Star(s): Lone-88\n\nPlanet(s): None\n\n A lone white star used as the base of operations for a shopkeeper. ", "Unlocked" },
        {"Proxima-16","Star(s): Proxima-16\n\nPlanet(s): Proxima-16-a\n\nA medium-small star with a pale yellow color. The singular planet in the solar system is notable for its mild climate and green flora. In the past, it was used as an industrial and recreational hub due to its similarities to Earth Prime, but due to industrial overuse, it became too polluted to continue operating. After a long period of neglect and oversight from the Intergalactic Federation, the planet has begun to recover. The planet is home to many divergent crab species, which sprung off from the crabs introduced for consumption in the past.", "Inherent"},
        {"Luxe-10","Star(s):\n\nPlanet(s):\n\nOnce a mining planet for luxury goods for the Lavender Region, this planet has been stripped of its natural resources, leaving only a barren wasteland behind. The purple from its soil comes from photosynthetic microorganisms.", "Locked"},
        {"Ridge-65", "Star(s):\n\nPlanet(s):\n\nA dusty, hot, sand planet with very little vegetation. There are no valuable resources on this planet except for probably trace amounts of spice bushes, but still, not an ideal climate for agriculture. Besides the small rodent-like creatures that wander the dunes, there are small brushes and other microflora.", "Locked"},
        {"Magnetic-73", "Star(s): Magnetic-73 (Black Hole)\n\nPlanet(s):\n\nMagnetic-73 is an uncommon system as the planetary bodies orbit a black hole. Ships should be advised to stay a considerable distance away from the black hole as escaping the gravitational pull is near impossible once close enough. The other 6 planets in the system aren't notable, as most of them are barren rock planets, with all lifeforms burnt away after the system’s supernova approximately 90,000,000 years ago, with the exception of the last planet, who is a gas giant.", "Locked"},
        {"1?", "Desc1", "Locked"},
        {"2?", "Desc2", "Locked"},
        {"3?", "Desc3", "Locked"}
    };
    private List<string[]> validSystemTexts = new List<string[]>();
    private string[,] allSpeciesTexts = {
        {"Human","The First.\n\nHumans are the most versatile and intelligent of species. Beginning sometimes in the late 2100s, Humans were the first species to discover space flight and employ it to an intergalactic degree. By the 2400s, classical species such as the Zwai'lek and Kainth were commonplace within their Intergalactic Federation and have formed a close bond to them. As they expanded more and more, they need more resources, more minerals, more energy to maintain the I.F. Upon hitting the border of the Universe, the Far Shore, and realising that many aspects of wellbeing in the Federation were dependent on the material resources, protocol [EXPUNGED] was enabled, to ensure [EXPUNGED] from [EXPUNGED].\n\n Today, humans are the most common of species within the I.F. and have enjoyed perks of being first-class citizens. Many humans with bloodlines going back millenias, have accumulated vast wealth over the generations, as such, they are the envy of other species.\n\nDepicted: Male and female", "Inherent"},
        {"Zwai'lek","Warm-blooded Workers.\n\nIF Threat Level: D\n\nIF Acquisition Prospect: Acquired\n\nHailing from the dusty planet of Zwai'wen, the Zwai'lek are an adaptable and affable people. They only live for a few decade, and have a preference for large families. In addition, after the acquisition of their planet by the Intergalactic Federation, they often work jobs requiring manual labor, making them a valuable asset. They are the second most common species in the IF, possibly to their planet's early acquisition and biology. This earned them the name of 'space rats' which is considered a slur.\n\nIn a traditional tribe of Zwai'lek, the females are the caretakers of children while the males are the hunter of prey, although these dynamics aren't rigid to gender as many perform the other's traditional roles. Zwai'lek culture emphasize family, ambition, and togetherness. Even if two Zwai'lek in the Intergalactic Federation never met, they are more than ready to share drinks and call each other familial endearments. Indeed, Zwai'lek's ability to form strong life-long bonds is something that should be appreciated by other species, but instead to them, they are merely 'dirty space rats'.\n\nDepicted: Male and female", "Locked"},
        {"Mamari","Fearsome warmongers.\n\nIF Threat Level: A\n\nIF Aquisition Prospect: K.O.S., refugees to be monitored by [EXPUNGED]\n\nNot much is known about them as they are hostile to other species and refused on numerous counts to join the Intergalactic Federation. Locating their homeworld is of utmost priority to the safety of I.F. citizens. They tend to strike vulnerable areas in small, agile fleets, making them hard to track down.\n\nFrom what is known about their society, the Mamari are matriachal, with female Mamari serving commanding roles in government and war, while males who are considerably smaller and weaker, serve under them in servant roles. Interestingly enough, the sex of Mamari children are indifferentiable until puberty, where they then diverge into female and male. What causes this differentiation is unknown, but it is speculated to have something to do with [EXPUNGED]. Children Mamari refugees in the Intergalactic Federation have all matured into male, with only one known exception in the last 400 years.\n\nEven with the large differentiation in body between the sexes, both male and female Mamari's outer exoskeleton is extremely tough. They possess vent-like openings in their arms, legs, necks, and stomach that allows them to breathe more efficiently and they can speed up or slow down their heart rate, much like many hibernating species, when they see suitable. Their blood quickly fuses in the case of an open wound, allowing them to fight in open-hand combat even if they have a mortal wound. In one such case, a male Mamari had all of his limbs torn off, and was able to survive in space for 21 days in a hibernation state before he was found by an IF scout pilot.\n\nDue to the past and current nature of Mamari relations, all Mamari in the IF are to be monitored closely. Further details to come.\n\nDepicted: Male and female", "Locked"},
        {"Wewari","Peaceful, Short-statured Aquatics.\n\nIF Threat Level: F\n\nIF Acquisition Prospect: In-Progress\n\nIt is speculated that the Wewari and [EXPUNGED] share a common heritage, suggesting they branched off from one another about 6 M.Y.A. It is suprising then, to learn that the Wewari are a completely pacifist species, with all members being functionally the same in social roles and hierarchy, with the exception of a tribe's King(s) and Queen(s).\n\nWewari are short with their average height being 4''. Though they may look bumbling and chunky on land, their build allows for them to maneuver in the waters of their homeworld efficiently. The Wewari are traditionalist in belief, and prefer to stick to themselves and their homeworld rather than colonizing other water planets. Their planet often freezes over in cycles, and to adapt, the Wewari can enter a stone-like hibernation state to escape unfavorable conditions, with warm weather reviving them from their stone-like hibernation state.\n\nMost Wewari lack sexual characteristics, with the King(s) and Queen(s) being the only reproducing pair(s). It is unknown what causes maturity of a Wewari into a King, but as they do, they gain male reproductive organs and act as a leader of their Wewari tribe. Kings, who have matured long enough, step down from their position to let other Kings take their place and transition into birthing Queens, who become physically larger and unable to maneuver well as her body morphs to take on the duty of birth-giving. It is important to note that these though these physical changes do exist, the hierarchy of those sexually-reproducing and asexuals are equally valued in contribution to the health of the tribe.\n\nDepicted: Asexual, male, and female", "Locked"},  
        //{"Shadowmen","Capable Huntresses\n\nIF Threat Level: D\n\nOver the millenias, the hermaphroditic Kainth evolved from weak bug-like creatures to capable hunters. They operate in solitary units naturally and within the I.F, making their threat level overall low. Kainth are naturally protective of their terrority and see other Kainths as competition. While this lead to deadly battles in the wild, in civilization, this manifests itself as a deep hatred of other of their kind. In fact, the only time a Kainth is compelled to be near another Kainth is in the heat of their reproductive cycle. This however, often leads to tragic ends when their short cycle passes. Kainth competition doesn't appear to manifest itself to other species, with an exception to that of the Mamari females. Perhaps their similiarity in aggressive natures triggers this.\n\nOnce a Kainth successfully subdues a mate, they lay a clutch of eggs, whom hatch into a tiny larvae. Larval Kainth are voracious eaters, and will eat any flesh, and each other if left alone. They then pupates into a young adult Kainth upon gaining enough nutrients. The larval state lasts anytime between 6-12 months and the pupation period lasts up to [EXPUNGED] years. Maturing as an adult is a great feat, as most larvae die within the first few months.\n\nIn the I.F, since solitary combat comes naturally to the Kainth, it is no suprise they often take up work as small-time bodyguards, bounty hunters, and the like. But it seems like no matter how civilized they appear to be, their inner hatred of their species stifles any semblance of progress they have as a species. \n\nDepicted: Average Kainth"},
        //{"Shadowmen","Gentle Aquatics\n\nIF Threat Level: F\n\nIF Acquisition Prospect: Acquired\n\nSquoobs are soft-bodied aquatic creatures. They can exchange thoughts by connecting their tentacles together in a 'handshake' and exchanging electromagnetic waves. Though they look soft and cute, their tentacles are capable of a deadly sting. They can read and decipher other beings' thoughts using their tentacles, which have helped them on their aquatic homeworld master their ocean habitat. In the I.F, since they are an fully-aquatic species, they require the assistance of a tanker. Once enabled, the Squoobs have excellent memory and make for great history-keepers and teachers, though they may be a bit deft when it comes to terrestial species' etiquette.\n\nDepicted: Male and female"}, 
        {"Shadowmen","The Outsiders\n\nIF Threat Level: S\n\nIF Acquisition Prospect: K.O.S.\n\nShadowmen, in their natural state, are formless beings. They must possess living beings to gain sustainable consciousness, and upon doing so, they are inclined to use their newfound intelligence to infect other conscious beings. There are no known signs of possession, thus making them a high-priority threat to the well being of the Intergalactic Federation. We do not know why or how they came to be or what their motives are, but one thing is true: They must be exterminated, at all cost. If seen, kill on sight. Do not hesitate, even if one reveals themselves to be your friend or family. It could be you.", "Locked"}
    };
    private List<string[]> validSpeciesTexts = new List<string[]>();

    void Start()
    {
        entries.SetActive(false);
        menu.SetActive(true);
        extractValidList(ref validJournalTextList, allJournalTexts);
        extractValidList(ref validSystemTexts, allSystemsTexts); 
        extractValidList(ref validSpeciesTexts, allSpeciesTexts); 
    }
    IEnumerator EnableInteractionAfterDelay(float delay) {
        yield return new WaitForSeconds(delay);
        canInteract = true; 
        displayMainMenuText(); 
    }
    void OnEnable()
    {
        menuTitle.text = "[BOOTING SYSTEM...]";
        menuBody.text = "";
        StartCoroutine(EnableInteractionAfterDelay(2f));
    } 
    void Update(){
        if (canInteract){
            Navigate(); 
        }
    }

    void OnDisable(){
        canInteract = false; 
    } 

    // Gets a list of valid elements from a masterlist (3rd attribute must be Unlocked or Inherent, not Locked or X, etc)
    private void extractValidList(ref List<string[]> subList, string[,] masterList)
    {
        Debug.Log("Calculating valid texts");
        List<string[]> tempList = new List<string[]>();
        for (int i = 0; i < masterList.GetLength(0); i++)
        {
            if (masterList[i, 2] == "Unlocked" || masterList[i, 2] == "Inherent")
            {
                string[] row = new string[masterList.GetLength(1)];
                for (int j = 0; j < masterList.GetLength(1); j++)
                {
                    row[j] = masterList[i, j];
                }
                tempList.Add(row);
                Debug.Log("Added row");
            }
        }
        subList = tempList; 
    }
    /*
    ids: 
    0 - main
    1 - solar ex 
    2 - species ex
    3 - journal ex
    4 - exit 
    5 - solar
    6 - species 
    7 - journal
    */
    void Navigate() {
        // Back key for astronomicon
        if (Input.GetKeyDown(KeyCode.E)){
            switch (screenID) {
                case MAIN_MENU_ID: 
                    canInteract = false; 
                    GameObject.Find("GameManager").GetComponent<GameManager>().goShip(); 
                    break;
                case SOLAR_EX_ID: 
                case SPECIES_EX_ID: 
                case JOURNAL_EX_ID:  // Back to main menu
                    menu.SetActive(true); 
                    entries.SetActive(false); 
                    menuBody.gameObject.SetActive(true); 
                    menuTitle.gameObject.SetActive(true);
                    speciesExBody.gameObject.SetActive(false); 
                    screenID = 0; 
                    displayMainMenuText();  
                    break;  
                case SOLAR_ID: 
                    menu.SetActive(true); 
                    entries.SetActive(false); 
                    menuBody.gameObject.SetActive(false); 
                    menuTitle.gameObject.SetActive(true);
                    speciesExBody.gameObject.SetActive(true); 
                    screenID = 1; 
                    displaySolarSystemText();  
                    break;  
                case SPECIES_ID: 
                    menu.SetActive(true); 
                    entries.SetActive(false); 
                    menuBody.gameObject.SetActive(false); 
                    menuTitle.gameObject.SetActive(true);
                    speciesExBody.gameObject.SetActive(true); 
                    screenID = 2; 
                    displaySpeciesText();  
                    break;  
                case JOURNAL_ID:  
                    menu.SetActive(true); 
                    entries.SetActive(false); 
                    menuBody.gameObject.SetActive(false); 
                    menuTitle.gameObject.SetActive(true);
                    speciesExBody.gameObject.SetActive(true); 
                    screenID = 3; 
                    displayJournalsText();  
                    break;   
            }
        } 
        // Space
        
        if (Input.GetKeyDown(KeyCode.Space) && canInteract) {
            if (screenID == MAIN_MENU_ID){
                if (menuCount == 0){ // Go to solar ex 
                    Debug.Log("Going to solar ex."); 
                    menuBody.gameObject.SetActive(false); 
                    speciesExBody.gameObject.SetActive(true); 
                    displaySolarSystemText(); 
                    screenID = SOLAR_EX_ID; 
                }
                else if (menuCount == 1){ // Go to species ex 
                    menuBody.gameObject.SetActive(false); 
                    speciesExBody.gameObject.SetActive(true); 
                    displaySpeciesText(); 
                    screenID = SPECIES_EX_ID; 
                }
                else if (menuCount == 2){ // Go to journal ex 
                    menuBody.gameObject.SetActive(false); 
                    speciesExBody.gameObject.SetActive(true); 
                    displayJournalsText();  
                    screenID = JOURNAL_EX_ID; 
                }
                else if (menuCount == 3){ // exit 
                    GameObject.Find("GameManager").GetComponent<GameManager>().goShip(); 
                    canInteract = false; 
                }
            }
            else if (screenID == 1){ // Planet ex
                screenID = SOLAR_ID; 
                menu.SetActive(false); 
                entries.gameObject.SetActive(true);  
                currentCount = planetCount;  
                speciesTitle.text = validSystemTexts[currentCount][0]; 
                speciesBody.text = validSystemTexts[currentCount][1]; 
                displaySpeciesImage("Blank"); 
            }
            else if (screenID == 2){ // Species ex
                screenID = SPECIES_ID; 
                menu.SetActive(false); 
                entries.gameObject.SetActive(true);  
                currentCount = speciesCount; 
                speciesTitle.text = validSpeciesTexts[currentCount][0]; 
                speciesBody.text = validSpeciesTexts[currentCount][1]; 
                Debug.Log(validSpeciesTexts[currentCount][0]); 
                displaySpeciesImage(validSpeciesTexts[currentCount][0]); 
            }
            else if (screenID == 3){ // Journal ex
                screenID = JOURNAL_ID; 
                menu.SetActive(false); 
                entries.gameObject.SetActive(true);  
                currentCount = journalCount; 
                speciesTitle.text = validJournalTextList[currentCount][0]; 
                speciesBody.text = validJournalTextList[currentCount][1]; 
                displaySpeciesImage("Blank"); 
            }
        }
        // Up/Down 
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)) {
            if (screenID == MAIN_MENU_ID){
                menuUp();
            }
            else if (screenID == SOLAR_EX_ID){
                planetUp();
            }
            else if (screenID == SPECIES_EX_ID){
                speciesUp();
            }
            else if (screenID == JOURNAL_EX_ID){
                journalUp();
            }
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S)) {
            if (screenID == MAIN_MENU_ID){
                menuDown();
            }
            else if (screenID == SOLAR_EX_ID){
                planetDown();
            }
            else if (screenID == SPECIES_EX_ID){
                speciesDown();
            }
            else if (screenID == JOURNAL_EX_ID){
                journalDown();
            } 
        }
    }  
    private void menuDown(){
        if (menuCount > -1 && menuCount < 3){
            menuCount++; 
            displayMainMenuText(); 
        }
    }
    private void menuUp(){
        if (menuCount > 0 && menuCount < 4){
            menuCount--;  
            displayMainMenuText(); 
        }
    }
    private void speciesDown(){
        if (speciesCount > -1 && speciesCount < validSpeciesTexts.Count-1){
            speciesCount++; 
            displaySpeciesText(); 
        }
    }
    private void speciesUp(){
        if (speciesCount > 0 && speciesCount < validSpeciesTexts.Count){
            speciesCount--;  
            displaySpeciesText(); 
        }
    }
    private void journalDown(){
        if (journalCount > -1 && journalCount < validJournalTextList.Count-1){
            journalCount++; 
            displayJournalsText(); 
        }
    }
    private void journalUp(){
        if (journalCount > 0 && journalCount < validJournalTextList.Count){
            journalCount--;  
            displayJournalsText(); 
        }
    }
    private void planetDown(){
        if (planetCount > -1 && planetCount < validSystemTexts.Count-1){
            planetCount++; 
            displaySolarSystemText(); 
        }
    }
    private void planetUp(){
        if (planetCount > 0 && planetCount < validSystemTexts.Count){
            planetCount--;  
            displaySolarSystemText(); 
        }
    }
    public void displayMainMenuText(){
        string s = ""; 
        switch (menuCount){
            case 0: 
                s = "> Solar Systems\nNotable Species\nJournal\nExit"; 
                break; 
            case 1: 
                s = "Solar Systems\n> Notable Species\nJournal\nExit"; 
                break; 
            case 2: 
                s = "Solar Systems\nNotable Species\n> Journal\nExit"; 
                break; 
            case 3: 
                s = "Solar Systems\nNotable Species\nJournal\n> Exit"; 
                break; 
            default:  
                break; 
        }
        menuTitle.text = "Astronomicon"; 
        menuBody.text = s;
    }

    public void displaySpeciesText(){
        string s = ""; 
        for (int i = 0; i < validSpeciesTexts.Count; i++){ 
            if (speciesCount == i)
            {
                s += "> ";
            }
            s += validSpeciesTexts[i][0] + "\n"; 
        }
        menuTitle.text = "Notable Species"; 
        speciesExBody.text = s;
    }

    public void displayJournalsText(){ 
        string s = ""; 
        for (int i = 0; i < validJournalTextList.Count; i++){
            if (journalCount == i){
                s += "> "; 
            }
            s += validJournalTextList[i][0] + "\n"; 
        }
        menuTitle.text = "?????'s Journal"; 
        speciesExBody.text = s;
    }

    public void displaySolarSystemText()
    {
        string s = "";
        for (int i = 0; i < validSystemTexts.Count; i++)
        {
            if (planetCount == i)
            {
                s += "> ";
            }
            s += validSystemTexts[i][0] + "\n";
        }
        menuTitle.text = "Solar Systems";
        speciesExBody.text = s;
    }

    public void displaySpeciesImage(string name){ 
        Color tempColor = astroImage.color;
        switch (name){  
            case "Blank": 
                tempColor.a = 0f;
                astroImage.color = tempColor; 
                break; 
            case "Human": 
                tempColor.a = 1f;
                astroImage.color = tempColor; 
                astroImage.sprite = spr_human; 
                break; 
            case "Zwai'lek": 
                tempColor.a = 1f;
                astroImage.color = tempColor; 
                astroImage.sprite = spr_zwailek; 
                break; 
            case "Mamari": 
                tempColor.a = 1f;
                astroImage.color = tempColor; 
                astroImage.sprite = spr_mamari; 
                break; 
            case "Wewari": 
                tempColor.a = 1f;
                astroImage.color = tempColor; 
                astroImage.sprite = spr_wewari; 
                break; 
            case "Shadowmen": 
                tempColor.a = 1f;
                astroImage.color = tempColor; 
                astroImage.sprite = spr_shadowmen; 
                break; 
        }
    }
}
