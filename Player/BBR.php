<?php

$servername = "http://localhost/BBR/BBR.php";
$username = "root";
$password = "";
$db = "BBR"; //Datenbank Name
// Create connection
$mysqli = new mysqli($servername, $username, $password,$db);

//Check ob die connection geklappt hat
if (mysqli_connect_errno()) {
    printf("Connect failed: %s\n", mysqli_connect_error());
    exit();
}

//Check which operation is beeing called
$register = $_POST["Register"];
$getuserdata = $_POST["getuserdata"];
$updateuserdata = $_POST["Update"];
$updateNickname = $_POST["UpdateNickname"];
$updateaftergame = $_POST["updateaftergame"];
$updateselectedskin = $_POST["updateselectedskin"];
$checkforexistance = $_POST["checkforexistance"];
$gettop = $_POST["gettop"];
$getshopinfo = $_POST["getshopinfo"];

//User Data if given
$id = $_POST["id"];
$Nickname = $_POST["Nickname"];
$SelectedSkin = $_POST["SelectedSkin"];
$Coins = $_POST["Coins"];
$Emeralds = $_POST["Emeralds"];
$Wins = $_POST["Wins"];
$Kills = $_POST["Kills"];
$Lvl = $_POST["Level"];

if($register == "1"){
    $querry = "INSERT INTO UserData (Nickname, SelectedSkin, Coins, Emeralds, Wins, Kills, Lvl) VALUES 
    ('" . $Nickname . "','" . $SelectedSkin . "','" . $Coins . "','" . $Emeralds . "','" . $Wins . "','" . $Kills . "','" . $Lvl . "');";
    //Send zur Datenbank und lasse eintragen
    $result = $mysqli->query($querry);
    if(!$result) {
        printf("%s\n", $mysqli -> error);
        echo("L");
        exit();
    }else{//Gib ID Raus
        echo $mysqli -> insert_id . "\t";
    }
}else if($updateuserdata == "1"){
    $querry = "UPDATE UserData SET SelectedSkin = '$SelectedSkin', Nickname = '$Nickname', Coins = '$Coins', Emeralds = '$Emeralds', Wins = '$Wins', Kills = '$Kills', Lvl = '$Lvl' WHERE UserData.id = $id";
    $result = $mysqli->query($querry);
    if(!$result){
        printf("%s\n", $mysqli -> error);
        exit();
    }
}else if($getuserdata == "1"){ //SELECT * FROM UserData WHERE id = 3
    $querry = "SELECT * FROM UserData WHERE id = $id";
    $result = $mysqli->query($querry);
    if(!$result){
        printf("%s\n", $mysqli -> error);
        exit();
    }else{
        //gebe die informationen aus
        $dsatz = $result->fetch_assoc();
        echo $dsatz["Nickname"] . "\t"
        . $dsatz["SelectedSkin"] . "\t" // \t um später in Unity die Daten auseinander zu nehmen
        . $dsatz["Coins"] . "\t"
        . $dsatz["Emeralds"] . "\t"
        . $dsatz["Wins"] . "\t"
        . $dsatz["Kills"] . "\t"
        . $dsatz["Lvl"] . "\t";
    }

}else if($updateNickname == "1"){
    $querry = "UPDATE UserData SET Nickname = '$Nickname' WHERE UserData.id = $id";
    $result = $mysqli->query($querry);
    if(!$result){
        printf("%s\n", $mysqli -> error);
        exit();
    }
}else if($updateaftergame == "1"){
    $querry = "UPDATE UserData SET Coins = '$Coins', Emeralds = '$Emeralds', Wins = '$Wins', Kills = '$Kills', Lvl = '$Lvl' WHERE UserData.id = $id";
    $result = $mysqli->query($querry);
    if(!$result){
        printf("%s\n", $mysqli -> error);
        exit();
    }
}else if($updateselectedskin == "1"){
    $querry = "UPDATE UserData SET SelectedSkin = '$SelectedSkin' WHERE UserData.id = $id";
    $result = $mysqli->query($querry);
    if(!$result){
        printf("%s\n", $mysqli -> error);
        exit();
    }
}else if($checkforexistance == "1"){
    $querry = "SELECT * FROM UserData WHERE id = $id";
    $result = $mysqli->query($querry);
    if(!$result){
        printf("%s\n", $mysqli -> error);
        exit();
    }else{
        if($result->num_rows > 0){
            echo("FriendExists");
        }
    }
}else if($gettop == "1"){
    $querry = "SELECT * FROM UserData ORDER BY Wins DESC, Lvl LIMIT 20";
    $result = $mysqli->query($querry);
    if(!$result){
        printf("%s\n", $mysqli -> error);
        exit();
    }else{
        $rows = $result->fetch_all(MYSQLI_ASSOC);
        foreach ($rows as $dsatz) {
            echo $dsatz["Nickname"] . "?"
            . $dsatz["SelectedSkin"] . "?" // \t um später in Unity die Daten auseinander zu nehmen
            . $dsatz["Coins"] . "?"
            . $dsatz["Emeralds"] . "?"
            . $dsatz["Wins"] . "?"
            . $dsatz["Kills"] . "?"
            . $dsatz["Lvl"] . "?"
            . $dsatz["id"] . "?" . "|";
        } 
    }
}else if($getshopinfo == "1"){
    $querry = "SELECT * FROM ShopInfo";
    $result = $mysqli->query($querry);
    if(!$result){
        printf("%s\n", $mysqli -> error);
        exit();
    }else{
        $rows = $result->fetch_all(MYSQLI_ASSOC);
        foreach ($rows as $dsatz) {
            echo $dsatz["Skin1"] . "?"
            . $dsatz["Rarity1"] . "?" // \t um später in Unity die Daten auseinander zu nehmen
            . $dsatz["Skin2"] . "?"
            . $dsatz["Rarity2"] . "?"
            . $dsatz["Skin3"] . "?"
            . $dsatz["Rarity3"] . "?"
            . $dsatz["Skin4"] . "?"
            . $dsatz["Rarity4"] . "?"
            . $dsatz["Skin5"] . "?"
            . $dsatz["Rarity5"]; //rarity ist die seltenheit angegeben von 1-5
        }
    }
}
$mysqli->close();
?>