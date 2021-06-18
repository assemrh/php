<pre>
<?php
/*
    pathinfo(), file_exists()
*/

$dosya = 'd:/data/liste.txt';
if (file_exists($dosya)) {
    print_r(pathinfo($dosya));
} else {
    echo "'$dosya' mevcut değil!";
}
