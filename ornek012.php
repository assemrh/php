<?php
/*
    Dosya ve dizin işlemleri
    scandir ile nir klasörün içeriğini listeleme
*/

$dizin = './veri';

$liste = scandir($dizin);
foreach ($liste as $ad) {
    if ($ad != '.' && $ad != '..') {
        if (is_dir($dizin.'/'.$ad)) {
            printf('%s<br>', $ad);
        } else {
            printf('%s : %d<br>', $ad, filesize($dizin.'/'.$ad));
        }
    }
}
