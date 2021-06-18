<?php
/*
    Dosya ve dizin işlemleri
    unlink
*/

$dosya = 'test.txt';
if(unlink($dosya)){
    printf('<b>%s</b> isimli dosya silindi.', $dosya);
} else {
    printf('<b>%s</b> isimli dosya silinirken hata oluştu!', $dosya);
}
