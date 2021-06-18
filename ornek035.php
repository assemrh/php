<?php

/**
 * PDO ile veritabanına bağlanma
 */
try {
    $db = new PDO('mysql:host=localhost;dbname=gakmyo', 'root', '');
    echo 'Bağlantı başarılı';
} catch (PDOException $e) {
    die('Bağlantı hatası: ' . $e->getMessage());
} catch (Exception $e) {
    die('Bilinmeyen hata: ' . $e->getMessage());
}
