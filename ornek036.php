<?php

/**
 * PDO ile veri döndürmeyen sorgular
 */
try {
    $db = new PDO('sqlite:veri/gakmyo.sqlite3');
    echo 'Bağlantı başarılı';
    $sql = 'create table ogrenciler (
        no varchar(15) PRIMARY KEY NOT NULL UNIQUE,
        adi varchar(50))';
    if ($db->exec($sql) === false) {
        print('Tablo oluşturma hatası:<pre>');
        print_r($db->errorInfo());
    } else {
        print('Tablo oluşturma başarılı!');
    }
} catch (PDOException $e) {
    die('Bağlantı hatası: ' . $e->getMessage());
} catch (Exception $e) {
    die('Bilinmeyen hata: ' . $e->getMessage());
}
