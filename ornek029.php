<pre><?php
/**
 * Veritabanından kayıt okuma
 * mysqli_fetch_array
 */
if (!$db = mysqli_connect('localhost', 'root', '')) {
    die('Veritabanı sunucuya bağlantı hatası!');
}
if (!mysqli_select_db($db, 'gakmyo')) {
    die('Veritabanı seçim hatası.');
}
mysqli_set_charset($db, 'utf8');


if ($sonuc = mysqli_query($db, 'select * from ogrenciler where id=999')) {
    echo 'Sorgu başarıyla çalıştı.<br>';
    print_r($sonuc);
    while($veri = mysqli_fetch_array($sonuc, MYSQLI_ASSOC)){
        print_r($veri);
    }
} else {
    echo 'Sorgu çalıştırma hatası: '.mysqli_error($db);
}
