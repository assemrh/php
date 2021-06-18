<pre><?php
/*
    SuperGloballer: $_GET
*/

print_r($_GET);
if($_GET['ad'] == 'ali' && $_GET['sifre'] == '321'){
    printf('Hoşgeldiniz sayın <b>%s</b><br>', $_GET['ad']);
} else {
    echo 'Geçersiz kullanıcı adı veya şifre<br>';
}