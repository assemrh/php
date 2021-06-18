<?php

/**
 * XML hava durumu alma
 */
$il = 'bursa';
$appid = '1d762dd3a9021b2b46b9d6dd58ef8066';
$mode = 'xml';
$url = "http://api.openweathermap.org/data/2.5/weather?q=$il&appid=$appid&mode=$mode&units=metric&lang=tr";
$zaman_asimi = time() - 600;
$dosya = "xml/hava-$il.xml";
if (!file_exists($dosya) || filemtime($dosya) < $zaman_asimi) {
    file_put_contents($dosya, file_get_contents($url));
}
if (!$xml = simplexml_load_file($dosya)) {
    die('XML okuma hatası!');
}

echo '<div>';
printf('<div>%s</div>', $xml->city['name']);
printf('<div><img src="http://openweathermap.org/img/w/%s.png"> %s</div>', $xml->weather['icon'], $xml->weather['value']);
printf('<div>Sıcaklık: %s &deg;C (%s - %s)</div>', $xml->temperature['value'], $xml->temperature['min'], $xml->temperature['max']);
printf('<div>Nem: %s %s</div>', $xml->humidity['value'], $xml->humidity['unit']);
printf('<div>Gün Doğumu: %s</div>', date('d.m.Y H:i', strtotime($xml->city->sun['rise']) + $xml->city->timezone));
printf('<div>Gün Batımı: %s</div>', date('d.m.Y H:i', strtotime($xml->city->sun['set']) + $xml->city->timezone));
printf('<div><small>Güncelleme: %s</small></div>', date('d.m.Y H:i', strtotime($xml->lastupdate['value']) + $xml->city->timezone));

echo '</div>';