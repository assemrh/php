<?php
/*
    Dosya ve dizin işlemleri
    mkdir
*/

$dizin = 'veri/ali/resim';
if(mkdir($dizin, 0777, true)){
    printf('<b>%s</b> isimli dizin oluşturuldu.', $dizin);
} else {
    printf('<b>%s</b> isimli dizin oluşturulurken hata oluştu!', $dizin);
}
