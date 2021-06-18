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
echo $xml->kitap[0]->baslik.'<br>';
echo $xml->kitap[1]->baslik.'<br>';
echo $xml->kitap[2]->baslik.'<br>';
echo $xml->kitap[3]->baslik.'<br>';
