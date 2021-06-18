<?php

/**
 * XML dizi içerikleri okuma
 */

$dosya = 'xml/kutuphane.xml';
if(!file_exists($dosya)){
    die('XML dosyası bulunamadı!');
}
if(!$xml = simplexml_load_file($dosya)){
    die('XML okuma hatası!');
}
foreach($xml->children() as $kitap){
    echo $kitap->baslik.'<br>';
}