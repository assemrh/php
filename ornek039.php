<pre>
<?php

/**
 * PDO ile veri döndüren sorgular: SELECT
 */
try {
    $db = new PDO('sqlite:veri/gakmyo.sqlite3');
    // $db = new PDO('mysql:host=localhost;dbname=gakmyo', 'root', '');
    $sql = 'select * from ogrenciler';
    $sorgu = $db->query($sql);
    if ($sorgu === false) {
        print('<br>Sorgu hatası:<pre>');
        print_r($db->errorInfo());
    } else {
        while ($kayit = $sorgu->fetch(PDO::FETCH_ASSOC)) {
            print_r($kayit);
        }
    }
} catch (PDOException $e) {
    die('Bağlantı hatası: ' . $e->getMessage());
} catch (Exception $e) {
    die('Bilinmeyen hata: ' . $e->getMessage());
}
