<?php
/**
 * Exception (istisna) kullanımı
 * Kullanıcı tanımlı istisnalar
 */
function Ortalama($v, $f){
    if(!is_numeric($v) || !is_numeric($f)){
        throw new Exception('Vize ve final sayısal olmalı.');
    }
    if($v<0 || $v>100){
        throw new Exception('Vize 0-100 arasında olmalı!');
    }
    if($f<0 || $f>100){
        throw new Exception('Final 0-100 arasında olmalı!');
    }
    return $v*0.4 + $f*0.6;
}
// Kullanım örneği
$v = 80;
$f = '-90';
try {
    printf('Vize=%d, Final=%d ise Ortalama=%.1f<br>', $v, $f, Ortalama($v, $f));
}
catch (Exception $hata){
    echo $hata->getMessage();
}