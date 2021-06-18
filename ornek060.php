<?php

/**
 * Günlük döviz kurlarını okuma
 */
$zaman_asimi = time() - 600;
$url = 'https://www.tcmb.gov.tr/kurlar/today.xml';
$dosya = 'xml/kurlar.xml';
if (!file_exists($dosya) || filemtime($dosya) < $zaman_asimi) {
    file_put_contents($dosya, file_get_contents($url));
}
if (!$xml = simplexml_load_file($dosya)) {
    die('XML okuma hatası!');
}

foreach ($xml->children() as $d) {
    if ($d['Kod'] == 'XDR') continue;
    printf('%s : %s <br>', $d['Kod'], $d->Isim);
}
