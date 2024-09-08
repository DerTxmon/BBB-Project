using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.U2D.Animation;
using UnityEditor;

public class Mate_Manager : MonoBehaviour
{
    float mapsizeX = 266f; 
    float mapsizeY = 266f;
    public GameObject Mate;
    private string[] Mate_Names = {"Sergej", "Hannes", "Wolfgang", "Joan", "Steven", "Peter", "Jakob", "Connor", "Dean", "Noah", "Oliver", "James", "Henry", "William", "Lucas", "Daniel", "Alexander", "Chris", "Timothy", "Vlad", "David", "Isaac", "Grayson", "Aaron", "Ryon", "Jason", "Joseppe", "Connor", "Austin", "Jordan", "ThunderStrike", "ShadowFury", "PixelKnight", "GhostReaper", "DragonSlayer", "CyberNinja", "MysticWarrior", "BlazeHunter", "TitanCrusher", "StormBringer", "VortexRider", "ApexPredator", "DarkPhoenix", "NeonSamurai", "InfernoBlaze", "ArcticAssassin", "RogueSniper", "VenomousViper", "SteelWarden", "ThunderBolt", "SilentShadow", "CrimsonFury", "BlitzRanger", "NightStalker", "RazorEdge", "AlphaGuardian", "SavageBeast", "OmegaBlade", "PhantomWarrior", "IronClaw", "FrostTitan", "ChaosReign", "StormBreaker", "ElectricSurge", "EternalFlame", "WildStrike", "DarkHawk", "BlazePhantom", "ShadowWraith", "VenomStrike", "ThunderClash", "MysticRogue", "EmberFlare", "SteelStrike", "ViperFang", "ApexHunter", "PhantomSniper", "CrimsonShadow", "SilentStrike", "ArcticFury"};
    public SpriteLibraryAsset[] Skins;
    public GameObject Player;
    private void Start() {
        if(Menu_Handler.DuoMode) StartCoroutine(SpawnMate());
    }

    public IEnumerator SpawnMate(){
        yield return new WaitForEndOfFrame();
        Mate_Spawn();
    }

    public void Mate_Spawn(){
        //Spawn Mate very near to the Player and give him a random name and Skin
        GameObject NewMate = Instantiate(Mate, new Vector3(Player.transform.position.x + Random.Range(-5, 5), Player.transform.position.y + Random.Range(-5, 5), 100f /*100 = Mate default height*/), Quaternion.identity); 
        NewMate.GetComponent<SpriteLibrary>().spriteLibraryAsset = Skins[Random.Range(0, Skins.Length)]; //Random Skin
        int Randint = Random.Range(0,80); //Random Int zwischen 0 und 20.
        string nextname = Mate_Names[Randint]; //Die zufällige zahl wird nun als index in der Array benutzt.
        NewMate.GetComponentInChildren<TextMeshPro>().text = nextname;
    }
    
}
