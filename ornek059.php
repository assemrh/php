<?php

/**
 * Günlük döviz kurlarını okuma
 */
$url = 'https://www.tcmb.gov.tr/kurlar/today.xml';
$dosya = 'xml/kurlar.xml';
if(!file_exists($dosya)){
    file_put_contents($dosya, file_get_contents($url));
}
/*
$dosya = 'xml/kutuphane.xml';
if(!file_exists($dosya)){
    die('XML dosyası bulunamadı!');
}
*/
if (!$xml = simplexml_load_file($dosya)) {
    die('XML okuma hatası!');
}

foreach ($xml->children() as $d) {
    if($d['Kod'] == 'XDR') continue;
    printf('%s : %s <br>', $d['Kod'], $d->Isim);
}
