<?php
// http://api.openweathermap.org/data/2.5/forecast/daily?q=bursa&appid=1d762dd3a9021b2b46b9d6dd58ef8066&mode=xml&units=metric
$appid = '1d762dd3a9021b2b46b9d6dd58ef8066';
$il = 'bursa';
// $il = 'istanbul';
// $il = 'antalya';
$mode = 'xml';
$unit = 'metric';
$gun = 14;
$url = "http://api.openweathermap.org/data/2.5/forecast/daily?q={$il}&appid={$appid}&mode={$mode}&units={$unit}&lang=tr&cnt={$gun}";
$dosya = "xml/hava-daily-{$il}.xml";
setlocale(LC_ALL, 'Turkey');
if (!file_exists($dosya) || filemtime($dosya) < time() - 60) {
    file_put_contents($dosya, file_get_contents($url));
}
if (!$xml = simplexml_load_file($dosya))
    die('XML yüklemede hata oluştu!');

$xml = simplexml_load_file($dosya);
echo '<div style="display: flex; flex-wrap:wrap">' . ' test';
foreach ($xml->forecast->children() as $time) {
    print '<div style="width: 200px; border:1px solid; text-align:center; padding: 5px;margin: 5px;vertical-align: top">';
    printf('<div>%s [%s]</div>', $xml->location->name, $xml->location->country);
    printf('<div>%s</div>', date('d.m.Y D', strtotime($time['day'])));
    printf(
        '<div><img src="http://openweathermap.org/img/w/%s.png"></div>',
        $time->symbol['var']
    );
    printf('<div>%s</div>', $time->symbol['name']);
    printf('<div><b>%s&deg;C</b></div>', $time->temperature['day']);
    printf('<div>%s&deg;C - %s&deg;C</div>', $time->temperature['min'], $time->temperature['max']);
    printf('<div>Rüzgar: %s m/s %s</div>', $time->windSpeed['name'], $time->windDirection['name']);
    printf('<div>Gün Doğumu: %s</div>', date('H:i', strtotime($time->sun['rise']) + $xml->location->timezone));
    printf('<div>Gün Batımı: %s</div>', date('H:i', strtotime($time->sun['set']) + $xml->location->timezone));
    print '</div>';
}
echo '</div>';