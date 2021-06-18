<?php
/*
    HTML form ve bu formu işleyen PHP kodunun tek dosya içerisinde birleştirilmesi:
    Login işlemi (oturum açma)
*/
if(isset($_POST['ad'])){
    // form submit edilmiştir
    print_r($_POST);
    if($_POST['ad'] == 'ali' && $_POST['sifre'] == '321'){
        printf('Hoşgeldiniz sayın <b>%s</b><br>', $_POST['ad']);
    } else {
        echo 'Geçersiz kullanıcı adı veya şifre<br>';
    }
} else {
    // sayfa ilk defa gösteriliyor
    echo '<form action="" method="POST">
        Kullanıcı Adı: <input type="text" name="ad"><br>
        Şifre: <input type="password" name="sifre"><br>
        <input type="submit" name="giris" value="Giriş">
    </form>';
}
