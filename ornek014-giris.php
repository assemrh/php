<pre><?php
/*
    SuperGloballer: $_POST
*/

// print_r($_POST);
if($_POST['ad'] == 'ali' && $_POST['sifre'] == '321'){
    printf('Hoşgeldiniz sayın <b>%s</b><br>', $_POST['ad']);
} else {
    echo 'Geçersiz kullanıcı adı veya şifre<br>';
}