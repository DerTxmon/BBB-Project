using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Net.NetworkInformation;
using TMPro;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine.Timeline;
using UnityEngine.Audio;
public class Menu_Handler : MonoBehaviour
{
    public GameObject Player, Shop_Button, Play_Button, Friends_Button, Inv_Button, Coin_Counter, Emerald_Counter, Player_Name_Button, Name_Input, BG1, BG2, BG3, BG4, ShopBG, Logo, Settings_Button, Settings_BG, Settings_Menu, Settings_Menu_X, AddBotButton, RemoveBotButton, Bot_Display;
    public GameObject LobbyMusic, Settings_X;
    public TextMeshProUGUI Coin_Count, Emerald_Count, Player_Name_Text;// Bot_Counter;
    public GameObject[] BGPoints;
    public int Coins; //Muss noch Automatisch gezählt werden wird aber vorerst auf 123 Gesetzt
    public static string Player_Name = "Player";
    public static int Menu_Bots = 19; //19 Bots werden an Map Handler gesendet damit auf der Map später 20 Man stehen
    public float CAMSPEED = 0.001f;
    public float Playerspinspeed;
    public Image Black;
    public static UserData localdata = new UserData(); //Sofort das Localdata object erstellen damit sofort zum launch des spiels daten gelesen/geschrieben werden können
    public static UserData loadeddata;
    private GameObject activeBG;
    public GameObject RewardWindowPrefab;
    public RectTransform RewardWindowPos;
    public GameObject MusicSlider;
    private string[] sonderzeichen = {"\""/*escaped ->"<-*/,"!","§", "'","$","%","&","/","(",")","=","?","}","]","[","{","³","²","^","°","<",">","|","+","*","~",",",":",";","-","_","\u00b4" /*´*/,"`", "'\u0022'"/*backslash*/};
    //Skins
    [SerializeField] private Sprite DefaultSkinSprite, BetaSkinSprite, AgentSkinSprite, OttoSkinSprite, ClownSkinSprite, AlienSkinSprite, ChrisSkinSprite;
    public GameObject Menu_Skin_Display;
    public Image DefaultSkinButton, BetaSkinButton, AgentSkinButton, ClownSkinButton, OttoSkinButton, ChrisSkinButton, AlienSkinButton;
    //
    //Alle Skin buttons
    //
    public Sprite DefaultSkinButtonGreen, DefaultSkinButtonRed, DefaultSkinButtonGray, BetaSkinButtonGreen, BetaSkinButtonGray, BetaSkinButtonRed,
    AgentSkinButtonGreen, AgentSkinButtonRed, AgentSkinButtonGray, ClownSkinButtonGreen, ClownSkinButtonRed, ClownSkinButtonGray,
    OttoSkinButtonGreen, OttoSkinButtonRed, OttoSkinButtonGray, ChrisSkinButtonGreen, ChrisSkinButtonRed, ChrisSkinButtonGray,
    AlienSkinButtonGreen, AlienSkinButtonRed, AlienSkinButtonGray;
    
    //Alle skins nochmal in high res fürs Display
    public Sprite DefaultDispalayModel, BetaDisplayModel, AgentDisplayModel, ClownDisplayModel, OttoDisplayModel, AlienDisplayModel, ChrisDisplayModel;
    //Selbe in einer Array
    public Sprite[] DisplayModels;

    //Inventory
    public GameObject InvBG;
    public GameObject Inv_Hud;
    public GameObject[] SkinFields;
    public GameObject X_Inv;

    //Level
    [SerializeField] private TextMeshProUGUI Leveltext;
    [SerializeField] private Image ProgressBar;
    [SerializeField] private Image ProgressBarBG;
    [SerializeField] private TextMeshProUGUI neededXP;

    //Friends, Online
    [SerializeField] private GameObject onlineBG;
    [SerializeField] private GameObject FriendWindow;
    [SerializeField] private GameObject LoadingText;
    [SerializeField] private GameObject IDText;
    [SerializeField] private GameObject FriendsScroll;
    [SerializeField] private GameObject X_Friends;
    [SerializeField] private GameObject[] FriendSlots = new GameObject[6];
    [SerializeField] private GameObject MESlot;
    [SerializeField] private GameObject FriendSlot;
    [SerializeField] private GameObject AddFriendSlot;
    [SerializeField] private GameObject AddFriendInput;
    [SerializeField] private List<GameObject> LoadedFriends;
    [SerializeField] private GameObject FriendListButton;
    [SerializeField] private GameObject GlobalLeaderboardButton;
    [SerializeField] private Sprite FriendListButton_Green;
    [SerializeField] private Sprite FriendListButton_Gray;
    [SerializeField] private Sprite GlobalLeaderboardButton_Green;
    [SerializeField] private Sprite GlobalLeaderboardButton_Gray;
    [SerializeField] private Sprite RankLeaderboardButton_Green;
    [SerializeField] private Sprite RankLeaderboardButton_Gray;
    [SerializeField] private Sprite InvButtonSprite;
    [SerializeField] private Sprite InvButtonSpriteNoConnection;
    [SerializeField] private GameObject GlobalLeaderBoardParent;
    [SerializeField] private GameObject GlobalLeaderboardScroll;
    [SerializeField] private Sprite Place1Sprite;
    [SerializeField] private Sprite Place2Sprite;
    [SerializeField] private Sprite Place3Sprite;
    [SerializeField] private Sprite MySelfSprite;
    [SerializeField] private Sprite FriendsButtonSprite;
    [SerializeField] private Sprite FriendsButtonSpriteNoConnection;
    //Shop
    [SerializeField] private GameObject ShopSlotPrefab; //Prefab
    [SerializeField] private Transform[] Shop_Slot;
    [SerializeField] private Sprite Normal_ShopSlot;
    [SerializeField] private Sprite Rare_ShopSlot;
    [SerializeField] private Sprite Superior_ShopSlot;
    [SerializeField] private Sprite Exquisit_ShopSlot;
    [SerializeField] private Sprite Extravagant_ShopSlot;
    [SerializeField] private Sprite CoinIcon;
    [SerializeField] private Sprite EmeraldIcon;
    [SerializeField] private GameObject ShopScroll;
    [SerializeField] private GameObject X_Shop;
    [SerializeField] private GameObject Skin_i;
    [SerializeField] private GameObject SkinInfoTafel; 
    [SerializeField] private Sprite BuyButton;
    [SerializeField] private Sprite SoldOutButton;
    [SerializeField] private bool infoswitch = false;
    [SerializeField] private Sprite ShopButtonSprite;
    [SerializeField] private Sprite ShopButtonSpriteNoConnection;
    [SerializeField] private GameObject NoConnectionIcon;
    //Ads
    [SerializeField] private GameObject AdButton;
    private static int AdCounter = 0;
    public static int Watchedads = 0;
    private static int loadadcounter = 0;
    public static bool EmeraldAd = false;
    [SerializeField] private Sprite EmeraldAdButtonSprite;
    [SerializeField] private Sprite CoinAdButtonSprite;
    private bool OfflineMode = false;
    //else
    private static Menu_Handler instance;
    private GameObject RewardWindowObject;
    public static int x;
    //Loading Screen
    [SerializeField] private GameObject BBRLogo_LoadingScreen;
    [SerializeField] public GameObject PlayerModel_LoadingScreen;
    public static Sprite PlayerModel_LoadingScreen_Image;
    [SerializeField] private Image VisionShootSwitchButton;
    [SerializeField] private Sprite VisionShootSwitchOff, VisionShootSwitchOn;
    public static int FreshXP;
    [SerializeField] private GameObject GrayBG;
    [SerializeField] private GameObject LevelText_Anim;
    [SerializeField] private GameObject XPNumber_Anim;
    [SerializeField] private GameObject XpBG_Anim;
    [SerializeField] private GameObject XPBar_Anim;
    [SerializeField] private GameObject XpOKButton;
    [SerializeField] private GameObject XPProgressBG;
    public static float XPProgressbeforesaveprcnt, AktuellXPBeforeSave, XPtoLevelUpBeforeSave;
    public static int Levelbeforesave;
    private float tempcounterval;
    [SerializeField] private TextMeshProUGUI DaylyRewardTimerText;
    public string RemainingTime, TimeNOW;
    public GameObject DaylyRewardButton;
    [SerializeField] private Image DaylyRewardButtonAusrufezeichen;
    public string _gettimeNOW;
    public string _getdateNOW;
    public TimeSpan TimetoWait;
    bool dontstarttimertwice = false;
    public Animator PlayButtonAnim;
    public Sprite DaylyrewardButtonGray;
    public Sprite DaylyrewardButton;
    public bool InMenu = true;
    public static bool DuoMode = false;
    public Button SoloButton, DuoButton;
    public AudioClip Menu_Click_Sound;
    public Toggle TurnOffAllBotsSwitch, UnlockFPSSwitch;
    public Slider SFXSlider;

    void Awake(){
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        //-High Performance settings-
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 120;
        //----------------------------
        instance = this;
        //50 50 ob Ad Button erscheint
        int randnum = UnityEngine.Random.Range(0, 2);
        if(randnum == 1) AdCounter = 0;
        else AdCounter = 1;
        AdCounter++;
        ChooseBG();
        DecideAdButton(); //Ad Button Erscheint nur jedes 2te mal
        StartCoroutine(PlayerSpin());
        StartCoroutine(BGMove());
        StartCoroutine(ButtonAnimation());
        StartCoroutine(TryToHoldConnection());
        StartCoroutine(AdAnimation());
        //Init Ads
        if(loadadcounter >= 1) AdButton.GetComponent<RewardedAdsButton>().LoadAd();  //Aber nur nach dem ersten start   
        loadadcounter++;
    }
    void Start(){
        if(FreshXP != 0){ //Check ob XP Counter Animation gespielt werden muss
            StartCoroutine(PlayXPAnimation());
        }
        try{
            Readdata(); //Versuche Spieler daten zu lesen und wenn keine da sind

            //Set Saved Player name
            Player_Name_Text.text = loadeddata.Saved_Player_Name;
            //Set Saved Coins to display
            Coin_Count.text = loadeddata.Saved_Coins.ToString();
            Emerald_Count.text = loadeddata.Saved_Emeralds.ToString();
            //Setz die anzeige sofort auf 1
            //Bot_Counter.text = Menu_Bots.ToString();
            InitSkinButtons();
            InitLevel();
            ApplySettings();
            InitSettingsWindow();
        }catch{
            StartCoroutine(InitUserData()); //Erstelle neue Spielerdaten und lese sie sofort
        }
        if(!localdata.FirstLaunch && localdata.DaylyRewardClaimed == true){ //Timer Läuft | Reward Claimed
            InitTimer();
            StartCoroutine(InitUserData3());
        }else{ //Kein Timer | Reward READY
            localdata.FirstLaunch = false;
            StartCoroutine(InitUserData3());
            DaylyRewardButton.SetActive(true); //Reward on first Launch
            DaylyRewardTimerText.gameObject.SetActive(false);
            DaylyRewardButton.GetComponent<Animator>().SetBool("Jiggle", true);
        }
    }

    public static void Writedata(UserData Data){
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(Application.persistentDataPath + "/save.dat", FileMode.Create);
        bf.Serialize(file, Data);
        file.Close();
    }

    public static void Readdata(){
        if(File.Exists(Application.persistentDataPath + "/save.dat")){
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/save.dat", FileMode.Open);
            loadeddata = (UserData)bf.Deserialize(file);
            file.Close();
        }else throw new NullReferenceException();
    }

    public IEnumerator InitUserData(){
        Writedata(new UserData()); //Erstelle frischen Spielstand
        //Record joined version
        yield return new WaitForEndOfFrame();
        Readdata();
        yield return new WaitForEndOfFrame();
        localdata = loadeddata;
        //Record first launch Version
        if(Application.version.Contains("0.9")){
            //Give Beta Skin
            localdata.Beta_Ownage = true;
        }
        Writedata(localdata);
        yield return new WaitForEndOfFrame();
        Readdata();
        yield return new WaitForEndOfFrame();
        //Register on Database
        StartCoroutine(OnlineManager.RegisterNewUserOnDatabase());
        while(OnlineManager.NewID == 0){ 
            yield return new WaitForEndOfFrame(); 
            //Check if Connection is still there
            //!!
        }
        localdata.PlayerID = OnlineManager.NewID;
        localdata.Saved_Player_Name = "Player#" + OnlineManager.NewID.ToString();
        Writedata(localdata);
        yield return new WaitForEndOfFrame();
        Readdata();
        localdata = loadeddata;
        //Set Saved Player name
        Player_Name_Text.text = loadeddata.Saved_Player_Name;
        //Set Saved Coins to display
        Coin_Count.text = loadeddata.Saved_Coins.ToString();
        Emerald_Count.text = loadeddata.Saved_Emeralds.ToString();
        //Setz die anzeige sofort auf 1
        //Bot_Counter.text = Menu_Bots.ToString();
        InitSkinButtons();
        InitLevel();
        ApplySettings();
        InitSettingsWindow();
        yield return null;
    }

    void ApplySettings(){ //Wende einmal alle gespeicherten Einstellungen an
        //Menu Music
        LobbyMusic.GetComponent<AudioSource>().volume = loadeddata.Menu_Music_Volume;
        //Music Slider
        MusicSlider.GetComponent<Slider>().value = loadeddata.Menu_Music_Volume;
        //SFX Slider
        SFXSlider.value = loadeddata.SFX_Volume;
        //Vision Switch
        if(loadeddata.VisionShootSwitch){
            VisionShootSwitchButton.sprite = VisionShootSwitchOn;
        }else{
            VisionShootSwitchButton.sprite = VisionShootSwitchOff;
        }
    }

    //User Saved Data Object
    [Serializable]
    public class UserData{
        //Save Data
        public string Saved_Player_Name = "Player#0000";
        public int Saved_Coins = 50;
        public int Saved_Emeralds = 5;
        public int Wins = 0;
        public int Kills = 0;
        //Skins
        public string SelectedSkin = "Default";
        public bool Default_Ownage = true;
        public bool Agent_Ownage = false;
        public bool Clown_Ownage = false;
        public bool Beta_Ownage = false;
        public bool Otto_Ownage = false;
        public bool Alien_Ownage = false;
        public bool Chris_Ownage = false;
        public int aktuelleXP = 0;
        public int TonextLevelXP = 50;
        public int Level = 1;
        public int PlayerID = 0; //Web Server teilt einmalig zu und wird ab dann nur noch gelesen.
        public string joinedversion;
        public int[] FriendsIDs = new int[6]; //Wird bei aufrufen der Freunde einmal durchgelooped
        public float Menu_Music_Volume = .5f;
        public bool VisionShootSwitch = false;
        public bool FirstLaunch = true;
        public string LastTimeUntillDaylyReward;
        public string LastrewardDate;
        public string LastrewardTime;
        public bool DaylyRewardClaimed = false;
        public bool TurnOffBots = false;
        public bool performancemode = true;
        public float SFX_Volume = 1;
    }
    public string[] GetTime(){
        string time = null;
        time = NTPClient.GetNetworkTime().ToString();
        while(time == null){} //Wait...
        //remove the date from the string
        string[] split = time.Split(' ');
        _gettimeNOW = split[1];
        _getdateNOW = split[0];
        return split;
    }
    public class NTPClient{ //Chatgpt
    private const string NtpServer = "pool.ntp.org";

    public static DateTime GetNetworkTime()
    {
        // NTP message size - 16 bytes of the digest (RFC 2030)
        var ntpData = new byte[48];

        //Setting the Leap Indicator, Version Number and Mode values
        ntpData[0] = 0x1B;

        var addresses = Dns.GetHostEntry(NtpServer).AddressList;

        // The UDP port number assigned to NTP is 123
        var ipEndPoint = new IPEndPoint(addresses[0], 123);
        var socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

        socket.Connect(ipEndPoint);

        //Send Request
        socket.Send(ntpData);
        socket.Receive(ntpData);
        socket.Close();

        //Offset to get to the "Transmit Timestamp" field (time at which the reply
        //departed the server for the client, in 64-bit timestamp format."
        const byte serverReplyTime = 40;

        //Get the seconds part
        ulong intPart = BitConverter.ToUInt32(ntpData, serverReplyTime);

        //Get the seconds fraction
        ulong fractPart = BitConverter.ToUInt32(ntpData, serverReplyTime + 4);

        //Convert From big-endian to little-endian
        intPart = SwapEndianness(intPart);
        fractPart = SwapEndianness(fractPart);

        var milliseconds = (intPart * 1000) + ((fractPart * 1000) / 0x100000000L);

        //**UTC** time
        var networkDateTime = (new DateTime(1900, 1, 1, 0, 0, 0, DateTimeKind.Utc)).AddMilliseconds((long)milliseconds);

        return networkDateTime.ToLocalTime();
    }

    private static uint SwapEndianness(ulong x)
    {
        return (uint)(((x & 0x000000ff) << 24) +
                      ((x & 0x0000ff00) << 8) +
                      ((x & 0x00ff0000) >> 8) +
                      ((x & 0xff000000) >> 24));
    }
    }

    private void InitTimer(){
        //Compare Time now with LastTimeUntillDaylyReward and then set the Timer to the remaining time
        string[] datearr = localdata.LastrewardDate.Split('.'); //Datum seit claim
        string[] timearr = localdata.LastrewardTime.Split(':'); //Zeit seit claim
        int year = Int32.Parse(datearr[2]);
        int month = Int32.Parse(datearr[1]);
        int day = Int32.Parse(datearr[0]);
        int hour = Int32.Parse(timearr[0]);
        int minute = Int32.Parse(timearr[1]);
        int second = Int32.Parse(timearr[2]);
        DateTime dateTime1 = new DateTime(year, month, day, hour, minute, second, DateTimeKind.Utc); //Altes Datum
        datearr = GetTime()[0].Split('.'); //Datum jetzt
        timearr = GetTime()[1].Split(':'); //Zeit jetzt
        year = Int32.Parse(datearr[2]);
        month = Int32.Parse(datearr[1]);
        day = Int32.Parse(datearr[0]);
        hour = Int32.Parse(timearr[0]);
        minute = Int32.Parse(timearr[1]);
        second = Int32.Parse(timearr[2]);
        DateTime dateTime2 = new DateTime(year, month, day, hour, minute, second, DateTimeKind.Utc); //Neues Datum

        //Debug.Log(dateTime2 + " - " + dateTime1);
        TimeSpan difference = dateTime2 - dateTime1;
        TimeSpan oneday = TimeSpan.FromDays(1); //1 Tag

        if(difference >= oneday){
            DaylyRewardButton.SetActive(true);
            DaylyRewardTimerText.gameObject.SetActive(false);
            ActivateDaylyReward();
        }else{
            difference = oneday - difference;
            RemainingTime = difference.ToString(@"hh\:mm\:ss");
            //Debug.Log("Zu Warten ist noch " + RemainingTime);
            //Show timer and set it to the remaining time
            //Debug.Log(difference + " Wird nun auf der Uhr angezeigt");
            if(!dontstarttimertwice){
                StartTimer();
                //Stop its Animation
                DaylyRewardButton.GetComponent<Image>().sprite = DaylyrewardButtonGray;
                DaylyRewardButton.GetComponent<Button>().enabled = false;
                DaylyRewardButton.GetComponent<Animator>().SetBool("Jiggle", false);
                DaylyRewardTimerText.gameObject.SetActive(true);
            }
        }
    }

    private void InitTimerAfterClaim(){
        TimeSpan difference = new TimeSpan(23, 59, 59); //1 Tag
        
        TimetoWait = difference;

        //Debug.Log("Zu Warten ist noch " + TimetoWait.ToString(@"hh\:mm\:ss"));

        RemainingTime = difference.ToString(@"hh\:mm\:ss");

        //Show timer and set it to the remaining time
        //Debug.Log(RemainingTime + " Wird nun auf der Uhr angezeigt");
        if(!dontstarttimertwice){
            StartTimer();
        }
    }
    
    public void StartTimer(){
        dontstarttimertwice = true;
        StartCoroutine(Timer());
    }
    private IEnumerator Timer(){
        while(true){
            yield return new WaitForSecondsRealtime(1f);
            UpdateDaylyRewardTimer();
        }
    }
    private void UpdateDaylyRewardTimer(){
        //Split Start Time String
        string[] StartTime = RemainingTime.Split(':');
        int StartHour = int.Parse(StartTime[0]);
        int StartMinute = int.Parse(StartTime[1]);
        int StartSecond = int.Parse(StartTime[2]);
        string StartHourString;
        string StartMinuteString;
        string StartSecondString;
        //Count one Second down and stop on 00:00:00
        StartSecond--;
        if(StartSecond < 0){
            if(StartMinute == 0 && StartHour == 0) ActivateDaylyReward(); //Ende
            StartSecond = 59;
            StartMinute--;
            if(StartMinute < 0){
                StartMinute = 59;
                StartHour--;
            }
        }
        //Convert right and then to sring
        if(StartMinute < 10) StartMinuteString = "0" + StartMinute.ToString();
        else StartMinuteString = StartMinute.ToString();
        if(StartSecond < 10) StartSecondString = "0" + StartSecond.ToString();
        else StartSecondString = StartSecond.ToString();
        if(StartHour < 10) StartHourString = "0" + StartHour.ToString();
        else StartHourString = StartHour.ToString();

        //Update Current Time to format 00:00:00
        RemainingTime = StartHourString + ":" + StartMinuteString + ":" + StartSecondString;
        //Update Timer Text
        DaylyRewardTimerText.text = RemainingTime;
        //Update LastTimeUntillDaylyReward every 10 seconds
        if(StartSecond % 10 == 0){
            localdata.LastTimeUntillDaylyReward = RemainingTime;
            StartCoroutine(InitUserData3()); //Save Data
        }
    }
    private void ActivateDaylyReward(){
        localdata.DaylyRewardClaimed = false;
        DaylyRewardTimerText.gameObject.SetActive(false);
        //Activate Button
        DaylyRewardButton.GetComponent<Image>().sprite = DaylyrewardButton;
        DaylyRewardButton.GetComponent<Animator>().SetBool("Jiggle", true);
        StartCoroutine(InitUserData3()); //Save Data
        Debug.Log("Daylyreward kann abgeholt werden");
    }

    public void OnDaylyRewardButtonPressed(){
        //Give Coins/Emerald 
        int rand = UnityEngine.Random.Range(0, 7);
        if(rand == 0) StartCoroutine(Menu_Handler.AddEmerald(1)); //(1/7)% chance
        else StartCoroutine(Menu_Handler.AddCoins(15)); //10% chance
        //Reset Timer
        DaylyRewardTimerText.gameObject.SetActive(true);
        //Make Button Gray
        DaylyRewardButton.GetComponent<Image>().sprite = DaylyrewardButtonGray;
        //DaylyRewardButton.gameObject.SetActive(false);
        localdata.LastrewardTime = null;
        localdata.LastrewardDate = null;
        while(localdata.LastrewardTime == null || localdata.LastrewardDate == null){
            localdata.LastrewardDate = GetTime()[0];
            localdata.LastrewardTime = GetTime()[1];
        }
        localdata.DaylyRewardClaimed = true;
        StartCoroutine(InitUserData3()); //Save Data

        InitTimerAfterClaim();
    }

    public void InitLevel(){
        //jedes level + 30xp
        //Überfällige XP berechnen
        //auf dem balken richtig anzeigen

        float displayprogressinprecent = (float)loadeddata.aktuelleXP / (float)loadeddata.TonextLevelXP;
        ProgressBar.fillAmount = displayprogressinprecent;

        Leveltext.text = "LEVEL " + loadeddata.Level.ToString();
        neededXP.text = loadeddata.aktuelleXP.ToString() + "XP" + "/" + loadeddata.TonextLevelXP.ToString() + "XP";
        //Debug.Log("Aktuelle XP: " + loadeddata.aktuelleXP + "XP" + " / " + loadeddata.TonextLevelXP + "XP");
    }
    public static void AddXP(int GiveXP){
        float xpuntillvlup = loadeddata.TonextLevelXP - loadeddata.aktuelleXP;
        //schau ob mehr als 1 level up geht Level hoch geht
        for( ; GiveXP != 0; GiveXP--){
            localdata.aktuelleXP++; //1XP pro durchlauf hinzufügen
            if(localdata.aktuelleXP == localdata.TonextLevelXP){
                localdata.Level++; //Level up
                localdata.TonextLevelXP += (int)(localdata.TonextLevelXP / 4) + 20; //Immer 30xp mehr pro höheres level
                localdata.aktuelleXP = 0; //Aktuelle XP auf 0 setzen
                if(localdata.Level % 10 == 0){ //Jedes 10 level eine coin
                    //AddEmerald(1);
                    //Darf erst ausgeführt werden wenn Title Menu Scene Geladen ist
                }
            }
        }
        Writedata(localdata); //Save
    }
    public static IEnumerator AddEmerald(int ammount){
        localdata = loadeddata;
        localdata.Saved_Emeralds += ammount;
        //Display
        GameObject.Find("Emerald Counter").transform.Find("Counter").GetComponent<TextMeshProUGUI>().text = localdata.Saved_Emeralds.ToString(); //Muss so gesucht werden methode statisch ist
        Writedata(localdata);
        yield return new WaitForEndOfFrame();
        Readdata();
        localdata = loadeddata;
        instance.StartCoroutine(RewardWindow("Emerald", ammount));
    }
    public static IEnumerator AddCoins(int ammount){
        localdata = loadeddata;
        localdata.Saved_Coins += ammount;
        //Display
        GameObject.Find("Coin Counter").transform.Find("Counter").GetComponent<TextMeshProUGUI>().text = localdata.Saved_Coins.ToString(); //Muss so gesucht werden methode statisch ist
        Writedata(localdata);
        yield return new WaitForEndOfFrame();
        Readdata();
        localdata = loadeddata;
        instance.StartCoroutine(RewardWindow("Coin", ammount));
    }
    public static IEnumerator RewardWindow(string type, int amount){ 
        // Hide UI
        instance.InMenu = false;
        instance.Player.SetActive(false);
        instance.Shop_Button.SetActive(false);
        instance.Play_Button.SetActive(false);
        instance.Coin_Counter.SetActive(false);
        instance.Emerald_Counter.SetActive(false);
        instance.Player_Name_Button.SetActive(false);
        instance.Player_Name_Text.gameObject.SetActive(false);
        instance.Friends_Button.gameObject.SetActive(false);
        instance.Inv_Button.gameObject.SetActive(false);
        instance.Settings_Button.gameObject.SetActive(false);
        instance.Logo.gameObject.SetActive(false);
        instance.Name_Input.SetActive(false);
        instance.Leveltext.gameObject.SetActive(false);
        instance.ProgressBar.gameObject.SetActive(false);
        instance.ProgressBarBG.gameObject.SetActive(false);
        instance.neededXP.gameObject.SetActive(false);
        instance.GlobalLeaderboardScroll.SetActive(false);
        instance.AdButton.SetActive(false);
        instance.DaylyRewardButton.SetActive(false);
        instance.DaylyRewardTimerText.gameObject.SetActive(false);

        // Show Reward Window
        instance.RewardWindowObject = Instantiate(instance.RewardWindowPrefab, instance.RewardWindowPos);
        
        if (type == "Coin"){ // Coin
            instance.RewardWindowObject.transform.parent = instance.RewardWindowPos;
            instance.RewardWindowObject.GetComponent<RectTransform>().localPosition = new Vector3(0, 0, 0); // Center
            // Set Icon
            instance.RewardWindowObject.transform.Find("Coin").gameObject.GetComponent<Image>().sprite = instance.CoinIcon;
            // Count up the coins
            yield return new WaitForSecondsRealtime(0.3f);
            
            float delay = 0.03f; // Initiale Verzögerung (schnellerer Start)
            float maxDelay = 0.5f; // Maximale Verzögerung
            float accelerationPoint = amount * 0.7f; // Punkt, ab dem die Verzögerung stärker zunimmt

            for (int i = 0; i <= amount; i++){
                instance.RewardWindowObject.transform.Find("Amount").gameObject.GetComponent<TextMeshProUGUI>().text = i.ToString();

                // Exponentiell langsamer werdendes Zählen mit späterer Entschleunigung
                yield return new WaitForSecondsRealtime(delay);

                if (i < accelerationPoint) {
                    delay = Mathf.Min(delay * 1.05f, maxDelay); // Leichter Anstieg der Verzögerung in den ersten 70% des Zählens
                } else {
                    delay = Mathf.Min(delay * 1.2f, maxDelay); // Stärkerer Anstieg der Verzögerung in den letzten 30%
                }
            }

            // OK Button
            instance.RewardWindowObject.transform.Find("Ok").GetComponent<Button>().onClick.AddListener(() => {
                Destroy(instance.RewardWindowObject);
                // Show UI
                instance.SettingsExit();
                instance.AdButton.gameObject.GetComponent<Image>().enabled = true;
                instance.AdButton.gameObject.GetComponent<Button>().enabled = true; // Hot Fix
            });

            // OK Button Sound on Click
            instance.RewardWindowObject.transform.Find("Ok").GetComponent<Button>().onClick.AddListener(() => {
                instance.GetComponentInChildren<AudioSource>().PlayOneShot(instance.Menu_Click_Sound);
            });
        } else { // Emerald
            instance.RewardWindowObject.transform.parent = instance.RewardWindowPos;
            instance.RewardWindowObject.GetComponent<RectTransform>().localPosition = new Vector3(0, 0, 0); // Center
            // Set Icon
            instance.RewardWindowObject.transform.Find("Coin").gameObject.GetComponent<Image>().sprite = instance.EmeraldIcon;
            // Count up the coins
            yield return new WaitForSecondsRealtime(0.3f);

            float delay = 0.03f; // Initiale Verzögerung (schnellerer Start)
            float maxDelay = 0.5f; // Maximale Verzögerung
            float accelerationPoint = amount * 0.7f; // Punkt, ab dem die Verzögerung stärker zunimmt

            for (int i = 0; i <= amount; i++){
                instance.RewardWindowObject.transform.Find("Amount").gameObject.GetComponent<TextMeshProUGUI>().text = i.ToString();

                // Exponentiell langsamer werdendes Zählen mit späterer Entschleunigung
                yield return new WaitForSecondsRealtime(delay);

                if (i < accelerationPoint) {
                    delay = Mathf.Min(delay * 1.05f, maxDelay); // Leichter Anstieg der Verzögerung in den ersten 70% des Zählens
                } else {
                    delay = Mathf.Min(delay * 1.2f, maxDelay); // Stärkerer Anstieg der Verzögerung in den letzten 30%
                }
            }

            // OK Button
            instance.RewardWindowObject.transform.Find("Ok").GetComponent<Button>().onClick.AddListener(() => {
                Destroy(instance.RewardWindowObject);
                // Show UI
                instance.SettingsExit();
            });
        }
    }


    public IEnumerator PlayXPAnimation(){
        //Grauer Hintergrund anzeigen
        GrayBG.SetActive(true);
        //Alle nötigen Objekte aktivieren
        XPProgressBG.SetActive(true);
        LevelText_Anim.SetActive(true);
        XPNumber_Anim.SetActive(true);
        XPBar_Anim.SetActive(true);
        XpBG_Anim.SetActive(true);
        XpOKButton.SetActive(true);

        //Set Start values
        LevelText_Anim.GetComponent<TextMeshProUGUI>().text = "Level " + Levelbeforesave.ToString();
        XPBar_Anim.GetComponent<Image>().fillAmount = XPProgressbeforesaveprcnt;
        XPNumber_Anim.GetComponent<TextMeshProUGUI>().text = AktuellXPBeforeSave.ToString() + "XP" + "/" + XPtoLevelUpBeforeSave.ToString() + "XP";
        LevelText_Anim.GetComponent<TextMeshProUGUI>().text = "Level " + Levelbeforesave.ToString();
        //Count up the XP
        float tempxp = AktuellXPBeforeSave;
        Image XPBarImage = XPBar_Anim.GetComponent<Image>();
        yield return new WaitForSecondsRealtime(1f); //Kurz warten vor der Animation
        for(int i = FreshXP; i > 0; i--){
            //Add 1 XP to bar
            tempxp++;
            //Refresh Bar progress and text
            XPBarImage.fillAmount = tempxp / XPtoLevelUpBeforeSave;
            XPNumber_Anim.GetComponent<TextMeshProUGUI>().text = tempxp.ToString() + "XP" + "/" + XPtoLevelUpBeforeSave.ToString() + "XP";
            //Check if level up
            if(tempxp == XPtoLevelUpBeforeSave){ //Level UP Logic
                //refresh values to next level
                XPtoLevelUpBeforeSave = XPtoLevelUpBeforeSave + 30;
                AktuellXPBeforeSave = 0;
                XPBar_Anim.GetComponent<Image>().fillAmount = XPProgressbeforesaveprcnt;
                XPNumber_Anim.GetComponent<TextMeshProUGUI>().text = AktuellXPBeforeSave.ToString() + "XP" + "/" + XPtoLevelUpBeforeSave.ToString() + "XP";
                tempxp = 0;
                XPBarImage.fillAmount = 0;
                Levelbeforesave++;
                StartCoroutine(LevelTextAnimation()); //Start Level Text Animation
            }
            yield return new WaitForSecondsRealtime(.008f);
        }

        yield return new WaitForSecondsRealtime(1.5f); //Kurz warten vor Animation

        //Start OK Button Animation
        StartCoroutine(OKButtonAnimation(XpOKButton));

        FreshXP = 0;
        yield return null;
    }
    private IEnumerator LevelTextAnimation(){
        //Level Text Animation
        TextMeshProUGUI LevelText = LevelText_Anim.GetComponent<TextMeshProUGUI>();
        LevelText.text = "Level " + Levelbeforesave.ToString();
        for(int i = 0; i < 10; i++){
            LevelText.fontSize = LevelText.fontSize + 0.7f;
            yield return new WaitForSecondsRealtime(0.05f);
        }
        for(int i = 0; i < 10; i++){
            LevelText.fontSize = LevelText.fontSize - 0.7f;
            yield return new WaitForSecondsRealtime(0.05f);
        }
        yield return new WaitForSecondsRealtime(2f);
    }
    public IEnumerator OKButtonAnimation(GameObject OKButton){
        RectTransform OKButtonTransform = OKButton.GetComponent<RectTransform>();
        while(true){
            for(int i = 0; i < 40; i++){
                OKButtonTransform.localScale = new Vector3(OKButtonTransform.localScale.x + 0.005f, OKButtonTransform.localScale.y + 0.005f, OKButtonTransform.localScale.z);
                yield return new WaitForSeconds(0.1f * Time.deltaTime);
            }
            for(int i = 0; i < 40; i++){
                OKButtonTransform.localScale = new Vector3(OKButtonTransform.localScale.x - 0.005f, OKButtonTransform.localScale.y - 0.005f, OKButtonTransform.localScale.z);
                yield return new WaitForSeconds(0.1f * Time.deltaTime);
            }
            yield return new WaitForSeconds(1.5f * Time.deltaTime);
        }
    }
    
    public void ChooseBG(){
        int BGnum = UnityEngine.Random.Range(0,4);
        if(BGnum == 0){
            BG1.SetActive(true);
            BG2.SetActive(false);
            BG3.SetActive(false);
            BG4.SetActive(false);
            activeBG = BG1;
        }else if(BGnum == 1){
            BG1.SetActive(false);
            BG2.SetActive(true);
            BG3.SetActive(false);
            BG4.SetActive(false);
            activeBG = BG2;
        }else if(BGnum == 2){
            BG1.SetActive(false);
            BG2.SetActive(false);
            BG3.SetActive(true);
            BG4.SetActive(false);
            activeBG = BG3;
        }else if(BGnum == 3){
            BG1.SetActive(false);
            BG2.SetActive(false);
            BG3.SetActive(false);
            BG4.SetActive(true);
            activeBG = BG4;
        }
    }
    public IEnumerator ButtonAnimation(){
        //Play Button Rein Raus zoom
        //Speicher einmal alle Buttons RectTransforms (Performance)
        RectTransform Play_ButtonRect = Play_Button.GetComponent<RectTransform>();
        RectTransform Friends_ButtonRect = Friends_Button.GetComponent<RectTransform>();
        RectTransform Shop_ButtonRect = Shop_Button.GetComponent<RectTransform>();
        RectTransform Inv_ButtonRect = Inv_Button.GetComponent<RectTransform>();
        while(true){
            for(int i = 0; i != 10; i++){ //ZOOM IN
                Play_ButtonRect.localScale = new Vector3(Play_ButtonRect.localScale.x +.01f,Play_ButtonRect.localScale.y + .01f,0f);
                yield return new WaitForSecondsRealtime(.03f);
            }
            for(int i = 10; i != 0; i--){ //ZOOM OUT
                Play_ButtonRect.localScale = new Vector3(Play_ButtonRect.localScale.x - .01f,Play_ButtonRect.localScale.y - .01f,0f);
                yield return new WaitForSecondsRealtime(.03f);
            }
            if(!OfflineMode){
                //Friends Button
                for(int i = 0; i != 10; i++){ //ZOOM IN
                    Friends_ButtonRect.localScale = new Vector3(Friends_ButtonRect.localScale.x +.01f,Friends_ButtonRect.localScale.y + .01f,0f);
                    yield return new WaitForSecondsRealtime(.025f);
                }
                for(int i = 10; i != 0; i--){ //ZOOM OUT
                    Friends_ButtonRect.localScale = new Vector3(Friends_ButtonRect.localScale.x - .01f,Friends_ButtonRect.localScale.y - .01f,0f);
                    yield return new WaitForSecondsRealtime(.025f);
                }
                //Shop Button
                for(int i = 0; i != 10; i++){ //ZOOM IN
                    Shop_ButtonRect.localScale = new Vector3(Shop_ButtonRect.localScale.x +.01f,Shop_ButtonRect.localScale.y + .01f,0f);
                    yield return new WaitForSecondsRealtime(.025f);
                }
                for(int i = 10; i != 0; i--){ //ZOOM OUT
                    Shop_ButtonRect.localScale = new Vector3(Shop_ButtonRect.localScale.x - .01f,Shop_ButtonRect.localScale.y - .01f,0f);
                    yield return new WaitForSecondsRealtime(.025f);
                }
            }
            //Inv Button
            for(int i = 0; i != 10; i++){ //ZOOM IN
                Inv_ButtonRect.localScale = new Vector3(Inv_ButtonRect.localScale.x +.01f,Inv_ButtonRect.localScale.y + .01f,0f);
                yield return new WaitForSecondsRealtime(.025f);
            }
            for(int i = 10; i != 0; i--){ //ZOOM OUT
                Inv_ButtonRect.localScale = new Vector3(Inv_ButtonRect.localScale.x - .01f,Inv_ButtonRect.localScale.y - .01f,0f);
                yield return new WaitForSecondsRealtime(.025f);
            }
            yield return new WaitForSecondsRealtime(3.5f);
        }
    }
    public IEnumerator AdAnimation(){
        RectTransform AdButtonRect = AdButton.GetComponent<RectTransform>();
        while(true){
            for(int i = 0; i != 10; i++){//Zoom den Adbutton rein
            AdButtonRect.localScale = new Vector3(AdButtonRect.localScale.x +.01f,AdButtonRect.localScale.y + .01f,0f);
            yield return new WaitForSeconds(.025f);
            }
            for(int i = 10; i != 0; i--){//Zoom den Adbutton raus
                AdButtonRect.localScale = new Vector3(AdButtonRect.localScale.x - .01f,AdButtonRect.localScale.y - .01f,0f);
                yield return new WaitForSeconds(.025f);
            }
            yield return new WaitForSeconds(1f);
        }
    }
    public void InitSettingsWindow(){
        //TurnOffBots
        if(loadeddata.TurnOffBots){
            TurnOffAllBotsSwitch.isOn = true;
        }else TurnOffAllBotsSwitch.isOn = false;
        //UnlockFPS
        if(loadeddata.performancemode){
            UnlockFPSSwitch.isOn = true;
        }else UnlockFPSSwitch.isOn = false;
    }
    public IEnumerator PlayerSpin(){
        while(true){
            Player.GetComponent<Rigidbody2D>().rotation += Playerspinspeed * Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
    }

    public void performanceswitch(bool mode){
        localdata.performancemode = mode;
        StartCoroutine(InitUserData3());
    }
    public void NoBotsSwitch(bool mode){
        localdata.TurnOffBots = mode;
        StartCoroutine(InitUserData3());
    }

    public IEnumerator BGMove(){
        while(true){
            int rand = UnityEngine.Random.Range(0,4); //Suche einen der 4 Punkte zum Moven aus
            Vector2 nextcampos = BGPoints[rand].transform.position;
            while(new Vector2(transform.position.x, transform.position.y) != new Vector2(BGPoints[rand].transform.position.x, BGPoints[rand].transform.position.y)){
                transform.position = Vector2.MoveTowards(transform.position, BGPoints[rand].transform.position, CAMSPEED * Time.deltaTime);
                yield return new WaitForEndOfFrame();
            }
            yield return new WaitForEndOfFrame();
        }

    }
    public static void DecideAdButton(){ //Checkt ob der User Eine Internet Verbindung hat und zeigt den Ad Button an oder nicht
        if(Application.internetReachability == NetworkReachability.NotReachable){
            instance.AdButton.SetActive(false);
        }else{
            //Schaue nun ob der nutzer schon ein Ad gesehen hat
            if(AdCounter % 2 == 0){ //Wenn der AdCounter gerade ist, dann zeige den Ad Button an
                if(Watchedads < 2){
                    //Zufall ob Emerald oder Coin
                    x = UnityEngine.Random.Range(0,10);
                    if(x == 5){
                        //Zeige den Emerald Ad Button
                        instance.AdButton.GetComponent<Image>().sprite = instance.EmeraldAdButtonSprite;
                        EmeraldAd = true;
                    }else{
                        //Zeige den Coin Ad Button
                        instance.AdButton.GetComponent<Image>().sprite = instance.CoinAdButtonSprite;
                        EmeraldAd = false;
                    }
                    instance.AdButton.SetActive(true);
                }
            }else{
                instance.AdButton.SetActive(false);
            }
        }
    }
    public IEnumerator TryToHoldConnection(){
        while(true){
            if(Application.internetReachability == NetworkReachability.NotReachable){
                //Mach Online Button Grau
                Friends_Button.GetComponent<Image>().sprite = FriendsButtonSpriteNoConnection;
                Friends_Button.GetComponent<Button>().interactable = false;
                //Shop Button Grau
                Shop_Button.GetComponent<Image>().sprite = ShopButtonSpriteNoConnection;
                Shop_Button.GetComponent<Button>().interactable = false;
                //Inventory Button Grau
                Inv_Button.GetComponent<Image>().sprite = InvButtonSpriteNoConnection;
                Inv_Button.GetComponent<Button>().interactable = false;
                //Schalte den Ad Button aus
                AdButton.SetActive(false);
                //DaylyReward aus
                DaylyRewardButton.GetComponent<Button>().interactable = false;
                //Zeige das No Connection Icon
                NoConnectionIcon.SetActive(true);
                //Zurück ins Menü
                SettingsExit();
                OfflineMode = true;
            }else{
                //Mach den Online Button wieder normal
                Friends_Button.GetComponent<Image>().sprite = FriendsButtonSprite;
                Friends_Button.GetComponent<Button>().interactable = true;
                //Shop Button normal
                Shop_Button.GetComponent<Image>().sprite = ShopButtonSprite;
                Shop_Button.GetComponent<Button>().interactable = true;
                //Inventory Button normal
                Inv_Button.GetComponent<Image>().sprite = InvButtonSprite;
                Inv_Button.GetComponent<Button>().interactable = true;
                //Schalte den Ad Button an
                //Nur wenn im Menü
                if(InMenu) DecideAdButton();
                //DaylyReward an machen wenn nicht schon abgeholt
                if(!localdata.DaylyRewardClaimed) DaylyRewardButton.GetComponent<Button>().interactable = true;
                //Zeige nicht das No Connection Icon
                NoConnectionIcon.SetActive(false);
                OfflineMode = false;
            }
            yield return new WaitForSeconds(5f);
        }
    }
    public static bool PingHost(string nameOrAddress){
        bool pingable = false;
        System.Net.NetworkInformation.Ping pinger = null; //Inspiriert von https://stackoverflow.com/questions/11800958/using-ping-in-c-sharp
        try{
            pinger = new System.Net.NetworkInformation.Ping();
            PingReply reply = pinger.Send(nameOrAddress);
            pingable = reply.Status == IPStatus.Success;
        }
        catch (PingException){
            // Discard PingExceptions and return false;
        }
        finally{
            if (pinger != null)
            {
                pinger.Dispose();
            }
        }
        return pingable;
    }
    public void FriendButtonMenu(){
        if(Application.internetReachability != NetworkReachability.NotReachable){
            //Schau ob der Server auch erreicht werden kann mit einem Ping
            LoadingText.GetComponent<Text>().text = "Loading...";
            StartCoroutine(FriendsWindowCR()); //Fetch Friends from Server
        }else{//Mach den Button Grau
            Friends_Button.GetComponent<Image>().sprite = FriendsButtonSpriteNoConnection;
            Friends_Button.GetComponent<Button>().interactable = false;
        }
    }
    public IEnumerator FriendsWindowCR(){
        //Deactivate Hud
        InMenu = false;
        Player.SetActive(false);
        Shop_Button.SetActive(false);
        Play_Button.SetActive(false);
        Coin_Counter.SetActive(false);
        Emerald_Counter.SetActive(false);
        Player_Name_Button.SetActive(false);
        Player_Name_Text.gameObject.SetActive(false);
        Friends_Button.gameObject.SetActive(false);
        Inv_Button.gameObject.SetActive(false);
        Settings_Button.gameObject.SetActive(false);
        Logo.gameObject.SetActive(false);
        Name_Input.SetActive(false);
        activeBG.SetActive(false);
        Leveltext.gameObject.SetActive(false);
        ProgressBar.gameObject.SetActive(false);
        ProgressBarBG.gameObject.SetActive(false);
        neededXP.gameObject.SetActive(false);
        GlobalLeaderboardScroll.SetActive(false);
        AdButton.SetActive(false);
        instance.AdButton.gameObject.GetComponent<Image>().enabled = false;
        instance.AdButton.gameObject.GetComponent<Button>().enabled = false;
        DaylyRewardButton.gameObject.SetActive(false);
        DaylyRewardTimerText.gameObject.SetActive(false);

        onlineBG.SetActive(true);
        X_Friends.SetActive(true);
        FriendWindow.SetActive(true);
        FriendListButton.SetActive(true);
        GlobalLeaderboardButton.SetActive(true);

        FriendListButton.GetComponent<Image>().sprite = FriendListButton_Green; //this
        GlobalLeaderboardButton.GetComponent<Image>().sprite = GlobalLeaderboardButton_Gray;

        LoadingText.SetActive(true);
        //Create you own Slot
        //Positionierung
        GameObject SlotToBeIn = MESlot;
        GameObject Slot = Instantiate(FriendSlot, SlotToBeIn.transform);
        Slot.GetComponent<Image>().sprite = MySelfSprite;
        LoadedFriends.Add(Slot); //um später beim neu laden zu löschen
        Slot.transform.parent = SlotToBeIn.transform;
        //Deaktiviere Remove Button
        Slot.transform.Find("Remove").gameObject.SetActive(false);
        Slot.GetComponent<RectTransform>().localPosition = new Vector3(0f,0f,-5000f); //setze genau auf sein Parent
        //Get Information from Server
        OnlineManager.RecievedData = null;
        StartCoroutine(OnlineManager.GetAllUserData(loadeddata.PlayerID.ToString())); //Start fetching own data
        while(OnlineManager.RecievedData == null){ //wait until 
            yield return new WaitForEndOfFrame();
        }
        //Apply Information
        Slot.transform.Find("Level").GetComponent<Text>().text = "Level:" + OnlineManager.RecievedData[6]; //Eigentlich Kills
        Slot.transform.Find("Wins").GetComponent<Text>().text = "Wins:" + OnlineManager.RecievedData[4];
        Slot.transform.Find("Name").GetComponent<Text>().text = "ME";
        Slot.transform.Find("Coins").GetComponent<Text>().text = OnlineManager.RecievedData[2];
        Slot.transform.Find("Emeralds").GetComponent<Text>().text = OnlineManager.RecievedData[3];
        //Skin
        Image SkinSlot = Slot.transform.Find("Skin").GetComponent<Image>();
        //Check which skin the database sends and then apply
        foreach(Sprite model in DisplayModels){
            if(model.name == OnlineManager.RecievedData[1]){
                SkinSlot.sprite = model;
            }
        }
        //Start getting Friends Data...
        int x = 0;
        foreach(int i in loadeddata.FriendsIDs){
            if(i != 0){
                //Positionierung
                SlotToBeIn = FriendSlots[x];
                Slot = Instantiate(FriendSlot, SlotToBeIn.transform);
                LoadedFriends.Add(Slot); //um später beim neu laden zu löschen
                Slot.transform.parent = SlotToBeIn.transform;
                Slot.GetComponent<RectTransform>().localPosition = new Vector3(0f,0f,-5000f); //setze genau auf sein Parent
                //Get Information from Server
                OnlineManager.RecievedData = null;
                StartCoroutine(OnlineManager.GetAllUserData(i.ToString())); //Start fetching Friends data
                while(OnlineManager.RecievedData == null){ //wait until 
                    yield return new WaitForEndOfFrame();
                }
                //Apply Information
                Slot.transform.Find("Level").GetComponent<Text>().text = "Level:" + OnlineManager.RecievedData[6];
                Slot.transform.Find("Wins").GetComponent<Text>().text = "Wins:" + OnlineManager.RecievedData[4];
                Slot.transform.Find("Name").GetComponent<Text>().text = OnlineManager.RecievedData[0];
                Slot.transform.Find("Coins").GetComponent<Text>().text = OnlineManager.RecievedData[2];
                Slot.transform.Find("Emeralds").GetComponent<Text>().text = OnlineManager.RecievedData[3];
                //Make Remove Button whith ID as parameter
                Button RemoveButton = Slot.transform.Find("Remove").GetComponent<Button>();
                RemoveButton.onClick.AddListener(() => {
                    RemoveFriendButton(i);
                });
                //Skin
                SkinSlot = Slot.transform.Find("Skin").GetComponent<Image>();
                //Check which skin the database sends and then apply
                foreach(Sprite model in DisplayModels){
                    if(model.name == OnlineManager.RecievedData[1]){
                        SkinSlot.sprite = model;
                    }
                }
                x++;
            }
        }
        //Füll den Rest mit Freund Add Slots
        for( ; x != 6; x++){
            SlotToBeIn = FriendSlots[x];
            Slot = Instantiate(AddFriendSlot, SlotToBeIn.transform);
            Slot.transform.parent = SlotToBeIn.transform;
            Slot.GetComponent<RectTransform>().localPosition = new Vector3(0f,0f,-5000f); //setze genau auf sein Parent

            //Funktion OnClick hinzufügen
            Slot.GetComponent<Button>().onClick.AddListener(() => {
                AddFriendButton();
            });
        }
        LoadingText.SetActive(false);
        FriendsScroll.SetActive(true);
        //Show ID
        IDText.SetActive(true);
        IDText.GetComponent<Text>().text = "ID: " + "#" + loadeddata.PlayerID;
    }
    public void AddFriendButton(){
        //deaktiviere Hintergrund
        FriendsScroll.SetActive(false);
        //Aktiviee Input
        AddFriendInput.SetActive(true);
    }   
    public void SubmitFriendButton(string ID){
        StartCoroutine(SubmitFriendButton2(ID));
    }
    private IEnumerator SubmitFriendButton2(string ID){
        //Schau ob die ID in der Datenbank vorhanden ist
        StartCoroutine(OnlineManager.CheckforExistence(ID));
        while(OnlineManager.FriendExistance == 0){
            yield return new WaitForEndOfFrame(); //warte bis Coroutine fertig ist
        } 
        if(OnlineManager.FriendExistance == 1){
            localdata = loadeddata;
            int arrayslot = 0; //Schaue welcher slot der nächste mit 0 is in der array um da die Friends ID rein zu packen.
            foreach(int i in loadeddata.FriendsIDs){
                if(i != 0){ //null == 0 bei int's
                    arrayslot++;
                }
            }
            localdata.FriendsIDs[arrayslot] = Int32.Parse(ID);
            StartCoroutine(InitUserData3());
            yield return new WaitForEndOfFrame(); //Weil zwischen Init User Data und Web Request ein frame sein muss
            StartCoroutine(RefreshFriendList());
            AddFriendInput.SetActive(false);
        }else if(OnlineManager.FriendExistance == 2){
            Debug.Log("Friend not found");
        }
    }  
    public void RemoveFriendButton(int ID){
        RemoveFriendButtonCR(ID);
    }
    private void RemoveFriendButtonCR(int ID){
        localdata = loadeddata;
        for(int i = 0; i != 6; i++){
            if(localdata.FriendsIDs[i] == ID){
                localdata.FriendsIDs[i] = 0;
            }
        }
        //resort array so no 0 is in the middle
        int[] temp = new int[6];
        int x = 0;
        for(int i = 0; i != 6; i++){
            if(localdata.FriendsIDs[i] != 0){
                temp[x] = localdata.FriendsIDs[i];
                x++;
            }
        }
        localdata.FriendsIDs = temp;
        StartCoroutine(InitUserData3());
        StartCoroutine(RefreshFriendList());
    }
    public void GlobalLeaderboardButtonFunc(){
        StartCoroutine(GlobalLeaderboardButtonFuncCR());
    }
    private IEnumerator GlobalLeaderboardButtonFuncCR(){
        //Activate this button and deactivate the others
        FriendsScroll.SetActive(false);
        AddFriendInput.SetActive(false);
        FriendListButton.GetComponent<Image>().sprite = FriendListButton_Gray;
        GlobalLeaderboardButton.GetComponent<Image>().sprite = GlobalLeaderboardButton_Green; //this

        LoadingText.SetActive(true);
        OnlineManager.GlobalBest[0][0] = null; //Auf null setzten damit wir hier warten bis die daten in die array geladen werden und nicht ausversehen den code überspringen weil wir nicht warten
        StartCoroutine(OnlineManager.GetTop20());
        while(OnlineManager.GlobalBest[0][0] == null){yield return new WaitForEndOfFrame();}//warten
        //Generate Global Top players with given data
        GameObject Slot;
        float nexty = 279f;
        for(int i = 0; i != 20; i++){
            if(OnlineManager.GlobalBest[i][7] != loadeddata.PlayerID.ToString()){ //Check if id is identicall to mine so i can mark myself on the leaderboard
                Slot = Instantiate(FriendSlot, GlobalLeaderBoardParent.transform); //spawn default slot
            }else{
                Slot = Instantiate(FriendSlot, GlobalLeaderBoardParent.transform); //Mark Slot as myself
            }
            //Slot Farbe
            if(i == 0) Slot.GetComponent<Image>().sprite = Place1Sprite;
            else if(i == 1) Slot.GetComponent<Image>().sprite = Place2Sprite;
            else if(i == 2) Slot.GetComponent<Image>().sprite = Place3Sprite;
            //check ob der slot man selber ist
            if(OnlineManager.GlobalBest[i][7] == loadeddata.PlayerID.ToString()) Slot.GetComponent<Image>().sprite = MySelfSprite;

            Slot.transform.parent = GlobalLeaderBoardParent.transform; //Parent to Root
            Slot.GetComponent<RectTransform>().localPosition = new Vector3(0f, nexty, -5000f); //Positionierung
            nexty -= 279f; //Next slot offset

            //Apply Information
            Slot.transform.Find("Level").GetComponent<Text>().text = "Level:" + OnlineManager.GlobalBest[i][6];
            Slot.transform.Find("Wins").GetComponent<Text>().text = "Wins:" + OnlineManager.GlobalBest[i][4];
            Slot.transform.Find("Name").GetComponent<Text>().text = OnlineManager.GlobalBest[i][0];
            Slot.transform.Find("Coins").GetComponent<Text>().text = OnlineManager.GlobalBest[i][2];
            Slot.transform.Find("Emeralds").GetComponent<Text>().text = OnlineManager.GlobalBest[i][3];

            //Remove Button
            Slot.transform.Find("Remove").gameObject.SetActive(false);

            //Skin
            Image SkinSlot = Slot.transform.Find("Skin").GetComponent<Image>();
            //Check which skin the database sends and then apply
            foreach(Sprite model in DisplayModels){
                if(model.name == OnlineManager.GlobalBest[i][1]){
                    SkinSlot.sprite = model;
                }
            }
        }
        LoadingText.SetActive(false);
        GlobalLeaderboardScroll.SetActive(true);
        yield return null;
    }
    public IEnumerator InitUserData3(){
        Writedata(localdata);
        yield return new WaitForEndOfFrame();
        Readdata();
        localdata = loadeddata;
    }
    public IEnumerator RefreshFriendList(){
        //Destroy all friend cards
        foreach(GameObject i in LoadedFriends){
            if(i != null){
                Destroy(i);
            }
        }
        //Build new
        LoadingText.SetActive(true);
        //Create you own Slot
        //Positionierung
        GameObject SlotToBeIn = MESlot;
        GameObject Slot = Instantiate(FriendSlot, SlotToBeIn.transform);
        LoadedFriends.Add(Slot); //um später beim neu laden zu löschen
        Slot.transform.parent = SlotToBeIn.transform;
        //Deaktiviere Remove Button
        Slot.transform.Find("Remove").gameObject.SetActive(false);
        Slot.GetComponent<RectTransform>().localPosition = new Vector3(0f,0f,-5000f); //setze genau auf sein Parent
        //Get Information from Server
        OnlineManager.RecievedData = null;
        StartCoroutine(OnlineManager.GetAllUserData(loadeddata.PlayerID.ToString())); //Start fetching own data
        while(OnlineManager.RecievedData == null){ //wait until 
            yield return new WaitForEndOfFrame();
        }
        //Apply Information
        Slot.transform.Find("Level").GetComponent<Text>().text = "Level:" + OnlineManager.RecievedData[6]; //Eigentlich Kills
        Slot.transform.Find("Wins").GetComponent<Text>().text = "Wins:" + OnlineManager.RecievedData[4];
        Slot.transform.Find("Name").GetComponent<Text>().text = "ME";
        Slot.transform.Find("Coins").GetComponent<Text>().text = OnlineManager.RecievedData[2];
        Slot.transform.Find("Emeralds").GetComponent<Text>().text = OnlineManager.RecievedData[3];
        //Skin
            Image SkinSlot = Slot.transform.Find("Skin").GetComponent<Image>();
            //Check which skin the database sends and then apply
            foreach(Sprite model in DisplayModels){
                if(model.name == OnlineManager.RecievedData[1]){
                    SkinSlot.sprite = model;
                }
            }
        //Start getting Friends Data...
        int x = 0;
        foreach(int i in loadeddata.FriendsIDs){
            if(i != 0){
                //Positionierung
                SlotToBeIn = FriendSlots[x];
                Slot = Instantiate(FriendSlot, SlotToBeIn.transform);
                LoadedFriends.Add(Slot); //um später beim neu laden zu löschen
                Slot.transform.parent = SlotToBeIn.transform;
                Slot.GetComponent<RectTransform>().localPosition = new Vector3(0f,0f,-5000f); //setze genau auf sein Parent
                //Get Information from Server
                OnlineManager.RecievedData = null;
                StartCoroutine(OnlineManager.GetAllUserData(i.ToString())); //Start fetching Friends data
                while(OnlineManager.RecievedData == null){ //wait until 
                    yield return new WaitForEndOfFrame();
                }
                //Apply Information
                Slot.transform.Find("Level").GetComponent<Text>().text = "Level:" + OnlineManager.RecievedData[6];
                Slot.transform.Find("Wins").GetComponent<Text>().text = "Wins:" + OnlineManager.RecievedData[4];
                Slot.transform.Find("Name").GetComponent<Text>().text = OnlineManager.RecievedData[0];
                Slot.transform.Find("Coins").GetComponent<Text>().text = OnlineManager.RecievedData[2];
                Slot.transform.Find("Emeralds").GetComponent<Text>().text = OnlineManager.RecievedData[3];
                //Skin
                SkinSlot = Slot.transform.Find("Skin").GetComponent<Image>();
                //Check which skin the database sends and then apply
                foreach(Sprite model in DisplayModels){
                    if(model.name == OnlineManager.RecievedData[1]){
                        SkinSlot.sprite = model;
                    }
                }
                x++;
            }
        }
        //Füll den Rest mit Freund Add Slots
        for( ; x != 6; x++){
            SlotToBeIn = FriendSlots[x];
            Slot = Instantiate(AddFriendSlot, SlotToBeIn.transform);
            Slot.transform.parent = SlotToBeIn.transform;
            Slot.GetComponent<RectTransform>().localPosition = new Vector3(0f,0f,-5000f); //setze genau auf sein Parent

            //Funktion OnClick hinzufügen
            Slot.GetComponent<Button>().onClick.AddListener(() => {
                AddFriendButton();
            });
        }
        LoadingText.SetActive(false);
        FriendsScroll.SetActive(true);
    }
    public void ShopButton(){
        InMenu = false;
        StartCoroutine(LoadShop());
    }
    public IEnumerator LoadShop(){
        //Deactivate Hud
        Player.SetActive(false);
        Shop_Button.SetActive(false);
        Play_Button.SetActive(false);
        Coin_Counter.SetActive(false);
        Emerald_Counter.SetActive(false);
        Player_Name_Button.SetActive(false);
        Player_Name_Text.gameObject.SetActive(false);
        Friends_Button.gameObject.SetActive(false);
        Inv_Button.gameObject.SetActive(false);
        Settings_Button.gameObject.SetActive(false);
        Logo.gameObject.SetActive(false);
        Name_Input.SetActive(false);
        activeBG.SetActive(false);
        Leveltext.gameObject.SetActive(false);
        ProgressBar.gameObject.SetActive(false);
        ProgressBarBG.gameObject.SetActive(false);
        neededXP.gameObject.SetActive(false);
        AdButton.SetActive(false);
        instance.AdButton.gameObject.GetComponent<Image>().enabled = false;
        instance.AdButton.gameObject.GetComponent<Button>().enabled = false;
        DaylyRewardButton.gameObject.SetActive(false);
        DaylyRewardTimerText.gameObject.SetActive(false);

        ShopBG.SetActive(true);
        X_Shop.SetActive(true);
        LoadingText.SetActive(true);
        Skin_i.SetActive(true);
        //Get Shop info from Server
        OnlineManager.Shopinfo[0] = null;
        StartCoroutine(OnlineManager.GetShopInfo());
        while(OnlineManager.Shopinfo[0] == null){
            yield return new WaitForEndOfFrame(); //Warte bis web rewuest fertig ist
        }
        //Bau den Shop
        string[] LocalShopInfo = OnlineManager.Shopinfo; //array in den cache schreiben
        int x = 0;
        bool drawprice = true;
        for(int i = 0; i != 5; i++){
            GameObject Slot = Instantiate(ShopSlotPrefab, Shop_Slot[i].transform);
            Slot.transform.parent = Shop_Slot[i];
            Slot.GetComponent<RectTransform>().localPosition = Vector3.zero;
            //Suche Skin und Name
            foreach(Sprite PlayerSprite in DisplayModels){
                if(PlayerSprite.name == LocalShopInfo[x]){
                    //Check for Ownage
                    Slot.transform.Find("Name").GetComponent<Text>().text = PlayerSprite.name;
                    Slot.transform.Find("Skin").GetComponent<Image>().sprite = PlayerSprite;
                    if(!CheckforSkinOwnage(PlayerSprite.name)){
                        Slot.transform.Find("BuyButton").GetComponent<Image>().sprite = BuyButton;
                        drawprice = false;
                        //Mach den Button
                        Slot.transform.Find("BuyButton").GetComponent<Button>().onClick.AddListener(() => {
                            BuySkin(PlayerSprite.name);
                        });
                    }else{
                        Slot.transform.Find("BuyButton").GetComponent<Image>().sprite = SoldOutButton;
                        drawprice = true;
                    }
                }
            }
            
            //Slot Farbe (Raritiy) + Price + Coin/Emerald
            x++; //Um auf den Raritäts Slot zu springen
            if(LocalShopInfo[x] == "1"){
                Slot.GetComponent<Image>().sprite = Normal_ShopSlot; 
                Slot.transform.Find("Price").GetComponent<Text>().text = "100";
                Slot.transform.Find("Coin Icon").GetComponent<Image>().sprite = CoinIcon;
            }else if(LocalShopInfo[x] == "2"){
                Slot.GetComponent<Image>().sprite = Rare_ShopSlot;
                Slot.transform.Find("Price").GetComponent<Text>().text = "200";
                Slot.transform.Find("Price").transform.Find("Coin Icon").GetComponent<Image>().sprite = CoinIcon; 
            }else if(LocalShopInfo[x] == "3"){
                Slot.GetComponent<Image>().sprite = Superior_ShopSlot;
                Slot.transform.Find("Price").GetComponent<Text>().text = "350";
                Slot.transform.Find("Price").transform.Find("Coin Icon").GetComponent<Image>().sprite = CoinIcon; 
            }else if(LocalShopInfo[x] == "4"){
                Slot.GetComponent<Image>().sprite = Exquisit_ShopSlot;
                Slot.transform.Find("Price").GetComponent<Text>().text = "500";
                Slot.transform.Find("Price").transform.Find("Coin Icon").GetComponent<Image>().sprite = CoinIcon; 
            }else if(LocalShopInfo[x] == "5"){
                Slot.GetComponent<Image>().sprite = Extravagant_ShopSlot;
                Slot.transform.Find("Price").GetComponent<Text>().text = "10";
                Slot.transform.Find("Price").transform.Find("Coin Icon").GetComponent<Image>().sprite = EmeraldIcon;
            }
            if(drawprice){ //damit nicht über den "Sold" button gemalt wird
                Slot.transform.Find("Price").gameObject.SetActive(false);
                Slot.transform.Find("Price").gameObject.SetActive(false); 
            }
            x++;
        }
        LoadingText.SetActive(false);
        ShopScroll.SetActive(true);
    }
    private bool CheckforSkinOwnage(string Skinname){
        Readdata();
        if(Skinname == "Beta" && loadeddata.Beta_Ownage) return true;
        else if(Skinname == "Otto" && loadeddata.Otto_Ownage) return true;
        else if(Skinname == "Alien" && loadeddata.Alien_Ownage) return true;
        else if(Skinname == "Chris" && loadeddata.Chris_Ownage) return true;
        else if(Skinname == "Agent" && loadeddata.Agent_Ownage) return true;
        else if(Skinname == "Clown" && loadeddata.Clown_Ownage) return true;
        else return false;
    }
    public void BuySkin(string Skinname){
        if(Skinname == "Otto" && localdata.Saved_Coins >= 200){
            localdata.Saved_Coins -= 200;
            localdata.Otto_Ownage = true;
        }else if(Skinname == "Alien" && localdata.Saved_Coins >= 500){
            localdata.Saved_Coins -= 500;
            localdata.Alien_Ownage = true;
        }else if(Skinname == "Chris" && localdata.Saved_Coins >= 350){
            localdata.Saved_Coins -= 350;
            localdata.Chris_Ownage = true;
        }else if(Skinname == "Agent" && localdata.Saved_Coins >= 350){
            localdata.Saved_Coins -= 350;
            localdata.Agent_Ownage = true;
        }else if(Skinname == "Clown" && localdata.Saved_Coins >= 500){
            localdata.Saved_Coins -= 500;
            localdata.Clown_Ownage = true;
        }else if(Skinname == "Beta"){
            //localdata.Saved_Coins -= 200;
            //localdata.Otto_Ownage = true;
            //Kann man nicht kaufen
        }
        //... Inventar Refresh
        StartCoroutine(RefreshShop(Skinname));
        Refresh_CoinCounter();
        //Update Database
        //Weiter Funktionen am ende von Refresh shop
    }
    public IEnumerator RefreshShop(string Skinname){

        ShopBG.SetActive(true);
        X_Shop.SetActive(true);
        LoadingText.SetActive(true);
        ShopScroll.SetActive(false);
        //Get Shop info from Server
        OnlineManager.Shopinfo[0] = null;
        StartCoroutine(OnlineManager.GetShopInfo());
        while(OnlineManager.Shopinfo[0] == null){
            yield return new WaitForEndOfFrame(); //Warte bis web rewuest fertig ist
        }
        //Bau den Shop
        string[] LocalShopInfo = OnlineManager.Shopinfo; //array in den cache schreiben
        int x = 0;
        bool drawprice = true;
        for(int i = 0; i != 5; i++){
            GameObject Slot = Instantiate(ShopSlotPrefab, Shop_Slot[i].transform);
            Slot.transform.parent = Shop_Slot[i];
            Slot.GetComponent<RectTransform>().localPosition = Vector3.zero;
            //Suche Skin und Name
            foreach(Sprite PlayerSprite in DisplayModels){
                if(PlayerSprite.name == LocalShopInfo[x]){
                    //Check for Ownage
                    Slot.transform.Find("Name").GetComponent<Text>().text = PlayerSprite.name;
                    Slot.transform.Find("Skin").GetComponent<Image>().sprite = PlayerSprite;
                    if(!CheckforSkinOwnage(PlayerSprite.name)){
                        Slot.transform.Find("BuyButton").GetComponent<Image>().sprite = BuyButton;
                        drawprice = false;
                        //Mach den Button
                        Slot.transform.Find("BuyButton").GetComponent<Button>().onClick.AddListener(() => {
                            BuySkin(PlayerSprite.name);
                        });
                    }else{
                        Slot.transform.Find("BuyButton").GetComponent<Image>().sprite = SoldOutButton;
                        drawprice = true;
                    }
                }
            }
            
            //Slot Farbe (Raritiy) + Price + Coin/Emerald
            x++; //Um auf den Raritäts Slot zu springen
            if(LocalShopInfo[x] == "1"){
                Slot.GetComponent<Image>().sprite = Normal_ShopSlot; 
                Slot.transform.Find("Price").GetComponent<Text>().text = "100";
                Slot.transform.Find("Coin Icon").GetComponent<Image>().sprite = CoinIcon;
            }else if(LocalShopInfo[x] == "2"){
                Slot.GetComponent<Image>().sprite = Rare_ShopSlot;
                Slot.transform.Find("Price").GetComponent<Text>().text = "200";
                Slot.transform.Find("Price").transform.Find("Coin Icon").GetComponent<Image>().sprite = CoinIcon; 
            }else if(LocalShopInfo[x] == "3"){
                Slot.GetComponent<Image>().sprite = Superior_ShopSlot;
                Slot.transform.Find("Price").GetComponent<Text>().text = "350";
                Slot.transform.Find("Price").transform.Find("Coin Icon").GetComponent<Image>().sprite = CoinIcon; 
            }else if(LocalShopInfo[x] == "4"){
                Slot.GetComponent<Image>().sprite = Exquisit_ShopSlot;
                Slot.transform.Find("Price").GetComponent<Text>().text = "500";
                Slot.transform.Find("Price").transform.Find("Coin Icon").GetComponent<Image>().sprite = CoinIcon; 
            }else if(LocalShopInfo[x] == "5"){
                Slot.GetComponent<Image>().sprite = Extravagant_ShopSlot;
                Slot.transform.Find("Price").GetComponent<Text>().text = "10";
                Slot.transform.Find("Price").transform.Find("Coin Icon").GetComponent<Image>().sprite = EmeraldIcon;
            }
            if(drawprice){ //damit nicht über den "Sold" button gemalt wird
                Slot.transform.Find("Price").gameObject.SetActive(false);
                Slot.transform.Find("Price").gameObject.SetActive(false); 
            }
            x++;
        }
        LoadingText.SetActive(false);
        ShopScroll.SetActive(true);
        Skinbutton(Skinname);
        StartCoroutine(InitUserData4());
        //Coins werden nicht geupdated!!
    }
    public IEnumerator InitUserData4(){
        Writedata(localdata);
        yield return new WaitForEndOfFrame();
        Readdata();
        localdata = loadeddata;
        StartCoroutine(OnlineManager.UpdateUserData());
    }
    public void SkinInformationButton(){
        if(infoswitch){ //Info wird nicht angezeigt
            SkinInfoTafel.SetActive(false);
            ShopScroll.SetActive(true);
            infoswitch = false;
        }else{ //Info wird angezeigt
            infoswitch = true;
            SkinInfoTafel.SetActive(true);
            ShopScroll.SetActive(false);
        }
    }

    public void InventoryButton(){
        Debug.Log("Inventory Button");
        //Deactivate Hud
        InMenu = false;
        Player.SetActive(false);
        Shop_Button.SetActive(false);
        Play_Button.SetActive(false);
        Coin_Counter.SetActive(false);
        Emerald_Counter.SetActive(false);
        Player_Name_Button.SetActive(false);
        Player_Name_Text.gameObject.SetActive(false);
        Friends_Button.gameObject.SetActive(false);
        Inv_Button.gameObject.SetActive(false);
        Settings_Button.gameObject.SetActive(false);
        Logo.gameObject.SetActive(false);
        Name_Input.SetActive(false);
        activeBG.SetActive(false);
        Leveltext.gameObject.SetActive(false);
        ProgressBar.gameObject.SetActive(false);
        ProgressBarBG.gameObject.SetActive(false);
        neededXP.gameObject.SetActive(false);
        instance.AdButton.gameObject.GetComponent<Image>().enabled = false;
        instance.AdButton.gameObject.GetComponent<Button>().enabled = false;
        DaylyRewardButton.gameObject.SetActive(false);
        DaylyRewardTimerText.gameObject.SetActive(false);


        //Activate Inventory
        InvBG.SetActive(true);
        Inv_Hud.SetActive(true);
        X_Inv.SetActive(true);
    }

    public void ChangeName(){
        Player.SetActive(false); //Deaktiviere das Ganze Menü und mache nur den Input sichtbar
        Shop_Button.SetActive(false);
        Play_Button.SetActive(false);
        Coin_Counter.SetActive(false);
        Emerald_Counter.SetActive(false);
        Player_Name_Button.SetActive(false);
        Player_Name_Text.gameObject.SetActive(false);
        Friends_Button.gameObject.SetActive(false);
        Inv_Button.gameObject.SetActive(false);
        Settings_Button.gameObject.SetActive(false);
        Logo.gameObject.SetActive(false);
        Leveltext.gameObject.SetActive(false);
        ProgressBar.gameObject.SetActive(false);
        ProgressBarBG.gameObject.SetActive(false);
        neededXP.gameObject.SetActive(false);
        instance.AdButton.gameObject.GetComponent<Image>().enabled = false;
        instance.AdButton.gameObject.GetComponent<Button>().enabled = false;
        DaylyRewardButton.gameObject.SetActive(false);
        DaylyRewardTimerText.gameObject.SetActive(false);

        Name_Input.SetActive(true); //Input Field
    }

    public void ChangeNameDone(string Nick){
        Player.SetActive(true); 
        Shop_Button.SetActive(true);
        Play_Button.SetActive(true);
        Coin_Counter.SetActive(true);
        Emerald_Counter.SetActive(true);
        Player_Name_Button.SetActive(true);
        Player_Name_Text.gameObject.SetActive(true);
        Friends_Button.gameObject.SetActive(true);
        Inv_Button.gameObject.SetActive(true);
        Settings_Button.gameObject.SetActive(true);
        Logo.gameObject.SetActive(true);
        Leveltext.gameObject.SetActive(true);
        ProgressBar.gameObject.SetActive(true);
        ProgressBarBG.gameObject.SetActive(true);
        neededXP.gameObject.SetActive(true);
        instance.AdButton.gameObject.GetComponent<Image>().enabled = true;
        instance.AdButton.gameObject.GetComponent<Button>().enabled = true;

        Name_Input.SetActive(false); //Input Field

        if(Nick == null){
            Player_Name = "Player";
            
        }else{
            int y = 0;
            foreach(string i in sonderzeichen){
                if(Nick.Contains(i)){
                    y = 1; //blocks if statement
                    //throw window with notifitation
                    Debug.Log("Sonderzeichen nicht erlaubt");
                }
            }
            if(y == 0){
                Player_Name = Nick;
                Refresh_PlayerName();
                StartCoroutine(OnlineManager.UpdateNickname());
            }
        } 
    }

    public void Refresh_CoinCounter(){
        Coin_Count.text = localdata.Saved_Coins.ToString();
        Writedata(localdata);
    }

    public void Refresh_PlayerName(){
        Player_Name_Text.text = Player_Name;
        //Write to save variable
        localdata = loadeddata;
        localdata.Saved_Player_Name = Player_Name; //Neuer Name
        Writedata(localdata);
    }

    public void StartGame(){
        //Show Solo/Duo Mode Animation
        PlayButtonAnim.SetBool("Select_Mode", true);
        //StartCoroutine(LoadGameSolo());
    }
    //Solo
    public void StartGameSolo(){
        StartCoroutine(LoadGameSolo());
        DuoMode = false;
    }
    //Duo
    public void StartGameDuo(){
        StartCoroutine(LoadGameSolo());
        DuoMode = true;
    }
    public IEnumerator LoadGameSolo(){
        //Blend langsam den Black screen ein und lad dann die scene
        //Gib dem Loadingscreen Player model den Skin den der player gerade spielt
        PlayerModel_LoadingScreen.GetComponent<Image>().sprite = Player.GetComponent<Image>().sprite;
        PlayerModel_LoadingScreen_Image = PlayerModel_LoadingScreen.GetComponent<Image>().sprite; //Für den Ingame Black screen
        Black.gameObject.SetActive(true); //muss vorher aus sein und dann aktiviert werden weil er sonst über allem liegt
        //Black Color Cache
        float Bcolorr = Black.color.r;
        float Bcolorg = Black.color.g;
        float Bcolorb = Black.color.b;
        //BBRLogo Color Cache
        Image BBRLogo_LoadingScreenImage = BBRLogo_LoadingScreen.GetComponent<Image>();
        float BBRColorR = BBRLogo_LoadingScreen.GetComponent<Image>().color.r;
        float BBRColorG = BBRLogo_LoadingScreen.GetComponent<Image>().color.g;
        float BBRColorB = BBRLogo_LoadingScreen.GetComponent<Image>().color.b;
        //PlayerModel Color Cache
        Image PlayerModel_LoadingScreenImage = PlayerModel_LoadingScreen.GetComponent<Image>();
        float PlayerLoadingColorR = PlayerModel_LoadingScreen.GetComponent<Image>().color.r;
        float PlayerLoadingColorG = PlayerModel_LoadingScreen.GetComponent<Image>().color.g;
        float PlayerLoadingColorB = PlayerModel_LoadingScreen.GetComponent<Image>().color.b;
        //RectTransform Cache
        RectTransform BBRLogoTrans = BBRLogo_LoadingScreen.GetComponent<RectTransform>();
        float BBRLogoTransY = BBRLogoTrans.localPosition.y; //X und Z bleiben gleich also in den Cache schreiben
        float BBRLogoTransZ = BBRLogoTrans.localPosition.z;
        RectTransform PlayerModelTrans = PlayerModel_LoadingScreen.GetComponent<RectTransform>();
        float PlayerModelY = PlayerModelTrans.localPosition.y; 
        float PlayerModelZ = PlayerModelTrans.localPosition.z;
        for(int i = 0; i < 90; i++){
            Black.color = new Color(Bcolorr,Bcolorg,Bcolorb,Black.color.a + 0.0111111111111111f); //Blendet das schwarz,Logo und Player in 90 schritten ein weil 90 x 0.0111111111111111f = 1;
            BBRLogo_LoadingScreenImage.color = new Color(BBRColorR, BBRColorG, BBRColorB, BBRLogo_LoadingScreenImage.color.a + 0.0111111111111111f);
            PlayerModel_LoadingScreenImage.color = new Color(PlayerLoadingColorR, PlayerLoadingColorG, PlayerLoadingColorB, PlayerModel_LoadingScreenImage.color.a + 0.0111111111111111f);
            BBRLogoTrans.localPosition = new Vector3(BBRLogoTrans.localPosition.x - 15.66666666666667f, BBRLogoTransY, BBRLogoTransZ); //Logo und Player wird in die Mite geschoben
            PlayerModelTrans.localPosition = new Vector3(PlayerModelTrans.localPosition.x + 11.68888888888889f, PlayerModelY, PlayerModelZ);
            yield return new WaitForSecondsRealtime(0.000001f);
        }
        
        SceneManager.LoadSceneAsync(sceneName:"Map_1");
    }
    public void Settings(){
        Player.SetActive(false); //Deaktiviere das Ganze Menü und mache nur die Settings sichtbar
        Shop_Button.SetActive(false);
        Play_Button.SetActive(false);
        Coin_Counter.SetActive(false);
        Emerald_Counter.SetActive(false);
        Player_Name_Button.SetActive(false);
        Player_Name_Text.gameObject.SetActive(false);
        Friends_Button.gameObject.SetActive(false);
        Inv_Button.gameObject.SetActive(false);
        Logo.gameObject.SetActive(false);
        Name_Input.SetActive(false);
        Settings_Button.gameObject.SetActive(false);
        AdButton.SetActive(false);
        AdButton.GetComponent<Image>().enabled = false;
        AdButton.GetComponent<Button>().enabled = false;
        ProgressBar.gameObject.SetActive(false);
        ProgressBarBG.gameObject.SetActive(false);
        DaylyRewardButton.gameObject.SetActive(false);
        DaylyRewardTimerText.gameObject.SetActive(false);
        

        Settings_Menu_X.gameObject.SetActive(true);
        Settings_Menu.gameObject.SetActive(true);
    }

    public void SettingsExit(){
        Player.SetActive(true); //Deaktiviere das Ganze Menü und mache nur die Settings sichtbar
        Shop_Button.SetActive(true);
        Play_Button.SetActive(true);
        Coin_Counter.SetActive(true);
        Emerald_Counter.SetActive(true);
        Player_Name_Button.SetActive(true);
        Player_Name_Text.gameObject.SetActive(true);
        Friends_Button.gameObject.SetActive(true);
        Inv_Button.gameObject.SetActive(true);
        Logo.gameObject.SetActive(true);
        Name_Input.SetActive(false);
        Settings_Button.gameObject.SetActive(true);
        //Ad
        instance.AdButton.gameObject.GetComponent<Image>().enabled = true;
        instance.AdButton.gameObject.GetComponent<Button>().enabled = true;

        Settings_Menu_X.gameObject.SetActive(false);
        Settings_Menu.gameObject.SetActive(false);

        //und Inventory weil gleicher button genutzt wird
        Inv_Hud.SetActive(false);
        InvBG.SetActive(false);
        activeBG.SetActive(true);
        X_Inv.SetActive(false);
        Leveltext.gameObject.SetActive(true);
        ProgressBar.gameObject.SetActive(true);
        ProgressBarBG.gameObject.SetActive(true);
        neededXP.gameObject.SetActive(true);
        ShopBG.gameObject.SetActive(false);
        //und online
        onlineBG.SetActive(false);
        X_Inv.SetActive(false);
        FriendWindow.SetActive(false);
        IDText.SetActive(false);
        X_Friends.SetActive(false);
        FriendsScroll.SetActive(false);
        FriendListButton.SetActive(false);
        GlobalLeaderboardButton.SetActive(false);
        AddFriendInput.SetActive(false);
        GlobalLeaderboardScroll.SetActive(false);
        LoadingText.SetActive(false);
        //und Shop
        ShopScroll.SetActive(false);
        ShopBG.SetActive(false);
        X_Shop.SetActive(false);
        Skin_i.SetActive(false);
        SkinInfoTafel.SetActive(false);
        //Dicide Ad Button
        DecideAdButton();
        AdButton.GetComponent<Image>().enabled = true;
        AdButton.GetComponent<Button>().enabled = true;
        //Und XP Progress
        XPProgressBG.SetActive(false);
        LevelText_Anim.SetActive(false);
        XPNumber_Anim.SetActive(false);
        XPBar_Anim.SetActive(false);
        XpBG_Anim.SetActive(false);
        XpOKButton.SetActive(false);
        GrayBG.SetActive(false);
        //Wenn Dayly Reward Claimable ist dann zeige den Button ind Normaler Farbe, Sonst Grau
        DaylyRewardButton.gameObject.SetActive(true);
        if(loadeddata.DaylyRewardClaimed){
            DaylyRewardButton.GetComponent<Image>().sprite = DaylyrewardButtonGray;
            DaylyRewardTimerText.gameObject.SetActive(true);
            DaylyRewardButton.GetComponent<Animator>().SetBool("Jiggle", false);
            DaylyRewardButton.GetComponent<Button>().enabled = false;
        }else{
            DaylyRewardButton.GetComponent<Image>().sprite = DaylyrewardButton;
            DaylyRewardButton.GetComponent<Animator>().SetBool("Jiggle", true);
            DaylyRewardButton.GetComponent<Button>().enabled = true;
            DaylyRewardButton.GetComponent<Image>().sprite = DaylyrewardButton;
            DaylyRewardTimerText.gameObject.SetActive(false);
        }
        InMenu = true;
    }
    public void VisionShootSwitch(){
        if(localdata.VisionShootSwitch){
            localdata.VisionShootSwitch = false;
            VisionShootSwitchButton.GetComponent<Image>().sprite = VisionShootSwitchOff;
        }else{
            localdata.VisionShootSwitch = true;
            VisionShootSwitchButton.GetComponent<Image>().sprite = VisionShootSwitchOn;
        }
        StartCoroutine(InitUserData3());
    }

    public void MusicSettingChanged(float newVolume){
        //Auf die neue Lautstärke aktualisieren
        LobbyMusic.GetComponent<AudioSource>().volume = newVolume;
        //und in die Daten speichern
        localdata = loadeddata;
        localdata.Menu_Music_Volume = newVolume;
        Writedata(localdata);
        //Music in Prozent im Menü anzeigen
    }
    public void SFXSettingChanged(float newVolume){
        localdata = loadeddata;
        localdata.SFX_Volume = newVolume;
        Writedata(localdata);
        //Music in Prozent im Menü anzeigen
    }

    public void Refresh_Menu_Skin(){
        switch(loadeddata.SelectedSkin){
            case "Default":
            Menu_Skin_Display.GetComponent<Image>().sprite = DefaultSkinSprite;
            break;

            case "Beta":
            Menu_Skin_Display.GetComponent<Image>().sprite = BetaSkinSprite;
            break;

            case "Agent":
            Menu_Skin_Display.GetComponent<Image>().sprite = AgentSkinSprite;
            break;

            case "Clown":
            Menu_Skin_Display.GetComponent<Image>().sprite = ClownSkinSprite;
            break;

            case "Alien":
            Menu_Skin_Display.GetComponent<Image>().sprite = AlienSkinSprite;
            break;

            case "Chris":
            Menu_Skin_Display.GetComponent<Image>().sprite = ChrisSkinSprite;
            break;

            case "Otto":
            Menu_Skin_Display.GetComponent<Image>().sprite = OttoSkinSprite;
            break;

            default:
            //Debug.Log("Couldnt find Skin you search for");
            break;

        }
    }
    public void Skinbutton(string Skinname){
        //Debug.Log(Skinname);
        localdata = loadeddata;
        //alle einmal abfragen weil ich keine lust habe beim Daten lesen alle in eine array zu schreiben
        if(Skinname == "Default"){
            if(loadeddata.Default_Ownage == true)localdata.SelectedSkin = "Default";
        }else if(Skinname == "Beta"){
            if(loadeddata.Beta_Ownage == true) localdata.SelectedSkin = "Beta";
        }else if(Skinname == "Agent"){
            if(loadeddata.Agent_Ownage == true) localdata.SelectedSkin = "Agent";
        }else if(Skinname == "Clown"){
            if(loadeddata.Clown_Ownage == true) localdata.SelectedSkin = "Clown";
        }else if(Skinname == "Alien"){
            if(loadeddata.Alien_Ownage == true) localdata.SelectedSkin = "Alien";
        }else if(Skinname == "Chris"){
            if(loadeddata.Chris_Ownage == true) localdata.SelectedSkin = "Chris";
        }else if(Skinname == "Otto"){
            if(loadeddata.Otto_Ownage == true) localdata.SelectedSkin = "Otto";
        }
        //Write to Savefile
        StartCoroutine(InitUserData2());
    }
    public IEnumerator InitUserData2(){
        Writedata(localdata);
        //Debug.Log("Write...");
        yield return new WaitForEndOfFrame();
        Readdata();
        //Debug.Log("Read...");
        //Refresh Skin buttons
        InitSkinButtons();
        //Write New Skin to Database
        StartCoroutine(OnlineManager.UpdateSelectedSkin());
    }
    public void InitSkinButtons(){
        //Debug.Log("Stell Farbe um");
        //Default Skin
        if(loadeddata.Default_Ownage == true){//Wenn Skin in besitz dann auf rot und gaanz am ende wird der der selected ist auf grün geschaltet
            DefaultSkinButton.sprite = DefaultSkinButtonRed;
        }else{ //und wenn nicht im besitz mach grau
            DefaultSkinButton.sprite = DefaultSkinButtonGray;
        }
        //Beta
        if(loadeddata.Beta_Ownage == true){
            BetaSkinButton.sprite = BetaSkinButtonRed;
        }else{
            BetaSkinButton.sprite = BetaSkinButtonGray;
        }
        //Agent
        if(loadeddata.Agent_Ownage == true){
            AgentSkinButton.sprite = AgentSkinButtonRed;
        }else{
            AgentSkinButton.sprite = AgentSkinButtonGray;
        }
        //Clown
        if(loadeddata.Clown_Ownage == true){
            ClownSkinButton.sprite = ClownSkinButtonRed;
        }else{
            ClownSkinButton.sprite = ClownSkinButtonGray;
        }
        //Otto
        if(loadeddata.Otto_Ownage == true){
            OttoSkinButton.sprite = OttoSkinButtonRed;
        }else{
            OttoSkinButton.sprite = OttoSkinButtonGray;
        }
        //Alien
        if(loadeddata.Alien_Ownage == true){
            AlienSkinButton.sprite = AlienSkinButtonRed;
        }else{
            AlienSkinButton.sprite = AlienSkinButtonGray;
        }
        //Chris
        if(loadeddata.Chris_Ownage == true){
            ChrisSkinButton.sprite = ChrisSkinButtonRed;
        }else{
            ChrisSkinButton.sprite = ChrisSkinButtonGray;
        }
        //Debug.Log(loadeddata.SelectedSkin);
        //Schalte den der gerade equiped ist auf grün (immernoch machbar mit arrays aber keine lust den grund code umzuschreiben (wegen Problemen beim Speichern/Lesen))
        if(loadeddata.SelectedSkin == "Default"){
            DefaultSkinButton.sprite = DefaultSkinButtonGreen;
            Menu_Skin_Display.GetComponent<Image>().sprite = DefaultDispalayModel;
        } 
        else if(loadeddata.SelectedSkin == "Beta"){
            BetaSkinButton.sprite = BetaSkinButtonGreen;
            Menu_Skin_Display.GetComponent<Image>().sprite = BetaDisplayModel;
        }  
        else if(loadeddata.SelectedSkin == "Agent"){
            AgentSkinButton.sprite = AgentSkinButtonGreen;
            Menu_Skin_Display.GetComponent<Image>().sprite = AgentDisplayModel;
        }
        else if(loadeddata.SelectedSkin == "Clown"){
            ClownSkinButton.sprite = ClownSkinButtonGreen;
            Menu_Skin_Display.GetComponent<Image>().sprite =   ClownDisplayModel;
        } 
        else if(loadeddata.SelectedSkin == "Otto"){
            OttoSkinButton.sprite = OttoSkinButtonGreen;
            Menu_Skin_Display.GetComponent<Image>().sprite = OttoDisplayModel;
        } 
        else if(loadeddata.SelectedSkin == "Chris"){
            ChrisSkinButton.sprite = ChrisSkinButtonGreen;
            Menu_Skin_Display.GetComponent<Image>().sprite = ChrisDisplayModel;
        } 
        else if(loadeddata.SelectedSkin == "Alien"){
            AlienSkinButton.sprite = AlienSkinButtonGreen;
            Menu_Skin_Display.GetComponent<Image>().sprite = AlienDisplayModel;
        } 
    }
}