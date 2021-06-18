<?php
/**
 * Exception (istisna) kullanımı
 * Kullanıcı tanımlı istisnalar
 */
class SifirdanKucukException extends Exception {

}

function cevre($r){
    if(!is_numeric($r)){
        throw new InvalidArgumentException('Yarıçap sayısal olmalı: '.$r.'<br>', 2);
    }
    if($r <= 0){
        throw new SifirdanKucukException('Yarıçap sıfırdan büyük olmalı: '.$r.'<br>', 1);
    }
    return 2 * M_PI * $r;
}

// fonksiyonun kullanımı
$r = 'ali';
try {
    $c = cevre($r);
    printf('Yarıçapı=%d ise Çevresi=%.1f', $r, $c);
} catch (InvalidArgumentException $e) {
    printf('Lütfen sayısal değer girin!');
} catch (SifirdanKucukException $e) {
    printf('Lütfen sıfırdan büyük değer girin!<br>');
} catch (Exception $e) {
    printf('Çevre hesaplarken bilinmeyen hata oluştu: (Kod=%d) %s<br>', $e->getCode(), $e->getMessage());
}