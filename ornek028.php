<?php
/**
 * Veritabanına kayıt ekleme
 */
if (!$db = mysqli_connect('localhost', 'root', '')) {
    die('Veritabanı sunucuya bağlantı hatası!');
}
if (!mysqli_select_db($db, 'gakmyo')) {
    die('Veritabanı seçim hatası.');
}
mysqli_set_charset($db, 'utf8');
if ($sonuc = mysqli_query($db, 'insert into ogrenciler (ogr_no, adi) values("1003", "Fatma PAK")')) {
    echo 'Sorgu başarıyla çalıştı.';
} else {
    echo 'Sorgu çalıştırma hatası: '.mysqli_error($db);
}
