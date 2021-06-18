<?php
function cevre($r){
    if($r <= 0){
        return;
    }
    return 2 * M_PI * $r;
}

// fonksiyonun kullanımı
$r = -5;
$c = cevre($r);
if(is_null($c)){
    echo 'Başka yarıçap girin.';
}else printf('Yarıçapı=%d ise Çevresi=%.1f', $r, $c);
