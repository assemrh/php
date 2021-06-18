<pre><?php
/**
 * Veritabanından kayıt okuma
 * mysqli_fetch_row
 * mysqli_fetch_assoc
 */
if (!$db = mysqli_connect('localhost', 'root', '')) {
    die('Veritabanı sunucuya bağlantı hatası!');
}
if (!mysqli_select_db($db, 'gakmyo')) {
    die('Veritabanı seçim hatası.');
}
mysqli_set_charset($db, 'utf8');
if ($sonuc = mysqli_query($db, 'select * from ogrenciler')) {
    echo 'Sorgu başarıyla çalıştı.<br>';
    print_r($sonuc);
    while($veri = mysqli_fetch_assoc($sonuc)){
        print_r($veri);
    }
} else {
    echo 'Sorgu çalıştırma hatası: '.mysqli_error($db);
}
