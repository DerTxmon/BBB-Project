using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class Menu_Handler : MonoBehaviour
{
    public GameObject Player, Shop_Button, Play_Button, Friends_Button, Inv_Button, Coin_Counter, Player_Name_Button, Name_Input, BG1, BG2, BG3, BG4, Logo, Settings_Button, Settings_BG, Settings_Menu;
    public GameObject LobbyMusic, Settings_X;
    public Text Coin_Count, Player_Name_Text, Bot_Counter;
    public GameObject[] BGPoints;
    public int Coins; //Muss noch Automatisch gezählt werden wird aber vorerst auf 123 Gesetzt
    public static string Player_Name = "Player";
    public static int Menu_Bots = 0;
    public float CAMSPEED = 0.001f;
    public float Playerspinspeed;
    public static bool performancemode = true;

    void Awake(){
        ChooseBG();
        StartCoroutine(PlayerSpin());
        StartCoroutine(BGMove());
        StartCoroutine(ButtonAnimation());
        Refresh_CoinCounter();
    }
    public void ChooseBG(){
        int BGnum = UnityEngine.Random.Range(0,4);
        if(BGnum == 0){
            BG1.SetActive(true);
            BG2.SetActive(false);
            BG3.SetActive(false);
            BG4.SetActive(false);
        }else if(BGnum == 1){
            BG1.SetActive(false);
            BG2.SetActive(true);
            BG3.SetActive(false);
            BG4.SetActive(false);
        }else if(BGnum == 2){
            BG1.SetActive(false);
            BG2.SetActive(false);
            BG3.SetActive(true);
            BG4.SetActive(false);
        }else if(BGnum == 3){
            BG1.SetActive(false);
            BG2.SetActive(false);
            BG3.SetActive(false);
            BG4.SetActive(true);
        }
    }
    public IEnumerator ButtonAnimation(){
        //Play Button Rein Raus zoom
        while(true){
            for(int i = 0; i != 10; i++){ //ZOOM IN
                Play_Button.GetComponent<RectTransform>().localScale = new Vector3(Play_Button.GetComponent<RectTransform>().localScale.x +.01f,Play_Button.GetComponent<RectTransform>().localScale.y + .01f,0f);
                yield return new WaitForSeconds(.03f);
            }
            for(int i = 10; i != 0; i--){ //ZOOM OUT
                Play_Button.GetComponent<RectTransform>().localScale = new Vector3(Play_Button.GetComponent<RectTransform>().localScale.x - .01f,Play_Button.GetComponent<RectTransform>().localScale.y - .01f,0f);
                yield return new WaitForSeconds(.03f);
            }
            //Friends Button
            for(int i = 0; i != 10; i++){ //ZOOM IN
                Friends_Button.GetComponent<RectTransform>().localScale = new Vector3(Friends_Button.GetComponent<RectTransform>().localScale.x +.01f,Friends_Button.GetComponent<RectTransform>().localScale.y + .01f,0f);
                yield return new WaitForSeconds(.025f);
            }
            for(int i = 10; i != 0; i--){ //ZOOM OUT
                Friends_Button.GetComponent<RectTransform>().localScale = new Vector3(Friends_Button.GetComponent<RectTransform>().localScale.x - .01f,Friends_Button.GetComponent<RectTransform>().localScale.y - .01f,0f);
                yield return new WaitForSeconds(.025f);
            }
            //Shop Button
            for(int i = 0; i != 10; i++){ //ZOOM IN
                Shop_Button.GetComponent<RectTransform>().localScale = new Vector3(Shop_Button.GetComponent<RectTransform>().localScale.x +.01f,Shop_Button.GetComponent<RectTransform>().localScale.y + .01f,0f);
                yield return new WaitForSeconds(.025f);
            }
            for(int i = 10; i != 0; i--){ //ZOOM OUT
                Shop_Button.GetComponent<RectTransform>().localScale = new Vector3(Shop_Button.GetComponent<RectTransform>().localScale.x - .01f,Shop_Button.GetComponent<RectTransform>().localScale.y - .01f,0f);
                yield return new WaitForSeconds(.025f);
            }
            //Inv Button
            for(int i = 0; i != 10; i++){ //ZOOM IN
                Inv_Button.GetComponent<RectTransform>().localScale = new Vector3(Inv_Button.GetComponent<RectTransform>().localScale.x +.01f,Inv_Button.GetComponent<RectTransform>().localScale.y + .01f,0f);
                yield return new WaitForSeconds(.025f);
            }
            for(int i = 10; i != 0; i--){ //ZOOM OUT
                Inv_Button.GetComponent<RectTransform>().localScale = new Vector3(Inv_Button.GetComponent<RectTransform>().localScale.x - .01f,Inv_Button.GetComponent<RectTransform>().localScale.y - .01f,0f);
                yield return new WaitForSeconds(.025f);
            }
            yield return new WaitForSeconds(3.5f);
        }
    }

    public IEnumerator PlayerSpin(){
        while(true){
            Player.GetComponent<Rigidbody2D>().rotation += Playerspinspeed * Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
    }

    public void performanceswitch(bool mode){
        performancemode = mode;
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
    public void ChangeName(){
        Player.SetActive(false); //Deaktiviere das Ganze Menü und mache nur den Input sichtbar
        Shop_Button.SetActive(false);
        Play_Button.SetActive(false);
        Coin_Counter.SetActive(false);
        Player_Name_Button.SetActive(false);
        Player_Name_Text.gameObject.SetActive(false);
        Friends_Button.gameObject.SetActive(false);
        Inv_Button.gameObject.SetActive(false);
        Settings_Button.gameObject.SetActive(false);
        Logo.gameObject.SetActive(false);

        Name_Input.SetActive(true); //Input Field
    }

    public void ChangeNameDone(string Nick){
        Player.SetActive(true); 
        Shop_Button.SetActive(true);
        Play_Button.SetActive(true);
        Coin_Counter.SetActive(true);
        Player_Name_Button.SetActive(true);
        Player_Name_Text.gameObject.SetActive(true);
        Friends_Button.gameObject.SetActive(true);
        Inv_Button.gameObject.SetActive(true);
        Settings_Button.gameObject.SetActive(true);
        Logo.gameObject.SetActive(true);

        Name_Input.SetActive(false); //Input Field

        Player_Name = Nick;
        
        Refresh_PlayerName();
    }

    public void Refresh_CoinCounter(){
        Coin_Count.GetComponent<Text>().text = Coins.ToString();
    }

    public void Refresh_PlayerName(){
        Player_Name_Text.GetComponent<Text>().text = Player_Name;
    }

    public void StartGame(){
        //Load the Game
        SceneManager.LoadScene(sceneName:"Game");
    }

    public void AddBot(){
        if(Menu_Bots < 20)Menu_Bots += 1;
        Bot_Counter.text = Menu_Bots.ToString();
    }

    public void RemoveBot(){
        if(Menu_Bots != 0) Menu_Bots -= 1;
        Bot_Counter.text = Menu_Bots.ToString();
    }
    public void Settings(){
        Player.SetActive(false); //Deaktiviere das Ganze Menü und mache nur die Settings sichtbar
        Shop_Button.SetActive(false);
        Play_Button.SetActive(false);
        Coin_Counter.SetActive(false);
        Player_Name_Button.SetActive(false);
        Player_Name_Text.gameObject.SetActive(false);
        Friends_Button.gameObject.SetActive(false);
        Inv_Button.gameObject.SetActive(false);
        Logo.gameObject.SetActive(false);
        Name_Input.SetActive(false);
        Settings_Button.gameObject.SetActive(false);

        Settings_Menu.gameObject.SetActive(true);
    }

    public void SettingsExit(){
        Player.SetActive(true); //Deaktiviere das Ganze Menü und mache nur die Settings sichtbar
        Shop_Button.SetActive(true);
        Play_Button.SetActive(true);
        Coin_Counter.SetActive(true);
        Player_Name_Button.SetActive(true);
        Player_Name_Text.gameObject.SetActive(true);
        Friends_Button.gameObject.SetActive(true);
        Inv_Button.gameObject.SetActive(true);
        Logo.gameObject.SetActive(true);
        Name_Input.SetActive(false);
        Settings_Button.gameObject.SetActive(true);

        Settings_Menu.gameObject.SetActive(false);
    }

    public void MusicSettingChanged(float newVolume){
        //Auf die neue Lautstärke aktualisieren
        LobbyMusic.GetComponent<AudioSource>().volume = newVolume;
        //Music in Prozent im Menü anzeigen
        
    }

}
