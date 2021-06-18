<?php

/**
 * PDO ile veri döndürmeyen sorgular: INSERT
 */
try {
    $db = new PDO('sqlite:veri/gakmyo.sqlite3');
    $sql = 'insert into ogrenciler (no, adi, vize, final) values ("3320060001", "Ali AK", 87, 99)';
    if ($db->exec($sql) === false) {
        print('<br>Sorgu hatası:<pre>');
        print_r($db->errorInfo());
    } else {
        print('<br>Yeni kayıt eklendi!');
    }
} catch (PDOException $e) {
    die('Bağlantı hatası: ' . $e->getMessage());
} catch (Exception $e) {
    die('Bilinmeyen hata: ' . $e->getMessage());
}
