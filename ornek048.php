<?php

/**
 * Çerezler ile kullanıcıyı tanıma
 */
if(count($_POST) > 0){
    if($_POST['kullanici'] == 'ali' && $_POST['sifre']=='123'){
        setcookie('adi', $_POST['kullanici']);
        header('location: '. basename(__FILE__));
        exit();
    } else {
        echo '<h3>Geçersiz kullanıcı adı veya şifre!</h3>';
    }
}
if (isset($_COOKIE['adi'])) {
    echo 'Merhaba ziyaretçi ' . $_COOKIE['adi'];
} else {
    ?>
    <form method="post">
        Kullanıcı Adı <input type="text" name="kullanici" ><br>
        Şifre <input type="password" name="sifre"><br>
        <input type="submit" value="Giriş">
    </form>
    <?php
}
