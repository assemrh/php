<?php
/**
 * Exception (istisna) kullanımı
 */
function cevre($r){
    if(!is_numeric($r)){
        throw new InvalidArgumentException('Yarıçap sayısal olmalı: '.$r.'<br>', 2);
    }
    if($r <= 0){
        throw new Exception('Yarıçap sıfırdan büyük olmalı: '.$r.'<br>', 1);
    }
    return 2 * M_PI * $r;
}

// fonksiyonun kullanımı
$r = '-5';
try {
    $c = cevre($r);
    printf('Yarıçapı=%d ise Çevresi=%.1f', $r, $c);
} catch (InvalidArgumentException $e) {
    printf('(Kod=%d) %s<br>', $e->getCode(), $e->getMessage());
} catch (Exception $e) {
    printf('Çevre hesaplarken hata oluştu: (Kod=%d) %s<br>', $e->getCode(), $e->getMessage());
}