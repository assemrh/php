<?php
/**
 * Exception (istisna) kullanımı
 */
function cevre($r){
    if($r <= 0){
        throw new Exception('Yarıçap sıfırdan büyük olmalı: '.$r.'<br>', 1);
    }
    return 2 * M_PI * $r;
}

// fonksiyonun kullanımı
$r = -5;
try {
    $c = cevre($r);
    printf('Yarıçapı=%d ise Çevresi=%.1f', $r, $c);
} catch (Exception $e) {
    printf('Çevre hesaplarken hata oluştu: (Kod=%d) %s<br>', $e->getCode(), $e->getMessage());
}