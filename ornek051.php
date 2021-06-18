<?php

/**
 * PHP SESSION ile kullanıcı tanıma
 */
session_start();

if (count($_POST) > 0) {
    if ($_POST['kullanici'] == 'ali' && $_POST['sifre'] == '123') {
        $_SESSION['adi'] = $_POST['kullanici'];
        header('location: ' . basename(__FILE__));
        exit();
    } else {
        echo '<h3>Geçersiz kullanıcı adı veya şifre!</h3>';
    }
} elseif (isset($_GET['id'])) {
    $_SESSION['sepet'][$_GET['id']] = ($_SESSION['sepet'][$_GET['id']] ?? 0) + 1;
}

if (isset($_SESSION['adi'])) {
    echo 'Merhaba ziyaretçi ' . $_SESSION['adi'] . '<br>';
?>
    <ul>
        <li>Ürün 1 <a href="<?php echo basename(__FILE__); ?>?id=1">Sepete Ekle</a></li>
        <li>Ürün 2 <a href="<?= basename(__FILE__) ?>?id=2">Sepete Ekle</a></li>
        <li>Ürün 3 <a href="<?= basename(__FILE__) ?>?id=3">Sepete Ekle</a></li>
        <li>Ürün 4 <a href="<?= basename(__FILE__) ?>?id=4">Sepete Ekle</a></li>
        <li>Ürün 5 <a href="<?= basename(__FILE__) ?>?id=5">Sepete Ekle</a></li>
        <li>Ürün 6 <a href="<?= basename(__FILE__) ?>?id=6">Sepete Ekle</a></li>
        <li>Ürün 7 <a href="<?= basename(__FILE__) ?>?id=7">Sepete Ekle</a></li>
        <li>Ürün 8 <a href="<?= basename(__FILE__) ?>?id=8">Sepete Ekle</a></li>
        <li>Ürün 9 <a href="<?= basename(__FILE__) ?>?id=9">Sepete Ekle</a></li>
    </ul>
    <?php
    if (isset($_SESSION['sepet'])) {
        echo '<ul>';
        foreach ($_SESSION['sepet'] as $id => $adet) {
            printf('<li>%d : %d adet</li>', $id, $adet);
        }
        echo '</ul>';
    }
} else {
    ?>
    <form method="post">
        Kullanıcı Adı <input type="text" name="kullanici"><br>
        Şifre <input type="password" name="sifre"><br>
        <input type="submit" value="Giriş">
    </form>
<?php
}
