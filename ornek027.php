<?php

/**
 * Veritabanı bağlantısı
 */
if (!$db = mysqli_connect('localhost', 'root', '')) {
    die('Veritabanı sunucuya bağlantı hatası!');
}
if (!mysqli_select_db($db, 'gakmyo')) {
    die('Veritabanı seçim hatası.');
}
if ($sonuc = mysqli_query($db, 'create table ogrenciler (id int(11) NOT NULL AUTO_INCREMENT, ogr_no varchar(20), PRIMARY KEY(id))
    ENGINE=MyISAM DEFAULT CHARSET=utf8 COLLATE=utf8_turkish_ci')) {
    echo 'Sorgu başarıyla çalıştı.';
} else {
    echo 'Sorgu çalıştırma hatası: '.mysqli_error($db);
}
