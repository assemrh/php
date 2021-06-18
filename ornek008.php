<?php
/*
    Dosya ve dizin işlemleri
    rename
*/

$kaynak = 'test.txt';
$hedef = 'yeni.txt';
if(rename($kaynak, $hedef)){
    printf('<b>%s</b> isimli dosya <b>%s</b> olarak değiştirildi.', $kaynak, $hedef);
} else {
    printf('<b>%s</b> isimli dosya <b>%s</b> olarak değiştirilirken hata oluştu!', $kaynak, $hedef);
}
