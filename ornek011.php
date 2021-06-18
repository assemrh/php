<?php
/*
    Dosya ve dizin işlemleri
    rmdir
*/

$dizin = 'veri/ali/resim';
if(rmdir($dizin)){
    printf('<b>%s</b> isimli dizin oluşturuldu.', $dizin);
} else {
    printf('<b>%s</b> isimli dizin oluşturulurken hata oluştu!', $dizin);
}
