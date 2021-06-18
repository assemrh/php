<?php
/*
    include ve require ile kod kütüphanesi oluşturma:
    Geometri Programı Kod Kütüphanesi
*/

function daire_alan($r, $pi=M_PI){
    return $pi * $r * $r;
}

function daire_cevre($r, $pi = M_PI){
    return 2 * $pi * $r;
}
