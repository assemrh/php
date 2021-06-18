<pre><?php
/*
    Farklı modlarda (okuma, yazma vb) dosya açma ve 
    fread(), fwrite(), fseek() kullanımı
*/

$dosya = 'webterimleri.txt';
if(file_exists($dosya)){
    $h = fopen($dosya, 'w+');
} else {
    $h = fopen($dosya, 'a+');
}
fwrite($h, "HTML=Hyper Text Markup Language\n");
fwrite($h, "CSS=Cascaded Style Sheets\n");
fwrite($h, "JSON=JavaScript Object Notation\n");
fseek($h, 0, SEEK_SET);
while($veri = fread($h, 100)){
    echo $veri;
}
flush();
fclose($h);