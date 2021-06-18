<?php

/**
 * XML içeriğini okuma
 */

$dosya = 'xml/eposta.xml';
if(!file_exists($dosya)){
    die('XML dosyası bulunamadı!');
}
if(!$xml = simplexml_load_file($dosya)){
    die('XML okuma hatası!');
}
echo $xml->kime.'<br>';
echo $xml->kimden.'<br>';
echo $xml->konu.'<br>';
echo $xml->mesaj.'<br>';
