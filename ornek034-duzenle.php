<?php

/**
 * Veritabanından kayıt okuma ve tablo olarak yazdırıp düzenlemeye izin verme
 */
if (!$db = mysqli_connect('localhost', 'root', '')) {
    die('Veritabanı sunucuya bağlantı hatası!');
}
if (!mysqli_select_db($db, 'gakmyo')) {
    die('Veritabanı seçim hatası.');
}
mysqli_set_charset($db, 'utf8');

if(count($_POST) > 0){
    // Form submit edilmiş (kaydedilmiş)
    // $sql = 'update ogrenciler set ogr_no="'.$_POST['ogr_no'].'", adi="Ali", vize=90, final=100 where id=1';
    $sql = sprintf('update ogrenciler set ogr_no="%s", adi="%s", vize=%d, final=%d where id=%d', 
        mysqli_real_escape_string($db, $_POST['ogr_no']), 
        mysqli_real_escape_string($db, $_POST['adi']), 
        mysqli_real_escape_string($db, $_POST['vize']), 
        mysqli_real_escape_string($db, $_POST['final']), 
        mysqli_real_escape_string($db, $_POST['id']));
    $sonuc = mysqli_query($db, $sql);
    if($sonuc){
        echo 'Güncelleme başarılı.';
    } else {
        echo 'Güncelleme hatası: '.mysqli_error($db);
    }
} else {
    // Formu görüntüle
    echo '<h1>Öğrenci Bilgilerini Güncelle</h1>';
    if (!isset($_GET['id'])) {
        die("Öğrenci ID'si verilmemiş");
    }
    $id = mysqli_real_escape_string($db, $_GET['id']);
    if ($sonuc = mysqli_query($db, 'select * from ogrenciler where id=' . $id)) {
        $veri = mysqli_fetch_assoc($sonuc);
        if(!$veri){
            echo 'Aranan öğrenci kaydı bulunamadı!';
        } else {
            printf('<form action="'.basename(__FILE__).'" method="post">
                Öğrenci No: <input type="text" name="ogr_no" value="%d"><br>
                Adı Soyadı: <input type="text" name="adi" value="%s"><br>
                Vize: <input type="text" name="vize" value="%d"><br>
                Final: <input type="text" name="final" value="%d"><br>
                <input type="hidden" name="id" value="%d">
                <input type="submit" value="Kaydet">
                </form>', $veri['ogr_no'], $veri['adi'], $veri['vize'], $veri['final'], $veri['id']);
        }
    } else {
        echo 'Sorgu çalıştırma hatası: '.mysqli_error($db);
    }
}