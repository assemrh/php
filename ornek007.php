<?php
/*
    Dosya ve dizin işlemleri
    copy
*/

$kaynak = 'test.txt';
$hedef = 'yeni.txt';
if(copy($kaynak, $hedef)){
    printf('<b>%s</b> isimli dosya <b>%s</b> olarak kopyalandı.', $kaynak, $hedef);
} else {
    printf('<b>%s</b> isimli dosya <b>%s</b> olarak kopyalanırken hata oluştu!', $kaynak, $hedef);
}
