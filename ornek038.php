<?php

/**
 * PDO ile veri döndürmeyen sorgular: UPDATE
 */
try {
    $db = new PDO('sqlite:veri/gakmyo.sqlite3');
    $sql = 'update ogrenciler set final=100 where id=10';
    $sonuc = $db->exec($sql);
    if ($sonuc === false) {
        print('<br>Sorgu hatası:<pre>');
        print_r($db->errorInfo());
    } else {
        print($sonuc. ' adet Kayıt güncellendi!');
    }
} catch (PDOException $e) {
    die('Bağlantı hatası: ' . $e->getMessage());
} catch (Exception $e) {
    die('Bilinmeyen hata: ' . $e->getMessage());
}
